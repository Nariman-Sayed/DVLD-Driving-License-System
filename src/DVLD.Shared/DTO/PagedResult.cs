namespace DVLD.Shared
{
    /// <summary>
    /// A single page of results together with the paging metadata.
    /// </summary>
    /// <typeparam name="T">The type of the items in the page.</typeparam>
    public sealed class PagedResult<T>
    {
        /// <summary>
        /// Gets or sets the items in the current page.
        /// </summary>
        public List<T> Items { get; set; } = new();

        /// <summary>
        /// Gets or sets the one-based page number.
        /// </summary>
        public int PageNumber { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum number of items in a page.
        /// </summary>
        public int PageSize { get; set; } = 20;

        /// <summary>
        /// Gets or sets the total number of items across all pages.
        /// </summary>
        public int TotalCount { get; set; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int TotalPages => PageSize > 0
            ? (int)Math.Ceiling(TotalCount / (double)PageSize)
            : 0;
    }
}
