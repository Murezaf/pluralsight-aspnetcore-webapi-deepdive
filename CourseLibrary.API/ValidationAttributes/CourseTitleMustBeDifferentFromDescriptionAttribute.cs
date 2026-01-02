using CourseLibrary.API.Models;
using System.ComponentModel.DataAnnotations;

public class CourseTitleMustBeDifferentFromDescriptionAttribute : ValidationAttribute
{
    public CourseTitleMustBeDifferentFromDescriptionAttribute()
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        CourseForManipulationDto? manipulationCourse = validationContext.ObjectInstance as CourseForManipulationDto;

        if (manipulationCourse == null)
        {
            return ValidationResult.Success;
        }

        //if (validationContext.ObjectInstance is not CourseForManipulationDto manipulationCourse)
        //{
        //    return ValidationResult.Success;
        //}

        if (manipulationCourse.Title == manipulationCourse.Description)
        {
            return new ValidationResult("Description should be different from Title.", new[] { "Course" });
        }

        return ValidationResult.Success;
    }
}