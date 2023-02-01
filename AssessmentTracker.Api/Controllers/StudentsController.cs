using AssessmentTracker.Api.Models;
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

    [HttpGet()]
    public async Task<StudentDto> GetStudent(Guid id)
    {
        var student = _context.Students
            .Include(x => x.Courses)
            .Include(x => x.AssessmentRecords)
            .FirstOrDefault(x => x.Id == id);

        if (student == null)
        {
            return new StudentDto();
        }

        return new StudentDto
        {
            Id = student.Id,
            FirstName = student.FirstName,
            LastName = student.LastName,
            Courses = student.Courses.Select(x => new CourseDto()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList()
        };
    }

    [HttpPost]
    public async Task<IActionResult> RegisterStudent(RegisterStudentRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.FirstName))
        {
            return BadRequest($"{nameof(request.FirstName)} must be provided.");
        }

        if (string.IsNullOrWhiteSpace(request.LastName))
        {
            return BadRequest($"{nameof(request.LastName)} must be provided.");
        }

        if (DateTime.UtcNow - request.DateOfBirth < TimeSpan.FromDays(365 * 18))
        {
            return BadRequest("Student must be at least 18 years old.");
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

        return Ok(new StudentRegistered
        {
            StudentId = student.Id
        });
    }


    [HttpPost]
    public async Task<IActionResult> RegisterStudentForCourse(RegisterStudentForCourseRequest request)
    {
        var student = await _context.Students.FindAsync(request.StudentId);
        if (student == null)
        {
            return BadRequest("Student not found.");
        }

        var course = await _context.Courses.FindAsync(request.CourseId);
        if (course == null)
        {
            return BadRequest("Course not found.");
        }

        student.RegisterCourse(course);

        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPost]
    public async Task<IActionResult> RecordAssessment(RecordAssessmentRequest request)
    {
        if (!Grade.IsValid(request.Grade))
        {
            return BadRequest("Grade invalid.");
        }

        var student = await _context.Students
            .Include(x => x.Courses)
            .ThenInclude(x => x.Assessments)
            .FirstOrDefaultAsync(x => x.Id == request.StudentId);

        if (student == null)
        {
            return BadRequest("Student not found.");
        }

        var assessment = student
            .Courses
            .SelectMany(x => x.Assessments)
            .FirstOrDefault(x => x.Id == request.AssessmentId);

        if (assessment == null)
        {
            return BadRequest("Assessment not found in student's courses.");
        }

        var assessmentRecord = new AssessmentRecord
        {
            Grade = new Grade(request.Grade)
        };

        student.RecordAssessment(assessmentRecord);
        assessment.BindRecord(assessmentRecord);

        await _context.SaveChangesAsync();
        return Ok();
    }
}