namespace CourseLibrary.Application.Contracts.RepositoryContracts;

public interface IRepository
{
    Task<bool> SaveAsync();
}