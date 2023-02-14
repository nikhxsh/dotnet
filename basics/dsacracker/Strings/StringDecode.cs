using System;

namespace DSACracker.Strings
{
	public class StringDecode
    {
        /// <summary>
        /// - If a = 1, b = 2, c = 3 ,..., z = 26. Given a string, find all possible codes that string can generate.
        /// - Example:
        ///     Input: "1123"
        ///     Output: aabc (a = 1, a = 1, b = 2, c = 3), 
        ///             kbc (k = 11, b = 2, c= 3), 
        ///             alc (a = 1, l = 12, c = 3), 
        ///             aaw (a= 1, a =1, w= 23), 
        ///             kw (k = 11, w = 23)
        /// </summary>
        public void StringDecodeRecursive(string input, string code)
        {
            if (string.IsNullOrEmpty(input))
            {
                if (!string.IsNullOrEmpty(code) && code.Length > 0)
                    Console.WriteLine(code);
                return;
            }
            StringDecodeRecursive(input.Substring(1), code + ('a' + (input[0] - '0') - 1));
            if (input.Length > 1)
            {
                var number = Convert.ToInt32(input.Substring(0, 2));
                if (number <= 26)
                    StringDecodeRecursive(input.Substring(2), code + ('a' + number - 1));
            }
        }
    }
}
