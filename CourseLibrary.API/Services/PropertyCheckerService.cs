using System.Reflection;

namespace CourseLibrary.API.Services;

public class PropertyCheckerService : IPropertyCheckerService
{
    public bool TypeHasProperties<T>(string? fields)
    {
        if (string.IsNullOrWhiteSpace(fields))
            return true;

        string[] fieldsAfterSplit = fields.Split(',');
        foreach (string field in fieldsAfterSplit)
        {
            string propertyName = field.Trim();

            var propertyInfo = typeof(T).GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if (propertyInfo == null)
                return false;
        }

        return true;
    }
}
