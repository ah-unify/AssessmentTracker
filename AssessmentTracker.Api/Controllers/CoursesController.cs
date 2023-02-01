using AssessmentTracker.Api.Models.Requests;
using AssessmentTracker.Api.Models.Responses;
using AssessmentTracker.Domain.Entities;
using AssessmentTracker.Domain.ValueObjects;
using AssessmentTracker.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AssessmentTracker.Api.Controllers;

public class CoursesController : ApiControllerBase
{
    private readonly DataContext _context;

    public CoursesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<Course?> GetCourse(Guid id)
    {
        return _context.Courses
            .Include(x => x.Assessments)
            .Include(x => x.Students)
            .FirstOrDefault(x => x.Id == id);
    }

    [HttpPost]
    public async Task<ApiResponse> RegisterCourse(CreateCourseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return ApiResponse.Failure("Name must be provided");
        }

        if (request.Assessments.Sum(x => x.CoursePercentage) != 100)
        {
            return ApiResponse.Failure("Assessment percentages must add up to a total of 100");
        }

        var course = new Course()
        {
            Name = request.Name
        };

        var assessments = request.Assessments.Select(x => new Assessment
        {
            Name = x.Name,
            CoursePercentage = new Percentage(x.CoursePercentage)
        }).ToList();

        course.AddAssessments(assessments);

        _context.Courses.Add(course);

        await _context.SaveChangesAsync();

        return ApiResponse.Success().WithCustomProperties(new
        {
            courseId = course.Id
        });
    }
}