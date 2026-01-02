namespace CourseLibrary.API.Repositories.Interfaces;

public interface IRepository
{
    Task<bool> SaveAsync();
}