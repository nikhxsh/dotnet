namespace WineryAPI.Models
{
	public class FetchRequest
	{
		public Filter[] Filters { get; set; }
		public Sort Sort { get; set; }
		public int Skip { get; set; }
		public int Take { get; set; } = 10;
	}

	public class Filter
	{
		public string Column { get; set; }
		public string Token { get; set; }
	}

	public class Sort
	{
		public string Column { get; set; }
		public SortOrder Order { get; set; }
	}

	public enum SortOrder { None, Asc, Desc }

	public class PagedResponse<T>
	{
		public int Total { get; set; }
		public int FilteredTotal { get; set; }
		public T Result { get; set; }
	}
}
