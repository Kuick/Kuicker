// EntityEnumerator.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kuicker.Data
{
	public class EntityEnumerator<T>
		: IEnumerator<List<T>>
		where T : class, IEntity<T>, new()
	{
		private const int DEFAULT_PAGE_SIZE = 100;
		private T _Instance;
		private Sql<T> _Sql;
		private int _Count = -1;
		private Paginator _Paginator;

		#region constructor
		public EntityEnumerator()
			: this(DEFAULT_PAGE_SIZE)
		{
		}

		public EntityEnumerator(int pageSize)
			: this(null, pageSize)
		{
		}

		public EntityEnumerator(Sql<T> sql)
			: this(sql, DEFAULT_PAGE_SIZE)
		{
		}

		public EntityEnumerator(Sql<T> sql, int pageSize)
		{
			this.PageSize = (pageSize <= 0)
				? DEFAULT_PAGE_SIZE
				: pageSize;
			this.Sql = sql;
			this.PageIndex = 0;
		}
		#endregion

		#region property
		public T Instance
		{
			get
			{
				if(null == _Instance) {
					_Instance = new T();
				}
				return _Instance;
			}
		}

		public int PageSize { get; private set; }

		public Sql<T> Sql
		{
			get
			{
				if(null == _Sql) {
					_Sql = Sql<T>.Create();
				}
				return _Sql;
			}
			private set
			{
				_Sql = value;
			}
		}

		public int Count
		{
			get
			{
				if(_Count == -1) {
					_Count = Sql.Count();
				}
				return _Count;
			}
		}

		public Paginator Paginator
		{
			get
			{
				if(null == _Paginator) {
					_Paginator = new Paginator(PageSize, 1, Count);
				}
				return _Paginator;
			}
		}

		public int PageIndex { get; private set; }
		#endregion

		#region IEnumerator<IEntity>
		public IQueryable<T> Current { get; private set; }
		#endregion

		#region IEnumerator<List<T>>
		List<T> IEnumerator<List<T>>.Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			_Paginator = null;
			_Instance = default(T);
		}
		#endregion

		#region IEnumerator
		object IEnumerator.Current
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public bool MoveNext()
		{
			if(Paginator.LastPageIndex < ++PageIndex) {
				return false;
			}

			Sql.SelectPage(PageSize, PageIndex);
			Current = Sql.Query();
			return Current.Any();
		}

		public void Reset()
		{
			PageIndex = 0;
		}
		#endregion
	}
}
