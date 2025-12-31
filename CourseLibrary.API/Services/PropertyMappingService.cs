using CourseLibrary.API.Entities;
using CourseLibrary.API.Models;
using System.Security.Principal;

namespace CourseLibrary.API.Services;

public class PropertyMappingService : IPropertyMappingService
{
    private readonly Dictionary<string, PropertyMappingValue> _authorPropertyMapping =
        new(StringComparer.OrdinalIgnoreCase)
        {
            { "Id", new PropertyMappingValue(new string[] {"Id"}) },
            { "MainCategory", new PropertyMappingValue(new string[] {"MainCategory"}) },
            { "Name", new PropertyMappingValue(new string[] {"FirstName", "LastName"}) },
            { "Age", new PropertyMappingValue(new string[] {"DateOfBirth"}, true) }
        };

    private readonly IList<IPropertyMapping> _propertyMappings = new List<IPropertyMapping>();
    public PropertyMappingService()
    {
        _propertyMappings.Add(new PropertyMapping<AuthorDto, Author>(_authorPropertyMapping));
    }
    public Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>()
    {
        IEnumerable<PropertyMapping<TSource, TDestination>> matchingMapping = _propertyMappings.OfType<PropertyMapping<TSource, TDestination>>();

        if (matchingMapping.Count() == 1)
            return matchingMapping.First().MappingDictionary;

        throw new Exception($"Cannot find exact property mapping instance for <{typeof(TSource)}, {typeof(TDestination)}>.");
    }

    public bool ValidMappingExist<TSource, TDestination>(string fields)
    {
        Dictionary<string, PropertyMappingValue> propertyMapping = GetPropertyMapping<AuthorDto, Author>();

        if(string.IsNullOrWhiteSpace(fields)) return true;

        string[] fieldsAfterSplit = fields.Split(',');
        foreach (string field in fieldsAfterSplit)
        {
            string trimmedField = field.Trim();
            
            int indexOfFirstSpace = trimmedField.IndexOf(' ');
            string propertyName = (indexOfFirstSpace == -1) ? trimmedField : trimmedField.Remove(indexOfFirstSpace);

            if(!propertyMapping.ContainsKey(propertyName))
                return false;
        }
    
        return true;
    }
}