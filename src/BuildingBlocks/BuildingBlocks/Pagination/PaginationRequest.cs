namespace BuildingBlocks.Pagination
{
    public record class PaginationRequest(int PageIndex = 0, int PageSize = 10);

}
