using WineryAPI.Models;
using WineryAPI.Storage.Models;

namespace WineryAPI.Mapper
{
	public static class WineMapper
	{
		public static WineDTO ToDTO(this Wine wine)
		{
			return new WineDTO
			{
				Id = wine.Id,
				Name = wine.Name,
				Color = (WineColorDTO)Enum.Parse(typeof(WineColorDTO), wine.Color.ToString()),
				Vintage = wine.Vintage,
				Price = wine.Price,
				IssueDate = wine.IssueDate,
				Note = wine.Note,
				WineryId = wine.WineryId
			};
		}

		public static IEnumerable<Wine> ToModels(this IEnumerable<WineDTO> wines)
		{
			return wines.Select(x => x.ToModel());
		}

		public static Wine ToModel(this WineDTO wine)
		{
			return new Wine
			{
				Id = wine.Id,
				Name = wine.Name,
				Color = (WineColor)Enum.Parse(typeof(WineColor), wine.Color.ToString()),
				Vintage = wine.Vintage,
				Price = wine.Price,
				IssueDate = wine.IssueDate,
				Note = wine.Note,
				WineryId = wine.WineryId
			};
		}
	}
}
