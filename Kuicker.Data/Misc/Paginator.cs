// Paginator.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;

namespace Kuicker.Data
{
	public class Paginator
	{
		public Paginator(int pageSize, int pageIndex, int count)
		{
			this.PageSize = pageSize;
			this.PageIndex = pageIndex;
			this.Count = count;

			this.PageSize = (this.PageSize < 1)
				? 1
				: this.PageSize;

			this.PageIndex = (this.PageIndex < 1)
				? 1
				: (this.PageIndex > this.LastPageIndex)
					? this.LastPageIndex
					: this.PageIndex;
		}

		public int PageSize { get; private set; }
		public int PageIndex { get; private set; }
		public int Count { get; private set; }

		public int PageRowCount
		{
			get
			{
				return (Count == 0 || PageIndex > LastPageIndex)
					? 0
					: RowTo - RowFrom + 1;
			}
		}

		public int FirstPageIndex
		{
			get
			{
				return 1;
			}
		}

		public int PrePageIndex
		{
			get
			{
				return (PageIndex > FirstPageIndex) 
					? PageIndex - 1 
					: FirstPageIndex;
			}
		}

		public int NextPageIndex
		{
			get
			{
				return (PageIndex < LastPageIndex) 
					? PageIndex + 1 
					: LastPageIndex;
			}
		}

		public int LastPageIndex
		{
			get
			{
				return (int)Math.Ceiling(
					(decimal)Count / (decimal)PageSize
				);
			}
		}

		public int RowFrom
		{
			get
			{
				if(Count == 0) { return 0; }
				return PageSize * (PageIndex - 1) + 1;
			}
		}

		public int RowTo
		{
			get
			{
				return Math.Min(PageSize * PageIndex, Count);
			}
		}

		public bool InPageRange(int index)
		{
			return (index >= RowFrom) && (index <= RowTo);
		}

		/// <summary>
		/// Page viewport size, for example:<br />
		/// If ViewportSize equals to 5, PageIndex equals to 14.
		/// You will find out Pages is [11,12,13,14,15].
		/// </summary>
		public int ViewportSize
		{
			get { return _ViewportSize; }
			set
			{
				_ViewportSize = value < 0 ? 0 : value;
				_Pages = null;
			}
		}
		private int _ViewportSize = 0;


		public int NextViewPortPageIndex
		{
			get
			{
				var i = Pages[Pages.Count - 1].PageIndex + 1;
				return i > LastPageIndex ? LastPageIndex : i;
			}
		}

		public int PreViewPortPageIndex
		{
			get
			{
				var i = Pages[0].PageIndex - 1;
				return i == 0 ? 1 : i;
			}
		}

		private List<PageOne> _Pages;
		public List<PageOne> Pages
		{
			get
			{
				if(null == _Pages) {
					_Pages = new List<PageOne>();
					var start = ViewportSize > 0
						? Convert.ToInt32(
							Math.Floor(
								Convert.ToDecimal(PageIndex - 1)
								/
								Convert.ToDecimal(ViewportSize)
							)
						) * ViewportSize + 1
						: 1;
					var end = ViewportSize > 0
						? start + ViewportSize - 1
						: LastPageIndex;
					if(end > LastPageIndex) { end = LastPageIndex; }
					for(var i = start; i <= end; i++) {
						_Pages.Add(new PageOne(i, PageIndex == i));
					}
				}
				return _Pages;
			}
		}
	}
}
