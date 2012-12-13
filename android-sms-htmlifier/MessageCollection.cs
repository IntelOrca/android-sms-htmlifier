/*****************************************************************************
 * android-sms-htmlifier
 * Converts android sms xml backup file to useful html format.
 * Copyright James Robertson, Ted John 2012
 * https://github.com/IntelOrca/android-sms-htmlifier
 *****************************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;

namespace rd3korca.AndroidSmsHtmlifier
{
	/// <summary>
	/// Represents a collection of Messages.
	/// </summary>
	class MessageCollection : List<IMessage>
	{
		public MessageCollection()
			: base()
		{
		}

		public MessageCollection(IEnumerable<IMessage> messages)
			: base(messages)
		{
		}

		/// <summary>
		/// Sorts the SMS collection by date from oldest to newest.
		/// </summary>
		public void SortByDate()
		{
			this.Sort(new Comparison<IMessage>((x, y) => x.Timestamp.CompareTo(y.Timestamp)));
		}

		public Conversation[] GetConversations()
		{
			List<Conversation> conversations = new List<Conversation>();
			foreach (IMessage msg in this) {
				bool foundConvo = false;
				foreach (Conversation conversation in conversations) {
					if (conversation.Participants.Equals(msg.Participants)) {
						conversation.AddMessage(msg);
						foundConvo = true;
						break;
					}
				}

				if (!foundConvo) {
					Conversation conversation = new Conversation(msg.Participants);
					conversation.AddMessage(msg);
					conversations.Add(conversation);
				}
			}
			conversations.Sort(new Comparison<Conversation>((x, y) => y.Count.CompareTo(x.Count)));

			return conversations.ToArray();
		}

		/// <summary>
		/// Outputs a simple text file log of all the SMSes.
		/// </summary>
		/// <param name="path">Path of the file to save the log to.</param>
		public void OutputTextLog(string path)
		{
			StringBuilder sb = new StringBuilder();
			foreach (Sms sms in this) {
				sb.AppendFormat("[{0}] {1," + sms.Contact.Length + "}: {2}", sms.Timestamp, sms.FromContact, sms.Text);
				sb.AppendLine();
			}
			File.WriteAllText(path, sb.ToString());
		}

		/// <summary>
		/// Outputs a complex HTML file containing all the SMSes.
		/// </summary>
		/// <param name="path">Path to where to save the HTML file.</param>
		public void OutputHtml(string path)
		{
			HtmlTemplateBlock block = new HtmlTemplateBlock(File.ReadAllText("template.html"));
			block.SetPlaceholder("TITLE", "SMSes");

			// Get converstations
			IEnumerable<Conversation> conversations = GetConversations();

			// Replace convo tempalte bit
			HtmlTemplateBlock listItemTemplate = block.GetBlock("LIST");
			List<HtmlTemplateBlock> listItems = new List<HtmlTemplateBlock>();
			foreach (Conversation conversation in conversations) {
				HtmlTemplateBlock listItem = (HtmlTemplateBlock)listItemTemplate.Clone();
				listItem.SetPlaceholder("CONVO_ID", conversation.Participants.ToString());
				listItem.SetPlaceholder("CONVO_ITEM", String.Format("{0} ({1})", conversation.Participants.ToString(), conversation.Count));
				listItems.Add(listItem);
			}

			block.ReplaceBlock("LIST", new HtmlTemplateBlock(listItems));

			// Now for each convo
			HtmlTemplateBlock convoTemplate = block.GetBlock("CONVO");
			List<HtmlTemplateBlock> convoHtmls = new List<HtmlTemplateBlock>();
			foreach (Conversation conversation in conversations) {
				HtmlTemplateBlock convoHtml = (HtmlTemplateBlock)convoTemplate.Clone();
				convoHtml.SetPlaceholder("CONVO_ID", conversation.Participants.ToString());
				convoHtml.SetPlaceholder("PEOPLE_LIST", (new Func<string>( () => {
					StringBuilder sb = new StringBuilder();
					foreach (string participant in conversation.Participants) {
						int msgCount = conversation.GetParticipantMessageCount(participant);
						sb.AppendFormat("{0} ({1}) ", participant, msgCount);
					}
					sb.Length--;
					return sb.ToString();
				}))());
				HtmlTemplateBlock messageTemplate = convoHtml.GetBlock("MESSAGE");
				List<HtmlTemplateBlock> messages = new List<HtmlTemplateBlock>();

				foreach (KeyValuePair<DateTime, IMessage[]> kvp in conversation.GetMessagesGroupedByDay()) {
					messages.Add(new HtmlTemplateBlock("<span class=\"daysep\">" + kvp.Key.ToString("d MMMM yyyy") + "</span>"));

					foreach (IMessage msg in kvp.Value) {
						HtmlTemplateBlock message = (HtmlTemplateBlock)messageTemplate.Clone();
						message.SetPlaceholder("TIME", msg.Timestamp.ToUnixTime().ToString());
						message.SetPlaceholder("SENDER", msg.FromContact);
						message.SetPlaceholder("CONTENT", msg.Text);
						messages.Add(message);
					}
				}

				convoHtml.ReplaceBlock("MESSAGE", new HtmlTemplateBlock(messages));
				convoHtmls.Add(convoHtml);
			}

			block.ReplaceBlock("CONVO", new HtmlTemplateBlock(convoHtmls));

			// Output to file
			File.WriteAllText(path, block.Html);

			string styleDestPath = Path.Combine(Path.GetDirectoryName(path), "style.css");
			string jsDestPath = Path.Combine(Path.GetDirectoryName(path), "script.js");
			File.Copy("style.css", styleDestPath, true);
			File.Copy("script.js", jsDestPath, true);
		}
	}
}
