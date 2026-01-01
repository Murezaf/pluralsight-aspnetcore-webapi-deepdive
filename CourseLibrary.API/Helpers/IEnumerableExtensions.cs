using System.Dynamic;
using System.Reflection;

namespace CourseLibrary.API.Helpers;

public static class IEnumerableExtensions
{
    public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string? fields)
    {
        if (source == null) throw new ArgumentNullException(nameof(source));

        List<ExpandoObject> expandoObjectList = new List<ExpandoObject>();
        List<PropertyInfo> propertyInfoList = new List<PropertyInfo>();

        if (fields == null)
        {
            PropertyInfo[] propertyInfos = typeof(TSource).GetProperties(
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
            propertyInfoList.AddRange(propertyInfos);
        }
        else
        {
            string[] fieldsAfterSplit = fields.Split(',');
            foreach (string field in fieldsAfterSplit)
            {
                string propertyName = field.Trim();

                PropertyInfo? propertyInfo = typeof(TSource).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    throw new Exception($"Property {propertyName} wasn't found on {nameof(TSource)}");

                propertyInfoList.Add(propertyInfo);
            }
        }

        foreach (TSource sourceObject in source)
        {
            ExpandoObject dataShapedObject = new ExpandoObject();

            foreach (PropertyInfo propertyInfo in propertyInfoList)
            {
                ((IDictionary<string, object?>)dataShapedObject).Add(
                    propertyInfo.Name, propertyInfo.GetValue(sourceObject));
            }

            expandoObjectList.Add(dataShapedObject);
        }

        return expandoObjectList;
    }
}