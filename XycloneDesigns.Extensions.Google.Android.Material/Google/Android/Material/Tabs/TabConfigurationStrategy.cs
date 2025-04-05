#nullable enable

using System;

namespace Google.Android.Material.Tabs
{
	public class TabConfigurationStrategy : Java.Lang.Object, TabLayoutMediator.ITabConfigurationStrategy
	{
		public Action<TabLayout.Tab, int>? ActionOnConfigureTab { get; set; }

		public void OnConfigureTab(TabLayout.Tab tab, int position) => ActionOnConfigureTab?.Invoke(tab, position);
	}
}