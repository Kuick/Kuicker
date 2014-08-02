// EnumRef.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Net.Mime;
using System.Linq;

namespace Kuicker
{
	public class EnumRef
	{
		public EnumRef()
		{
			this.Items = new List<EnumItem>();
		}

		public Type Type { get; internal set; }
		public string FullName { get; internal set; }
		public string Description { get; internal set; }
		public string Category { get; internal set; }
		public string DefaultValue { get; internal set; }
		public List<EnumItem> Items { get; internal set; }

		
		private EnumItem _Default;
		public EnumItem Default
		{
			get{
				if(null == _Default) {
					_Default = Get(DefaultValue);
					if(null == _Default) {
						if(Items.IsNullOrEmpty()) {
							_Default = null;
						} else {
							_Default = Items[0];
						}
					}
				}
				return _Default;
			}
		}

		public EnumItem Get(string valueOrDescription)
		{
			if(valueOrDescription.IsNullOrEmpty()) {
				return null;
			}
			valueOrDescription = valueOrDescription.Trim();
			EnumItem item = Items.FirstOrDefault(x => 
				x.Name.Equals(
					valueOrDescription, 
					StringComparison.OrdinalIgnoreCase
				)
			);
			if(null == item) {
				item = Items.FirstOrDefault(x =>
					x.Description.Equals(
						valueOrDescription,
						StringComparison.OrdinalIgnoreCase
					)
				);
			}

			return item;
		}

		private List<Any> _Anys;
		public List<Any> Anys
		{
			get
			{
				if(null == _Anys) {
					_Anys = new List<Any>();
					foreach(var ei in Items) {
						_Anys.Add(ei.ToAny());
					}
				}
				return _Anys;
			}
		}
	}
}
