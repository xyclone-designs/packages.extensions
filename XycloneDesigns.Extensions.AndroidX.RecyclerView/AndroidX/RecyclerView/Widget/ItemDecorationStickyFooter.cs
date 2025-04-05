#nullable enable

using Android.Content.Res;
using Android.Graphics;
using Android.Views;

using System;

namespace AndroidX.RecyclerView.Widget
{
	public class ItemDecorationStickyFooter : RecyclerView.ItemDecoration
	{
		public override void OnDrawOver(Canvas canvas, RecyclerView parent, RecyclerView.State state)
		{
			base.OnDrawOver(canvas, parent, state);

			RecyclerView.Adapter? adapter = parent.GetAdapter();
			LinearLayoutManager? layoutManager = parent.GetLayoutManager() as LinearLayoutManager;

			if (adapter is null || layoutManager is null || adapter.ItemCount == 1)
				return;

			int lastVisiblePosition = layoutManager.FindLastVisibleItemPosition();
			int firstVisiblePosition = layoutManager.FindFirstCompletelyVisibleItemPosition();

			if (firstVisiblePosition != 0 || lastVisiblePosition != (adapter.ItemCount - 1))
				return;

			View? summaryView = parent.GetChildAt(parent.ChildCount - 1);
			RecyclerView.LayoutParams? summaryViewLayoutParams = summaryView?.LayoutParameters as RecyclerView.LayoutParams;

			if (summaryView is null || summaryViewLayoutParams is null)
				return;

			int topOffset = parent.Height - summaryView.Height;
			int leftOffset = summaryViewLayoutParams.LeftMargin;
			canvas.Save();
			canvas.Translate(leftOffset, topOffset);
			summaryView.Draw(canvas);
			canvas.Restore();
			summaryView.SetVisibility(ViewStates.Gone);
		}
	}
}