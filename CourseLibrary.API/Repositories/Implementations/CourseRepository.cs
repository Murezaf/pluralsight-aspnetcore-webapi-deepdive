using CourseLibrary.API.DbContexts;
using CourseLibrary.API.Entities;
using CourseLibrary.API.Repositories.Interfaces;
using CourseLibrary.API.Services;
using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Repositories.Implementations;

public class CourseRepository : ICourseRepository
{
    private readonly CourseLibraryContext _context;

    public CourseRepository(CourseLibraryContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    public async Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        return await _context.Courses.Where(c => c.AuthorId == authorId).ToListAsync();
    }

    public async Task<Course> GetCourseAsync(Guid authorId, Guid courseId)
    {
        if (courseId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(courseId));
        }

        return await _context.Courses.Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefaultAsync();
    }

    public void AddCourse(Guid authorId, Course course)
    {
        if (authorId == Guid.Empty)
        {
            throw new ArgumentNullException(nameof(authorId));
        }

        if (course == null)
        {
            throw new ArgumentNullException(nameof(course));
        }

        course.AuthorId = authorId;
        
        _context.Courses.Add(course);
    }

    public void DeleteCourse(Course course)
    {
        _context.Courses.Remove(course);  //No null check needed here. The controller already ensures the entity exists. In addition, SaveChangesAsync() will affect 0 rows if the entity was already deleted.
    }

    public async Task<bool> SaveAsync()
    {
        return await _context.SaveChangesAsync() >= 0;
    }
}