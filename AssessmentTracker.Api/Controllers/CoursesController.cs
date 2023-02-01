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
    public async Task<IActionResult> RegisterCourse(RegisterCourseRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Name))
        {
            return BadRequest("Name must be provided");
        }

        if (request.Assessments.Sum(x => x.CoursePercentage) != 100)
        {
            return BadRequest("Assessment percentages must add up to a total of 100");
        }

        if (_context.Courses.Any(x => x.Name == request.Name))
        {
            return BadRequest("A course with this name already exists.");
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

        return Ok(new CourseRegistered
        {
            CourseId = course.Id
        });
    }
}