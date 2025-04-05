#nullable enable

using Android.Views;

using AndroidX.RecyclerView.Widget;

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;

namespace AndroidX.RecyclerView.Widget
{
	public abstract class RecyclerViewAdapterDefault : RecyclerView.Adapter
	{
		public const int ItemViewType_Normal = -1;
		public const int ItemViewType_Header = -2;
		public const int ItemViewType_Footer = -3;
		public const int ItemViewType_OnEmpty = -4;

		public RecyclerView? Parent { get; set; }
		public View? HeaderView { get; set; }
		public View? FooterView { get; set; }
		public View? EmptyView { get; set; }
	}
	public class RecyclerViewAdapterDefault<T> : RecyclerViewAdapterDefault
	{
		public RecyclerViewAdapterDefault() : this(new List<T> { }) { }
		public RecyclerViewAdapterDefault(IList<T>? items) : this(items, null, null, null) { }
		public RecyclerViewAdapterDefault(IList<T>? items, View? headerView, View? footerView, View? onEmptyView)
		{
			HasStableIds = false;
			HeaderView = headerView;
			FooterView = footerView;
			EmptyView = onEmptyView;
			Items = items ?? new List<T>();
		}

		public IList<T> Items { get; set; }
		
		public int? SelectedCurrentIndex { get; set; }
		public int? SelectedPreviousIndex { get; set; }
		public RecyclerViewViewHolderDefault? SelectedCurrentViewHolder { get; set; }
		public RecyclerViewViewHolderDefault? SelectedPreviousViewHolder { get; set; }

		public Action<RecyclerViewViewHolderDefault, int>? ViewHolderOnBind { get; set; }
		public Func<ViewGroup, int, View?>? ViewOnCreate { get; set; }
		public Func<ViewGroup, int, RecyclerViewViewHolderDefault?>? ViewHolderOnCreate { get; set; }
		public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnCheckChange { get; set; }
		public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnClick { get; set; }
		public Action<RecyclerViewViewHolderDefault.ViewHolderEventArgs>? ViewHolderOnLongClick { get; set; }

		protected virtual void ViewHolder_OnCheckChange(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			ViewHolderOnCheckChange?.Invoke(args);
		}
		protected virtual void ViewHolder_OnClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			SelectedPreviousIndex = SelectedCurrentIndex;
			SelectedPreviousViewHolder = SelectedCurrentViewHolder;
			SelectedCurrentIndex = args.ViewHolder.Index;
			SelectedCurrentViewHolder = args.ViewHolder;

			if (SelectedCurrentIndex.HasValue) NotifyItemChanged(SelectedCurrentIndex.Value);
			if (SelectedPreviousIndex.HasValue) NotifyItemChanged(SelectedPreviousIndex.Value);

			ViewHolderOnClick?.Invoke(args);
		}
		protected virtual void ViewHolder_OnLongClick(object? sender, RecyclerViewViewHolderDefault.ViewHolderEventArgs args)
		{
			ViewHolderOnLongClick?.Invoke(args);
		}

		public virtual View OnCreateView(ViewGroup parent, int viewtype) 
		{
			return ViewOnCreate?.Invoke(parent, viewtype) ?? viewtype switch
			{
				ItemViewType_Footer when FooterView is not null => FooterView,
				ItemViewType_Header when HeaderView is not null => HeaderView,
				ItemViewType_OnEmpty when EmptyView is not null => EmptyView,

				_ => new View(parent.Context) { }
			};
		}
		
		public override int ItemCount
		{
			get
			{
				int itemCount = Items.Count;

				if (itemCount == 0)
				{
					if (EmptyView != null)
						return 1;
					else return 0;
				}

				if (HeaderView != null)
					itemCount += 1;

				if (FooterView != null)
					itemCount += 1;

				return itemCount;
			}
		}
		public override long GetItemId(int position)
		{
			return position;
		}
		public override int GetItemViewType(int position)
		{
			int itemCount = ItemCount;

			if (Items.Count == 0 && EmptyView != null)
				return ItemViewType_OnEmpty;

			if (itemCount == 0)
				return ItemViewType_Normal;

			if (HeaderView != null && position == 0)
				return ItemViewType_Header;

			if (FooterView != null && position == itemCount - 1)
				return ItemViewType_Footer;

			return ItemViewType_Normal;
		}
		public override void OnViewAttachedToWindow(Java.Lang.Object holder)
		{
			base.OnViewAttachedToWindow(holder);

			RecyclerViewViewHolderDefault viewholder = (RecyclerViewViewHolderDefault)holder;

			viewholder.OnCheckedChange += ViewHolder_OnCheckChange;
			viewholder.OnClick += ViewHolder_OnClick;
			viewholder.OnLongClick += ViewHolder_OnLongClick;
		}
		public override void OnViewDetachedFromWindow(Java.Lang.Object holder)
		{
			base.OnViewDetachedFromWindow(holder);

			RecyclerViewViewHolderDefault viewholder = (RecyclerViewViewHolderDefault)holder;

			viewholder.OnCheckedChange -= ViewHolder_OnCheckChange;
			viewholder.OnClick -= ViewHolder_OnClick;
			viewholder.OnLongClick -= ViewHolder_OnLongClick;
		}
		public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
		{
			RecyclerViewViewHolderDefault holderDefault = (RecyclerViewViewHolderDefault)holder;

			if ((position = GetBindViewHolderPosition(position)) == -1)
				return;

			holderDefault.Index = position;

			ViewHolderOnBind?.Invoke(holderDefault, position);
		}
		public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewtype)
		{
			return 
				ViewHolderOnCreate?.Invoke(parent, viewtype) ?? 
				new RecyclerViewViewHolderDefault(OnCreateView(parent, viewtype));
		}

		public void ItemsClear()
		{
			int size = Items.Count;
			Items.Clear();
			NotifyItemRangeRemoved(0, size);
		}
		public void ItemsAdd(T item)
		{
			Items.Add(item);
			NotifyItemInserted(Items.Count - 1);
		}
		public void ItemsAdd(IEnumerable<T> items)
		{
			int size = items.Count();
			foreach (T item in items) Items.Add(item);
			NotifyItemRangeInserted(size - 1, Items.Count);
		}
		public void ItemsClearThenAdd(IEnumerable<T> items)
		{
			if (items == null)
				ItemsClear();
			else
			{
				int oldSize = Items.Count;
				int newSize = items.Count();
				Items.Clear();
				foreach (T item in items) Items.Add(item);
				if (oldSize == newSize)
					NotifyDataSetChanged(); //NotifyItemRangeChanged(0, oldSize);
				else if (oldSize > newSize)
				{
					NotifyDataSetChanged();
					//NotifyItemRangeChanged(0, newSize);
					//NotifyItemRangeRemoved(newSize - 1, oldSize - newSize);
				}
				else
				{
					NotifyDataSetChanged();
					//NotifyItemRangeChanged(0, oldSize);
					//NotifyItemRangeInserted(Math.Max(oldSize - 1, 0), newSize - oldSize);
				}
			}
		}
		public int GetBindViewHolderPosition(int position)
		{
			bool no =
				(EmptyView != null && ItemCount == 1) ||
				(HeaderView != null && position == 0) ||
				(FooterView != null && position == ItemCount - 1);

			if (no) return -1;
			if (HeaderView != null) return position - 1;
			return position;
		}
	}	
}