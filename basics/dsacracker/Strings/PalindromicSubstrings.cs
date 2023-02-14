using System;
using System.Linq;

namespace DSACracker.Strings
{
	public class PalindromicSubstrings
	{
		public PalindromicSubstrings()
		{
			string s1 = "abc";
			Console.WriteLine(GetMaxSubstrings(s1, 0));

			string s2 = "aaa";
			Console.WriteLine(GetMaxSubstrings(s2, 0));

			string s3 = "aababaabce";
			Console.WriteLine(GetMaxSubstrings(s3, 0));

			string s4 = "ababaeocco";
			Console.WriteLine(GetLongestPalindrome(s4, 4));
		}

		public int GetMaxSubstrings(string s, int k)
		{
			int count = 0;
			for (int i = 0; i < s.Length; i++)
			{
				int left = i, right = i;
				count += ExpandFromCenter(s, left, right, k);
				if (i < s.Length - 1)
					count += ExpandFromCenter(s, i, i + 1, k);
			}
			return count;
		}

		public string GetLongestPalindrome(string s, int k)
		{
			int start = 0, end = 0, length1 = 0, length2 = 0;
			for (int i = 0; i < s.Length; i++)
			{
				length1 = ExpandFromCenter(s, i, i, k);
				length2 = ExpandFromCenter(s, i, i + 1, k);
				var length = Math.Max(length1, length2);
				if (length > end - start)
				{
					start = i - (length - 1) / 2;
					end = i + length / 2;
				}
			}
			return s.Substring(start, end + 1);
		}

		public int ExpandFromCenter(string s, int left, int right, int length)
		{
			int count = 0;
			while (left >= 0 && right < s.Length && s[left] == s[right])
			{
				count++;
				left--;
				right++;
			}
			return right - left - 1;
		}
	}
}
