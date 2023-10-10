using System;

namespace DSACracker.Basic
{
	public class Fibonacci
	{
		public Fibonacci()
		{
			Console.WriteLine(Sequential(6));
			Console.WriteLine(Recursive(6));
		}

		private int Sequential(int n)
		{
			var a = 0;
			var b = 1;
			var c = 0;

			if (n == 0)
				return a;

			for (var i = 2; i <= n; i++)
			{
				c = a + b;
				a = b;
				b = c;
			}

			return c;
		}

		private int Recursive(int n)
		{
			if (n <= 1)
				return n;
			return Recursive(n - 1) + Recursive(n - 2);
		}
	}
}
