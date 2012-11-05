using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Represents a conversation, a list of messages between a group of participants.
	/// </summary>
	class Conversation : IEnumerable<IMessage>
	{
		private List<IMessage> mMessages;
		private ParticipantList mParticipants;

		public Conversation(ParticipantList participants)
		{
			mMessages = new List<IMessage>();
			mParticipants = participants;
		}

		public void AddMessage(IMessage message)
		{
			mMessages.Add(message);
		}

		public int GetParticipantMessageCount(string participant)
		{
			return mMessages.Where(x => x.FromContact == participant).Count();
		}

		public override string ToString()
		{
			return mParticipants.ToString();
		}

		public IEnumerable<IMessage> Messages
		{
			get
			{
				return mMessages;
			}
		}

		public ParticipantList Participants
		{
			get
			{
				return mParticipants;
			}
		}

		public int Count
		{
			get
			{
				return mMessages.Count;
			}
		}

		public IEnumerator<IMessage> GetEnumerator()
		{
			return mMessages.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mMessages.GetEnumerator();
		}
	}
}
