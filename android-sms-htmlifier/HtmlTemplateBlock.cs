using System;
using System.Collections.Generic;
using System.Text;

namespace rd3korca.AndroidSmsHtmlifier
{
	class HtmlTemplateBlock : ICloneable
	{
		private string mHtml;

		public HtmlTemplateBlock(string html)
		{
			mHtml = html;
		}

		public HtmlTemplateBlock(IEnumerable<HtmlTemplateBlock> blocks)
		{
			StringBuilder sb = new StringBuilder();
			foreach (HtmlTemplateBlock block in blocks)
				sb.Append(block.Html);
			mHtml = sb.ToString();
		}

		public string HtmlSpecialChars(string s)
		{
			s = s.Replace("&", "&amp;");
			s = s.Replace("<", "&lt;");
			return s;
		}

		public void SetPlaceholder(string name, string value)
		{
			mHtml = mHtml.Replace(String.Format("{{{{{0}}}}}", name), HtmlSpecialChars(value));
		}

		public HtmlTemplateBlock GetBlock(string name)
		{
			string begin_identifier = String.Format("{{{{BEGIN_{0}}}}}", name);
			string end_identifier = String.Format("{{{{END_{0}}}}}", name);

			int start = mHtml.IndexOf(begin_identifier);
			int end = mHtml.IndexOf(end_identifier);
			string blockHtml = mHtml.Substring(start + begin_identifier.Length, end - start - begin_identifier.Length);
			return new HtmlTemplateBlock(blockHtml);
		}

		public void ReplaceBlock(string name, HtmlTemplateBlock block)
		{
			string begin_identifier = String.Format("{{{{BEGIN_{0}}}}}", name);
			string end_identifier = String.Format("{{{{END_{0}}}}}", name);

			int start = mHtml.IndexOf(begin_identifier);
			int end = mHtml.IndexOf(end_identifier);
			mHtml = mHtml.Remove(start, end - start);
			mHtml = mHtml.Replace(end_identifier, block.Html);
		}

		public object Clone()
		{
			return new HtmlTemplateBlock(mHtml);
		}

		public string Html
		{
			get
			{
				return mHtml;
			}
		}
	}
}
