using System.Collections.Generic;

namespace System.Linq
{
	public static class ICollectionExtensions
	{
		public static void AddFormat(this ICollection<string> collection, string format, params object?[] args)
		{
			collection.Add(string.Format(format, args));
		}
		public static void RemoveFormat(this ICollection<string> collection, string format, params object?[] args)
		{
			collection.Remove(string.Format(format, args));
		}
	}
}
