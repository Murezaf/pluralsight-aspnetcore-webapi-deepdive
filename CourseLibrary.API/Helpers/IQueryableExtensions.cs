using CourseLibrary.API.Services;
using System.Linq.Dynamic.Core;

namespace CourseLibrary.API.Helpers;

public static class IQueryableExtensions
{
    public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy, Dictionary<string, PropertyMappingValue> mappingDictionary)
    {
        if(source == null) throw new ArgumentNullException(nameof(source));
        if(mappingDictionary == null) throw new ArgumentNullException(nameof(mappingDictionary));
        if (orderBy == null) return source;

        string orderByString = string.Empty;
        var orderByAfterSplit = orderBy.Split(',');

        foreach(string orderByClause in orderByAfterSplit)
        {
            string trimmedOrderByClause = orderByClause.Trim();

            bool isOrderDescending = trimmedOrderByClause.EndsWith("desc");

            int indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ");
            string propertyName = indexOfFirstSpace == -1 ? trimmedOrderByClause : trimmedOrderByClause.Remove(indexOfFirstSpace);

            if (!mappingDictionary.ContainsKey(propertyName))
                throw new ArgumentException($"Key mapping for {propertyName} is missing");

            PropertyMappingValue propertyMappingValue = mappingDictionary[propertyName];
            if (propertyMappingValue == null) 
                throw new ArgumentNullException(nameof(propertyMappingValue));

            if (propertyMappingValue.Revert == true)
                isOrderDescending = !(isOrderDescending);

            foreach(string destinationProperty in propertyMappingValue.DestinationProperties)
            {
                orderByString += string.IsNullOrWhiteSpace(orderByString) ? string.Empty : ", ";
                orderByString += destinationProperty;
                orderByString += " ";
                orderByString += (isOrderDescending) ? "descending" : "ascending";
            }
        }

        return source.OrderBy(orderByString);
    }
}