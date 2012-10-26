/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;

namespace rd3korca.AndroidSmsHtmlifier
{
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

		public string Sender
		{
			get
			{
				return (mSent ? mOwnerName : mContact);
			}
		}

		public string Receiver
		{
			get
			{
				return (mSent ? mContact : mOwnerName);
			}
		}

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
