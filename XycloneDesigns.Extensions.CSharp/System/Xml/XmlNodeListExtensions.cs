using System.Collections.Generic;

namespace System.Xml
{
	public static class XmlNodeListExtensions
	{
		public static IEnumerable<XmlNode> AsEnumerable(this XmlNodeList xmlnodelist)
		{
			for (int index = 0; index < xmlnodelist.Count; index++)
				if (xmlnodelist.Item(index) is XmlNode xmlnode)
					yield return xmlnode;
		}		
	}
}