using System;
using System.Collections.Generic;
using System.Text;

namespace Vekalat.Application.Common
{
    public class TextLength
    {
		public static string DisplayTextLength(string inputString, int maxLength)
		{
			// If the string isn't null or empty
			if (!String.IsNullOrEmpty(inputString))
			{
				// Return the appropriate string size
				return (inputString.Length <= maxLength) ? inputString : inputString.Substring(0, maxLength) + "...";
			}
			else
			{
				// Otherwise return the empty string
				return "";
			}
		}
	}
}
