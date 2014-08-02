// SqlConvention.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Reflection;

namespace Kuicker.Data
{
	public sealed class SqlConvention
	{
		private static Dictionary<string, ISqlConvention> 
			_Conventions = new Dictionary<string, ISqlConvention>();

		public static ISqlConvention Default { get; set; }


		// ToTableName
		public static string ToTableName<T>()
			where T : class, IEntity, new()
		{
			return ToTableName(typeof(T));
		}
		public static string ToTableName(Type type)
		{
			ISqlConvention c = Get(type.Name);
			return c.ToTableName(type);
		}


		// ToColumnName
		public static string ToColumnName<T>(
			PropertyInfo info)
			where T : class, IEntity, new()
		{
			return ToColumnName(typeof(T), info);
		}
		public static string ToColumnName(
			Type type, PropertyInfo info)
		{
			ISqlConvention c = Get(type.Name);
			return c.ToColumnName(type, info);
		}


		// Get
		private static ISqlConvention Get<TEntity>()
			where TEntity : class, IEntity, new()
		{
			return Get(typeof(TEntity).Name);
		}
		private static ISqlConvention Get(string entityName)
		{
			ISqlConvention c;
			if(_Conventions.TryGetValue(entityName, out c)) {
				return c;
			}
			return Default;
		}


		// Assign
		public static void Assign<TEntity, TConvention>()
			where TEntity : class, IEntity, new()
			where TConvention : class, ISqlConvention, new()
		{
			Assign<TConvention>(typeof(TEntity).Name);
		}
		public static void Assign<TConvention>(string entityName)
			where TConvention : class, ISqlConvention, new()
		{
			_Conventions.SafeAdd(entityName, new TConvention());
		}
	}
}
