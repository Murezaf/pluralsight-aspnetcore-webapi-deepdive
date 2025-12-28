using CourseLibrary.API.Models;
using System.ComponentModel.DataAnnotations;

public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
{
    public CourseTitleMustBeDifferentFromDescriptionAttribute()
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        //var manipulationCourse = validationContext.ObjectInstance as CourseForManipulationDto;

        //if (manipulationCourse == null)
        //{
            //throw new Exception($"Attribute {nameof(CourseTitleMustBeDifferentFromDescriptionAttribute)} must be applied to a {nameof(CourseForManipulationDto)} or derrived type.");
        //}

        if (validationContext.ObjectInstance is not CourseForManipulationDto manipulationCourse)
        {
            throw new Exception($"Attribute {nameof(CourseTitleMustBeDifferentFromDescriptionAttribute)} must be applied to a {nameof(CourseForManipulationDto)} or derrived type.");
        }

        if (manipulationCourse.Title == manipulationCourse.Description)
        {
            return new ValidationResult("Description should be different from Title.", new[] { "Course" });
        }

        return ValidationResult.Success;
    }
}