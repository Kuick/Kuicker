// Column.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;

namespace Kuicker.Data
{
	[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
	public sealed class Column : Attribute
	{
		public Column()
			: this(string.Empty)
		{
		}
		public Column(string columnname)
		{
			this.ColumnName = columnname;
		}

		public string ColumnName { get; set; }

		public PropertyInfo Property { get; internal set; }
		public string TableName { get; internal set; }
		public string EntityName { get; internal set; }

		public bool AllowDBNull { get; internal set; }
		public bool Identity { get; internal set; }
		public bool PrimaryKey { get; internal set; }
		public bool ReadOnly { get; internal set; }
		public bool Unique { get; internal set; }
		public int MaxLength { get; internal set; }

		public string Category { get; internal set; }
		public string Description { get; internal set; }
		public object DefaultValue { get; internal set; }
		public int Order { get; internal set; }
		public bool IgnoreDifference { get; internal set; }



		public DbType DbType { get; internal set; }



		private Table _Table;
		public Table Table
		{
			get
			{
				if(null == _Table) {
					_Table = EntityCache.GetTable(EntityName);
				}
				return _Table;
			}
		}

		private IEntity _Schema;
		public IEntity Schema
		{
			get
			{
				if(null == _Schema) {
					_Schema = EntityCache.Get(EntityName);
				}
				return _Schema;
			}
		}

		private int _Index;
		public int Index
		{
			get
			{
				if(_Index < 0) {
					_Index = Table.GetColumnIndex(ColumnName);
				}
				return _Index;
			}
		}

		private string _Alias;
		public string Alias
		{
			get
			{
				if(_Alias.IsNullOrEmpty()) {
					_Alias = Table.GetColumnAlias(ColumnName);
				}
				return _Alias;
			}
		}

		private bool _GotFormat = false;
		private DataFormat _Format;
		public DataFormat Format
		{
			get
			{
				if(!_GotFormat) {
					_Format = Property.PropertyType.GetFormat();
					_GotFormat = true;
				}
				return _Format;
			}
		}


		private string _PropertyName;
		public string PropertyName
		{
			get
			{
				if(null == _PropertyName) {
					_PropertyName = Property.Name;
				}
				return _PropertyName;
			}
		}


		private string _DbFullName;
		public string DbFullName
		{
			get
			{
				if(null == _DbFullName) {
					_DbFullName = string.Format(
						"{0}.{1}",
						TableName,
						ColumnName
					);
				}
				return _DbFullName;
			}
		}


		private string _EntityFullName;
		public string EntityFullName
		{
			get
			{
				if(null == _EntityFullName) {
					_EntityFullName = string.Format(
						"{0}.{1}", 
						EntityName, 
						Property.Name
					);
				}
				return _EntityFullName;
			}
		}





		public bool IsNumber
		{
			get
			{
				return Format.In(
					DataFormat.Boolean,
					DataFormat.Byte,
					DataFormat.Decimal,
					DataFormat.Double,
					DataFormat.Float,
					DataFormat.Integer,
					DataFormat.Long,
					DataFormat.Short
				);
			}
		}



		public bool IsDateTime
		{
			get
			{
				return Format == DataFormat.DateTime;
			}
		}



		#region Evaluate
		public static Column Evaluate<T>(
			Expression<Func<T, object>> exp)
			where T : class, IEntity<T>, new()
		{
			return Evaluate<T>(exp.Body);
		}

		public static Column Evaluate<T>(Expression exp)
			where T : class, IEntity<T>, new()
		{
			if(null == exp) { return null; }

			MemberInfo mInfo = null;
			MemberExpression mExp = null;
			MethodCallExpression mcallExp = null;
			UnaryExpression uExp = null;

			switch(exp.NodeType) {
				case ExpressionType.Convert:
					uExp = exp as UnaryExpression;
					mExp = uExp.Operand as MemberExpression;
					mInfo = mExp.Member;
					break;

				case ExpressionType.Call:
					mcallExp = exp as MethodCallExpression;
					mInfo = mcallExp.Method;
					break;

				case ExpressionType.MemberAccess:
					mExp = exp as MemberExpression;
					mInfo = mExp.Member;
					break;

				default:
					throw new NotImplementedException(
						LogRecord
							.Create()
							.SetMessage(
								"Unhandled Expression NodeType: ",
								exp.NodeType.ToStringX()
							)
							.Error()
							.Message
					);
			}

			T schema = EntityCache.Get<T>();
			var info = mInfo as PropertyInfo;
			var column = schema.GetColumn(mInfo.Name);

			return column;
		}
		#endregion
	}
}
