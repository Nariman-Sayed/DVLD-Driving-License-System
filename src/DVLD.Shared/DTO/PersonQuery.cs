namespace DVLD.Shared
{
    /// <summary>
    /// Paging and optional filter values for the person list.
    /// </summary>
    /// <param name="PageNumber">The one-based page number.</param>
    /// <param name="PageSize">The requested page size.</param>
    /// <param name="FilterOn">The allow-listed field to filter on.</param>
    /// <param name="FilterQuery">The value to filter by.</param>
    public sealed record PersonQuery(
        int PageNumber = 1,
        int PageSize = 20,
        string? FilterOn = null,
        string? FilterQuery = null);
}
