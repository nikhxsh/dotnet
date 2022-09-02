using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tools;

namespace DSACracker.Array
{
	internal class MinMaxElement
	{
		public MinMaxElement()
		{
			var input = new int[] { 1000, 11, 445, 1, 330, 3000 };
			input.Print("Input");
			var result1 = LinearSearch(input, input.Length);
			Console.WriteLine($"LinearSearch > Min: {result1.MinValue}, Max: {result1.MaxValue}");
			var result2 = TournamentSearch(input, 0, input.Length - 1);
			Console.WriteLine($"TournamentSearch > Min: {result2.MinValue}, Max: {result2.MaxValue}");
		}

		private class MinMaxPair
		{
			public int MinValue { get; set; }
			public int MaxValue { get; set; }
		}

		/// <summary>
		/// Time Complexity: O(n)
		/// Auxiliary Space: O(1) as no extra space was needed
		/// In this method, the total number of comparisons is 1 + 2(n-2) in the worst case and 1 + n – 2 in the best case
		/// </summary>
		/// <param name="input"></param>
		/// <param name="len"></param>
		/// <returns></returns>
		private MinMaxPair LinearSearch(int[] input, int len)
		{
			var result = new MinMaxPair();

			if (len == 1)
			{
				result.MinValue = input[0];
				result.MaxValue = input[0];
				return result;
			}

			if (input[0] > input[1])
			{
				result.MinValue = input[1];
				result.MaxValue = input[0];
			}

			if (input[1] > input[0])
			{
				result.MinValue = input[0];
				result.MaxValue = input[1];
			}

			for (int i = 2; i < len; i++)
			{
				if (input[i] > result.MaxValue)
				{
					result.MaxValue = input[i];
				}
				else if (input[i] < result.MinValue)
				{
					result.MinValue = input[i];
				}
			}

			return result;
		}

		/// <summary>
		/// Time Complexity: O(n)
		/// Auxiliary Space: O(log n) as the stack space will be filled for the maximum height of the tree formed during
		/// recursive calls same as a binary tree
		/// Algorithmic Paradigm: Divide and Conquer
		/// </summary>
		/// <param name="input"></param>
		/// <param name="low"></param>
		/// <param name="high"></param>
		/// <returns></returns>
		private MinMaxPair TournamentSearch(int[] input, int low, int high)
		{
			var result = new MinMaxPair();
			var left = new MinMaxPair();
			var right = new MinMaxPair();
			int mid;

			if (low == high)
			{
				result.MinValue = input[low];
				result.MaxValue = input[low];
				return result;
			}
			else if (high == low + 1)
			{
				if (input[low] > input[high])
				{
					result.MaxValue = input[low];
					result.MinValue = input[high];
				}
				else
				{
					result.MinValue = input[low];
					result.MaxValue = input[high];
				}
				return result;
			}
			mid = (low + high) / 2;
			left = TournamentSearch(input, low, mid);
			right = TournamentSearch(input, mid + 1, high);

			if (left.MinValue < right.MinValue)
			{
				result.MinValue = left.MinValue;
			}
			else
			{
				result.MinValue = right.MinValue;
			}

			if (left.MaxValue > right.MaxValue)
			{
				result.MaxValue = left.MaxValue;
			}
			else
			{
				result.MaxValue = right.MaxValue;
			}

			return result;
		}
	}
}
