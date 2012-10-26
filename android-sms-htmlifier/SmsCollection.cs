/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace rd3korca.AndroidSmsHtmlifier
{
	class SmsCollection : List<Sms>
	{
		public SmsCollection()
			: base()
		{
		}

		public SmsCollection(IEnumerable<Sms> smses)
			: base(smses)
		{
		}

		public void SortByDate()
		{
			this.Sort(new Comparison<Sms>((x, y) => x.Timestamp.CompareTo(y.Timestamp)));
		}

		public static SmsCollection FromXmlFile(string xmlPath, string ownerName)
		{
			SmsCollection smsCollection = new SmsCollection();

			// Load xml document
			XmlDocument doc = new XmlDocument();
			doc.Load(xmlPath);

			// Read each sms element
			foreach (XmlNode smsNode in doc.SelectNodes("smses/sms")) {
				Sms sms = new Sms();
				sms.OwnerName = ownerName;
				sms.Sent = (smsNode.Attributes["type"].Value == "2");
				sms.Timestamp = Program.FromUnixTime(smsNode.Attributes["date"].Value);
				sms.Number = smsNode.Attributes["address"].Value;
				sms.Contact = smsNode.Attributes["contact_name"].Value;
				sms.Body = smsNode.Attributes["body"].Value;

				if (sms.Contact == "(Unknown)")
					sms.Contact = sms.Number;

				smsCollection.Add(sms);
			}

			return smsCollection;
		}

		public void OutputTextLog(string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Sms sms in this) {
				sb.AppendFormat("[{0}] {1," + sms.Contact.Length + "}: {2}", sms.Timestamp, sms.Sender, sms.Body);
				sb.AppendLine();
			}
			File.WriteAllText(path, sb.ToString());
		}

		public void OutputHtml(string path)
		{
			// Get template from resource
			string html = File.ReadAllText("../../template.html");

			// Find message template within page template
			int start = html.IndexOf("{{BEGIN_MESSAGE}}");
			int end = html.IndexOf("{{END_MESSAGE}}");
			string msgHtml = html.Substring(start + 17, end - start - 17);
			html = html.Remove(start, end - start);

			// Change title
			html = html.Replace("{{TITLE}}", Program.HtmlSpecialChars("SMS of "));

			// Store participants and message counts
			Dictionary<String, int> people = new Dictionary<string, int>();

			// Messages
			StringBuilder sb = new StringBuilder();
			foreach (Sms sms in this) {
				string newMessage = msgHtml;
				string sender = Program.HtmlSpecialChars(sms.Sender);
				newMessage = newMessage.Replace("{{TIME}}", Program.HtmlSpecialChars(sms.Timestamp.ToString()));
				newMessage = newMessage.Replace("{{RELTIME}}", sms.Timestamp.ToFileTime().ToString());
				newMessage = newMessage.Replace("{{SENDER}}", sender);
				newMessage = newMessage.Replace("{{CONTENT}}", Program.HtmlSpecialChars(sms.Body));
				sb.Append(newMessage);
				if (people.ContainsKey(sender)) {
					people[sender]++;
				} else {
					people.Add(sender, 1);
				}
			}
			html = html.Replace("{{END_MESSAGE}}", sb.ToString());

			string peopleText = "";
			foreach (KeyValuePair<string, int> kvp in people) {
				peopleText += kvp.Value + " from " + kvp.Key + "<br/>";
			}

			// Add people list
			html = html.Replace("{{PEOPLE}}", peopleText);

			// Output to file
			File.WriteAllText(path, html);
		}
	}
}
