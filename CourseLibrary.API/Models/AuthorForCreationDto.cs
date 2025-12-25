namespace CourseLibrary.API.Models
{
    public class AuthorForCreationDto
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public DateTimeOffset DateOfBirth { get; set; }
        public string MainCategory { get; set; } = string.Empty;

        //To add an author and its courses at the same time(in one action):
        public ICollection<CourseForCreationDto> courses { get; set; } = new List<CourseForCreationDto>();
    }
}
