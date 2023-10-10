using System;

namespace DSACracker.Basic
{
	/// <summary>
	/// - A number greater than 1 with exactly two factors, i.e., 1 and the number itself is a prime number. 
	///   For example, 
	///    - 7 has only 2 factors, 1 and 7 itself. So, it is a prime number. 
	///    - However, 6 has four factors, 1, 2, 3 and 6. Therefore, it is not a prime number. It is a composite number.
	/// </summary>
	public class Prime
	{
		public Prime()
		{
			SimpleIsPrimeCheck(17); //prime
			SqrtIsPrimeCheck(546); //not prime
		}

		/// <summary>
		/// Time Complexity: O(n), where n is the given number.
		/// Space Complexity: O(1)
		/// 
		/// Note: We take (N/2) as the upper limit because there are no numbers between (N/2) and N that can divide N completely.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public bool SimpleIsPrimeCheck(int n)
		{
			var flag = false;

			for (int i = 2; i <= n/2; i++)
			{
				// if N is perfectly divisible by i
				if (n % i == 0)
				{
					flag = true;
					break;
				}
			}

			return flag;
		}


		/// <summary>
		/// Time Complexity: O(n1/2), where n is the given number.
		/// Auxiliary Space: O(1)
		/// 
		/// Note: The smallest and greater than one factor of a number cannot be more than the square root of that number.
		/// </summary>
		/// <param name="n"></param>
		/// <returns></returns>
		public bool SqrtIsPrimeCheck(int n)
		{
			var flag = false;

			var sqrt = Math.Sqrt(n);

			for (int i = 2; i <= sqrt; i++)
			{
				// if N is perfectly divisible by i
				if (n % i == 0)
				{
					flag = true;
					break;
				}
			}

			return flag;
		}
	}
}
