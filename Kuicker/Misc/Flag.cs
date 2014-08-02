// Flag.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;

namespace Kuicker
{
	public class Flag
	{
		public static EnumRef Get(Type type)
		{
			return EnumCache.Get(type);
		}
		public static EnumRef Get<T>()
		{
			return EnumCache.Get<T>();
		}

		public static int Grant(Type type, int current, int grant)
		{
			IsFlag(type);
			return (int)Enum.Parse(
				type, 
				(current | grant).ToString()
			);
		}

		public static T Grant<T>(int current, int grant)
		{
			Type type = typeof(T);
			IsFlag(type);
			return (T)Enum.Parse(
				type, 
				(current | grant).ToString()
			);
		}

		public static int Revoke(Type type, int current, int revoke)
		{
			IsFlag(type);
			return (int)Enum.Parse(
				type, 
				(current & (current ^ revoke)).ToString()
			);
		}

		public static T Revoke<T>(int current, int revoke)
		{
			Type type = typeof(T);
			IsFlag(type);
			return (T)Enum.Parse(
				type, 
				(current & (current ^ revoke)).ToString()
			);
		}

		public static bool Check(int current, int check)
		{
			return (current & check) == check;
		}

		public static bool Check<T>(T current, T check)
		{
			Type type = typeof(T);
			IsFlag(type);
			return Check(current.ToInteger(), check.ToInteger());
		}

		public static List<Any> ToAnys<T>()
		{
			Type type = typeof(T);
			IsFlag(type);
			return ToAnys(type);
		}

		public static List<Any> ToAnys(Type type)
		{
			IsFlag(type);
			var ef = EnumCache.Get(type);
			return ef.Anys;
		}

		public static List<EnumItem> CombinedBy(Type type, int check)
		{
			var ef = Get(type);
			var list = new List<EnumItem>();
			foreach(var ei in ef.Items) {
				if(Check(ei.Value, check)) {
					list.Add(ei);
				}
			}

			return list;
		}

		private static void IsFlag<T>()
		{
			IsFlag(typeof(T));
		}

		private static void IsFlag(Type type)
		{
			if(!TryIsFlag(type)) {
				throw new ArgumentException(string.Format(
					"'{0}' -- Only flag enum was allowed!",
					type.Name
				));
			}
		}

		private static bool TryIsFlag<T>()
		{
			return TryIsFlag(typeof(T));
		}
		private static bool TryIsFlag(Type type)
		{
			if(null == type) { return false; }
			return type.IsEnum
				? type.HasAttribute<FlagsAttribute>()
				: false;
		}
	}
}
