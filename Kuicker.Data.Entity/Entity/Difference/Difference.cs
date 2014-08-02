// Difference.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace Kuicker.Data
{
	public class Difference
	{
		public Difference()
		{
			this.Values = new List<DiffValue>();
		}

		public string Who { get; set; }
		public string EntityName { get; set; }
		public string PrimaryValue { get; set; }
		public DiffMethod Method { get; set; }
		public List<DiffValue> Values { get; set; }

		private IEntity _Schema;
		[XmlIgnore]
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

		public string Title
		{
			get
			{
				return string.Format(
					"{0} {1}", EntityName, Method.ToStringX()
				);
			}
		}


		public static Action<DiffMethod, IEntity, IEntity> Handler
		{ get; set; }

		public static Func<string, string, List<Difference>> History
		{ get; set; }
	}
}