using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Represents a conversation, a list of messages between a group of participants.
	/// </summary>
	public class Conversation : IEnumerable<IMessage>
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

		public IEnumerable<KeyValuePair<DateTime, IMessage[]>> GetMessagesGroupedByDay()
		{
			Dictionary<DateTime, IMessage[]> dic = new Dictionary<DateTime, IMessage[]>();
			List<IMessage> messages = new List<IMessage>();
			DateTime currentDay = DateTime.MinValue;

			foreach (IMessage msg in mMessages.OrderBy(x => x.Timestamp)) {
				if (currentDay.DayOfYear == msg.Timestamp.DayOfYear && currentDay.Year == msg.Timestamp.Year) {
					messages.Add(msg);
					continue;
				}

				if (messages.Count > 0)
					dic.Add(currentDay, messages.ToArray());

				messages.Clear();
				currentDay = msg.Timestamp;
				messages.Add(msg);
			}

			if (messages.Count > 0)
				dic.Add(currentDay, messages.ToArray());

			return dic;
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
