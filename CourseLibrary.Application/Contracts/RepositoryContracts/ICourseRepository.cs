using CourseLibrary.Domain;

namespace CourseLibrary.Application.Contracts.RepositoryContracts;

public interface ICourseRepository : IRepository
{
    Task<IEnumerable<Course>> GetCoursesAsync(Guid authorId);
    Task<Course> GetCourseAsync(Guid authorId, Guid courseId);
    void AddCourse(Guid authorId, Course course);
    void DeleteCourse(Course course);
}