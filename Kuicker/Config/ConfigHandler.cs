// ConfigHandler.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Xml;
using Newtonsoft.Json;

namespace Kuicker
{
	// <Kuicker>
	//     <Kernel>
	//         <Add Name="" Value="">
	//     </Kernel>
	//     <Builtin>
	//         <Add Group="{BuiltinName}" Name="" Value="">
	//     </Builtin>
	//     <Plugin>
	//         <Add Group="{PluginName}" Name="" Value="">
	//     </Plugin>
	//     <Application>
	//         <Add Group="" Name="" Value="">
	//     </Application>
	// </Kuicker>
	public sealed class ConfigHandler : IConfigurationSectionHandler
	{
		private static object _Lock = new object();
		private static Config _Config;

		public object Create(
			object parent, object configContext, XmlNode section)
		{
			if(null == _Config) {
				lock(_Lock) {
					if(null == _Config) {
						_Config = new Config();
						if(section.IsNullOrEmpty()) {
							return _Config;
						}

						foreach(XmlNode node in section.ChildNodes) {
							if(node.IsNullOrEmpty()) { continue; }

							switch(node.Name.ToPascalCasing()) {
								case Config.Xml.Kernel:
									_Config.KernelSection =
										ParsedAsAnys(node);
									break;
								case Config.Xml.Builtin:
									_Config.BuiltinSection =
										ParsedAsManys(node);
									break;
								case Config.Xml.Plugin:
									_Config.PluginSection =
										ParsedAsManys(node);
									break;
								case Config.Xml.Application:
									_Config.ApplicationSection =
										ParsedAsManys(node);
									break;
							}
						}
					}
				}
			}

			return _Config;
		}

		public List<Any> ParsedAsAnys(XmlNode node)
		{
			var list = new List<Any>();

			foreach(XmlNode child in node.ChildNodes) {
				string name = child.Attribute("Name");
				string value = child.Attribute("Value");
				list.SafeAdd(name, value);
			}

			return list;
		}

		public List<Many> ParsedAsManys(XmlNode node)
		{
			var list = new List<Many>();

			foreach(XmlNode child in node.ChildNodes) {
				string group = child.Attribute("Group");
				string name = child.Attribute("Name");
				string value = child.Attribute("Value");
				list.SafeAdd(group, name, value);
			}

			return list;
		}
	}
}
