// SqlClientBuilder.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Kuicker.Data
{
	/// <summary>
	/// DiffEntity
	/// </summary>
	[Description("DiffEntity")]
	[DataContract, Table]
	public class DiffEntity : Entity<DiffEntity>
	{
		#region constructor
		public DiffEntity()
			: base()
		{
		}
		#endregion

		#region property
		[Description("Difference ID")]
		[DataMember, Column, PrimaryKey]
		public string DiffId { get; set; }

		[Description("Title")]
		[DataMember, Column]
		public string Title { get; set; }

		[Description("Who")]
		[DataMember, Column]
		public string Who { get; set; }

		[Description("Entity Name")]
		[DataMember, Column]
		public string EntityName { get; set; }

		[Description("All primary key's value join by '__x__'")]
		[DataMember, Column]
		public string KeyValue { get; set; }

		[Description("Method")]
		[DataMember, Column]
		public DiffMethod Method { get; set; }

		[Description("Difference Json")]
		[DataMember, Column]
		public string Json { get; set; }

		[Description("Created Date")]
		[DataMember, Column]
		public DateTime CreatedDate { get; set; }
		#endregion

		#region IEntity
		public override List<Index> __Indexes
		{
			get
			{
				var list = new List<Index>();
				list.Add(new Index<DiffEntity>()
					.AddColumn(x => x.Who)
					.AddColumn(x => x.EntityName)
					.AddColumn(x => x.KeyValue)
				);
				return list;
			}
		}

		public override void OnInit()
		{
			DiffId = Guid.NewGuid().ToString();
			CreatedDate = DateTime.Now;
			base.OnInit();
		}
		#endregion

		#region static
		#endregion

		#region instance
		#endregion

		#region private
		#endregion

		#region event handler
		#endregion
	}
}
