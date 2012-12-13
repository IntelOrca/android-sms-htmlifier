using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace rd3korca.AndroidSmsHtmlifier
{
	class ParticipantList : IEnumerable
	{
		private string[] mParticipants;

		public ParticipantList(IEnumerable<string> participants)
		{
			mParticipants = participants.ToArray();
		}

		public override bool Equals(object obj)
		{
			ParticipantList other = obj as ParticipantList;
			if (other == null)
				return false;

			foreach (string participant in mParticipants)
				if (!other.mParticipants.Contains(participant))
					return false;

			return true;
		}

		public override int GetHashCode()
		{
			return mParticipants.GetHashCode();
		}

		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			foreach (string participant in mParticipants)
				sb.AppendFormat("{0}, ", participant);
			sb.Remove(sb.Length - 2, 2);
			return sb.ToString();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return mParticipants.GetEnumerator();
		}
	}
}
