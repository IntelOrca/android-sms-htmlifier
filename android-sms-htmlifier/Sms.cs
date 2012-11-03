/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Represents a single SMS.
	/// </summary>
	struct Sms
	{
		private string mOwnerName;

		private bool mSent;
		private DateTime mTimestamp;
		private string mNumber;
		private string mContact;
		private string mBody;

		public override string ToString()
		{
			return String.Format("From = {0} To = {1} Content = {2}", Sender, Receiver, mContact.Substring(0, Math.Min(8, mContact.Length)));
		}

		/// <summary>
		/// Gets the name of the person that sent the SMS.
		/// </summary>
		public string Sender
		{
			get
			{
				return (mSent ? mOwnerName : mContact);
			}
		}

		/// <summary>
		/// Gets the name of the receiving person of the SMS.
		/// </summary>
		public string Receiver
		{
			get
			{
				return (mSent ? mContact : mOwnerName);
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
		public string Body
		{
			get
			{
				return mBody;
			}
			set
			{
				mBody = value;
			}
		}
	}
}
