// Reflector.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using System.Web.Hosting;

namespace Kuicker
{
	public class Reflector
	{
		private static object _Lock = new object();

		#region SetValue
		public static bool SetValue(
			object instance, string name, object val)
		{
			// property
			var propertyInfo = GetProperty(instance.GetType(), name);
			if(null != propertyInfo) {
				SetValue(instance, propertyInfo, val);
				return true;
			}

			// field
			var fieldInfo = GetField(instance.GetType(), name);
			if(null != fieldInfo) {
				SetValue(instance, fieldInfo, val);
				return true;
			}

			return false;
		}

		private static object Parse(Type type, object val)
		{
			if(type.IsString()) { return val.ToString(); }
			if(type.IsDateTime()) { return val.ToDateTime(); }
			if(type.IsBoolean()) { return val.ToBoolean(); }
			if(type.IsChar()) { return val.ToChar(); }
			if(type.IsByte()) { return val.ToByte(); }
			if(type.IsShort()) { return val.ToShort(); }
			if(type.IsInteger()) { return val.ToInt(); }
			if(type.IsLong()) { return val.ToLong(); }
			if(type.IsFloat()) { return val.ToFloat(); }
			if(type.IsDouble()) { return val.ToDouble(); }
			if(type.IsDecimal()) { return val.ToDecimal(); }
			if(type.IsBytes()) { return val.ToBytes(); }
			if(type.IsStream()) { return val.ToStream(); }
			if(type.IsGuid()) { return val.ToGuid(); }
			if(type.IsColor()) { return val.ToColor(); }
			return val.ToStringX();
		}

		public static void SetValue(
			object instance, PropertyInfo info, object val)
		{
			if(null == val) { val = string.Empty; }
			if(!info.CanWrite) { return; }

			try {
				Type type = info.PropertyType;
				if(type.IsEnum) {
					// Enum
					if(null == val) {
						val = Reflector.GetEnumDefaultValue(
							info.PropertyType
						);
					}
					val = Enum.Parse(type, val.ToString(), true);
				} else {
					val = Parse(type, val);
				}

				info.SetValue(instance, val, new object[0]);
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}
		}

		public static void SetValue(
			object instance, FieldInfo info, object val)
		{
			if(null == val) { val = string.Empty; }

			try {
				Type type = info.FieldType;
				if(type.IsEnum) {
					// Enum
					if(null == val) {
						val = Reflector.GetEnumDefaultValue(
							info.FieldType
						);
					}
					val = Enum.Parse(type, val.ToString(), true);
				} else {
					val = Parse(type, val);
				}

				info.SetValue(instance, val);
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}
		}
		#endregion

		#region GetValue
		public static object GetValue(string name, object obj)
		{
			if(null == obj) { return null; }
			if(name.IsNullOrEmpty()) { return null; }

			// property
			var propertyInfo = GetProperty(obj.GetType(), name);
			if(null != propertyInfo) {
				return GetValue(propertyInfo, obj);
			}

			// field
			var fieldInfo = GetField(obj.GetType(), name);
			if(null != fieldInfo) {
				return GetValue(fieldInfo, obj);
			}

			return null;
		}

		public static object GetValue(PropertyInfo info, object obj)
		{
			try {
				if(null != info) {
					if(!info.CanRead) { return null; }
					return info.GetValue(obj, new object[0]);
				}
			} catch {
				// swallow it
			}
			return null;
		}

		public static object GetValue(FieldInfo info, object obj)
		{
			try {
				if(null != info) {
					return info.GetValue(obj);
				}
			} catch {
				// swallow it
			}
			return null;
		}
		#endregion

		#region Clone
		public static T ForceClone<T>(T original)
		{
			Type type = typeof(T);
			T clone = CreateInstance<T>();
			if(null == original) { return clone; }

			// Field
			var fields = type.GetFields();
			foreach(var field in fields) {
				var value = GetValue(field, original);
				if(null == value) { continue; }
				SetValue(clone, field, value);
			}

			// Property
			var properties = type.GetProperties();
			foreach(var property in properties) {
				var value = GetValue(property, original);
				if(null == value) { continue; }
				SetValue(clone, property, value);
			}

			return clone;
		}
		#endregion


		#region CreateInstance
		public static T CreateInstance<T>()
		{
			try {
				return (T)CreateInstance(typeof(T));
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
				return default(T);
			}
		}

		public static object CreateInstance(Type type)
		{
			try {
				var info = type.GetConstructor(new Type[0]);
				if(null != info) {
					return info.Invoke(new object[0]);
				}
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}
			return null;
		}

		public static T CreateInstance<T>(
			Type[] types, object[] objs)
		{
			try {
				return (T)CreateInstance(typeof(T), types, objs);
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
				return default(T);
			}
		}

		public static object CreateInstance(
			Type type, Type[] types, object[] objs)
		{
			try {
				var info = type.GetConstructor(types);
				return info.Invoke(objs) ?? null;
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
				return null;
			}
		}
		#endregion

		#region GetField
		public static FieldInfo GetField<T>(string fieldName)
		{
			return GetField(typeof(T), fieldName);
		}

		public static FieldInfo GetField(Type type, string fieldName)
		{
			try {
				// USER_NAME >> username
				string newFieldName = fieldName.Replace(
					Symbol.UnderScore,
					string.Empty
				).ToLower();

				fieldName = fieldName.ToLower();

				var infos = type.GetFields();
				if(null == infos) { return null; }

				FieldInfo maybeInfo = null;
				foreach(FieldInfo info in infos) {
					if(fieldName.Equals(info.Name.ToLower())) {
						return info;
					}
					if(newFieldName.Equals(info.Name.ToLower())) {
						maybeInfo = info;
					}
				}

				return maybeInfo;
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
				return null;
			}
		}
		#endregion

		#region GetProperty
		public static PropertyInfo GetProperty<T>(
			string propertyName)
		{
			return GetProperty(typeof(T), propertyName);
		}

		public static PropertyInfo GetProperty(
			Type type, string propertyName)
		{
			try {
				var infos = type.GetProperties();
				if(null == infos) { return null; }
				foreach(PropertyInfo info in infos) {
					if(propertyName.Equals(info.Name)) {
						return info;
					}
				}
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}
			return null;
		}
		#endregion

		#region GetMethod
		public static MethodInfo GetMethod<T>(string methodName)
		{
			return GetMethod(typeof(T), methodName);
		}

		public static MethodInfo GetMethod(
			Type type, string methodName)
		{
			MethodInfo mi = null;
			try {
				MethodInfo[] infos = type.GetMethods(
					BindingFlags.FlattenHierarchy
					|
					BindingFlags.Static
					|
					BindingFlags.Instance
					|
					BindingFlags.Public
					|
					BindingFlags.NonPublic
				);

				if(null == infos) { return null; }

				foreach(MethodInfo info in infos) {
					if(methodName.Equals(
						info.Name,
						StringComparison.OrdinalIgnoreCase)) {
						if(mi == null) {
							mi = info;
						} else {
							throw new AmbiguousMatchException(
								"Overload Method Found."
							);
						}
					}
				}
			} catch(AmbiguousMatchException exAm) {
				LogRecord.Create().Add(exAm).Error();
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}

			return mi;
		}
		#endregion

		#region Invoke
		public static object Invoke<T>(
			string name, object obj, params object[] parameters)
		{
			return Invoke(name, obj, parameters);
		}

		public static object Invoke(
			string name, object obj, params object[] parameters)
		{
			try {
				var info = GetMethod(obj.GetType(), name);
				if(null == info) {
					throw new Exception(
						new[]{
							obj.GetType().FullName,
							" type have not such method called ",
							name,
						}.Join()
					);
				}
				return info.Invoke(obj, parameters);
			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}

			return null;
		}
		#endregion

		#region Assembly
		private static List<Assembly> _AppCodeAssemblies;
		public static List<Assembly> AppCodeAssemblies
		{
			get
			{
				if(
					null == _AppCodeAssemblies ||
					!_AppCodeAssemblies.Any()) {
					try {
						_AppCodeAssemblies = new List<Assembly>();

						if(HostingEnvironment.IsHosted) {
							if(null != BuildManager.CodeAssemblies) {
								_AppCodeAssemblies.AddRange(
									BuildManager
										.CodeAssemblies
										.OfType<Assembly>()
										.ToArray()
								);
							}
						}
					} catch(Exception ex) {
						LogRecord.Create().Add(ex).Error();
					}
				}
				return _AppCodeAssemblies;
			}
		}



		private static ReadOnlyCollection<Assembly> _Assemblies;
		private static object _AssemblyLock = new object();
		public static ReadOnlyCollection<Assembly> Assemblies
		{
			get
			{
				if(null != _Assemblies) { return _Assemblies; }

				lock(_AssemblyLock) {
					if(null != _Assemblies) { return _Assemblies; }
					var assemblies = new List<Assembly>();

					using(var il = new ILogger()) {
						// load from folder
						il.Record.Add("Section", "load from folder");
						var files = Directory
							.GetFiles(RunTime.BinFolder)
							.Where(x =>
								!x.EndsX(".vshost.exe")
								&&
								x.EndsX(".dll", ".exe")
							);
						foreach(var file in files) {
							if(SkipAssembly(file)) { continue; }
							assemblies.Add(Assembly.LoadFile(file));
						}

						// AppCode
						il.Record.Add("Section", "AppCode");
						assemblies.AddRange(
							AppCodeAssemblies
						);

						// log
						il.Record.Add(
							"Assemblies",
							assemblies
								.Select(x => x.FullName)
								.Join(Environment.NewLine)
						);
					}

					_Assemblies = assemblies.AsReadOnly();
				}

				return _Assemblies;
			}
		}

		private static bool SkipAssembly(string assemblyFile)
		{
			var name = Path.GetFileNameWithoutExtension(
				assemblyFile
			);

			// default skip
			if(name.StartsX(Constants.SkipAssemblyPrefixes)) {
				return true;
			}

			// skip
			if(!Config.Kernel.SkipAssemblyPrefixes.IsNullOrEmpty()) {
				if(name.StartsX(Config.Kernel.SkipAssemblyPrefixes)) {
					return true;
				}
			}

			// only
			if(!Config.Kernel.OnlyAssemblyPrefixes.IsNullOrEmpty()) {
				if(!name.StartsX(Config.Kernel.OnlyAssemblyPrefixes)) {
					return true;
				}
			}

			return false;
		}
		#endregion

		#region Collect
		public static IDictionary<string, Type> CollectImplementedType<T>()
			where T : class
		{
			var list = new Dictionary<string, Type>();
			foreach(var asm in Reflector.Assemblies) {
				var types = CollectImplementedType<T>(asm);
				if(types.Count == 0) { continue; }
				foreach(var one in types) {
					list.SafeAdd(one.Key, one.Value);
				}
			}
			return list;
		}

		public static IDictionary<string, Type> CollectImplementedType<T>(
			Assembly asm)
			where T : class
		{
			var list = new Dictionary<string, Type>();
			var types = asm.GatherByInterface<T>();
			if(types.Any()) {
				foreach(var type in types) {
					if(type.IsAbstract) { continue; }
					list.Add(type.FullName, type);
				}
			}
			return list;
		}

		public static IDictionary<string, T> CollectImplementedObject<T>()
			where T : class
		{
			var list = new Dictionary<string, T>();
			foreach(var asm in Reflector.Assemblies) {
				var objects = CollectImplementedObject<T>(asm);
				if(objects.Count == 0) { continue; }
				foreach(var one in objects) {
					list.SafeAdd(one.Key, one.Value);
				}
			}
			return list;
		}

		public static IDictionary<string, T> CollectImplementedObject<T>(
			Assembly asm)
			where T : class
		{
			var list = new Dictionary<string, T>();
			var types = asm.GatherByInterface<T>();
			if(types.Any()) {
				foreach(var type in types) {
					if(type.IsAbstract) { continue; }

					var one = Activator.CreateInstance(type) as T;
					if(null == one) { continue; }

					list.Add(type.FullName, one);
				}
			}
			return list;
		}





		public static IDictionary<string, object> CollectAttributedObject<T>()
			where T : Attribute
		{
			var list = new Dictionary<string, object>();
			foreach(var asm in Reflector.Assemblies) {
				var objects = CollectAttributedObject<T>(asm);
				if(objects.Count == 0) { continue; }
				foreach(var one in objects) {
					list.SafeAdd(one.Key, one.Value);
				}
			}
			return list;
		}

		public static IDictionary<string, object> CollectAttributedObject<T>(
			Assembly asm)
			where T : Attribute
		{
			var list = new Dictionary<string, object>();
			var types = asm.GatherByAttribute<T>();
			if(types.Any()) {
				foreach(var type in types) {
					if(type.IsAbstract) { continue; }
					var one = Activator.CreateInstance(type);
					list.Add(type.FullName, one);
				}
			}
			return list;
		}

		public static IDictionary<string, O> CollectAttributedObject<T, O>()
			where T : Attribute
			where O : class
		{
			var list = new Dictionary<string, O>();
			foreach(var asm in Reflector.Assemblies) {
				var objects = CollectAttributedObject<T, O>(asm);
				if(objects.Count == 0) { continue; }
				foreach(var one in objects) {
					list.SafeAdd(one.Key, one.Value);
				}
			}
			return list;
		}

		public static IDictionary<string, O> CollectAttributedObject<T, O>(
			Assembly asm)
			where T : Attribute
			where O : class
		{
			var list = new Dictionary<string, O>();
			var types = asm.GatherByAttribute<T>();
			if(types.Any()) {
				foreach(var type in types) {
					if(type.IsAbstract) { continue; }
					var one = Activator.CreateInstance(type) as O;
					if(null == one) { continue; }
					list.Add(type.FullName, one);
				}
			}
			return list;
		}

		public static IDictionary<string, Type> CollectAttributedType<T>()
			where T : Attribute
		{
			var list = new Dictionary<string, Type>();
			foreach(var asm in Reflector.Assemblies) {
				var objects = CollectAttributedType<T>(asm);
				if(objects.Count == 0) { continue; }
				foreach(var one in objects) {
					list.SafeAdd(one.Key, one.Value);
				}
			}
			return list;
		}

		public static IDictionary<string, Type> CollectAttributedType<T>(
			Assembly asm)
			where T : Attribute
		{
			var list = new Dictionary<string, Type>();
			var types = asm.GatherByAttribute<T>();
			if(types.Any()) {
				foreach(var type in types) {
					if(type.IsAbstract) { continue; }
					list.Add(type.FullName, type);
				}
			}
			return list;
		}
		#endregion

		#region ValueType
		public static Type GetElementType(Type type)
		{
			var elementType = GetElementTypeMain(type);
			if(null == elementType) { return type; }
			return elementType.GetGenericArguments()[0];
		}

		private static Type GetElementTypeMain(Type type)
		{
			if(
				null == type
				||
				typeof(string) == type) {
				return null;
			}

			if(type.IsArray) {
				return typeof(IEnumerable<>).MakeGenericType(
					type.GetElementType()
				);
			}

			if(type.IsGenericType) {
				foreach(var x in type.GetGenericArguments()) {
					var ienum = typeof(IEnumerable<>)
						.MakeGenericType(x);
					if(ienum.IsAssignableFrom(type)) {
						return ienum;
					}
				}
			}

			var ifaces = type.GetInterfaces();
			if(ifaces != null && ifaces.Length > 0) {
				foreach(var x in ifaces) {
					var ienum = GetElementTypeMain(x);
					if(null != ienum) { return ienum; }
				}
			}

			if(
				null != type.BaseType
				&&
				typeof(object) != type.BaseType) {
				return GetElementTypeMain(type.BaseType);
			}

			return null;
		}
		#endregion

		#region SetDefaultValue
		internal static void SetDefaultValue(
			object instance, PropertyInfo propInfo)
		{
			if(null == instance) { return; }
			var dv = propInfo.GetAttribute<DefaultValueAttribute>();
			if(null != dv) {
				SetValue(instance, propInfo, dv.Value);
			}
		}
		#endregion

		#region NullToEmptyAndTrim
		internal static object NullToEmptyAndTrim(
			Type objType,
			object objValue,
			PropertyInfo propertyInfo,
			object propertyValue)
		{
			try {
				// 1. objType is null
				if(null == objType) { return null; }

				// 2. objValue is null
				if(null == objValue) {
					// 2.1 ValueType
					if(objType.IsValueType) { return objValue; }

					// 2.2 String
					if(objType.Equals(typeof(string))) {
						return string.Empty;
					}

					// 2.3 CreateInstance
					objValue = Reflector.CreateInstance(objType);
					objValue = NullToEmptyAndTrim(
						objType, objValue, null, null
					);
					return objValue;
				}

				// 3. propertyInfo is null
				if(null == propertyInfo) {
					// 3.1 ValueType
					if(objType.IsValueType) { return objValue; }

					// 3.2 String
					if(objType.Equals(typeof(string))) {
						string v = objValue as string;
						return v.Trim();
					}

					// 3.3 Properties
					foreach(var pi in objType.GetProperties()) {
						if(!pi.CanRead || !pi.CanWrite) { continue; }
						object v = Reflector.GetValue(pi, objValue);
						NullToEmptyAndTrim(objType, objValue, pi, v);
					}
					return objValue;
				}

				var propertyType = propertyInfo.PropertyType;

				// 4. propertyValue is null
				if(null == propertyValue) {
					// 4.1 ValueType
					if(propertyType.IsValueType) {
						return propertyValue;
					}

					// 4.2 String
					if(propertyType.Equals(typeof(string))) {
						Reflector.SetValue(
							objValue, propertyInfo, string.Empty
						);
						return objValue;
					}

					// 4.3 IEnumerable
					if(propertyType.IsDerived<IEnumerable>()) {
						var v = typeof(List<>);
						var genericArgs = propertyType.GetGenericArguments();
						if(null != genericArgs) {
							var concreteType = v.MakeGenericType(genericArgs);
							var newList = Activator.CreateInstance(concreteType);
							Reflector.SetValue(objValue, propertyInfo, newList);
						}
						return objValue;
					}

					// 4.4 Properties
					object x = Reflector.CreateInstance(propertyType);
					foreach(var pi in propertyType.GetProperties()) {
						if(!pi.CanRead || !pi.CanWrite) { continue; }
						object v = pi.GetValue(x, new object[0]);
						x = NullToEmptyAndTrim(propertyType, x, pi, v);
					}
					Reflector.SetValue(objValue, propertyInfo, x);
					return objValue;
				}

				// 5. all is not null
				// 5.1 ValueType
				if(propertyType.IsValueType) {
					return objValue;
				}

				// 5.2 String
				if(propertyType.Equals(typeof(string))) {
					string v = propertyValue as string;
					propertyValue = v.Trim();
					Reflector.SetValue(objValue, propertyInfo, v);
					return objValue;
				}

				// 5.3 IEnumerable
				if(propertyType.IsDerived<IEnumerable>()) {
					var v = typeof(List<>);
					var genericArgs = propertyType.GetGenericArguments();
					if(null != genericArgs) {
						var concreteType = v.MakeGenericType(genericArgs);
						var newList = Activator.CreateInstance(concreteType);
						var list = propertyValue as IEnumerable;
						foreach(var one in list) {
							object o = one;
							object o2 = NullToEmptyAndTrim(
								genericArgs[0], o, null, null
							);
							Reflector.Invoke("Add", newList, new { o2 });
						}
						Reflector.SetValue(objValue, propertyInfo, newList);
					}
					return objValue;
				}

				// 5.4 Properties
				object y = propertyValue;
				foreach(var pi in propertyType.GetProperties()) {
					if(!pi.CanRead || !pi.CanWrite) { continue; }
					object v = pi.GetValue(propertyValue, new object[0]);
					y = NullToEmptyAndTrim(propertyType, y, pi, v);
				}
				Reflector.SetValue(objValue, propertyInfo, y);

			} catch(Exception ex) {
				LogRecord.Create().Add(ex).Error();
			}

			return objValue;
		}
		#endregion

		#region Enum
		public static string GetEnumDefaultValue(Type type)
		{
			string value = string.Empty;
			object[] attrs = type.GetCustomAttributes(
				typeof(DefaultValueAttribute), false
			);
			if(!attrs.IsNullOrEmpty()) {
				value = ((DefaultValueAttribute)attrs[0])
					.Value
					.ToString();
			}

			// get the first enum value!
			if(value.IsNullOrEmpty()) {
				foreach(var x in type.GetFields()) {
					if(!x.IsLiteral) { continue; }
					value = x.Name;
					break;
				}
			}

			return value;
		}
		public static List<string> GetEnumPossibleValues(Type type)
		{
			var list = new List<string>();
			foreach(var info in type.GetFields()) {
				if(!info.IsLiteral) { continue; }
				list.Add(info.Name);
			}
			return list;
		}
		#endregion

		#region Type
		public static Type GetPrimitiveType(string fullName)
		{
			try {
				return Type.GetType(fullName);
			} catch(Exception ex) {
				LogRecord
					.Create()
					.Add(ex)
					.Error();
				return default(Type);
			}
		}
		#endregion

		#region Private
		public static bool SupportedBaseType(Type type)
		{
			if(type.IsString()) { return true; }
			if(type.IsDateTime()) { return true; }
			if(type.IsBoolean()) { return true; }
			if(type.IsChar()) { return true; }
			if(type.IsByte()) { return true; }
			if(type.IsShort()) { return true; }
			if(type.IsInteger()) { return true; }
			if(type.IsLong()) { return true; }
			if(type.IsFloat()) { return true; }
			if(type.IsDouble()) { return true; }
			if(type.IsDecimal()) { return true; }

			if(type.IsStream()) { return true; }
			if(type.IsGuid()) { return true; }
			if(type.IsEnum()) { return true; }
			if(type.IsBytes()) { return true; }
			if(type.IsColor()) { return true; }
			return false;
		}
		#endregion
	}
}
