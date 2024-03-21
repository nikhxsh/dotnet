using System.Collections.Generic;

namespace DSACracker.Basic
{
	/// <summary>
	/// https://www.mathsisfun.com/metric-numbers.html
	/// </summary>
	public class NumberToText
	{
		private readonly Dictionary<long, string> digitsWords = new()
		{
			{ 1, "One" },
			{ 2, "Two"},
			{ 3, "Three"},
			{ 4, "Four"},
			{ 5, "Five"},
			{ 6, "Six" },
			{ 7, "Seven" },
			{ 8, "Eight" },
			{ 9, "Nine" },
			{ 10, "Ten" },
			{ 11, "Eleven"},
			{ 12, "Twelve" },
			{ 13, "Thirteen"},
			{ 14, "Fourteen" },
			{ 15, "Fifteen" },
			{ 16, "Sixteen" },
			{ 17, "Seventeen"},
			{ 18, "Eighteen" },
			{ 19, "Nineteen" },
			{ 20, "Twenty" },
			{ 30, "Thirty" },
			{ 40, "Fourty" },
			{ 50, "Fifty"},
			{ 60, "Sixty" },
			{ 70, "Seventy" },
			{ 80, "Eighty" },
			{ 90, "Ninety" },
			{ 100, "Hundred"},
			{ 1000, "Thousands" },
			{ 1000000, "Million" },
			{ 1000000000, "Billion" },
			{ 1000000000000, "Trillion" }
		};

		public NumberToText()
		{
			var text = CovertToText(1000000000000);
		}

		public string CovertToText(long number)
		{
			var text = string.Empty;

			long remainder = 0;
			long division = 0;

			remainder = number % 1000000000000;
			division = number / 1000000000000;

			if (division != 0)
			{
				text += $"{digitsWords[division]} {digitsWords[1000000000000]}";
			}

			return text;
		}
	}
}
