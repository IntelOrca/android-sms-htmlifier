/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.Xml;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Represents a single SMS.
	/// </summary>
	struct Sms : IMessage
	{
		private string mOwnerName;

		private bool mSent;
		private DateTime mTimestamp;
		private string mNumber;
		private string mContact;
		private string mText;

		public ParticipantList Participants
		{
			get
			{
				return new ParticipantList(new string[] { mOwnerName, mContact });
			}
		}

		public override string ToString()
		{
			return String.Format("From = {0} To = {1} Content = {2}", FromContact, ToContacts[0], mContact.Substring(0, Math.Min(8, mContact.Length)));
		}

		/// <summary>
		/// Opens the specified path to an xml file of backed up SMSes and creates an SMS collection.
		/// </summary>
		/// <param name="xmlPath">Path to the xml file containing SMSes.</param>
		/// <param name="ownerName">Name of the owner of the phone containing the SMSes.</param>
		/// <returns>an SMS collection of all the SMSes in the xml file.</returns>
		public static Sms[] FromXmlFile(string xmlPath, string ownerName)
		{
			List<Sms> smsCollection = new List<Sms>();

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
				sms.Text = smsNode.Attributes["body"].Value;

				if (sms.Contact == "(Unknown)")
					sms.Contact = sms.Number;

				smsCollection.Add(sms);
			}

			return smsCollection.ToArray();
		}

		/// <summary>
		/// Gets the name of the person that sent the SMS.
		/// </summary>
		public string FromContact
		{
			get
			{
				return (mSent ? mOwnerName : mContact);
			}
		}

		/// <summary>
		/// Gets the name of the receiving person of the SMS.
		/// </summary>
		public string[] ToContacts
		{
			get
			{
				return new string[] { (mSent ? mContact : mOwnerName) };
			}
		}

		/// <summary>
		/// Gets or sets the name of the owner of the phone carrying the SMS.
		/// </summary>
		public string OwnerName
		{
			get
			{
				return mOwnerName;
			}
			set
			{
				mOwnerName = value;
			}
		}

		/// <summary>
		/// Gets or sets whether the SMS is a received or sent SMS.
		/// </summary>
		public bool Sent
		{
			get
			{
				return mSent;
			}
			set
			{
				mSent = value;
			}
		}

		/// <summary>
		/// Gets or sets the date and time of when the SMS was sent or received.
		/// </summary>
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

		/// <summary>
		/// Gets or sets the number of the contact.
		/// </summary>
		public string Number
		{
			get
			{
				return mNumber;
			}
			set
			{
				mNumber = value;
			}
		}

		/// <summary>
		/// Get or sets the contact name that the SMS is either to or from.
		/// </summary>
		public string Contact
		{
			get
			{
				return mContact;
			}
			set
			{
				mContact = value;
			}
		}

		/// <summary>
		/// Gets or sets the body / content of the SMS.
		/// </summary>
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
