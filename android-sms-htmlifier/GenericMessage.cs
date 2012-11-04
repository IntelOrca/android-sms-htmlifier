using System;
using System.Collections.Generic;
using System.Xml;

namespace rd3korca.AndroidSmsHtmlifier
{
	struct GenericMessage : IMessage
	{
		private DateTime mTimestamp;
		private string mFromContact;
		private string[] mToContacts;
		private string mText;

		public override string ToString()
		{
			return String.Format("{0}: {1}", mFromContact, mText);
		}

		public static GenericMessage[] FromXmlFile(string path)
		{
			List<GenericMessage> genericMessages = new List<GenericMessage>();

			// Load xml document
			XmlDocument doc = new XmlDocument();
			doc.Load(path);

			// Read each message element
			foreach (XmlNode messageNode in doc.SelectNodes("Log/Message")) {
				GenericMessage message = new GenericMessage();

				message.Timestamp = DateTime.Parse(messageNode.Attributes["DateTime"].Value);
				message.FromContact = messageNode.SelectSingleNode("From/User").Attributes["FriendlyName"].Value;

				List<string> toContacts = new List<string>();
				foreach (XmlNode userNode in messageNode.SelectNodes("To/User")) {
					XmlAttribute attribute = userNode.Attributes["FriendlyName"];
					if (attribute == null)
						continue;
					toContacts.Add(attribute.Value);
				}
				message.ToContacts = toContacts.ToArray();

				message.Text = messageNode.SelectSingleNode("Text").InnerText;

				genericMessages.Add(message);
			}

			return genericMessages.ToArray();
		}

		public DateTime Timestamp
		{
			get
			{
				return mTimestamp;
			}
			set
			{
				mTimestamp = value;
			}
		}

		public string FromContact
		{
			get
			{
				return mFromContact;
			}
			set
			{
				mFromContact = value;
			}
		}

		public string[] ToContacts
		{
			get
			{
				return mToContacts;
			}
			set
			{
				mToContacts = value;
			}
		}

		public string Text
		{
			get
			{
				return mText;
			}
			set
			{
				mText = value;
			}
		}
	}
}
