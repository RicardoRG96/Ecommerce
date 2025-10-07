namespace SharedKernel
{
    public class PaginatedList<T>
    {
        public List<T> Items { get; set; }
        public int PageNumber { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        private PaginatedList(List<T> items, int count, int pageNumber, int pageSize)
        {
            Items = items;
            PageNumber = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
        }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public static PaginatedList<T> Create(
            List<T> items,
            int count,
            int pageNumber, 
            int pageSize)
        {
            return new PaginatedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
