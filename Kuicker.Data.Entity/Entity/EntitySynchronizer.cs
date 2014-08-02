// EntityEnumerator.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kuicker.Data
{
	public class EntitySynchronizer<TFrom, TTo>
		where TFrom : class, IEntity<TFrom>, new()
		where TTo : class, IEntity<TTo>, new()
	{
		private static TFrom _From;
		private static TTo _To;
		private static object _Lock = new object();

		public EntitySynchronizer()
		{
			// property
			this.EnumeratorPageSize = 100;

			// Func
			this.FromFunc = this.From;
			this.CombineFunc = this.Combine;
			this.EqualsFunc = this.Equals;
			this.SkipInsertOneFunc = this.SkipInsertOne;
			this.SkipDeleteOneFunc = this.SkipDeleteOne;

			// Action
			this.BeforeInsertNewDataAction = this.BeforeInsertNewData;
			this.BeforeUpdateDataAction = this.BeforeUpdateData;
			this.AfterInsertNewDataAction = this.AfterInsertNewData;
			this.AfterUpdateDataAction = this.AfterUpdateData;
		}

		#region Entity
		protected TFrom __From
		{
			get
			{
				if(null == _From) {
					lock(_Lock) {
						if(null == _From) {
							_From = EntityCache.Get<TFrom>();
						}
					}
				}
				return _From;
			}
		}

		protected TTo __To
		{
			get
			{
				if(null == _To) {
					lock(_Lock) {
						if(null == _To) {
							_To = EntityCache.Get<TTo>();
						}
					}
				}
				return _To;
			}
		}
		#endregion

		#region property
		public virtual int EnumeratorPageSize { get; private set; }
		#endregion

		#region Skip
		public virtual bool SkipInsert { get { return false; } }
		public virtual bool SkipUpdate { get { return false; } }
		public virtual bool SkipDelete { get { return false; } }
		public virtual bool SkipDeleteWhenFromEmpty { get { return true; } }
		public virtual bool DisableWhenDelete { get { return true; } }
		#endregion

		#region Func
		public Func<TFrom, TTo> FromFunc
		{ private get; set; }
		public Func<TFrom, TTo, TTo> CombineFunc
		{ private get; set; }
		public Func<TFrom, TTo, bool> EqualsFunc
		{ private get; set; }
		public Func<TFrom, bool> SkipInsertOneFunc
		{ private get; set; }
		public Func<TTo, bool> SkipDeleteOneFunc
		{ private get; set; }
		public Action<TFrom> BeforeInsertNewDataAction
		{ private get; set; }
		public Action<TFrom, TTo> BeforeUpdateDataAction
		{ private get; set; }
		public Action<Result, TFrom> AfterInsertNewDataAction
		{ private get; set; }
		public Action<Result, TFrom, TTo> AfterUpdateDataAction
		{ private get; set; }
		#endregion

		public virtual TTo From(TFrom from)
		{
			TTo to = new TTo();
			return CombineFunc(from, to);
		}

		public virtual TTo Combine(TFrom from, TTo to)
		{
			foreach(var fromColumn in from.__Columns) {
				var name = fromColumn.PropertyName;
				var column = to.SaftGetColumn(name);
				if(null == column) { continue; }

				var value = from.GetValue(name);
				to.SetValue(name, value);
			}

			return to;
		}

		public virtual bool Equals(TFrom from, TTo to)
		{
			return from.Equals(to);
		}

		public virtual bool SkipInsertOne(TFrom from)
		{
			return false;
		}

		public virtual bool SkipUpdateOne(TFrom from, TTo to)
		{
			return false;
		}

		public virtual bool SkipDeleteOne(TTo to)
		{
			return false;
		}

		public virtual void BeforeInsertNewData(TFrom from)
		{
			// do nothoing
		}
		public virtual void BeforeUpdateData(
			TFrom from, TTo to)
		{
			// do nothoing
		}
		public virtual void AfterInsertNewData(Result result, TFrom from)
		{
			// do nothoing
		}
		public virtual void AfterUpdateData(Result result, TFrom from, TTo to)
		{
			// do nothoing
		}

		private Result InsertNewData(TFrom from)
		{
			// SkipInsertOne
			if(SkipInsertOne(from)) {
				return Result.BuildSuccess();
			}

			// BeforeInsertNewDataAction
			BeforeInsertNewDataAction(from);

			// 
			TTo to = FromFunc(from);
			Result result = to.Add();

			// AfterInsertNewDataAction
			AfterInsertNewDataAction(result, from);

			var log = LogRecord
				.Create()
				.Add("EntityName", to.__EntityName)
				.AddRange(to.__PropertyNameValues);
			if(result.Success) {
				log.Debug();
			} else {
				log.Add(result.Exception).Error();
			}

			return result;
		}

		private Result UpdateData(TFrom from, TTo to)
		{
			// SkipUpdateOne
			if(SkipUpdateOne(from, to)) {
				return Result.BuildSuccess();
			}

			if(EqualsFunc(from, to)) {
				return Result.BuildSuccess();
			}

			TTo newTo = CombineFunc(from, to);

			// BeforeUpdateDataAction
			BeforeUpdateDataAction(from, to);

			// 
			Result result = newTo.Modify();

			// AfterUpdateDataAction
			AfterUpdateDataAction(result, from, to);

			var log = LogRecord
				.Create()
				.Add("EntityName", newTo.__EntityName)
				.AddRange(newTo.__PropertyNameValues);
			if(result.Success) {
				log.Debug();
			} else {
				log.Add(result.Exception).Error();
			}

			return new Result();
		}

		public virtual void Sync()
		{
			#region delete
			if(
				SkipDelete
				||
				(
					SkipDeleteWhenFromEmpty
					&&
					Entity.Count(__From.__EntityName) == 0
			)) {
				LogRecord
					.Create()
					.SetMessage("Skip delete")
					.Add("From Entity Name", __From.__EntityName)
					.Add("To Entity Name", __To.__EntityName)
					.Debug();
			} else {
				var deleteList = new List<List<Any>>();
				using(var enumerator =
					new EntityEnumerator<TTo>(EnumeratorPageSize)
				) {
					while(enumerator.MoveNext()) {
						var tos = enumerator.Current;
						foreach(TTo to in tos) {
							if(!Entity.Exists(
								__From.__EntityName,
								__From.__KeyColumnNameValues.ToArray())) {
								deleteList.Add(to.__KeyColumnNameValues);
							}
						}
					}
				}
				foreach(var keyValues in deleteList) {
					TTo destination =
						Entity<TTo>.Get(keyValues.ToArray());
					if(!SkipDeleteOne(destination)) {
						destination.Modify();
					}
				}
			}
			#endregion

			#region insert & update
			using(var enumerator =
				new EntityEnumerator<TFrom>(EnumeratorPageSize)
			) {
				while(enumerator.MoveNext()) {
					var froms = enumerator.Current;
					foreach(TFrom from in froms) {
						TTo to = Entity<TTo>.Get(
							from.__PropertyNameValues.ToArray()
						);
						if(null == to) {
							// Insert
							if(!SkipInsert) { InsertNewData(from); }
						} else {
							// Update
							if(!SkipUpdate) { UpdateData(from, to); }
						}
					}
				}
			}
			#endregion
		}
	}
}
