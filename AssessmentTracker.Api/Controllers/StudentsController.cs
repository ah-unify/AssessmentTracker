using AssessmentTracker.Api.Models.Requests;
using AssessmentTracker.Api.Models.Responses;
using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Domain.ValueObjects;
using AssessmentTracker.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentTracker.Api.Controllers;

public class StudentsController : ApiControllerBase
{
    private readonly DataContext _context;

    public StudentsController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<Student?> GetStudent(Guid id)
    {
        return _context.Students
            .Include(x => x.Courses)
            .Include(x => x.AssessmentRecords)
            .FirstOrDefault(x => x.Id == id);
    }

    [HttpPost]
    public async Task<ApiResponse> RegisterStudent(CreateStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return ApiResponse.Failure("FirstName must be provided");
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return ApiResponse.Failure("LastName must be provided");
        }

        if (DateTime.UtcNow - request.DateOfBirth < TimeSpan.FromDays(365 * 18))
        {
            return ApiResponse.Failure("Student must be at least 18 years old.");
        }

        var student = new Student
        {
            FirstName = request.FirstName,
            LastName = request.LastName,
            DateOfBirth = request.DateOfBirth.ToUniversalTime(),
            RegisterDate = DateTime.UtcNow
        };

        _context.Students.Add(student);

        await _context.SaveChangesAsync();

        return ApiResponse.Success().WithCustomProperties(new
        {
            StudentId = student.Id
        });
    }

    [HttpPost]
    public async Task<ApiResponse> RegisterStudentForCourse(RegisterStudentForCourseRequest request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null)
        {
            return ApiResponse.Failure("Student not found by ID");
        }

        var course = await _context.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            return ApiResponse.Failure("Course not found by ID");
        }

        student.RegisterCourse(course);

        await _context.SaveChangesAsync();
        return ApiResponse.Success();
    }

    [HttpPost]
    public async Task<ApiResponse> RecordAssessment(RecordAssessmentRequest request)
    {
        if (!Grade.IsValid(request.Grade))
        {
            return ApiResponse.Failure("Not a valid grade.");
        }
        
        var student = await _context.Students
            .Include(x => x.Courses)
            .ThenInclude(x => x.Assessments)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            return ApiResponse.Failure("Student not found.");
        }

        var assessment = student
            .Courses
            .SelectMany(x => x.Assessments)
            .FirstOrDefault(x => x.Id == request.AssessmentId);
        
        if (assessment == null)
        {
            return ApiResponse.Failure("Assessment not found in student's courses.");
        }

        var assessmentRecord = new AssessmentRecord
        {
            Grade = new Grade(request.Grade)
        };
        
        student.RecordAssessment(assessmentRecord);
        assessment.BindRecord(assessmentRecord);

        await _context.SaveChangesAsync();
        return ApiResponse.Success();
    }
}