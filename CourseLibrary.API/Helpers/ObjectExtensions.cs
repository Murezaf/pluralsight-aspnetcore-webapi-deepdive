using System.Dynamic;
using System.Reflection;

namespace CourseLibrary.API.Helpers;

public static class ObjectExtensions
{
    public static ExpandoObject ShapeData<TSource>(this TSource source, string? fields)
    {
        if (source == null)
            throw new ArgumentNullException(nameof(source));

        ExpandoObject dataShapedObject = new ExpandoObject();

        if (string.IsNullOrWhiteSpace(fields))
        {
            PropertyInfo[] propertyInfos = typeof(TSource).GetProperties(
                BindingFlags.IgnoreCase | BindingFlags.Instance | BindingFlags.Public);

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                ((IDictionary<string, object?>)dataShapedObject).Add(propertyInfo.Name, propertyInfo.GetValue(source));
            }
            
            return dataShapedObject;
        }

        string[] fieldsAfterSplit = fields.Split(',');
        foreach (string field in fieldsAfterSplit)
        {
            string propertyName = field.Trim();
            var propertyInfo = typeof(TSource).GetProperty(propertyName,
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);

            if(propertyInfo == null)
                throw new Exception($"Property Name {propertyName} wasn't found on {typeof(TSource)}");

            ((IDictionary<string, object?>)dataShapedObject).Add(propertyInfo.Name, propertyInfo.GetValue(source));
        }

        return dataShapedObject;
    }
}
