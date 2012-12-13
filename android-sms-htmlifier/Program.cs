/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;

namespace rd3korca.AndroidSmsHtmlifier
{
	static class Program
	{
		static void Main(string[] args)
		{
			if (args.Length < 2) {
				Console.WriteLine("Android SMS HTMLifer");
				Console.WriteLine("Usage: android-sms-htmlifier.exe name filename");
				Console.WriteLine();
				return;
			}

			string ownerName = args[0];
			string xmlPath = args[1];
			string htmlPath = Path.ChangeExtension(xmlPath, ".html");

			MessageCollection msgCollection = new MessageCollection();
			Array.ForEach(Sms.FromXmlFile(xmlPath, ownerName), x => msgCollection.Add(x));
			msgCollection.OutputHtml(htmlPath);
			
			


			/*
			string ownerName = "Ted John";
			string dir = @"C:\Users\Ted\Documents\Years\2010\My Recieved Files\ted2741495219\History";
			string[] files = Directory.GetFiles(dir, "*.xml");
			
			Console.WriteLine("Reading files...");
			foreach (string file in files)
				if (file.Contains("kmp"))
					Array.ForEach(GenericMessage.FromXmlFile(file), x => msgCollection.Add(x));

			Console.WriteLine("Outputing html...");
			msgCollection.OutputHtml("../../test/test.html", ownerName);
			*/
 
			/*
			string ownerName = "Ted John";
			Sms[] test = Sms.FromXmlFile("../../test/test.xml", ownerName);
			Array.ForEach(test, x => msgCollection.Add (x));
			msgCollection.OutputHtml("../../test/test.html", ownerName);
			*/
		}

		public static string HtmlSpecialChars(string text)
		{
			return text;
		}

		/// <summary>
		/// Converts a unix time to a DateTime object.
		/// </summary>
		/// <param name="unixTime">Unix time to convert.</param>
		/// <returns>a DateTime object.</returns>
		public static DateTime FromUnixTime(string unixTime)
		{
			return FromUnixTime(Int64.Parse(unixTime));
		}

		/// <summary>
		/// Converts a unix time to a DateTime object.
		/// </summary>
		/// <param name="unixTime">Unix time to convert.</param>
		/// <returns>a DateTime object.</returns>
		public static DateTime FromUnixTime(long unixTime)
		{
			var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
			return epoch.AddMilliseconds(unixTime);
		}

		/// <summary>
		/// Converts a DateTime object to a unix timestamp.
		/// </summary>
		/// <param name="dt">DateTime object to convert to a unix timestamp.</param>
		/// <returns>a long representing a unix timestamp.</returns>
		public static long ToUnixTime(this DateTime dt)
		{
			return (long)(dt - new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc).ToLocalTime()).TotalMilliseconds;
		}
	}
}
