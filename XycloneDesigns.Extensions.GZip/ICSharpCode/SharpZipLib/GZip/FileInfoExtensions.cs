using System.IO;

namespace ICSharpCode.SharpZipLib.GZip
{
	public static class FileInfoExtensions
	{
		public static string GZipFile(this FileInfo fileinfo)
		{
			return fileinfo.GZipFile(Path.Combine(fileinfo.FullName.Split('\\')[0..^1]), fileinfo.FullName);
		}
		public static string GZipFile(this FileInfo fileinfo, string? outputdirectory, string? outputfilename)
		{
			outputdirectory ??= Path.Combine(fileinfo.FullName.Split('\\')[0..^1]);
			outputfilename ??= fileinfo.Name;

			string gzipfilepath = string.Format("{0}\\{1}.gz", outputdirectory, outputfilename);

			using FileStream filestream = File.OpenRead(fileinfo.FullName);
			using FileStream filestreamgzip = File.Create(gzipfilepath);

			GZip.Compress(filestream, filestreamgzip, true, 512, 6);

			return gzipfilepath;
		}
	}
}