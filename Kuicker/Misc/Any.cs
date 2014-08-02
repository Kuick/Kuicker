// Any.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Kuicker
{
	public class Any
	{
		#region constructor
		public Any()
			: this(string.Empty, string.Empty)
		{
		}

		public Any(string name)
			: this(name, string.Empty)
		{
		}

		public Any(string name, params object[] values)
		{
			this.Name = name;
			if(null == values) {
				this.Value = values;
			} else {
				if(values.Length == 1) {
					this.Value = values[0];
				} else {
					this.Value = values;
				}
			}
		}
		#endregion

		#region property
		public string Name { get; set; }
		public object Value { get; set; }

		[JsonConverter(typeof(StringEnumConverter))]
		public DataFormat Format
		{
			get
			{
				if(null != Value) {
					return Value.GetType().GetFormat();
				}
				return DataFormat.Unknown;
			}
		}
		#endregion

		#region ToXxx
		public string[] ToStrings()
		{
			return ToStrings(new string[0]);
		}
		public string[] ToStrings(string[] airBag)
		{
			return ToStrings(Symbol.Comma, airBag);
		}
		public string[] ToStrings(string separator)
		{
			return ToStrings(separator, new string[0]);
		}
		public string[] ToStrings(string separator, string[] airBag)
		{
			var list = new List<string>();
			var parts = ToString().SplitAndTrim(separator);
			foreach(var part in parts) {
				if(part.IsNullOrEmpty()) { continue;}
				list.Add(part);
			}
			if(list.IsNullOrEmpty()) { return airBag; }
			return list.ToArray();
		}

		public int[] ToIntegers()
		{
			return ToIntegers(new int[0]);
		}
		public int[] ToIntegers(int[] airBag)
		{
			return ToIntegers(Symbol.Comma, airBag);
		}
		public int[] ToIntegers(string separator)
		{
			return ToIntegers(separator, new int[0]);
		}
		public int[] ToIntegers(string separator, int[] airBag)
		{
			var list = new List<int>();
			var parts = ToString().SplitAndTrim(separator);
			foreach(var part in parts) {
				if(part.IsNumeric()) {
					int value = part.ToInt();
					list.Add(value);
				}
			}
			if(list.IsNullOrEmpty()) { return airBag; }
			return list.ToArray();
		}

		public override string ToString()
		{
			return ToString(string.Empty);
		}

		public string ToString(string airBag)
		{
			switch(Format) {
				case DataFormat.Unknown:
				case DataFormat.String:
					return null == Value ? airBag : Value.ToString();
				case DataFormat.DateTime:
					return ToDateTime().yyyy_MM_dd_HH_mm_ss();
				case DataFormat.Boolean:
					return ToBoolean().ToString().ToLower();
				case DataFormat.Char:
					return null == Value ? airBag : Value.ToString();
				case DataFormat.Byte:
					return ToByte().ToString();
				case DataFormat.Short:
					return ToShort().ToString();
				case DataFormat.Integer:
					return ToInteger().ToString();
				case DataFormat.Long:
					return ToLong().ToString();
				case DataFormat.Float:
					return ToFloat().ToString("0.00");
				case DataFormat.Double:
					return ToDecimal().ToString("0.00");
				case DataFormat.Decimal:
					return ToDecimal().ToString("0.00");
				case DataFormat.Enum:
					return Value.ToString();
				case DataFormat.Object:
					return null == Value ? airBag : Value.ToString();
				case DataFormat.Objects:
					//return Values.ToStringX();
					return Value.ToStringX();
				case DataFormat.ByteArray:
					return ToByteArray().ToString();
				case DataFormat.Color:
					return ToColor().ToString();
				case DataFormat.Guid:
					return ToGuid().ToString();
				case DataFormat.TimeSpan:
					return ToLong().ToString();

				default:
					return null == Value ? airBag : Value.ToString();
			}
		}

		public DateTime ToDateTime()
		{
			return ToDateTime(DateTime.MinValue);
		}

		public DateTime ToDateTime(DateTime airBag)
		{
			if(Format == DataFormat.DateTime) {
				return null == Value ? airBag : (DateTime)Value;
			} else {
				return Value.ToDateTime(airBag);
			}
		}

		public bool ToBoolean()
		{
			return ToBoolean(false);
		}

		public bool ToBoolean(bool airBag)
		{
			if(Format == DataFormat.Boolean) {
				return null == Value ? airBag : (bool)Value;
			} else {
				return Value.ToBoolean(airBag);
			}
		}

		public char ToChar()
		{
			return ToChar(default(char));
		}

		public char ToChar(char airBag)
		{
			if(Format == DataFormat.Char) {
				return null == Value ? airBag : (char)Value;
			} else {
				return Value.ToChar(airBag);
			}
		}

		public byte ToByte()
		{
			return ToByte(default(byte));
		}

		public byte ToByte(byte airBag)
		{
			if(Format == DataFormat.Byte) {
				return null == Value ? airBag : (byte)Value;
			} else {
				return Value.ToByte(airBag);
			}
		}

		public short ToShort()
		{
			return ToShort(default(short));
		}

		public short ToShort(short airBag)
		{
			if(Format == DataFormat.Short) {
				return null == Value ? airBag : (short)Value;
			} else {
				return Value.ToShort(airBag);
			}
		}

		public int ToInteger()
		{
			return ToInteger(default(int));
		}

		public int ToInteger(int airBag)
		{
			if(Format == DataFormat.Integer) {
				return null == Value ? airBag : (int)Value;
			} else {
				return Value.ToInteger(airBag);
			}
		}

		public long ToLong()
		{
			return ToLong(default(long));
		}

		public long ToLong(long airBag)
		{
			if(Format == DataFormat.Long) {
				return null == Value ? airBag : (long)Value;
			} else {
				return Value.ToLong(airBag);
			}
		}

		public float ToFloat()
		{
			return ToFloat(default(float));
		}

		public float ToFloat(float airBag)
		{
			if(Format == DataFormat.Float) {
				return null == Value ? airBag : (float)Value;
			} else {
				return Value.ToFloat(airBag);
			}
		}

		public double ToDouble()
		{
			return ToDouble(default(double));
		}

		public double ToDouble(double airBag)
		{
			if(Format == DataFormat.Double) {
				return null == Value ? airBag : (double)Value;
			} else {
				return Value.ToDouble(airBag);
			}
		}

		public decimal ToDecimal()
		{
			return ToDecimal(default(decimal));
		}

		public decimal ToDecimal(decimal airBag)
		{
			if(Format == DataFormat.Decimal) {
				return null == Value ? airBag : (decimal)Value;
			} else {
				return Value.ToDecimal(airBag);
			}
		}

		public Color ToColor()
		{
			return ToColor(default(Color));
		}

		public Color ToColor(Color airBag)
		{
			if(Format == DataFormat.Color) {
				return null == Value ? airBag : (Color)Value;
			} else {
				return Value.ToColor(airBag);
			}
		}

		public Guid ToGuid()
		{
			return ToGuid(default(Guid));
		}

		public Guid ToGuid(Guid airBag)
		{
			if(Format == DataFormat.Guid) {
				return null == Value ? airBag : (Guid)Value;
			} else {
				return Value.ToGuid(airBag);
			}
		}

		public byte[] ToByteArray()
		{
			return ToByteArray(new byte[0]);
		}

		public byte[] ToByteArray(byte[] airBag)
		{
			if(Format == DataFormat.ByteArray) {
				return null == Value 
					? airBag 
					: (byte[])Value;
			} else {
				try {
					return null == Value
						? airBag
						: (byte[])Convert.ChangeType(
							Value, typeof(byte[])
						);
				} catch(Exception ex) {
					LogRecord
						.Create()
						.Add(ex)
						.Error();
					return airBag;
				}
			}
		}

		public T ToEnum<T>()
		{
			return ToEnum<T>(default(T));
		}

		public T ToEnum<T>(T airBag)
		{
			var type = typeof(T);

			if(Format == DataFormat.Enum) {
				return (T)Enum.Parse(type, Value.ToString(), true);
			} else {
				if(type.IsEnum) {
					var ef = EnumCache.Get(type);
					var ei = ef.Get(ToString());

					return null == ei
						? ef.DefaultValue.ToEnum<T>(airBag)
						: ei.ToEnum<T>();
				}
				return airBag;
			}
		}

		public TimeSpan ToTimeSpan()
		{
			return ToTimeSpan(default(TimeSpan));
		}

		public TimeSpan ToTimeSpan(TimeSpan airBag)
		{
			if(Format == DataFormat.TimeSpan) {
				return null == Value ? airBag : (TimeSpan)Value;
			} else {
				return Value.ToTimeSpan(airBag);
			}
		}
		#endregion

		#region Concat
		public static List<Any> Concat(params Any[] anys)
		{
			List<Any> list = new List<Any>();
			if(!anys.IsNullOrEmpty()) {
				foreach(var any in anys) {
					list.SafeAdd(any);
				}
			}
			return list;
		}
		#endregion

		#region override
		public override bool Equals(object obj)
		{
			if(null == obj) { return false; }
			Any any = obj as Any;
			if(null == any) { return false; }
			return this.Name.Equals(
				any.Name, StringComparison.OrdinalIgnoreCase
			);
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}
		#endregion

		public Many ToMany(string group)
		{
			Many many = new Many(
				group, 
				Name, 
				Value
			);
			return many;
		}
	}
}
