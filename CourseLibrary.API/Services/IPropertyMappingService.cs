
namespace CourseLibrary.API.Services
{
    public interface IPropertyMappingService
    {
        Dictionary<string, PropertyMappingValue> GetPropertyMapping<TSource, TDestination>();
        bool ValidMappingExist<TSource, TDestination>(string fields);
    }
}