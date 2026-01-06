namespace CourseLibrary.Application.Contracts;

public interface IRepository
{
    Task<bool> SaveAsync();
}