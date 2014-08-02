// RunTime.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Web;
using System.Web.Hosting;

namespace Kuicker
{
	public sealed class RunTime
	{
		private static object _Lock = new object();

		public static string CalleeFullName()
		{
			return CalleeFullName(2);
		}

		public static string CalleeFullName(int deep)
		{
			deep++;
			var stackTrace = new StackTrace();
			var stackFrame = stackTrace.GetFrame(deep);
			var method = stackFrame.GetMethod();
			var type = method.ReflectedType;
			var assemblyName = type.Assembly.GetName().Name;
			var className = type.Name;
			var methodName = method.Name;

			return string.Format(
				"{0}.{1}.{2}", 
				assemblyName, 
				className, 
				methodName
			);
		}

		#region property
		public static string UserDomainName
		{
			get
			{
				return Environment.UserDomainName;
			}
		}

		public static string ComputerName
		{
			get
			{
				return Environment.MachineName;
			}
		}

		public static string UserName
		{
			get
			{
				return Environment.UserName;
			}
		}

		public static string OperatingSystemBits
		{
			get
			{
				return Environment.Is64BitOperatingSystem ? "x64" : "x86";
			}
		}

		public static string ProcessorBits
		{
			get
			{
				return Environment.Is64BitProcess ? "x64" : "x86";
			}
		}

		public static string OSVersion
		{
			get
			{
				return Environment.OSVersion.VersionString;
			}
		}

		public static bool IsWebApp
		{
			get
			{
				if(null == HttpContext.Current) { return false; }
				return HostingEnvironment.IsHosted;
			}
		}

		public static bool ReadyForRequest
		{
			get
			{
				if(!IsWebApp) { return false; }
				try {
					return null != HttpContext.Current.Request;
				} catch(HttpException he) {
					LogRecord.Create().Add(he).Error();
				} catch(Exception ex) {
					LogRecord.Create().Add(ex).Error();
				}
				return false;
			}
		}

		private static string _BinFolder;
		public static string BinFolder
		{
			get
			{
				if(null == _BinFolder) {
					lock(_Lock) {
						if(null == _BinFolder) {
							_BinFolder = IsWebApp
								? Path.Combine(
									AppDomain.CurrentDomain.BaseDirectory,
									Constants.Folder.Bin
								)
								: AppDomain.CurrentDomain.BaseDirectory;
						}
					}
				}
				return _BinFolder;
			}
		}
		#endregion

		private static Int64 _NextIndex = 0;
		public static Int64 NextIndex()
		{
			lock(_Lock) {
				return _NextIndex++;
			}
		}

		public static string ToPhysicalPath(string path)
		{
			string physicalPath = path;
			try {
				if(!Path.IsPathRooted(path)) {
					if(IsWebApp) {
						physicalPath = HostingEnvironment.MapPath(path);
					} else {
						physicalPath = Path.Combine(
							AppDomain.CurrentDomain.BaseDirectory,
							path
						);
					}
				}
				return physicalPath;
			} catch(Exception ex) {
				LogRecord
					.Create()
					.Add(ex)
					.Add("path", path)
					.Add("IsWebApp", IsWebApp)
					.Error();
				throw;
			}
		}

		public static string CreateFolder(string path)
		{
			var physicalPath = ToPhysicalPath(path);

			try {
				if(!Directory.Exists(physicalPath)) {
					var info = Directory.CreateDirectory(physicalPath);
					if(null == info) {
						throw new Exception(String.Format(
							"Create folder failure ({0}).", physicalPath
						));
					}
				}
				return physicalPath;
			} catch(Exception ex) {
				LogRecord
					.Create()
					.Add(ex)
					.Add("path", path)
					.Error();
				throw;
			}
		}

		private static string _ServerIP;
		public static string ServerIP
		{
			get
			{
				if(null == _ServerIP) {
					if(ReadyForRequest) {
						_ServerIP = HttpContext
							.Current
							.Request
							.ServerVariables["LOCAL_ADDR"];
					} else {
						if(_ServerIP.IsNullOrEmpty()) {
							if(!NetworkInterface.GetIsNetworkAvailable()) {
								return Constants.Ip.LoopBack;
							}

							var host = Dns.GetHostEntry(
								Dns.GetHostName()
							);
							foreach(var ip in host.AddressList) {
								if(
									ip.AddressFamily
									==
									AddressFamily.InterNetwork) {
									_ServerIP = ip.ToString();
									break;
								}
							}
						}
					}
				}

				return _ServerIP;
			}
		}

		public static string GetClientIP()
		{
			if(ReadyForRequest) {
				var ip = "?";
				var x = HttpContext
					.Current
					.Request
					.ServerVariables["HTTP_X_FORWARDED_FOR"];
				if(string.IsNullOrEmpty(x)) {
					ip = HttpContext
						.Current
						.Request
						.ServerVariables["REMOTE_ADDR"];
				} else {
					if(x.Contains(",")) {
						var ips = ip.Split(',');
						ip = ips[0];
					} else {
						ip = x;
					}
				}
				//if(ip == "::1") { ip = Constants.LocalIp; } // IPv6
				return ip;
			} else {
				return ServerIP;
			}
		}
	}
}
