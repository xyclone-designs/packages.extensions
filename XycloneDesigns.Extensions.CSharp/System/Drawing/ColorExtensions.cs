#nullable enable

namespace System.Drawing
{
	public static class ColorExtensions
	{
		public static Color Randomise(this Color _)
		{
			Random random = new();

			return Color.FromArgb(random.Next(0, 255), random.Next(0, 255), random.Next(0, 255));
		}		
		public static string ToStringHex(this Color color, bool includealpha = false)
		{
			return includealpha is false
				? string.Format("#{0:X2}{1:X2}{2:X2}", color.R, color.G, color.B)
				: string.Format("#{0:X2}{1:X2}{2:X2}{3:X2}", color.A, color.R, color.G, color.B);
		}		
	}
}