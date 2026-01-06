namespace CourseLibrary.Application.Models;

public class AuthorForCreationWithDateOfDeathDto : AuthorForCreationDto
{
    public DateTimeOffset DateOfDeath { get; set; }
}