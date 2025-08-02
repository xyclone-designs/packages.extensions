using Android.Runtime;

using System;

namespace Android.Media.Audiofx
{
	public class LoudnessEnhancerFX : LoudnessEnhancer
	{
		public LoudnessEnhancerFX(int audioSession) : base(audioSession) { }
		protected LoudnessEnhancerFX(IntPtr javaReference, JniHandleOwnership transfer) : base(javaReference, transfer) { }

		private Settings? _Properties;
		public Settings? Properties
		{
			get => _Properties;
			set
			{
				_Properties = value;

				SetTargetGain(_Properties?.TargetGain ?? 0);
			}
		}

		public class Settings : Java.Lang.Object
		{
			public int TargetGain { get; set; }
		}
	}
}