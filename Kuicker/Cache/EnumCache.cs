// EnumCache.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Kuicker
{
	public class EnumCache
	{
		private static object _UnderlyingTypeLock = new object();
		private static Dictionary<Type, Type> _UnderlyingTypes =
			new Dictionary<Type, Type>();
		public static Type GetUnderlyingType<T>()
		{
			Type key = typeof(T);
			if(!key.IsEnum) {
				throw new NotSupportedException(
					LogRecord
						.Create()
						.SetMessage("Only enum type allowed")
						.Add("Type", key.FullName)
						.Error()
						.Message
				);
			}

			lock(_UnderlyingTypeLock) {
				Type value;
				if(!_UnderlyingTypes.TryGetValue(key, out value)) {
					value = Enum.GetUnderlyingType(typeof(T));
					_UnderlyingTypes.Add(key, value);
				}
				return value;
			}
		}

		public static EnumRef Get<T>()
		{
			return Get(typeof(T));
		}

		private static object _EnumRefLock = new object();
		private static Dictionary<string, EnumRef> _EnumRefs =
			new Dictionary<string, EnumRef>();
		public static EnumRef Get(Type type)
		{
			if(!type.IsEnum) {
				throw new NotSupportedException(
					LogRecord
						.Create()
						.SetMessage("Only enum type allowed")
						.Add("Type", type.FullName)
						.Error()
						.Message
				);
			}

			EnumRef ef;

			if(_EnumRefs.TryGetValue(type.FullName, out ef)) {
				return ef;
			}

			lock(_EnumRefLock) {
				if(_EnumRefs.TryGetValue(type.FullName, out ef)) {
					return ef;
				}

				ef = new EnumRef() {
					DefaultValue = type.GetDefaultValue().ToStringX(),
					Description = type.GetDescription().AirBag(type.Name),
					Category = type.GetCategory(),
					FullName = type.FullName,
					Type = type,
				};

				var infos = type.GetFields().ToList();
				string[] names = Enum.GetNames(type);
				Array values = Enum.GetValues(type);
				for(int i = 0; i < names.Length; i++) {
					var info = infos.FirstOrDefault(x => 
						x.Name == names[i]
					);
					if(null == info) {
						throw new NotSupportedException(
							LogRecord
								.Create()
								.SetMessage("Enum cache error")
								.Add("Type", type.FullName)
								.Error()
								.Message
						);
					}

					var ei = new EnumItem() {
						Category = info.GetCategory(),
						Description = info.GetDescription().AirBag(info.Name),
						Type = type,
						Name = info.Name,
						Value = (int)values.GetValue(i),
					};

					ef.Items.Add(ei);
				}
				_EnumRefs.Add(type.FullName, ef);

				return ef;
			}
		}
	}
}
