using Tools;

namespace DSACracker.Array
{
	internal class ReverseAnArray
	{
		public ReverseAnArray()
		{
			var input = new int[] { 1, 2, 3, 4, 5, 6 };
			int start = 0, end = input.Length - 1;
			input.Print("Input");
			var output = IterativeWay(input, start, end);
			output.Print("Reverse using Iterative Way");

		}

		/// <summary>
		/// Time Complexity : O(n)
		/// Space Complexity: O(1)
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		private int[] IterativeWay(int[] input, int start, int end)
		{
			while(start < end)
			{
				var temp = input[start];
				input[start] = input[end];
				input[end] = temp;
				start++;
				end--;
			}

			return input;
		}

		/// <summary>
		/// Time Complexity : O(n)
		/// </summary>
		/// <param name="input"></param>
		/// <param name="start"></param>
		/// <param name="end"></param>
		private void RecursiveWay(int[] input, int start, int end)
		{
			if (start >= end)
				return;

			var temp = input[start];
			input[start] = input[end];
			input[end] = temp;

			RecursiveWay(input, start + 1, end - 1);
		}
	}
}
