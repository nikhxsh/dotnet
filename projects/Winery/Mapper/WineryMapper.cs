using WineryAPI.Models;
using WineryAPI.Storage.Models;

namespace WineryAPI.Mapper
{
	public static class WineryMapper
	{
		public static WineryDTO ToDTO(this Winery wine)
		{
			return new WineryDTO
			{
				Id = wine.Id,
				Name = wine.Name,
				Country = wine.Country,
				Region = wine.Region
			};
		}

		public static IEnumerable<Winery> ToModels(this IEnumerable<WineryDTO> wines)
		{
			return wines.Select(x => x.ToModel());
		}

		public static Winery ToModel(this WineryDTO wine)
		{
			return new Winery
			{
				Id = wine.Id,
				Name = wine.Name,
				Country = wine.Country,
				Region = wine.Region
			};
		}
	}
}
