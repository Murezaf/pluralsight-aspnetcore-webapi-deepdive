using System.ComponentModel.DataAnnotations;

namespace CourseLibrary.API.Models
{
    [CourseTitleMustBeDifferentFromDescription]
    public abstract class CourseForManipulationDto //: IValidatableObject
    {
        [Required(ErrorMessage = "You should fillout the title.")]
        [MaxLength(100, ErrorMessage = "Title can't have more than 100 characters.")]
        public string Title { get; set; } = string.Empty;

        [MaxLength(1500, ErrorMessage = "Description can't have more than 1500 characters.")]
        //public string Description { get; set; } = string.Empty;
        public virtual string Description { get; set; } = string.Empty; //allow Description to be null

        //public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        //{
        //    if (Title == Description)
        //    {
        //        yield return new ValidationResult(
        //            "Description should be different from Title.", new[] { "Course" } );
        //        //yield return new ValidationResult("Description should be different from Title.", new[] { nameof(Title), nameof(Description) } );
        //    }
        //}
    }
}
