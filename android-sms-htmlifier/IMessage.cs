using System;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Interface to represent a single message from someone to someone.
	/// </summary>
	interface IMessage
	{
		DateTime Timestamp
		{
			get;
		}

		string FromContact
		{
			get;
		}

		string[] ToContacts
		{
			get;
		}

		string Text
		{
			get;
		}
	}
}
