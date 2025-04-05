
namespace Android.Content
{
	public enum IntentTypes
	{
		All,
		Application,
		Application_PDF,
		Audio,
		Audio_FLAC,
		Audio_M4A,
		Audio_MP3,
		Audio_WAV,
		Image,
		Image_GIF,
		Image_JPG,
		Image_JPEG,
		Image_PNG,
		Text,
		Text_HTML,
		Text_JSON,
		Text_Plain,
		Text_RTF,
		Video,
		Video_MP4,
		Video_3GP,
	}

	public static class IntentTypesExtensions
	{
		public static string? AsIntentType(this IntentTypes intenttype)
		{
			return intenttype switch
			{
				IntentTypes.All => "*/*",
				IntentTypes.Application => "application/*",
				IntentTypes.Application_PDF => "application/pdf",
				IntentTypes.Audio => "audio/*",
				IntentTypes.Audio_FLAC => "audio/flac",
				IntentTypes.Audio_M4A => "audio/m4a",
				IntentTypes.Audio_MP3 => "audio/mp3",
				IntentTypes.Audio_WAV => "audio/wav",
				IntentTypes.Image => "image/*",
				IntentTypes.Image_GIF => "image/gif",
				IntentTypes.Image_JPG => "image/jpg",
				IntentTypes.Image_JPEG => "image/jpeg",
				IntentTypes.Image_PNG => "image/png",
				IntentTypes.Text => "text/*",
				IntentTypes.Text_HTML => "text/html",
				IntentTypes.Text_JSON => "text/json",
				IntentTypes.Text_Plain => "text/plain",
				IntentTypes.Text_RTF => "text_rtf/text_rtf",
				IntentTypes.Video => "video/*",
				IntentTypes.Video_MP4 => "video/mp4",
				IntentTypes.Video_3GP => "video/3gp",

				_ => null
			};
		}
	}
}