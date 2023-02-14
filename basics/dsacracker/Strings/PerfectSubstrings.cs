using System;
using System.Collections.Generic;

namespace DSACracker.Strings
{
	public class PerfectSubstrings
	{
		public PerfectSubstrings()
		{
			var str1 = "1102021222";
			var k1 = 2;
			Console.WriteLine(NumberOfSubStrings(str1, k1)); //2
		}

		private int NumberOfSubStrings(string str, int k)
		{
			var chars = str.ToCharArray();
			var count = 0;

			for (var i = 0; i < chars.Length - 1; i++)
			{
				var dict = new Dictionary<char, int>
				{
					{ chars[i], 1 }
				};

				for (int j = i + 1; j < chars.Length; j++)
				{
					if (dict.ContainsKey(chars[j]))
						dict[chars[j]]++;
					else
						dict[chars[j]] = 1;

					if(Check(dict, k))
						count++;
				}
			}
			return count;
		}

		private bool Check(Dictionary<char, int> dict, int k)
		{
			foreach (var item in dict)
			{
				if (item.Value != k)
					return false;
			}
			return true;
		}
	}
}
