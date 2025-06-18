using System.Collections.Generic;

namespace System.IO
{
	public class StreamWriters : Dictionary<string, StreamWriter>, IDisposable
	{
		Dictionary<string, FileStream> FileStreams { get; set; } = [];

		public string? PathBase { get; set; }

		public bool TryAdd(string key, string path, bool relative = false)
		{
			if (ContainsKey(key))
				return false;

			if (relative)
				path = Path.Combine(PathBase ?? throw new ArgumentException("If relative = true, 'StreamWriters.PathBase' must be set"), path);

			FileStreams.Add(key, File.Open(path, FileMode.OpenOrCreate, FileAccess.ReadWrite));
			
			Add(key, new StreamWriter(FileStreams[key]));

			return true;
		}

		public void PerformAction(Action<StreamWriter> act, params string[] keys)
		{
			foreach (string key in keys)
				if (TryGetValue(key, out StreamWriter? value) && value is not null)
					act.Invoke(value);
		}
		public void Add(string key, string path, bool relative = false)
		{
			if (relative)
				path = Path.Combine(PathBase ?? throw new ArgumentException("If relative = true, 'StreamWriters.PathBase' must be set"), path);

			if (TryAdd(key, path) is false)
				throw new ArgumentException(key);
		}
		public void Dispose(bool remove = true, params string[] keys)
		{
			foreach (string key in keys)
			{
				this[key].Close();
				FileStreams[key].Close();

				this[key].Dispose();
				FileStreams[key].Dispose();

				if (remove)
				{
					Remove(key);
					FileStreams.Remove(key);
				}
			}
		}
		public void Dispose()
		{
			foreach (string key in Keys)
			{
				this[key].Close();
				this[key].Dispose();
				this.Remove(key);
			}

			foreach (string key in FileStreams.Keys)
			{
				FileStreams[key].Close();
				FileStreams[key].Dispose();
				FileStreams.Remove(key);
			}
		}
	}
}
