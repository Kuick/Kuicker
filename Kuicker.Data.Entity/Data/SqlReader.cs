// SqlReader.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.Linq;

namespace Kuicker.Data
{
	internal class SqlReader : IDisposable
	{
		#region constructor
		internal SqlReader(DbDataReader reader)
		{
			this.Reader = reader;
		}
		#endregion

		#region Reader
		internal DbDataReader Reader { get; set; }

		internal bool Read()
		{
			return Reader.Read();
		}

		public void Close()
		{
			Reader.Close();
		}

		public int GetOrdinal(string name)
		{
			try {
				int order;
				if(FieldOrders.TryGetValue(name.ToUpper(), out order)) {
					return order;
				}
				return -1;
			} catch {
				return -1;
			}
		}

		public object this[string columnName]
		{
			get
			{
				int index = GetOrdinal(columnName);
				return this[index];
			}
		}

		public object this[int index]
		{
			get
			{
				if(index < 0 || null == Reader) { return null; }
				return Reader[index];
			}
		}
		#endregion

		#region IDisposable
		public void Dispose()
		{
			Reader.Dispose();
		}
		#endregion

		// Current
		private IEntity CurrentSchema { get; set; }
		private Column CurrentColumn { get; set; }

		// Bind
		internal void Bind(IntervalLogger il, IEntity instance)
		{
			this.CurrentSchema = instance;

			foreach(Column column in instance.__Columns) {
				this.CurrentColumn = column;

				string asName = Contains(column.ColumnName)
					? column.ColumnName
					: Contains(column.Alias)
						? column.Alias
						: null;
				if(null == asName) { continue; }

				try {
					object val = null;

					switch(column.Format) {
						case DataFormat.String:
							val = this.GetString(asName);
							break;
						case DataFormat.DateTime:
							val = this.GetDateTime(asName);
							break;
						case DataFormat.Boolean:
							val = this.GetInteger(asName) == 1
								? true
								: false;
							break;
						case DataFormat.Char:
							val = this.GetString(asName);
							break;
						case DataFormat.Byte:
							val = this.GetByte(asName);
							break;
						case DataFormat.Short:
							val = this.GetShort(asName);
							break;
						case DataFormat.Integer:
							val = this.GetInteger(asName);
							break;
						case DataFormat.Long:
							val = this.GetLong(asName);
							break;
						case DataFormat.Float:
							val = this.GetFloat(asName);
							break;
						case DataFormat.Double:
							val = this.GetDouble(asName);
							break;
						case DataFormat.Decimal:
							val = this.GetDecimal(asName);
							break;
						case DataFormat.ByteArray:
							val = this.GetBytes(asName);
							break;
						case DataFormat.Color:
							int color = this.GetInteger(asName);
							val = Color.FromArgb(color);
							break;
						case DataFormat.Guid:
							val = this.GetGuid(asName);
							break;
						case DataFormat.Enum:
							string original = this.GetString(asName);
							if(original.IsNullOrEmpty()) {
								original = string.Empty;
							}

							var enumRef = EnumCache.Get(
								column.Property.PropertyType
							);
							var enumItem = enumRef.Get(original);

							if(
								!original.IsNullOrEmpty() 
								&& 
								null == enumItem) {
								LogRecord
									.Create()
									.SetMessage(
										"Dirty enum value alert!"
									)
									.AddRange(Currents)
									.Add("Dirty Value", original)
									.Add(
										"Possible Values", 
										enumRef
											.Items
											.Join(x => x.Name, ", ")
									)
									.Error();
							}

							var enumVal = null == enumItem
								? enumRef.DefaultValue
								: enumItem.Name;
							val = Enum.Parse(
								column.Property.PropertyType,
								enumVal, 
								true
							);
							break;
						case DataFormat.TimeSpan:
							val = this.GetValue(asName).ToTimeSpan();
							break;

						default:
							throw new NotImplementedException(
								LogRecord
									.Create()
									.SetMessage(
										"Unhandled Property Type: ",
										column.Property.PropertyType.Name,
										" at ",
										column.EntityFullName
									)
									.Error()
									.Message
							);
					}
					column.Property.SetValue(
						instance, val, new Object[0]
					);

				} catch(Exception ex) {
					WriteErrorMessage("Bind", ex);
				}
			}
		}

		#region Get
		// GetString
		private string GetString(string columnName)
		{
			return GetString(GetOrdinal(columnName));
		}
		private string GetString(int index)
		{
			if(IsDBNull(index)) { return default(string); }
			try {
				return Reader.GetString(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetString", ex);
				return default(string);
			}
		}

		// GetDateTime
		private DateTime GetDateTime(string columnName)
		{
			return GetDateTime(GetOrdinal(columnName));
		}
		private DateTime GetDateTime(int index)
		{
			if(IsDBNull(index)) { return default(DateTime); }
			try {
				return Reader.GetDateTime(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetDateTime", ex);
				return default(DateTime);
			}
		}

		// GetInteger
		private int GetInteger(string columnName)
		{
			return GetInteger(GetOrdinal(columnName));
		}
		private int GetInteger(int index)
		{
			if(IsDBNull(index)) { return default(int); }
			try {
				int v = default(int);
				object obj = Reader.GetValue(index);
				if(null == obj) { obj = default(int); }
				if(!int.TryParse(obj.ToString(), out v)) {
					string s = obj.ToString();
					v = s.In("True", "T", "Yes", "Y", "1")
						? 1 : 0;
				}
				return v;
			} catch(Exception ex) {
				WriteErrorMessage("GetInteger", ex);
				return default(int);
			}
		}

		// GetByte
		private byte GetByte(string columnName)
		{
			return GetByte(GetOrdinal(columnName));
		}
		private byte GetByte(int index)
		{
			if(IsDBNull(index)) { return default(byte); }
			try {
				return Reader.GetByte(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetByte", ex);
				return default(byte);
			}
		}

		// GetShort
		private short GetShort(string columnName)
		{
			return GetShort(GetOrdinal(columnName));
		}
		private short GetShort(int index)
		{
			if(IsDBNull(index)) { return default(short); }
			try {
				return Reader.GetInt16(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetShort", ex);
				return default(short);
			}
		}

		// GetLong
		private long GetLong(string columnName)
		{
			return GetLong(GetOrdinal(columnName));
		}
		private long GetLong(int index)
		{
			if(IsDBNull(index)) { return default(long); }
			try {
				return Reader.GetInt64(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetLong", ex);
				return default(long);
			}
		}

		// GetFloat
		private float GetFloat(string columnName)
		{
			return GetFloat(GetOrdinal(columnName));
		}
		private float GetFloat(int index)
		{
			if(IsDBNull(index)) { return default(float); }
			try {
				return Reader.GetFloat(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetFloat", ex);
				return default(float);
			}
		}

		// GetDouble
		private double GetDouble(string columnName)
		{
			return GetDouble(GetOrdinal(columnName));
		}
		private double GetDouble(int index)
		{
			if(IsDBNull(index)) { return default(double); }
			try {
				return Reader.GetDouble(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetDouble", ex);
				return default(double);
			}
		}

		// GetDecimal
		private decimal GetDecimal(string columnName)
		{
			return GetDecimal(GetOrdinal(columnName));
		}
		private decimal GetDecimal(int index)
		{
			if(IsDBNull(index)) { return default(decimal); }
			try {
				return Reader.GetDecimal(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetDecimal", ex);
				return default(decimal);
			}
		}

		// GetBytes
		private byte[] GetBytes(string columnName)
		{
			return GetBytes(GetOrdinal(columnName));
		}
		private byte[] GetBytes(int index)
		{
			if(IsDBNull(index)) { return default(byte[]); }
			try {
				return Reader.GetStream(index).ToBytes();
			} catch(Exception ex) {
				WriteErrorMessage("GetBytes", ex);
				return default(byte[]);
			}
		}

		// GetGuid
		private Guid GetGuid(string columnName)
		{
			return GetGuid(GetOrdinal(columnName));
		}
		private Guid GetGuid(int index)
		{
			if(IsDBNull(index)) { return default(Guid); }
			try {
				return Reader.GetGuid(index);
			} catch(Exception ex) {
				WriteErrorMessage("GetGuid", ex);
				return default(Guid);
			}
		}
		#endregion

		#region utility
		// IsDBNull
		private bool IsDBNull(string columnName)
		{
			return IsDBNull(GetOrdinal(columnName));
		}
		private bool IsDBNull(int index)
		{
			if(index < 0) { return true; }
			try {
				return Reader.IsDBNull(index);
			} catch(Exception ex) {
				WriteErrorMessage("IsDBNull", ex);
				return true;
			}
		}

		// Fields
		private Dictionary<string, int> _FieldOrders;
		private Dictionary<string, int> FieldOrders
		{
			get
			{
				if(null == _FieldOrders) {
					_FieldOrders = new Dictionary<string, int>();
					for(int i = 0; i < Reader.FieldCount; i++) {
						string name = Reader.GetName(i).ToUpper();
						int order = Reader.GetOrdinal(name);
						_FieldOrders.Add(name, order);
					}
				}
				return _FieldOrders;
			}
		}

		// Contains
		private bool Contains(string fieldName)
		{
			return FieldOrders.ContainsKey(fieldName.ToUpper());
		}

		// WriteErrorMessage
		private void WriteErrorMessage(
			string methodName, Exception ex)
		{
			LogRecord
				.Create()
				.SetMessage(
					this.GetType().FullName,
					":",
					methodName,
					"()"
				)
				.AddRange(Currents)
				.Add(ex)
				.Error();
		}

		private List<Any> Currents
		{
			get
			{
				var list = new List<Any>();
				list.Add("Table Name", CurrentSchema.__Table.TableName);
				list.Add("Column Name", CurrentColumn.ColumnName);
				list.Add("Entity Name", CurrentSchema.__Table.EntityName);
				list.Add("Property Name", CurrentColumn.Property.Name);
				list.Add("Property Type", CurrentColumn.Property.PropertyType);
				return list;
			}
		}
		#endregion
	}
}
