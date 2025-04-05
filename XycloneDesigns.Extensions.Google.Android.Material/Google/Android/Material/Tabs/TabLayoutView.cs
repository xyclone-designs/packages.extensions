using Android.Content;
using Android.Content.Res;
using Android.Util;
using Android.Views;
using AndroidX.AppCompat.Widget;

using System;

using XycloneDesigns.Extensions.Google.Android.Material;

namespace Google.Android.Material.Tabs
{
	public class TabLayoutView : TabLayout
	{
		public TabLayoutView(Context context) : this(context, null) { }
		public TabLayoutView(Context context, IAttributeSet? attrs) : this(context, attrs, default) { }
		public TabLayoutView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) 
		{
			Context = context;

			TypedArray styledattributes = Context.ObtainStyledAttributes(attrs, Resource.Styleable.TabLayoutView);

			TabViewSpacing = styledattributes.GetDimensionPixelSize(Resource.Styleable.TabLayoutView_tabViewSpacing, TabViewSpacing);
			TabViewStyle = styledattributes.GetResourceId(Resource.Styleable.TabLayoutView_tabViewStyle, TabViewStyle);

			styledattributes?.Recycle();
		}

		public new Context Context { get; }
		public int TabViewSpacing { get; set; }
		public int TabViewStyle { get; set; }

		public override Tab NewTab()
		{
			Tab tab = base.NewTab();
			tab.SetCustomView(new TabView(Context, null, TabViewStyle));
			return tab;
		}
		public Tab NewTab(Action<TabView> ontabview)
		{
			Tab tab = base.NewTab();
			TabView tabview = new (Context);

			ontabview.Invoke(tabview);
			tab.SetCustomView(tabview);

			return tab;
		}

		public void ConfigureTab(Tab tab)
		{
			if (tab.CustomView is not null)
			{
				tab.CustomView.Selected = tab.IsSelected;

				if (tab.Position == 0) tab.CustomView.SetMarginStart(TabViewSpacing); tab.CustomView.SetMarginEnd(TabViewSpacing);
			}
		}
		public void ConfigureTab(Tab tab, Action<TabView> viewaction)
		{
			ConfigureTab(tab);

			if (tab.CustomView is TabView tabview) viewaction.Invoke(tabview);
		}
		public void ConfigureTab<TView>(Tab tab, Action<TView> viewaction) where TView : TabView
		{
			ConfigureTab(tab);

			if (tab.CustomView is TView tview) viewaction.Invoke(tview);
		}

		public new class TabView : AppCompatTextView
		{
			public TabView(Context context) : base(context) { }
			public TabView(Context context, IAttributeSet? attrs) : base(context, attrs) { }
			public TabView(Context context, IAttributeSet? attrs, int defStyleAttr) : base(context, attrs, defStyleAttr) { }

			protected string? TextNormal { get; set; }
			protected string? TextSelected { get; set; }

			public void SetTexts(string normal, string selected)
			{
				TextNormal = normal;
				TextSelected = selected;

				SetText(TextNormal, null);
			}
			public void SetTexts(int normal, int selected)
			{
				TextNormal = Context?.Resources?.GetString(normal);
				TextSelected = Context?.Resources?.GetString(selected);

				SetText(TextNormal, null);
			}

			public override bool Selected 
			{ 
				get => base.Selected;
				set
				{
					if (base.Selected = value)
						SetText(TextSelected, null);
					else
						SetText(TextNormal, null);
				}
			}
		}
	}
}