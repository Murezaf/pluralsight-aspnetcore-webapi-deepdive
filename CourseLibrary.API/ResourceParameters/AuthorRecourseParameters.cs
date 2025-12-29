namespace CourseLibrary.API.ResourceParameters
{
    public class AuthorRecourseParameters
    {
        const int maxPagesSize = 20;
        public string? MainCategory { get; set; }
        public string? SearchQuery { get; set; }
        public int PageNumber { get; set; } = 1;
        private int pageSize = 10;
        public int PageSize
        {
            get
            {
                return pageSize;
            }
            set
            {
                pageSize = value > maxPagesSize ? maxPagesSize : value;
            }
        }
    }
}
