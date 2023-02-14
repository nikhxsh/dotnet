using System;

namespace DSACracker.Strings
{
	public class BackspaceString
	{
		public BackspaceString()
		{
			string s1 = "equ#ual";
			Console.WriteLine(RemoveBackspaces(s1)); // equal
		}

		string RemoveBackspaces(string str)
		{
			int backSpaceIndex = 0;

			var strArray = str.ToCharArray();

			for (int i = 0; i < strArray.Length; i++)
			{
				if(str[i] != '#')
				{
					strArray[backSpaceIndex] = strArray[i];
					backSpaceIndex++;
				}
				else if(str[i] == '#' && backSpaceIndex > 0)
				{
					backSpaceIndex--;
				}

				if(backSpaceIndex < 0)
					backSpaceIndex = 0;
			}

			var backspacedString = new string(strArray[0..backSpaceIndex]);
			return backspacedString;
		}
	}
}
