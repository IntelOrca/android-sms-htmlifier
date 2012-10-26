/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;
using System.Linq;

namespace rd3korca.AndroidSmsHtmlifier
{
	class Program
	{
		static void Main(string[] args)
		{
			SmsCollection smses = SmsCollection.FromXmlFile(@"", "");
			SmsCollection outSmses = smses;

			outSmses.SortByDate();
			outSmses.OutputHtml(@"");
		}

		public static string HtmlSpecialChars(string text)
		{
			return text;
		}

		public static DateTime FromUnixTime(string unixTime)
		{
			return FromUnixTime(Int64.Parse(unixTime));
		}

		public static DateTime FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddMilliseconds(unixTime);
		}

	}
}
