using System;

namespace Tools
{
	public static class ConsoleExtensions
	{
		public static void Print<T>(this T[] input, string lable)
		{
			var output = string.Join(",", input);
			Console.WriteLine($" -- {lable} -- ");
			Console.WriteLine(output);
		}
	}
}
