namespace RestaurantAPI.Core.DTO;
public class RestaurantGetAllQuery
{
	public string? SearchPhrase { get; init; }
	public int PageNumber { get; init; }
	public int PageSize { get; init; }
	public SortBy? SortBy { get; init; }
	public SortDirection? SortDirection { get; init; }
}

public enum SortDirection
{
	Asc,
	Desc
}

public enum SortBy
{
	Default = 0,
	Name,
	CreationData,
	LastUpdateDate,
	Rate
}
