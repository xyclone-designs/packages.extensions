using System.IO.Compression;
using System.Linq;

namespace System.IO
{
	public static class FileInfoExtensions
	{
		public static string ZipFile(this FileInfo fileinfo)
		{
			return fileinfo.ZipFile(Path.Combine(fileinfo.FullName.Split('\\')[0..^1]), fileinfo.FullName);
		}
		public static string ZipFile(this FileInfo fileinfo, string? outputdirectory, string? outputfilename)
		{
			string name = fileinfo.FullName.Split("\\").Last();
			string zipfilepath = fileinfo.ZipFilePath(outputdirectory, outputfilename);

			using FileStream filestream = File.OpenRead(fileinfo.FullName);
			using FileStream filestreamzip = File.Create(zipfilepath);
			using ZipArchive ziparchive = new(filestreamzip, ZipArchiveMode.Create, true);
			using Stream stream = ziparchive
				.CreateEntry(name)
				.Open();

			filestream.CopyTo(stream);
			filestream.Close();

			return zipfilepath;
		}

        public static string ZipFilePath(this FileInfo fileinfo)
        {
            return fileinfo.ZipFilePath(Path.Combine(fileinfo.FullName.Split('\\')[0..^1]), fileinfo.FullName);
        }
        public static string ZipFilePath(this FileInfo fileinfo, string? outputdirectory, string? outputfilename)
        {
            outputdirectory ??= Path.Combine(fileinfo.FullName.Split('\\')[0..^1]);
            outputfilename ??= fileinfo.Name;

            string name = fileinfo.FullName.Split("\\").Last();
            
			return string.Format("{0}.zip", Path.Combine(outputdirectory, outputfilename));
        }
    }
}