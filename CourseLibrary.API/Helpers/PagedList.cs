using Microsoft.EntityFrameworkCore;

namespace CourseLibrary.API.Helpers;

public class PagedList<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int TotalCount { get; private set; }
    public bool HasNext
    {
        get
        {
            return CurrentPage < TotalPages;
        }
    }
    public bool HasPrevious
    {
        get
        {
            return CurrentPage > 1;
        }
    }

    public PagedList(List<T> items, int count, int pageNumber, int pageSize)
    {
        CurrentPage = pageNumber;
        TotalCount = count;
        PageSize = pageSize;
        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
        this.AddRange(items);
    }

    public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int pageNumber, int pageSize)
    {
        int count = source.Count();

        source = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        List<T> items = await source.ToListAsync();

        return new PagedList<T>(items, count, pageNumber, pageSize);
    }
}