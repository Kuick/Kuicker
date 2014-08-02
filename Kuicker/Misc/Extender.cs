// Extender.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Drawing;
using System.Collections.Specialized;
using System.Xml.Linq;
using System.Security;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.Data;
using System.Reflection;
using System.ComponentModel;
using System.Runtime.Serialization.Formatters.Binary;
using System.Web;

namespace Kuicker
{
	public static class Extender
	{
		private static object _Lock = new object();

		#region ArrayList
		public static bool IsNullOrEmpty(this ArrayList arrayList)
		{
			return null == arrayList || arrayList.Count == 0;
		}
		#endregion

		#region char
		public static bool IsUpper(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsUpper(c);
		}
		public static bool IsLower(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsLower(c);
		}
		public static bool IsDigit(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsDigit(c);
		}
		public static bool IsLetter(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsLetter(c);
		}
		public static bool IsLetterOrDigit(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsLetterOrDigit(c);
		}
		public static bool IsNumber(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsNumber(c);
		}
		public static bool IsWhiteSpace(this char c)
		{
			if(char.MinValue == c) { return false; }
			return char.IsWhiteSpace(c);
		}
		public static bool IsUnderScore(this char c)
		{
			if(char.MinValue == c) { return false; }
			return c == Symbol.Char.UnderScore;
		}
		#endregion

		#region char[]
		public static string[] ToStringArray(this char[] cs)
		{
			var list = new List<string>();
			foreach(var c in cs) {
				list.Add(c.ToString());
			}
			return list.ToArray();
		}
		#endregion

		#region string
		public static bool IsNullOrEmpty(this string str)
		{
			return null == str || string.IsNullOrEmpty(str);
		}

		public static bool IsNullOrWhiteSpace(this string txt)
		{
			return string.IsNullOrWhiteSpace(txt);
		}

		public static string Left(this string input, int length)
		{
			if(input.IsNullOrEmpty()) { return string.Empty; }
			string left = input.Substring(
				0,
				Math.Min(length, input.Length)
			);
			return left;
		}

		public static string Right(this string input, int length)
		{
			if(input.IsNullOrEmpty()) { return string.Empty; }
			if(length >= input.Length) { return input; }
			return input.Substring(input.Length - length, length);
		}

		public static string[] Split(
			this string input, string pattern, RegexOptions options)
		{
			if(input.IsNullOrEmpty()) { return new string[0]; }
			var parts = Regex.Split(input, pattern, options);
			List<string> list = new List<string>();
			foreach(string x in parts) {
				list.Add(x);
			}
			return list.ToArray();
		}

		public static string[] Split(
			this string input, params string[] symbols)
		{
			if(input.IsNullOrEmpty()) { return new string[0]; }
			var parts = input.Split(
				symbols,
				StringSplitOptions.None
			);
			List<string> list = new List<string>();
			foreach(string x in parts) {
				list.Add(x);
			}
			return list.ToArray();
		}

		public static string[] SplitAndTrim(
			this string input, string pattern, RegexOptions options)
		{
			if(input.IsNullOrEmpty()) { return new string[0]; }
			var parts = Regex.Split(input, pattern, options);
			List<string> list = new List<string>();
			foreach(string x in parts) {
				list.Add(x.Trim());
			}
			return list.ToArray();
		}

		public static string[] SplitAndTrim(
			this string input, params string[] symbols)
		{
			if(input.IsNullOrEmpty()) { return new string[0]; }
			var parts = input.Split(
				symbols,
				StringSplitOptions.None
			);
			List<string> list = new List<string>();
			foreach(string x in parts) {
				list.Add(x.Trim());
			}
			return list.ToArray();
		}

		public static byte[] ToBytes(this string input)
		{
			return input.ToBytes(null);
		}

		public static byte[] ToBytes(this string input, Encoding encoding)
		{
			if(string.IsNullOrEmpty(input)) { return new byte[0]; }
			if(null == encoding) { encoding = Encoding.UTF8; }

			if(encoding == Encoding.ASCII) {
				return UnicodeEncoding.ASCII.GetBytes(input);
			} else if(encoding == Encoding.Default) {
				return UnicodeEncoding.Default.GetBytes(input);
			} else if(encoding == Encoding.Unicode) {
				return UnicodeEncoding.Unicode.GetBytes(input);
			} else if(encoding == Encoding.UTF32) {
				return UnicodeEncoding.UTF32.GetBytes(input);
			} else if(encoding == Encoding.UTF7) {
				return UnicodeEncoding.UTF7.GetBytes(input);
			} else if(encoding == Encoding.UTF8) {
				return UnicodeEncoding.UTF8.GetBytes(input);
			} else {
				return UnicodeEncoding.UTF8.GetBytes(input);
			}
		}

		public static Stream ToStream(this string input)
		{
			if(String.IsNullOrEmpty(input)) { return null; }
			var bytes = ToBytes(input);
			return ToStream(bytes);
		}

		public static string ToBase64(this string input)
		{
			if(input.IsNullOrEmpty()) { return string.Empty; }
			var bytes = ToBytes(input);
			return Convert.ToBase64String(bytes);
		}

		public static string FromBase64(this string input)
		{
			if(input.IsNullOrEmpty()) { return string.Empty; }
			var bytes = Convert.FromBase64String(input);
			return ToString(bytes);
		}

		public static string Repeat(this string seed, int times)
		{
			StringBuilder sb = new StringBuilder();
			for(int i = 0; i < times; i++) {
				sb.Append(seed);
			}
			return sb.ToString();
		}

		public static string ToMD5(this string str)
		{
			return str.Encode("MD5");
		}

		public static string ToSHA(this string str)
		{
			return str.Encode("SHA512");
		}
		public static string Encode(this string str, string encodeBy)
		{
			if(str.IsNullOrEmpty()) { return string.Empty; }
			if(encodeBy.IsNullOrEmpty()) { return string.Empty; }
			bool correct = encodeBy.In(
				Constants.Encode.MD5,
				Constants.Encode.SHA512
			);
			if(!correct) {
				throw new ArgumentException(
					"Encode method must be MD5 or SHA512",
					"encodeBy"
				);
			}
			var buffer = str.ToBytes(Encoding.Unicode);
			var hash = HashAlgorithm
				.Create(encodeBy)
				.ComputeHash(buffer);
			string rtn = Convert.ToBase64String(hash);
			return rtn;
		}

		public static byte[] HexToBytes(this string hex)
		{
			if(null == hex) {
				return null;
			} else if((hex.Length % 2) != 0) {
				throw new Exception(
					"Hex string length must be a multiple of 2."
				);
			}

			int len = hex.Length / 2;
			var result = new Byte[len];
			string h = hex.ToUpper();

			for(int i = 0; i < len; i++) {
				char c = h[2 * i];
				int index = Constants.HexCode.IndexOf(c);
				if(index == -1) {
					throw new Exception(
						"Hex string can't contain '" + c + "'"
					);
				}

				int j = 16 * index;
				c = h[(2 * i) + 1];
				index = Constants.HexCode.IndexOf(c);
				if(index == -1) {
					throw new Exception(
						"Hex string can't contain '" + c + "'"
					);
				}
				j += index;
				result[i] = (byte)(j & 0xFF);
			}

			return result;
		}



		public static string AirBag(this string value)
		{
			return AirBag(value, string.Empty);
		}

		public static string AirBag(this string value, string airBag)
		{
			if(value.IsNullOrEmpty()) {
				if(airBag.IsNullOrEmpty()) {
					return string.Empty;
				} else {
					return airBag;
				}
			} else {
				return value;
			}
		}

		public static DateTime ToDateTime(this string value)
		{
			return value.ToDateTime(default(DateTime));
		}

		public static DateTime ToDateTime(
			this string value, DateTime airBag)
		{
			DateTime val;
			return DateTime.TryParse(value, out val) ? val : airBag;
		}

		public static bool ToBoolean(this string value)
		{
			return value.ToBoolean(default(bool));
		}

		public static bool ToBoolean(this string value, bool airBag)
		{
			return value.IsNullOrEmpty()
				? airBag
				: value.ToLower().In(
					"1", "t", "y", "true", "yes", "on"
				);
		}

		public static char ToChar(this string value)
		{
			return value.ToChar(default(char));
		}

		public static char ToChar(
			this string value, char airBag)
		{
			char val;
			return char.TryParse(value, out val) ? val : airBag;
		}

		public static byte ToByte(this string value)
		{
			return value.ToByte(default(byte));
		}

		public static byte ToByte(
			this string value, byte airBag)
		{
			byte val;
			return byte.TryParse(value, out val) ? val : airBag;
		}

		public static short ToShort(this string value)
		{
			return value.ToShort(default(short));
		}

		public static short ToShort(
			this string value, short airBag)
		{
			short val;
			return short.TryParse(value, out val) ? val : airBag;
		}

		public static int ToInt(this string value)
		{
			return value.ToInt(default(int));
		}

		public static int ToInt(this string value, int airBag)
		{
			int val;
			return int.TryParse(value, out val) ? val : airBag;
		}

		public static long ToLong(this string value)
		{
			return value.ToLong(default(long));
		}

		public static long ToLong(this string value, long airBag)
		{
			long val;
			return long.TryParse(value, out val) ? val : airBag;
		}

		public static float ToFloat(this string value)
		{
			return value.ToFloat(default(float));
		}

		public static float ToFloat(this string value, float airBag)
		{
			float val;
			return float.TryParse(value, out val) ? val : airBag;
		}

		public static double ToDouble(
			this string value)
		{
			return value.ToDouble(default(double));
		}

		public static double ToDouble(
			this string value, double airBag)
		{
			double val;
			return double.TryParse(value, out val) ? val : airBag;
		}

		public static decimal ToDecimal(this string value)
		{
			return value.ToDecimal(default(decimal));
		}

		public static decimal ToDecimal(
			this string value, decimal airBag)
		{
			decimal val;
			return decimal.TryParse(value, out val) ? val : airBag;
		}

		public static T ToEnum<T>(this string value)
		{
			return value.ToEnum<T>(default(T));
		}

		public static T ToEnum<T>(this string value, T airBag)
		{
			if(!typeof(T).IsEnum) { return airBag; }

			try {
				return (T)Enum.Parse(typeof(T), value, true);
			} catch {
				return airBag;
			}
		}


		public static Color ToColor(this string value)
		{
			return value.ToColor(Color.White);
		}

		public static Color ToColor(this string value, Color airBag)
		{
			if(value.IsNullOrEmpty()) { return airBag; }

			try {
				// int
				if(value.IsInteger()) {
					int argb = value.ToInteger();
					Color color = Color.FromArgb(argb);
					return color;
				}

				// Html
				if(Regex.IsMatch(value, @"^#(?:[0-9a-fA-F]{3}){1,2}$")) {
					Color color = ColorTranslator.FromHtml(value);
					return color;
				}

				// RGB
				if(value.StartsX("RGB(") && value.StartsX(")")) {
					string[] rgb = Regex.Split(value, @"\d+");
					if(rgb.Length != 3) { return airBag; }
					Color color = Color.FromArgb(
						rgb[0].ToInt(),
						rgb[1].ToInt(),
						rgb[2].ToInt()
					);
					return color;
				}
			} catch(Exception ex) {
				LogRecord
					.Create()
					.Add(ex)
					.Error();
			}

			return airBag;
		}


		public static Guid ToGuid(this string value)
		{
			return value.ToGuid(new Guid());
		}

		public static Guid ToGuid(this string value, Guid airBag)
		{
			Guid val;
			return Guid.TryParse(value, out val) ? val : airBag;
		}

		public static string ToValidFileName(this string fileName)
		{
			var sb = new StringBuilder(fileName);
			foreach(char c in Path.GetInvalidFileNameChars()) {
				sb.Replace(c, '_');
			}
			return sb.ToString();
		}

		public static string ToPopularCodeName(this string codeName)
		{
			var sb = new StringBuilder();
			foreach(char c in codeName.ToCharArray()) {
				sb.Append(c.In(Symbol.Char.All) ? "_" : c.ToString());
			}
			return sb.ToString();
		}


		public static bool In(
			this string value, params string[] values)
		{
			return value.In(
				StringComparison.OrdinalIgnoreCase, values
			);
		}

		public static bool In(
			this string value,
			StringComparison comparison,
			params string[] values)
		{
			if(values.IsNullOrEmpty()) { return false; }

			foreach(string x in values) {
				if(null == x && null == value) { return true; }
				if(null == x || null == value) { return false; }
				if(value.Trim().Equals(x.Trim(), comparison)) {
					return true;
				}
			}
			return false;
		}


		public static bool EqualsX(
			this string value, string match)
		{
			return value.Equals(
				match, StringComparison.OrdinalIgnoreCase
			);
		}

		public static bool StartsX(
			this string value, params string[] values)
		{
			return value.StartsX(
				StringComparison.OrdinalIgnoreCase, values
			);
		}

		public static bool StartsX(
			this string value,
			StringComparison comparison,
			params string[] values)
		{
			if(values.IsNullOrEmpty()) { return false; }

			foreach(string x in values) {
				if(null == x && null == value) { return true; }
				if(null == x || null == value) { return false; }
				if(value.StartsWith(x.Trim(), comparison)) {
					return true;
				}
			}
			return false;
		}

		public static bool EndsX(
			this string value, params string[] values)
		{
			return value.EndsX(
				StringComparison.OrdinalIgnoreCase, values
			);
		}

		public static bool EndsX(
			this string value,
			StringComparison comparison,
			params string[] values)
		{
			if(values.IsNullOrEmpty()) { return false; }

			foreach(string x in values) {
				if(null == x && null == value) { return true; }
				if(null == x || null == value) { return false; }
				if(value.EndsWith(x.Trim(), comparison)) {
					return true;
				}
			}
			return false;
		}

		public static bool IsMatch(this string value, string pattern)
		{
			return value.IsMatch(pattern, false);
		}

		public static bool IsMatch(
			this string value, string pattern, bool ignoreCase)
		{
			return Regex.IsMatch(
				value,
				pattern,
				ignoreCase
					? RegexOptions.Compiled | RegexOptions.IgnoreCase
					: RegexOptions.Compiled
			);
		}

		public static string TrimStart(
			this string value,
			params string[] trimStrings)
		{
			return value.TrimStart(
				StringComparison.OrdinalIgnoreCase, trimStrings
			);
		}

		public static string TrimStart(
			this string value,
			StringComparison comparison,
			params string[] trimStrings)
		{
			if(value.IsNullOrEmpty()) { return value; }
			foreach(string trimString in trimStrings) {
				if(trimString.Length > value.Length) { continue; }
				if(value.StartsX(comparison, trimString)) {
					return value.Substring(
						trimString.Length,
						value.Length - trimString.Length
					);
				}
			}
			return value;
		}

		public static string TrimEnd(
			this string value,
			params string[] trimStrings)
		{
			return value.TrimEnd(
				StringComparison.OrdinalIgnoreCase, trimStrings
			);
		}

		public static string TrimEnd(
			this string value,
			StringComparison comparison,
			params string[] trimStrings)
		{
			if(value.IsNullOrEmpty()) { return value; }
			foreach(string trimString in trimStrings) {
				if(trimString.Length > value.Length) { continue; }
				if(value.EndsX(comparison, trimString)) {
					return value.Substring(
						0,
						value.Length - trimString.Length
					);
				}
			}
			return value;
		}

		public static string AppendPrefix(
			this string value,
			string prefix)
		{
			return value.AppendPrefix(
				StringComparison.OrdinalIgnoreCase, prefix
			);
		}

		public static string AppendPrefix(
			this string value,
			StringComparison comparison,
			string prefix)
		{
			if(value.IsNullOrEmpty()) { return value; }
			if(prefix.IsNullOrEmpty()) { return value; }

			return value.StartsX(comparison, prefix)
				? value
				: prefix + value;
		}

		public static string AppendSuffix(
			this string value,
			string suffix)
		{
			return value.AppendSuffix(
				StringComparison.OrdinalIgnoreCase, suffix
			);
		}

		public static string AppendSuffix(
			this string value,
			StringComparison comparison,
			string suffix)
		{
			if(value.IsNullOrEmpty()) { return value; }
			if(suffix.IsNullOrEmpty()) { return value; }

			return value.EndsX(comparison, suffix)
				? value
				: value + suffix;
		}

		public static string AppendLineFirst(
			this string value,
			string first)
		{
			if(value.IsNullOrEmpty()) { return first; }

			return value
				.SplitAndTrim(Environment.NewLine)
				.Join(
					Environment.NewLine,
					first,
					""
				);
		}

		public static string AppendLineLast(
			this string value,
			string last)
		{
			if(value.IsNullOrEmpty()) { return last; }

			return value
				.SplitAndTrim(Environment.NewLine)
				.Join(
					Environment.NewLine,
					"",
					last
				);
		}

		public static string AppendLine(
			this string value,
			string first,
			string last)
		{
			if(value.IsNullOrEmpty()) { return first + last; }

			return value
				.SplitAndTrim(Environment.NewLine)
				.Join(
					Environment.NewLine,
					first,
					last
				);
		}

		public static string ToUnicodeEntity(this string source)
		{
			var sb = new StringBuilder();
			foreach(char c in source) {
				var bytes = Encoding
					.Unicode
					.GetBytes(c.ToString());
				sb.Append(Constants.Unicode.Prefix);
				for(int i = bytes.Length - 1; i >= 0; i--) {
					sb.Append(
						Convert
							.ToString(bytes[i], 16)
							.PadLeft(2, '0')
					);
				}
				sb.Append(Constants.Unicode.Suffix);
			}
			return sb.ToString();
		}

		public static string[] ToWords(this string txt)
		{
			var words = new List<string>();
			if(txt.IsNullOrEmpty()) { return words.ToArray(); }

			char pre = default(char);
			char now = default(char);
			string word = string.Empty;
			foreach(var one in txt) {
				now = one;

				if(now.IsUnderScore()) {
					words.Add(word);
					word = string.Empty;
				} else if(now.IsUpper()) {
					if(pre.IsLower()) {
						words.Add(word);
						word = string.Empty;
					}
					word += now;
				} else if(now.IsLower()) {
					word += now;
				} else if(now.IsNumber()) {
					if(!pre.IsNumber()) {
						words.Add(word);
						word = string.Empty;
					}
					word += now;
				} else {
					word += now;
					//throw new ArgumentException(
					//	new[]{
					//		"Argument can only be ",
					//		"upper or lower case letters ",
					//		"or number or underscore."
					//	}.Join(),
					//	"txt"
					//);
				}

				pre = now;
			}
			words.Add(word);

			var list = new List<string>();
			foreach(var one in words) {
				if(one.IsNullOrEmpty() || one.IsNullOrWhiteSpace()) {
					continue;
				}
				list.Add(one);
			}

			return list.ToArray();
		}

		public static bool AllUpper(this string txt)
		{
			if(txt.IsNullOrEmpty()) { return false; }

			foreach(var one in txt) {
				if(!one.IsLetter()) { continue; }
				if(!one.IsUpper()) { return false; }
			}

			return true;
		}

		public static bool AllLower(this string txt)
		{
			if(txt.IsNullOrEmpty()) { return false; }

			foreach(var one in txt) {
				if(!one.IsLetter()) { continue; }
				if(!one.IsLower()) { return false; }
			}

			return true;
		}

		public static string Remove(this string txt, params string[] words)
		{
			if(txt.IsNullOrEmpty()) { return string.Empty; }
			if(words.IsNullOrEmpty()) { return txt; }
			foreach(var word in words) {
				txt = txt.Replace(word, "");
			}
			return txt;
		}



		public static string ToUpperCasing(this string txt)
		{
			var words = txt.ToWords();
			return words.Join(x => x.ToUpper(), "_");
		}

		public static string ToLowerCasing(this string txt)
		{
			var words = txt.ToWords();
			return words.Join(x => x.ToLower(), "_");
		}

		public static string ToCompactUpperCasing(this string txt)
		{
			if(txt.IsNullOrEmpty()) { return string.Empty; }
			return txt.ToUpper().Remove("_");
		}

		public static string ToCompactLowerCasing(this string txt)
		{
			if(txt.IsNullOrEmpty()) { return string.Empty; }
			return txt.ToLower().Remove("_");
		}

		public static string ToPascalCasing(this string txt)
		{
			var words = txt.ToWords();
			return words.Join(x => {
				if(x.Length == 1) { return x.ToUpper(); }
				if(x.AllUpper()) {
					return
						x.Left(1).ToUpper() +
						x.Substring(1).ToLower();
				} else {
					return
						x.Left(1).ToUpper() +
						x.Substring(1);
				}
			});
		}

		public static string ToCamelCasing(this string txt)
		{
			var words = txt.ToWords();
			if(words.IsNullOrEmpty()) {
				return txt;
			}

			string first = words[0];
			var others = words.Length > 1
				? words.Reverse().Take(words.Length - 1).Reverse()
				: new string[0];
			string part1 = first.ToLower();
			string part2 = others.Join().ToPascalCasing();
			return part1 + part2;
		}



		public static bool IsDateTime(this string value)
		{
			DateTime result;
			return DateTime.TryParse(value, out result);
		}

		public static bool IsBoolean(this string value)
		{
			bool result;
			return bool.TryParse(value, out result);
		}

		public static bool IsChar(this string value)
		{
			char result;
			return char.TryParse(value, out result);
		}

		public static bool IsByte(this string value)
		{
			byte result;
			return byte.TryParse(value, out result);
		}

		public static bool IsShort(this string value)
		{
			short result;
			return short.TryParse(value, out result);
		}
		public static bool IsInteger(this string value)
		{
			int result;
			return int.TryParse(value, out result);
		}

		public static bool IsLong(this string value)
		{
			long result;
			return long.TryParse(value, out result);
		}

		public static bool IsFloat(this string value)
		{
			float result;
			return float.TryParse(value, out result);
		}

		public static bool IsDouble(this string value)
		{
			double result;
			return double.TryParse(value, out result);
		}

		public static bool IsDecimal(this string value)
		{
			decimal result;
			return decimal.TryParse(value, out result);
		}



		public static bool IsTaiwanIdNumber(this string value)
		{
			if(!Regex.IsMatch(
				value,
				@"^[A-Z][12]\d{8}$",
				RegexOptions.IgnoreCase | RegexOptions.Compiled)) {
				return false;
			}

			char[] id = value.ToUpper().ToCharArray();
			int c;
			var magic = new Dictionary<char, int>() 
			{
				{'A',  1}, {'B', 10}, {'C', 19}, {'D', 28}, 
				{'E', 37}, {'F', 46}, {'G', 55}, {'H', 64}, 
				{'I', 39}, {'J', 73}, {'K', 82}, {'L',  2},
				{'M', 11}, {'N', 20}, {'O', 48}, {'P', 29}, 
				{'Q', 38}, {'R', 47}, {'S', 56}, {'T', 65}, 
				{'U', 74}, {'V', 83}, {'W', 21}, {'X',  3},
				{'Y', 12}, {'Z', 30}
			};
			if(!magic.TryGetValue(id[0], out c)) { return false; }

			int[] b = { 1, 8, 7, 6, 5, 4, 3, 2, 1 };
			for(var i = b.Length - 1; i > 0; i--) {
				c += int.Parse(id[i].ToString()) * b[i];
			}

			return (10 - c % 10) % 10 == int.Parse(id[9].ToString());
		}

		public static bool IsEmail(this string value)
		{
			var regex = new Regex(
				Constants.Pattern.Email, RegexOptions.Compiled
			);
			return regex.IsMatch(value);
		}

		public static bool IsIp(this string value)
		{
			return Regex.IsMatch(
				value, Constants.Pattern.Ip, RegexOptions.Compiled
			);
		}

		public static bool IsLoopBackIp(this string value)
		{
			if(!IsIp(value)) { return false; }
			return value.StartsWith("127.");
		}

		public static long IpToLong(this string ip)
		{
			// Checker
			if(!ip.IsIp()) { return -1; }

			// Convert
			IPAddress ipAddress;
			if(!IPAddress.TryParse(ip, out ipAddress)) { return -1; }

			string[] ips = ip.Split(Symbol.Char.Period);
			if(ips.Length != 4) { return -1; }

			return
				(long.Parse(ips[0]) << 24) +
				(long.Parse(ips[1]) << 16) +
				(long.Parse(ips[2]) << 8) +
				(long.Parse(ips[3]));
		}

		public static bool IsPrivateIp(this string value)
		{
			return IsIp(value)
				? IsPrivateIp(value.IpToLong())
				: false;
		}

		public static bool IsNullIp(this string value)
		{
			return "0.0.0.0" == value || !IsIp(value);
		}

		public static bool IsAlphabet(this string value)
		{
			var regex = new Regex(
				Constants.Pattern.Alphabet, RegexOptions.Compiled
			);
			return regex.IsMatch(value);
		}

		public static bool IsNumeric(this string value)
		{
			var regex = new Regex(
				Constants.Pattern.Numeric, RegexOptions.Compiled
			);
			return regex.IsMatch(value);
		}

		public static bool IsAlphaNumeric(this string value)
		{
			var regex = new Regex(
				Constants.Pattern.AlphaNumeric, RegexOptions.Compiled
			);
			return regex.IsMatch(value);
		}

		public static bool IsUri(this string value)
		{
			var regex = new Regex(
				Constants.Pattern.Uri, RegexOptions.Compiled
			);
			return regex.IsMatch(value);
		}

		public static bool Like(this string value, string like)
		{
			if(value.IsNullOrEmpty()) { return false; }
			if(like.IsNullOrEmpty()) { return true; }

			return value.IndexOf(like) > -1;
		}

		public static HtmlString ToHtml(this string value)
		{
			if(value.IsNullOrEmpty()) { return new HtmlString(string.Empty); }
			return new HtmlString(value);
		}
		#endregion

		#region string[]
		public static bool IsNullOrEmpty(this string[] cols)
		{
			if(null == cols) { return true; }
			foreach(string x in cols) {
				if(
					!string.IsNullOrEmpty(x) &&
					!string.IsNullOrWhiteSpace(x)) {
					return false;
				}
			}
			return true;
		}

		public static string Join(this string[] cols)
		{
			return cols.Join(string.Empty);
		}

		public static string Join(this string[] cols, string separator)
		{
			return cols.Join(separator, string.Empty);
		}

		public static string Join(
			this string[] cols,
			string separator,
			string quoter)
		{
			return cols.Join(separator, quoter, quoter);
		}

		public static string Join(
			this string[] cols,
			string separator,
			string quoter0,
			string quoter9)
		{
			return cols.Join(null, separator, quoter0, quoter9);
		}

		public static string Join(
			this string[] cols,
			Func<string, string> transfer)
		{
			return cols.Join(transfer, string.Empty);
		}

		public static string Join(
			this string[] cols,
			Func<string, string> transfer,
			string separator)
		{
			return cols.Join(transfer, separator, string.Empty);
		}

		public static string Join(
			this string[] cols,
			Func<string, string> transfer,
			string separator,
			string quoter)
		{
			return cols.Join(transfer, separator, quoter, quoter);
		}

		public static string Join(
			this string[] cols,
			Func<string, string> transfer,
			string separator,
			string quoter0,
			string quoter9)
		{
			if(cols.IsNullOrEmpty()) { return string.Empty; }
			if(separator.IsNullOrEmpty()) { separator = string.Empty; }
			if(quoter0.IsNullOrEmpty()) { quoter0 = string.Empty; }
			if(quoter9.IsNullOrEmpty()) { quoter9 = string.Empty; }

			var sb = new StringBuilder();
			foreach(string x in cols) {
				sb.AppendFormat(
					"{0}{1}{2}",
					quoter0,
					null == transfer ? x : transfer(x),
					quoter9
				);
				sb.Append(separator);
			}

			return sb.ToString().TrimEnd(separator.ToCharArray());
		}
		#endregion

		#region byte[]
		public static bool IsNullOrEmpty(this byte[] input)
		{
			return null == input || input.Length == 0;
		}

		public static string ToString(this byte[] input)
		{
			if(input.IsNullOrEmpty()) { return string.Empty; }
			var sb = new StringBuilder();
			for(int i = 0; i < input.Length; i++) {
				sb.Append(input[i].ToString("x2"));
			}
			return sb.ToString();
		}

		public static Stream ToStream(this byte[] input)
		{
			if(input.IsNullOrEmpty()) { return null; }
			return new MemoryStream(input);
		}

		public static string ToHex(this byte[] b)
		{
			var sb = new StringBuilder();

			for(int i = 0; i < b.Length; i++) {
				int j = ((int)b[i]) & 0xFF;

				char first = Constants.HexCode[j / 16];
				char second = Constants.HexCode[j % 16];

				sb.Append(first);
				sb.Append(second);
			}

			return sb.ToString();
		}
		#endregion

		#region Stream
		public static bool IsNullOrEmpty(this Stream input)
		{
			return null == input || input.Length == 0;
		}

		public static string ToString(this Stream input)
		{
			if(null == input) { return null; } //?: or string.Empty
			if(input.Length == 0) { return string.Empty; }
			var bs = input.ToBytes();
			string str = bs.ToString();
			return str;
		}

		public static byte[] ToBytes(this Stream input)
		{
			if(null == input) { return new byte[0]; }
			var bytes = new byte[input.Length];
			input.Read(bytes, 0, bytes.Length);
			input.Seek(0, SeekOrigin.Begin);
			return bytes;
		}
		#endregion

		#region DateTime
		public static bool IsNullOrEmpty(this DateTime dateTime)
		{
			return dateTime == DateTime.MinValue;
		}


		public static string yyyy(this DateTime date)
		{
			return date.ToString("yyyy");
		}

		public static string yyyy_MM(this DateTime date)
		{
			return date.ToString("yyyy-MM");
		}

		public static string yyyyMM(this DateTime date)
		{
			return date.ToString("yyyyMM");
		}

		public static string yyyyMMdd(this DateTime date)
		{
			return date.ToString("yyyyMMdd");
		}

		public static string yyyy_MM_dd(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd");
		}

		public static string yyyyMMddHHmmss(this DateTime date)
		{
			return date.ToString("yyyyMMddHHmmss");
		}

		public static string yyyy_MM_dd_HH_mm_ss(this DateTime date)
		{
			return date.ToString("yyyy-MM-dd HH:mm:ss");
		}

		public static string yyyyMMddHHmmssfff(this DateTime date)
		{
			return date.ToString("yyyyMMddHHmmssfff");
		}

		public static string yyyy_MM_dd_HH_mm_ss_fff(
			this DateTime date)
		{
			return date.ToString("yyyy-MM-dd HH:mm:ss.fff");
		}

		public static string MM(this DateTime date)
		{
			return date.ToString("MM");
		}

		public static string dd(this DateTime date)
		{
			return date.ToString("dd");
		}

		public static string hh(this DateTime date)
		{
			return date.ToString("hh");
		}

		public static string mm(this DateTime date)
		{
			return date.ToString("mm");
		}

		public static string ss(this DateTime date)
		{
			return date.ToString("ss");
		}

		public static string hhmm(this DateTime date)
		{
			return date.ToString("hhmm");
		}

		public static string hh_mm(this DateTime date)
		{
			return date.ToString("hh:mm");
		}

		public static string hhmmss(this DateTime date)
		{
			return date.ToString("hhmmss");
		}

		public static string hh_mm_ss(this DateTime date)
		{
			return date.ToString("hh:mm:ss");
		}

		public static DateTime AirBag(this DateTime dateTime)
		{
			return dateTime.AirBag(DateTime.MinValue);
		}

		public static DateTime AirBag(
			this DateTime dateTime, DateTime airBag)
		{
			return dateTime.IsNullOrEmpty() ? airBag : dateTime;
		}

		public static DateTime StartOfYear(this DateTime value)
		{
			return new DateTime(value.Year, 1, 1, 0, 0, 0, 0);
		}

		public static DateTime StartOfMonth(this DateTime value)
		{
			return new DateTime(value.Year, value.Month, 1, 0, 0, 0, 0);
		}

		public static DateTime StartOfWeek(this DateTime value)
		{
			return value.StartOfWeek(DayOfWeek.Sunday);
		}

		public static DateTime StartOfWeek(
			this DateTime value, DayOfWeek weekStart)
		{
			var date = new DateTime(
				value.Year, value.Month, value.Day, 0, 0, 0
			);
			int difference = (int)weekStart - (int)date.DayOfWeek;
			if(difference > 0) { difference -= 6; }
			return date.AddDays(difference);
		}

		public static DateTime StartOfDay(this DateTime value)
		{
			return new DateTime(
				value.Year, value.Month, value.Day, 0, 0, 0, 0
			);
		}

		public static DateTime StartOfHour(this DateTime value)
		{
			return new DateTime(
				value.Year, value.Month, value.Day, value.Hour, 0, 0
			);
		}

		public static DateTime StartOfMinute(this DateTime value)
		{
			return new DateTime(
				value.Year,
				value.Month,
				value.Day,
				value.Hour,
				value.Minute,
				0
			);
		}

		public static DateTime EndOfYear(this DateTime value)
		{
			return new DateTime(
				value.Year,
				12,
				DateTime.DaysInMonth(value.Year, 12),
				23,
				59,
				59,
				999
			);
		}

		public static DateTime EndOfMonth(this DateTime value)
		{
			return new DateTime(
				value.Year,
				value.Month,
				DateTime.DaysInMonth(value.Year, value.Month),
				23,
				59,
				59,
				999
			);
		}

		public static DateTime EndOfWeek(this DateTime value)
		{
			return value.EndOfWeek(DayOfWeek.Sunday);
		}

		public static DateTime EndOfWeek(
			this DateTime value, DayOfWeek weekStart)
		{
			var date = new DateTime(
				value.Year, value.Month, value.Day, 23, 59, 59
			);
			int difference = (int)weekStart - (int)date.DayOfWeek;
			if(difference > 0) { difference -= 6; }
			return date.AddDays(difference + 6);
		}

		public static DateTime EndOfDay(this DateTime value)
		{
			return new DateTime(
				value.Year, value.Month, value.Day, 23, 59, 59, 999
			);
		}

		public static DateTime EndOfHour(this DateTime value)
		{
			return new DateTime(
				value.Year,
				value.Month,
				value.Day,
				value.Hour,
				59,
				59,
				999
			);
		}

		public static DateTime EndOfMinute(this DateTime value)
		{
			return new DateTime(
				value.Year,
				value.Month,
				value.Day,
				value.Hour,
				value.Minute,
				59,
				999
			);
		}

		public static double ToUnixTimestamp(this DateTime value)
		{
			return Math.Floor(
				(value - new DateTime(1970, 1, 1, 0, 0, 0))
				.TotalSeconds
			);
		}
		#endregion

		#region int
		public static int AirBag(this int value)
		{
			return value.AirBag(default(int));
		}

		public static int AirBag(this int value, int airBag)
		{
			return (value == -1) ? airBag : value;
		}

		public static string Order(this int no, int max)
		{
			return no.Order(max, string.Empty);
		}

		public static string Order(
			this int no, int max, string suffix)
		{
			int miniLength = (int)Math.Ceiling(Math.Log10(max));
			return no.OrderList(miniLength, suffix);
		}

		public static string OrderList(this int no, int miniLength)
		{
			return no.OrderList(miniLength, ".");
		}

		public static string OrderList(
			this int no, int miniLength, string suffix)
		{
			string s = no.ToString();
			int len = s.Length;
			string pattern = string.Concat("{0}{1}", suffix);
			return string.Format(
				pattern,
				len >= miniLength
					? string.Empty
					: "0".Repeat(miniLength - len),
				s
			);
		}

		public static bool IsEven(this int value)
		{
			return !value.IsOdd();
		}

		public static bool IsOdd(this int value)
		{
			return value >= 0 && ((value & 1) == 1);
		}

		public static int Ceiling(this int value, int criticalValue)
		{
			return value > criticalValue ? criticalValue : value;
		}

		public static int Floor(this int value, int criticalValue)
		{
			return value < criticalValue ? criticalValue : value;
		}

		public static int Within(
			this int value, int ceilingValue, int floorValue)
		{
			// correct
			int max = Math.Max(ceilingValue, floorValue);
			int min = Math.Min(ceilingValue, floorValue);

			return value > max
				? max
				: value < min
					? min
					: value;
		}

		public static bool Divisible(this int dividend, int divisor)
		{
			return divisor >= 0 && dividend >= 0 && ((dividend % divisor) == 0);
		}

		public static int Remainder(this int dividend, int divisor)
		{
			if(divisor >= 0 && dividend >= 0) {
				return dividend % divisor;
			} else {
				return -1;
			}
		}

		public static int Quotient(this int dividend, int divisor)
		{
			return (dividend - Remainder(dividend, divisor)) / divisor;
		}
		#endregion

		#region long
		public static long AirBag(this long value)
		{
			return (value == -1) ? 0 : value;
		}

		public static long AirBag(this long value, long airBag)
		{
			return (value == -1) ? airBag : value;
		}

		public static bool IsEven(this long value)
		{
			return value >= 0 && ((value & 1) == 0);
		}

		public static bool IsOdd(this long value)
		{
			return value >= 0 && ((value & 1) == 1);
		}

		public static bool Divisible(this long dividend, long divisor)
		{
			return divisor >= 0 && dividend >= 0 && ((dividend % divisor) == 0);
		}

		public static long Remainder(this long dividend, long divisor)
		{
			if(divisor >= 0 && dividend >= 0) {
				return dividend % divisor;
			} else {
				return -1;
			}
		}

		public static long Quotient(this long dividend, long divisor)
		{
			return (dividend - Remainder(dividend, divisor)) / divisor;
		}

		public static bool IsIp(this long ip)
		{
			return ip.Between(Constants.Ip.MinAsLong, Constants.Ip.MaxAsLong);
		}

		public static bool IsPrivateIp(this long ip)
		{
			if(!ip.IsIp()) {
				return false;
			}

			return
				ip.Between( // Class A
					Constants.Ip.Private.ClassA.StartAsLong,
					Constants.Ip.Private.ClassA.EndAsLong
				)
				||
				ip.Between( // Class B
					Constants.Ip.Private.ClassB.StartAsLong,
					Constants.Ip.Private.ClassB.EndAsLong
				)
				||
				ip.Between( // Class C
					Constants.Ip.Private.ClassC.StartAsLong,
					Constants.Ip.Private.ClassC.EndAsLong
				)
				||
				ip.Between( // Loopback
					Constants.Ip.Private.Loopback.StartAsLong,
					Constants.Ip.Private.Loopback.EndAsLong
				);
		}
		#endregion

		#region float
		public static float AirBag(this float value)
		{
			return (value == -1) ? 0 : value;
		}

		public static float AirBag(this float value, float airBag)
		{
			return (value == -1) ? airBag : value;
		}
		#endregion

		#region bool
		public static string If(this bool isTrue, string text)
		{
			return isTrue ? text : string.Empty;
		}
		#endregion

		#region ArrayList
		//public static ArrayList Sort(this ArrayList al, string propertyName)
		//{
		//	if(al.IsNullOrEmpty()) { return new ArrayList(); }

		//	int times = 10000;
		//	SortedList sort = new SortedList();
		//	foreach(object x in al) {
		//		times++;
		//		object key = Reflector.GetValue(propertyName, x) ?? x;
		//		key = key.ToString() + times.ToString();
		//		sort.Add(key, x);
		//	}

		//	var list = new ArrayList();
		//	for(int i = 0; i < sort.Count; i++) {
		//		object s = sort.GetByIndex(i);
		//		list.Add(s);
		//	}
		//	return list;
		//}
		#endregion

		#region Hashtable
		public static bool IsNullOrEmpty(this Hashtable ht)
		{
			return null == ht || ht.Count == 0;
		}

		public static Hashtable AirBag(this Hashtable ht)
		{
			return ht.IsNullOrEmpty() ? new Hashtable() : ht;
		}
		#endregion

		#region NameValueCollection
		public static bool IsNullOrEmpty(
			this NameValueCollection nvc)
		{
			return null == nvc || nvc.Count == 0;
		}

		public static bool Exists(
			this NameValueCollection nvc, params string[] names)
		{
			if(nvc.IsNullOrEmpty()) { return false; }
			foreach(string key in nvc.AllKeys) {
				foreach(string name in names) {
					if(name.IsNullOrEmpty()) { continue; }
					if(key.IsNullOrEmpty()) { continue; }
					if(key.Equals(
						name, StringComparison.OrdinalIgnoreCase)) {
						return true;
					}
				}
			}
			return false;
		}

		public static bool SafeAdd(
			this NameValueCollection nvc, string name, string value)
		{
			if(null == nvc) { return false; }
			lock(_Lock) {
				if(nvc.Exists(name)) {
					nvc.Remove(name);
				}
				nvc.Add(name, value);
				return true;
			}
		}

		public static bool SafeRemove(
			this NameValueCollection nvc, string name)
		{
			if(null == nvc) { return false; }
			lock(_Lock) {
				if(nvc.Exists(name)) {
					nvc.Remove(name);
				}
				return true;
			}
		}

		public static List<Any> ToAnys(this NameValueCollection nvc)
		{
			var anys = new List<Any>();
			if(nvc.IsNullOrEmpty()) { return anys; }
			foreach(string key in nvc.AllKeys) {
				anys.Add(key, nvc.Get(key));
			}
			return anys;
		}
		#endregion

		#region List<T>
		public static bool IsNullOrEmpty(this List<string> list)
		{
			return null == list || list.Count == 0;
		}

		public static List<T> Distinct<T>(this List<T> list)
		{
			var distincted = Enumerable.Distinct<T>(list);
			var n = new List<T>(distincted);
			return n;
		}

		public static List<O> ConvertAll<T, O>(this List<T> list)
			where T : O
		{
			return list.ConvertAll<O>(x => (O)x);
		}

		public static bool Contains<T>(
			this List<T> list,
			Predicate<T> predicate)
		{
			foreach(T one in list) {
				if(predicate(one)) { return true; }
			}
			return false;
		}

		public static List<T> AddRange<T>(
			this List<T> list,
			bool condition,
			params T[] values)
		{
			if(!condition) { return list; }
			list.AddRange(values);
			return list;
		}

		public static List<Any> ToAnys<T>(
			this List<T> objs,
			Func<T, string> nameFunc,
			Func<T, object> valueFunc)
		{
			List<Any> list = new List<Any>();
			if(objs.IsNullOrEmpty()) { return list; }
			return objs.ToArray().ToAnys(nameFunc, valueFunc);
		}
		#endregion

		#region double
		public static string MillisecondToSecond(this double millisecond)
		{
			return string.Format("{0:0.000}", millisecond / 1000f);
		}
		#endregion

		#region Type
		public static bool IsString(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(string));
		}

		public static bool IsDateTime(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(DateTime));
		}

		public static bool IsBoolean(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Boolean));
		}

		public static bool IsChar(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(char));
		}

		public static bool IsByte(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Byte));
		}

		public static bool IsShort(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Int16));
		}

		public static bool IsInteger(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Int32));
		}

		public static bool IsLong(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Int64));
		}

		public static bool IsFloat(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Single));
		}

		public static bool IsDouble(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Double));
		}

		public static bool IsDecimal(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Decimal));
		}

		public static bool IsBytes(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(byte[]));
		}



		public static bool IsEnum(this Type type)
		{
			if(null == type) { return false; }
			return type.IsEnum;
		}

		public static bool IsStream(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Stream));
		}

		public static bool IsGuid(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Guid));
		}

		public static bool IsColor(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(Color));
		}

		public static bool IsTimeSpan(this Type type)
		{
			if(null == type) { return false; }
			return type.Equals(typeof(TimeSpan));
		}



		public static bool IsDerived<B>(this Type type)
		{
			Type baseType = typeof(B);
			return type.IsDerived(baseType);
		}

		public static bool IsDerived(this Type type, Type baseType)
		{
			if(null == type) { return false; }
			if(null == baseType) { return false; }
			if(baseType.FullName == type.FullName) { return true; }

			if(type.IsClass) {
				return baseType.IsClass
					? type.IsSubclassOf(baseType)
					: baseType.IsInterface
						? type.IsImplemented(baseType)
						: false;
			} else if(type.IsInterface && baseType.IsInterface) {
				return type.IsImplemented(baseType);
			}
			return false;
		}

		public static bool IsSubclassOf<B>(this Type type)
		{
			Type baseType = typeof(B);
			return type.IsSubclassOf(baseType);
		}

		public static bool IsImplemented<B>(this Type type)
		{
			Type baseType = typeof(B);
			return type.IsImplemented(baseType);
		}

		public static bool IsImplemented(this Type type, Type baseType)
		{
			if(null == type) { return false; }
			if(null == baseType) { return false; }
			if(!baseType.IsInterface) { return false; }

			var faces = type.GetInterfaces();
			foreach(var x in faces) {
				if(baseType.Name.Equals(x.Name)) { return true; }
			}
			return false;
		}

		public static DataFormat GetFormat(this Type type)
		{
			if(null == type) { return DataFormat.Unknown; }

			if(type.IsArray) {
				if(type.GetElementType().IsByte()) {
					return DataFormat.ByteArray;
				} else {
					return DataFormat.Objects;
				}
			}

			if(type.IsString()) { return DataFormat.String; }
			if(type.IsDateTime()) { return DataFormat.DateTime; }
			if(type.IsBoolean()) { return DataFormat.Boolean; }
			if(type.IsChar()) { return DataFormat.Char; }
			if(type.IsByte()) { return DataFormat.Byte; }
			if(type.IsShort()) { return DataFormat.Short; }
			if(type.IsInteger()) { return DataFormat.Integer; }
			if(type.IsLong()) { return DataFormat.Long; }
			if(type.IsFloat()) { return DataFormat.Float; }
			if(type.IsDouble()) { return DataFormat.Double; }
			if(type.IsDecimal()) { return DataFormat.Decimal; }
			if(type.IsEnum) { return DataFormat.Enum; }
			if(type.IsColor()) { return DataFormat.Color; }
			if(type.IsGuid()) { return DataFormat.Guid; }

			return DataFormat.Object;
		}

		public static DbType GetDbType(this Type type)
		{
			if(null == type) { return DbType.String; }

			if(type.IsArray) {
				return DbType.Binary;
			} else if(type.IsString()) {
				return DbType.String;
			} else if(type.IsChar()) {
				return DbType.String;
			} else if(type.IsBoolean()) {
				return DbType.Boolean;
			} else if(type.IsDateTime()) {
				return DbType.DateTime2;
			} else if(type.IsInteger()) {
				return DbType.Int32;
			} else if(type.IsDecimal()) {
				return DbType.Decimal;
			} else if(type.IsDouble()) {
				return DbType.Double;
			} else if(type.IsByte()) {
				return DbType.Byte;
			} else if(type.IsShort()) {
				return DbType.Int16;
			} else if(type.IsLong()) {
				return DbType.Int64;
			} else if(type.IsFloat()) {
				return DbType.Single;
			} else if(type.IsEnum) {
				return DbType.String;
			} else if(type.IsColor()) {
				return DbType.UInt32;
			} else {
				return DbType.Object;
			}
		}

		public static bool HasAttribute<T>(this Type type)
			where T : Attribute
		{
			return null != type.GetAttribute<T>();
		}

		public static T GetAttribute<T>(this Type type)
			where T : Attribute
		{
			if(null == type) { return default(T); }

			List<T> list = new List<T>();
			foreach(object x in type.GetCustomAttributes(false)) {
				if(typeof(T) == x.GetType()) {
					T t = x as T;
					return t;
				}
			}
			return default(T);
		}

		public static T[] GetAttributes<T>(this Type type)
			where T : Attribute
		{
			if(null == type) { return new T[0]; }

			List<T> list = new List<T>();
			foreach(object x in type.GetCustomAttributes(false)) {
				if(typeof(T) == x.GetType()) {
					T t = x as T;
					list.Add(t);
				}
			}
			return list.ToArray();
		}

		public static string GetDescription(this Type type)
		{
			string description = string.Empty;
			if(null == type) { return description; }

			var attr = type.GetAttribute<DescriptionAttribute>();
			if(null != attr) { description = attr.Description; }

			return description;
		}

		public static string GetCategory(this Type type)
		{
			string category = string.Empty;
			if(null == type) { return category; }

			var attr = type.GetAttribute<CategoryAttribute>();
			if(null != attr) { category = attr.Category; }

			return category;
		}

		public static object GetDefaultValue(this Type type)
		{
			object defaultValue = null;
			if(null == type) { return defaultValue; }

			var attr = type.GetAttribute<DefaultValueAttribute>();
			if(null != attr) { defaultValue = attr.Value; }

			return defaultValue;
		}
		#endregion

		#region FiledInfo
		public static T GetAttribute<T>(this FieldInfo info)
		{
			if(null == info) { return default(T); }
			foreach(var x in info.GetCustomAttributes(false)) {
				if(x is T) { return (T)x; }
			}
			return default(T);
		}

		public static string GetDescription(this FieldInfo info)
		{
			string description = string.Empty;
			if(null == info) { return description; }

			var attr = info.GetAttribute<DescriptionAttribute>();
			if(null != attr) { description = attr.Description; }

			return description;
		}

		public static string GetCategory(this FieldInfo info)
		{
			string category = string.Empty;
			if(null == info) { return category; }

			var attr = info.GetAttribute<CategoryAttribute>();
			if(null != attr) { category = attr.Category; }

			return category;
		}

		public static object GetDefaultValue(this FieldInfo info)
		{
			object value = null;
			if(null == info) { return value; }

			var attr = info.GetAttribute<DefaultValueAttribute>();
			if(null != attr) { value = attr.Value; }

			return value;
		}
		#endregion

		#region Assembly
		public static List<Type> GatherByAttribute<T>(
			this Assembly assembly)
			where T : Attribute
		{
			if(null == assembly) { return new List<Type>(); }

			var types = new Type[0];
			try {
				types = assembly.GetTypes();
			} catch(Exception ex) {
				var anys = new List<Any>();
				anys.Add("AssemblyFullName", assembly.FullName);
				var e = ex as ReflectionTypeLoadException;
				if(null != e) {
					foreach(var eOne in e.LoaderExceptions) {
						anys.Add("LoadExceptions", eOne.Message);
					}
				}

				throw;
			}

			if(null == types || types.Length == 0) {
				return new List<Type>();
			}

			var oType = typeof(T);
			var list = new List<Type>();
			foreach(var x in types) {
				if(!x.IsClass || x.IsAbstract) { continue; }

				foreach(var attr in x.GetCustomAttributes(false)) {
					try {
						if(attr.GetType().Equals(oType)) {
							list.Add(x);
							break;
						}
					} catch(Exception ex) {
						LogRecord.Create().Add(ex).Error();
						throw;
					}
				}
			}
			return list;
		}

		public static List<Type> GatherByInterface<T>(
			this Assembly assembly)
			where T : class
		{
			if(null == assembly) { return new List<Type>(); }

			var types = new Type[0];
			try {
				types = assembly.GetTypes();
			} catch(Exception ex) {
				var anys = new List<Any>();
				anys.Add("AssemblyFullName", assembly.FullName);
				var e = ex as ReflectionTypeLoadException;
				if(null != e) {
					foreach(var x in e.LoaderExceptions) {
						anys.Add("LoadExceptions", x.Message);
					}
				}
				throw new Exception("...");
			}

			if(null == types || types.Length == 0) {
				return new List<Type>();
			}

			var interfaceName = typeof(T).Name;
			var list = new List<Type>();
			foreach(var x in types) {
				if(!x.IsClass || x.IsAbstract) { continue; }
				if(!x.IsDerived(typeof(T))) { continue; }
				list.Add(x);
			}
			return list;
		}
		#endregion

		#region T
		public static bool Between<T>(
			this T comparable, T comparableA, T comparableB)
			where T : IComparable
		{
			// two parameters
			// ----|XXXX|----

			if(null == comparable) { return false; }
			if(null == comparableA) { return false; }
			if(null == comparableB) { return false; }

			if(comparableA.CompareTo(comparableB) > 0) {
				return Between<T>(
					comparable, comparableA, comparableB
				);
			}

			return
				comparable.CompareTo(comparableA) >= 0
				&&
				comparable.CompareTo(comparableB) <= 0;
		}

		public static bool Besides<T>(
			this T comparable, T comparableA, T comparableB)
			where T : IComparable
		{
			// two parameters
			// XXXX|----|XXXX

			if(null == comparable) { return false; }
			if(null == comparableA) { return false; }
			if(null == comparableB) { return false; }

			if(comparableA.CompareTo(comparableB) > 0) {
				return Besides<T>(
					comparable, comparableA, comparableB
				);
			}

			return
				comparable.CompareTo(comparableA) < 0
				||
				comparable.CompareTo(comparableB) > 0;
		}

		public static bool Greater<T>(
			this T comparable, T comparableA)
			where T : IComparable
		{
			// one parameter
			// ----|XXXXXXXXX
			return comparable.CompareTo(comparableA) > 0;
		}

		public static bool Smaller<T>(
			this T comparable, T comparableA)
			where T : IComparable
		{
			// one parameter
			// XXXXXXXXX|----
			return comparable.CompareTo(comparableA) < 0;
		}

		public static bool In<T>(this T value, params T[] values)
		{
			if(null == value) { return false; }
			if(values.IsNullOrEmpty()) { return false; }
			foreach(T x in values) {
				if(value.Equals(x)) { return true; }
			}
			return false;
		}

		public static string ToJson<T>(this T obj)
		{
			return JsonConvert.SerializeObject(obj);
		}

		public static List<Any> ToAnys<T>(
			this T obj)
		{
			return obj.ToAnys(true);
		}

		public static List<Any> ToAnys<T>(
			this T obj, bool writable)
		{
			var list = new List<Any>();
			if(null == obj) { return list; }
			if(obj.IsDerived<IEnumerable>()) { return list; }

			var props = obj.GetType().GetProperties();
			if(props.IsNullOrEmpty()) { return list; }

			foreach(var x in props) {
				if(!x.CanRead) { continue; }
				if(writable && !x.CanWrite) { continue; }
				object val = obj.GetValue(x.Name);
				list.Add(x.Name, val);
			}

			return list;
		}

		public static List<Many> ToManys<T>(
			this T obj)
		{
			return obj.ToManys(true);
		}

		public static List<Many> ToManys<T>(
			this T obj, bool writable)
		{
			var anys = obj.ToAnys(writable);
			var manys = anys.ToManys(obj.GetType().Name);
			return manys;
		}

		public static void NullToEmptyAndTrim<T>(this T obj)
		{
			if(null == obj) { return; }
			Type type = typeof(T);
			if(Reflector.SupportedBaseType(type)) { return; }
			Reflector.NullToEmptyAndTrim(type, obj, null, null);
		}


		public static T ShallowCopy<T>(this T obj)
			where T : class
		{
			return (T)Reflector.Invoke<T>("MemberwiseClone", obj);
		}

		public static T DeepCopy<T>(this T obj)
		{
			try {
				MemoryStream m = new MemoryStream();
				BinaryFormatter b = new BinaryFormatter();
				b.Serialize(m, obj);
				m.Position = 0;
				return (T)b.Deserialize(m);
			} catch(Exception ex) {
				LogRecord
					.Create()
					.SetMessage("DeepCopy fails switch to ForceCopy")
					.Add("Type", typeof(T).FullName)
					.Add(ex)
					.Error();
				return obj.ForceCopy();
			}
		}

		public static T ForceCopy<T>(this T obj)
		{
			return Reflector.ForceClone<T>(obj);
		}
		#endregion

		#region T[]
		public static bool IsNullOrEmpty<T>(this T[] objs)
		{
			if(null == objs || objs.Length == 0) { return true; }

			foreach(var one in objs) {
				if(null != one) { return false; }
			}
			return true;
		}

		public static ArrayList ToArrayList<T>(this T[] cols)
		{
			if(null == cols) { return new ArrayList(); }
			return new ArrayList(cols);
		}

		public static List<T> ToList<T>(this T[] cols)
		{
			if(null == cols) { return new List<T>(); }
			return new List<T>(cols);
		}

		public static List<Any> ToAnys<T>(
			this T[] objs,
			Func<T, string> nameFunc,
			Func<T, object> valueFunc)
		{
			List<Any> list = new List<Any>();
			if(objs.IsNullOrEmpty()) { return list; }
			if(null == nameFunc) { return list; }

			foreach(T obj in objs) {
				string name = nameFunc(obj);
				object value = null == valueFunc
					? null
					: valueFunc(obj);
				list.Add(name, value);
			}

			return list;
		}

		public static void Swap<T>(this T[] cols, int x, int y)
		{
			if(cols.IsNullOrEmpty()) { return; }
			int len = cols.Length;

			if(0 <= x && x < len && 0 <= y && y < len && x != y) {
				T tmp = cols[x];
				cols[x] = cols[y];
				cols[y] = tmp;
			}
		}

		public static string ToJson<T>(this List<T> objs)
		{
			return JsonConvert.SerializeObject(objs);
		}
		#endregion

		#region XmlNodeList
		public static bool IsNullOrEmpty(this XmlNodeList list)
		{
			return null == list || list.Count == 0;
		}
		#endregion

		#region XmlNode
		public static bool IsNullOrEmpty(this XmlNode node)
		{
			return null == node;
		}

		public static bool ChildNodesIsNullOrEmpty(this XmlNode node)
		{
			return null == node || node.ChildNodes.IsNullOrEmpty();
		}

		public static bool TryGetAttribute(
			this XmlNode node, string name, out string value)
		{
			value = string.Empty;

			if(null == node || null == node.Attributes) {
				return false;
			}
			var attrs = node.Attributes.GetEnumerator();
			while(attrs.MoveNext()) {
				XmlAttribute attr = (XmlAttribute)attrs.Current;
				if(attr.Name.Equals(
					name, StringComparison.OrdinalIgnoreCase)) {
					value = attr.Value.IsNullOrEmpty()
						? string.Empty
						: attr.Value.Trim();
					return true;
				}
			}
			return false;
		}

		public static bool AttributeExists(
			this XmlNode node, string name)
		{
			string value;
			return node.TryGetAttribute(name, out value);
		}

		public static string Attribute(
			this XmlNode node, string name)
		{
			return Attribute(node, name, string.Empty);
		}
		public static string Attribute(
			this XmlNode node, string name, string airBag)
		{
			string value;
			if(node.TryGetAttribute(name, out value)) {
				return value;
			}
			return airBag;
		}


		public static string InnerTextOrValue(this XmlNode node)
		{
			return node.InnerTextOrValue(string.Empty);
		}

		public static string InnerTextOrValue(
			this XmlNode node, string airBag)
		{
			if(null == node) { return airBag; }

			try {
				string txt = node
					.NodeType
					.EnumIn(XmlNodeType.Text, XmlNodeType.CDATA)
						? node.Value
						: node.InnerText;
				return txt.IsNullOrEmpty() ? airBag : txt.Trim();
			} catch {
				//} catch(Exception ex) {
				return airBag;
			}
		}


		public static string ChildInnerText(this XmlNode node, string tag)
		{
			return node.ChildInnerText(tag, string.Empty);
		}

		public static string ChildInnerText(
			this XmlNode node, string tag, string airBag)
		{
			if(null == node) { return airBag; }

			try {
				var subNode = node.SelectSingleNode(tag);
				return subNode.InnerTextOrValue(airBag);
			} catch {
				//} catch(Exception ex) {
				return airBag;
			}
		}
		#endregion

		#region XmlDocument, XDocument
		public static XmlDocument ToXmlDocument(
			this XDocument xDocument)
		{
			using(var xmlReader = xDocument.CreateReader()) {
				var xmlDocument = new XmlDocument();
				xmlDocument.Load(xmlReader);
				return xmlDocument;
			}
		}

		public static XDocument ToXDocument(
			this XmlDocument xmlDocument)
		{
			using(var nodeReader = new XmlNodeReader(xmlDocument)) {
				nodeReader.MoveToContent();
				return XDocument.Load(nodeReader);
			}
		}
		#endregion

		#region Exception
		public static List<Any> ToAnys(
			this Exception ex, params Any[] anys)
		{
			List<Any> list = new List<Any>();
			if(!anys.IsNullOrEmpty()) { list.AddRange(anys); }
			if(null != ex) {
				list.Add(new Any("source", ex.Source));
				list.Add(new Any("Message", ex.Message));
				list.Add(new Any("TargetSite", ex.TargetSite));
				list.Add(new Any("HelpLink", ex.HelpLink));
				list.Add(new Any(
					"Stack Trace",
					"..." + Environment.NewLine + ex.StackTrace
				));
			}
			return list;
		}
		#endregion

		#region object
		public static bool IsString(this object value)
		{
			if(null == value) { return false; }
			return value is string;
		}

		public static bool IsDateTime(this object value)
		{
			if(null == value) { return false; }
			return value is DateTime;
		}

		public static bool IsBoolean(this object value)
		{
			if(null == value) { return false; }
			return value is Boolean;
		}

		public static bool IsChar(this object value)
		{
			if(null == value) { return false; }
			return value is char;
		}

		public static bool IsByte(this object value)
		{
			if(null == value) { return false; }
			return value is Byte;
		}

		public static bool IsShort(this object value)
		{
			if(null == value) { return false; }
			return value is Int16;
		}

		public static bool IsInteger(this object value)
		{
			if(null == value) { return false; }
			return value is Int32;
		}

		public static bool IsLong(this object value)
		{
			if(null == value) { return false; }
			return value is Int64;
		}

		public static bool IsFloat(this object value)
		{
			if(null == value) { return false; }
			return value is Single;
		}

		public static bool IsDouble(this object value)
		{
			if(null == value) { return false; }
			return value is Double;
		}

		public static bool IsDecimal(this object value)
		{
			if(null == value) { return false; }
			return value is Decimal;
		}



		public static bool IsEnum(this object value)
		{
			if(null == value) { return false; }
			return value is Enum;
		}

		public static bool IsStream(this object value)
		{
			if(null == value) { return false; }
			return value is Stream;
		}

		public static bool IsGuid(this object value)
		{
			if(null == value) { return false; }
			return value is Guid;
		}

		public static bool IsColor(this object value)
		{
			if(null == value) { return false; }
			return value is Color;
		}

		public static bool IsTimeSpan(this object value)
		{
			if(null == value) { return false; }
			return value is TimeSpan;
		}


		public static string ToStringX(this object value)
		{
			return value.ToStringX(default(string));
		}
		public static string ToStringX(
			this object value, string airBag)
		{
			if(null == value) { return airBag; }
			Type type = value.GetType();

			if(value.IsDateTime()) {
				return value.ToDateTime().yyyy_MM_dd_HH_mm_ss_fff();
			} else if(value.IsBoolean()) {
				return value.ToBoolean() ? "True" : "False";
			} else if(value.IsChar()) {
				return value.ToString();
			} else if(value.IsByte()) {
				return string.Format(
					"{0:#0}",
					value.ToByte()
				);
			} else if(value.IsShort()) {
				return string.Format(
					"{0:#0}",
					value.ToShort()
				);
			} else if(value.IsInteger()) {
				return string.Format(
					"{0:#0}",
					value.ToInt()
				);
			} else if(value.IsLong()) {
				return string.Format(
					"{0:#0}",
					value.ToLong()
				);
			} else if(value.IsFloat()) {
				return string.Format(
					"{0:#0.000}",
					value.ToFloat()
				);
			} else if(value.IsDouble()) {
				return string.Format(
					"{0:#0.000}",
					value.ToDouble()
				);
			} else if(value.IsDecimal()) {
				return string.Format(
					"{0:#0.000}",
					value.ToDecimal()
				);
			} else if(value.IsEnum()) {
				return value.ToEnum().ToString();
			} else if(value.IsStream()) {
				return value.ToStream().ToString();
			} else if(value.IsGuid()) {
				return value.ToGuid().ToString();
			} else if(value.IsColor()) {
				return value.ToColor().ToHtml();
			} else if(type.IsArray) {
				var ary = value as Array;
				if(null == ary) { return value.ToString(); }
				var list = new List<string>();
				foreach(var one in ary) {
					list.Add(one.ToStringX());
				}
				return list.Join(", ");
			} else {
				return value.ToString();
			}
		}


		public static DateTime ToDateTime(this object value)
		{
			return value.ToDateTime(default(DateTime));
		}
		public static DateTime ToDateTime(
			this object value, DateTime airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsDateTime()) {
				return value.ToString().ToDateTime(airBag);
			}
			return (DateTime)value;
		}

		public static bool ToBoolean(this object value)
		{
			return value.ToBoolean(default(bool));
		}
		public static bool ToBoolean(
			this object value, bool airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsBoolean()) {
				return value.ToString().ToBoolean(airBag);
			}
			return (bool)value;
		}

		public static char ToChar(this object value)
		{
			return value.ToChar(default(char));
		}
		public static char ToChar(
			this object value, char airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsChar()) {
				return value.ToString().ToChar(airBag);
			}
			return (char)value;
		}

		public static byte ToByte(this object value)
		{
			return value.ToByte(default(byte));
		}
		public static byte ToByte(
			this object value, byte airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsByte()) {
				return value.ToString().ToByte(airBag);
			}
			return (byte)value;
		}

		public static short ToShort(this object value)
		{
			return value.ToShort(default(short));
		}
		public static short ToShort(
			this object value, short airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsShort()) {
				return value.ToString().ToShort(airBag);
			}
			return (short)value;
		}

		public static int ToInt(this object value)
		{
			return value.ToInt(default(int));
		}
		public static int ToInt(
			this object value, int airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsInteger()) {
				return value.ToString().ToInt(airBag);
			}
			return (int)value;
		}

		public static int ToInteger(this object value)
		{
			return value.ToInt();
		}
		public static int ToInteger(
			this object value, int airBag)
		{
			return value.ToInt(airBag);
		}

		public static long ToLong(this object value)
		{
			return value.ToLong(default(long));
		}
		public static long ToLong(
			this object value, long airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsLong()) {
				return value.ToString().ToLong(airBag);
			}
			return (long)value;
		}

		public static float ToFloat(this object value)
		{
			return value.ToFloat(default(float));
		}
		public static float ToFloat(
			this object value, float airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsFloat()) {
				return value.ToString().ToFloat(airBag);
			}
			return (float)value;
		}

		public static double ToDouble(this object value)
		{
			return value.ToDouble(default(double));
		}
		public static double ToDouble(
			this object value, double airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsDouble()) {
				return value.ToString().ToDouble(airBag);
			}
			return (double)value;
		}

		public static decimal ToDecimal(this object value)
		{
			return value.ToDecimal(default(decimal));
		}
		public static decimal ToDecimal(
			this object value, decimal airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsDecimal()) {
				return value.ToString().ToDecimal(airBag);
			}
			return (decimal)value;
		}


		public static Enum ToEnum(this object value)
		{
			return value.ToEnum(default(Enum));
		}
		public static Enum ToEnum(
			this object value, Enum airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsEnum()) {
				return value.ToString().ToEnum(airBag);
			}
			return (Enum)value;
		}

		public static Stream ToStream(this object value)
		{
			return value.ToStream(default(Stream));
		}
		public static Stream ToStream(
			this object value, Stream airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsStream()) { return airBag; }
			return (Stream)value;
		}

		public static byte[] ToBytes(this object value)
		{
			return value.ToBytes(default(byte[]));
		}
		public static byte[] ToBytes(
			this object value, byte[] airBag)
		{
			if(null == value) { return airBag; }
			if(!(value is byte[])) { return airBag; }
			return (byte[])value;
		}

		public static Guid ToGuid(this object value)
		{
			return value.ToGuid(default(Guid));
		}
		public static Guid ToGuid(
			this object value, Guid airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsGuid()) {
				return value.ToString().ToGuid(airBag);
			}
			return (Guid)value;
		}

		public static Color ToColor(this object value)
		{
			return value.ToColor(default(Color));
		}
		public static Color ToColor(
			this object value, Color airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsColor()) {
				return value.ToString().ToColor(airBag);
			}
			return (Color)value;
		}

		public static TimeSpan ToTimeSpan(this object value)
		{
			return value.ToTimeSpan(default(TimeSpan));
		}
		public static TimeSpan ToTimeSpan(
			this object value, TimeSpan airBag)
		{
			if(null == value) { return airBag; }
			if(!value.IsTimeSpan()) {
				return TimeSpan.Parse(value.ToString());
			}
			return (TimeSpan)value;
		}


		public static object GetValue(
			this object obj,
			string propertyOrFieldName)
		{
			return Reflector.GetValue(propertyOrFieldName, obj);
		}

		public static bool SetValue(
			this object obj,
			string propertyOrFieldName,
			object value)
		{
			return Reflector.SetValue(
				obj, propertyOrFieldName, value
			);
		}

		public static int SetValues(
			this object obj,
			params Any[] anys)
		{
			int successCount = 0;
			if(null == obj) { return successCount; }
			foreach(Any any in anys) {
				if(obj.SetValue(any.Name, any.Value)) {
					successCount++;
				}
			}
			return successCount;
		}

		public static int SetValues(
			this object obj,
			NameValueCollection nvc)
		{
			var anys = nvc.ToAnys();
			return obj.SetValues(anys.ToArray());
		}

		public static void SetDefaultValue(this object obj)
		{
			if(null == obj) { return; }
			foreach(var x in obj.GetType().GetProperties()) {
				Reflector.SetDefaultValue(obj, x);
			}
		}



		public static bool IsDerived<B>(this object obj)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsDerived<B>();
		}

		public static bool IsDerived(this object obj, Type baseType)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsDerived(baseType);
		}

		public static bool IsSubclassOf<B>(this object obj)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsSubclassOf<B>();
		}

		public static bool IsSubclassOf(
			this object obj, Type baseType)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsSubclassOf(baseType);
		}

		public static bool IsImplemented<B>(this object obj)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsImplemented<B>();
		}

		public static bool IsImplemented(
			this object obj, Type baseType)
		{
			if(null == obj) { return false; }
			return obj.GetType().IsImplemented(baseType);
		}

		public static DataFormat GetFormat(this object value)
		{
			if(null == value) { return DataFormat.Unknown; }
			return value.GetType().GetFormat();
		}
		#endregion

		#region Object[]
		public static List<Any> ToAnys(
			this object[] objs,
			string namePropertyName,
			string valuePropertyName)
		{
			List<Any> list = new List<Any>();
			if(objs.IsNullOrEmpty()) { return list; }

			foreach(object obj in objs) {
				string name = Reflector
					.GetValue(namePropertyName, obj)
					.ToString();
				object value = Reflector
					.GetValue(valuePropertyName, obj);
				list.Add(name, value);
			}

			return list;
		}


		#endregion

		#region StringBuilder
		public static bool IsNullOrEmpty(
			this StringBuilder sb)
		{
			return null == sb || sb.Length == 0;
		}

		public static StringBuilder RemoveEnd(
			this StringBuilder sb, int i)
		{
			if(null == sb) { return sb; }
			int len = sb.Length;
			if(len == 0) { return sb; }
			if(len < i) { return sb.Remove(0, len); }
			return sb.Remove(len - i, i);
		}

		public static StringBuilder RemoveStart(
			this StringBuilder sb, int i)
		{
			if(sb.IsNullOrEmpty()) { return sb; }
			int len = sb.Length;
			if(len == 0) { return sb; }
			return sb.Remove(0, i > len ? len : i);
		}

		public static StringBuilder Prepend(
			this StringBuilder sb, string str)
		{
			if(null == sb) { sb = new StringBuilder(); }
			return sb.Insert(0, str);
		}

		public static string ToString(
			this StringBuilder sb, int startIndex)
		{
			if(null == sb) { return string.Empty; }
			int len = sb.Length;
			return startIndex >= len
				? string.Empty
				: sb.ToString(startIndex, len - startIndex);
		}

		public static StringBuilder AppendLineFormat(
			this StringBuilder sb,
			string pattern,
			params string[] values)
		{
			sb.AppendFormat(pattern, values).AppendLine();
			return sb;
		}
		#endregion

		#region IPAddress
		public static int[] Split(this IPAddress ipAddress)
		{
			var parts = ipAddress.ToString().SplitAndTrim(".");
			if(parts.Length != 4) {
				throw new ArgumentOutOfRangeException(
					"ipAddress",
					"IPAddress can't splited into 4 parts of integer."
				);
			}
			var list = new int[] { 
				parts[0].ToInt(), 
				parts[1].ToInt(), 
				parts[2].ToInt(), 
				parts[3].ToInt(), 
			};
			return list;
		}

		public static bool InRange(
			this IPAddress ipAddress, string start, string end)
		{
			if(null == ipAddress) { return false; }
			long startL = BitConverter.ToInt32(
				IPAddress.Parse(start).GetAddressBytes(), 0
			);
			long endL = BitConverter.ToInt32(
				IPAddress.Parse(end).GetAddressBytes(), 0
			);
			return ipAddress.InRange(startL, endL);
		}

		public static bool InRange(
			this IPAddress ipAddress, long start, long end)
		{
			// Check
			if(null == ipAddress) { return false; }
			if(-1 == start || -1 == end) { return false; }

			// Compare
			long ip = BitConverter.ToInt32(
				ipAddress.GetAddressBytes(), 0
			);
			return ip.Between(start, end);
		}

		public static bool InRange(
			this IPAddress ipAddress, IPAddress start, IPAddress end)
		{
			if(null == ipAddress) { return false; }
			long startL = BitConverter.ToInt32(
				start.GetAddressBytes(), 0
			);
			long endL = BitConverter.ToInt32(
				end.GetAddressBytes(), 0
			);
			return ipAddress.InRange(startL, endL);
		}

		public static bool IsPrivate(this IPAddress ipAddress)
		{
			if(null == ipAddress) { return false; }

			var parts = ipAddress.Split();
			if(parts[0] == 10) {
				return true;
			}
			if(parts[0] == 172 && parts[1].Between(16, 31)) {
				return true;
			}
			if(parts[0] == 192 && parts[1] == 168) {
				return true;
			}

			return false;
		}
		#endregion

		#region FileStream
		public static string ToMD5(this FileStream fileStream)
		{
			if(null == fileStream) { return string.Empty; }
			byte[] hash = HashAlgorithm
				.Create(Constants.Encode.MD5)
				.ComputeHash(fileStream);
			return Convert.ToBase64String(hash);
		}
		public static string ToSHA(this FileStream fileStream)
		{
			if(null == fileStream) { return string.Empty; }
			byte[] hash = HashAlgorithm
				.Create(Constants.Encode.SHA512)
				.ComputeHash(fileStream);
			return Convert.ToBase64String(hash);
		}
		#endregion

		#region IDictionary
		public static bool SafeAdd<TKey, TValue>(
			this IDictionary<TKey, TValue> dict,
			TKey key, TValue value)
		{
			if(null == dict) { return false; }
			lock(_Lock) {
				bool containsKey = dict.ContainsKey(key);
				if(containsKey) {
					dict[key] = value;
				} else {
					dict.Add(key, value);
				}
				return containsKey;
			}
		}

		public static bool SafeRemove<TKey, TValue>(
			this IDictionary<TKey, TValue> dict, TKey key)
		{
			if(null == dict) { return false; }
			if(null == key) { return false; }
			lock(_Lock) {
				bool containsKey = dict.ContainsKey(key);
				return containsKey ? dict.Remove(key) : false;
			}
		}

		public static Dictionary<TKey, TValue> Clone<TKey, TValue>(
			this Dictionary<TKey, TValue> dict)
		{
			var clone = new Dictionary<TKey, TValue>();
			if(null != dict) {
				foreach(var key in dict.Keys) {
					clone.Add(key, dict[key]);
				}
			}
			return clone;
		}

		public static T[] ToArray<T>(
			this SortedList<string, T> sortedList)
		{
			List<T> list = new List<T>();
			if(null != sortedList.Values && sortedList.Values.Count > 0) {
				foreach(T value in sortedList.Values) {
					list.Add(value);
				}
			}
			return list.ToArray();
		}
		#endregion

		#region Dictionary<,>.ValueCollection
		public static T[] ToArray<T>(
			this Dictionary<string, T>.ValueCollection values)
		{
			List<T> list = new List<T>();
			if(null == values) {
				foreach(T value in values) {
					list.Add(value);
				}
			}
			return list.ToArray();
		}
		#endregion

		#region Color
		public static string ToHtml(this Color color)
		{
			if(null == color) { color = Color.White; }
			return ColorTranslator.ToHtml(color);
		}
		#endregion

		#region Generic
		public static bool In(this Enum val, params Enum[] vals)
		{
			return Array.IndexOf(vals, val) != -1;
		}

		public static bool EnumIn<T>(this T val, params T[] vals)
			where T : struct
		{
			if(null == vals) { return false; }

			Type type = typeof(T);
			if(!type.IsEnum) {
				throw new ArgumentException(
					"Only Enum type allowed."
				);
			}

			foreach(T x in vals) {
				if(val.Equals(x)) { return true; }
			}

			return false;
		}
		#endregion

		#region Image
		public static string ToBase64(this Image image)
		{
			MemoryStream ms = new MemoryStream();
			image.Save(ms, image.RawFormat);
			byte[] bytes = ms.ToArray();
			string base64 = Convert.ToBase64String(bytes);
			return base64;
		}
		#endregion

		#region IEnumerable
		public static bool IsNullOrEmpty<T>(
			this IEnumerable<T> enumerable)
		{
			return enumerable == null || enumerable.Count() == 0;
		}

		public static string Join(this IEnumerable<string> list)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list.ToArray().Join(string.Empty);
		}

		public static string Join(
			this IEnumerable<string> list,
			string separator)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list.ToArray().Join(separator);
		}

		public static string Join(
			this IEnumerable<string> list,
			string separator,
			string quoter)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list.ToArray().Join(separator, quoter);
		}

		public static string Join(
			this IEnumerable<string> list,
			string separator,
			string quoter0,
			string quoter9)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list.ToArray().Join(separator, quoter0, quoter9);
		}

		public static string Join(
			this IEnumerable<string> list,
			Func<string, string> transfer,
			string separator)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list
				.ToArray()
				.Join(transfer, separator, string.Empty);
		}

		public static string Join(
			this IEnumerable<string> list,
			Func<string, string> transfer,
			string separator,
			string quoter)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list
				.ToArray()
				.Join(transfer, separator, quoter, quoter);
		}

		public static string Join(
			this IEnumerable<string> list,
			Func<string, string> transfer,
			string separator,
			string quoter0,
			string quoter9)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }
			return list
				.ToArray()
				.Join(transfer, separator, quoter0, quoter9);
		}
		#endregion

		#region IEnumerable<T>
		public static IEnumerable<T> Page<T>(
			this IEnumerable<T> all, int pageSize, int pageIndex)
		{
			return all
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize);
		}
		#endregion

		#region RNGCryptoServiceProvider
		public static int Next(
			this RNGCryptoServiceProvider rng, int max)
		{
			return Next(rng, 0, max);
		}

		public static int Next(
			this RNGCryptoServiceProvider rng, int min, int max)
		{
			if(min > max) {
				throw new ArgumentOutOfRangeException("min");
			}
			if(min == max) { return min; }
			Int64 diff = max - min;
			byte[] _uint32Buffer = new byte[4];

			while(true) {
				rng.GetBytes(_uint32Buffer);
				UInt32 rand = BitConverter.ToUInt32(_uint32Buffer, 0);
				Int64 maxValue = (1 + (Int64)UInt32.MaxValue);
				Int64 remainder = maxValue % diff;
				if(rand < maxValue - remainder) {
					return (Int32)(min + (rand % diff));
				}
			}
		}
		#endregion

		#region SecureString
		public static SecureString ToSecureString(this string str)
		{
			SecureString secureStr = new SecureString();
			if(!str.IsNullOrEmpty()) {
				str.ToList().ForEach(x =>
					secureStr.AppendChar(x)
				);
			}
			return secureStr;
		}

		// non secure
		//public static string ToString(this SecureString secureStr)
		//{
		//	if(null == secureStr) {
		//		throw new ArgumentNullException("secureStr");
		//	}

		//	IntPtr bstr = Marshal.SecureStringToBSTR(secureStr);
		//	try {
		//		return Marshal.PtrToStringBSTR(bstr);
		//	} finally {
		//		Marshal.FreeBSTR(bstr);
		//	}
		//}
		#endregion

		#region IQueryable<T>
		public static List<Any> ToAnys<T>(
			this IQueryable<T> all,
			Func<T, string> toName,
			Func<T, object> toValue)
		{
			var list = new List<Any>();
			if(null == all || null == toName || null == toValue) { return list; }
			foreach(T one in all) {
				list.Add(new Any(toName(one), toValue(one)));
			}
			return list;
		}

		public static IQueryable<T> Page<T>(
			this IQueryable<T> all, int pageSize, int pageIndex)
		{
			return all
				.Skip((pageIndex - 1) * pageSize)
				.Take(pageSize);
		}
		#endregion

		#region XmlAttributeCollection
		public static bool IsNullOrEmpty(
			XmlAttributeCollection xmlAttrs)
		{
			return null == xmlAttrs || xmlAttrs.Count == 0;
		}
		#endregion

		#region XmlAttribute
		public static bool IsNullOrEmpty(XmlAttribute xmlAttr)
		{
			return null == xmlAttr || xmlAttr.Value.IsNullOrEmpty();
		}
		#endregion

		#region List<Any>
		public static Any Get(
			this List<Any> anys, string name)
		{
			if(null == anys) {
				throw new ArgumentNullException("anys");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			var any = anys.FirstOrDefault(x => x.Name == name);
			return any;
		}

		public static List<Any> Add(
			this List<Any> anys, string name, object value)
		{
			if(null == anys) {
				throw new ArgumentNullException("anys");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			anys.Add(new Any(name, value));
			return anys;
		}

		public static List<Any> SafeAdd(
			this List<Any> anys, string name, object value)
		{
			if(null == anys) {
				throw new ArgumentNullException("anys");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			anys.SafeAdd(new Any(name, value));
			return anys;
		}

		public static List<Any> SafeAdd(
			this List<Any> anys, params Any[] list)
		{
			if(null == anys) {
				throw new ArgumentNullException("anys");
			}
			if(list.IsNullOrEmpty()) { return anys; }

			foreach(var any in list) {
				if(null == any) { continue; }

				if(anys.Contains(any.Name)) {
					anys.Remove(any.Name);
				}
				anys.Add(any);
			}
			return anys;
		}

		public static List<Any> Remove(
			this List<Any> list, params string[] names)
		{
			if(list.IsNullOrEmpty()) { return new List<Any>(); }
			if(names.IsNullOrEmpty()) { return list; }

			var newList = list.Where(x => x.Name.In(names) == false);

			return newList.ToList();
		}

		// ToIntegers
		public static int[] ToIntegers(
			this List<Any> anys, string name)
		{
			return anys.ToIntegers(name, Symbol.Comma);
		}

		public static int[] ToIntegers(
			this List<Any> anys, string name, string separator)
		{
			return anys.ToIntegers(name, separator, new int[0]);
		}

		public static int[] ToIntegers(
			this List<Any> anys, string name, int[] airBag)
		{
			return anys.ToIntegers(
				name, Symbol.Comma, airBag
			);
		}

		public static int[] ToIntegers(
			this List<Any> anys,
			string name,
			string separator,
			int[] airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToIntegers(separator, airBag);
		}

		// ToStrings
		public static string[] ToStrings(
			this List<Any> anys, string name)
		{
			return anys.ToStrings(name, Symbol.Comma);
		}

		public static string[] ToStrings(
			this List<Any> anys, string name, string separator)
		{
			return anys.ToStrings(
				name, Symbol.Comma, new string[0]
			);
		}

		public static string[] ToStrings(
			this List<Any> anys, string name, string[] airBag)
		{
			return anys.ToStrings(
				name, Symbol.Comma, airBag
			);
		}

		public static string[] ToStrings(
			this List<Any> anys,
			string name,
			string separator,
			string[] airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToStrings(separator, airBag);
		}

		public static string ToStringX(this List<Any> anys)
		{
			return anys.ToStringX(true);
		}
		public static string ToStringX(
			this List<Any> anys, bool withOrder)
		{
			if(anys.IsNullOrEmpty()) { return string.Empty; }

			StringBuilder sb = new StringBuilder();
			int index = 0;
			int length = (int)Math.Ceiling(
				Math.Log10(anys.Count() + 1)
			);
			int maxLen = 0;
			foreach(var any in anys) {
				if(null == any.Name) { continue; }
				maxLen = Math.Max(maxLen, any.Name.Length);
			}
			foreach(var any in anys) {
				if(index > 0) { sb.AppendLine(); }

				string name = any.Name.AirBag();

				if(null != any.Value) {
					Type type = any.Value.GetType();

					// Array
					if(type.IsArray) {
						Array array = any.Value as Array;
						if(null != array) {
							++index;
							int idx = 0;
							int len = (int)Math.Ceiling(
								Math.Log10(array.Length)
							);
							foreach(var x in array) {
								if(idx > 0) { sb.AppendLine(); }
								string val = x.ToString();
								if(val.Contains(
									Environment.NewLine)) {
									val = val.Replace(
										Environment.NewLine,
										String.Concat(
											Environment.NewLine,
											" ".Repeat(
												len + maxLen + 5
											)
										)
									);
								}

								sb.AppendFormat(
									"{0} {1}{2} = {3} {4}",
									withOrder 
										? index.OrderList(length)
										: Environment.NewLine,
									name,
									" ".Repeat(
										maxLen - name.Length
									),
									(++idx).OrderList(len),
									val
								);
							}
							continue;
						}
					}
				}

				string value = any.ToString();

				if(value.Contains(Environment.NewLine)) {
					value = value.Replace(
						Environment.NewLine,
						String.Concat(
							Environment.NewLine,
							" ".Repeat(length + maxLen + 5)
						)
					);
				}

				if(sb.Length > 0 && !withOrder) {
					sb.AppendLine();
				}

				sb.AppendFormat(
					"{0} {1}{2} = {3}",
					withOrder 
						? (++index).OrderList(length)
						: string.Empty,
					name,
					" ".Repeat(maxLen - name.Length),
					value
				);
			}
			return sb.ToString();
		}

		public static string ToString(
			this List<Any> anys, string name)
		{
			return anys.ToString(name, string.Empty);
		}

		public static string ToString(
			this List<Any> anys, string name, string airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToString(airBag);
		}

		public static DateTime ToDateTime(
			this List<Any> anys, string name)
		{
			return anys.ToDateTime(name, default(DateTime));
		}

		public static DateTime ToDateTime(
			this List<Any> anys, string name, DateTime airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToDateTime(airBag);
		}

		public static bool ToBoolean(
			this List<Any> anys, string name)
		{
			return anys.ToBoolean(name, default(bool));
		}

		public static bool ToBoolean(
			this List<Any> anys, string name, bool airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToBoolean(airBag);
		}

		public static char ToChar(this List<Any> anys, string name)
		{
			return anys.ToChar(name, default(char));
		}
		public static char ToChar(
			this List<Any> anys, string name, char airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToChar(airBag);
		}

		public static byte ToByte(this List<Any> anys, string name)
		{
			return anys.ToByte(name, default(byte));
		}

		public static byte ToByte(
			this List<Any> anys, string name, byte airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToByte(airBag);
		}

		public static short ToShort(this List<Any> anys, string name)
		{
			return anys.ToShort(name, default(short));
		}

		public static short ToShort(
			this List<Any> anys, string name, short airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToShort(airBag);
		}

		public static int ToInteger(this List<Any> anys, string name)
		{
			return anys.ToInteger(name, default(int));
		}

		public static int ToInteger(
			this List<Any> anys, string name, int airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToInteger(airBag);
		}

		public static long ToLong(this List<Any> anys, string name)
		{
			return anys.ToLong(name, default(long));
		}

		public static long ToLong(
			this List<Any> anys, string name, long airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToLong(airBag);
		}

		public static float ToFloat(this List<Any> anys, string name)
		{
			return anys.ToFloat(name, default(float));
		}

		public static float ToFloat(
			this List<Any> anys, string name, float airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToFloat(airBag);
		}

		public static double ToDouble(
			this List<Any> anys, string name)
		{
			return anys.ToDouble(name, default(double));
		}

		public static double ToDouble(
			this List<Any> anys, string name, double airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToDouble(airBag);
		}

		public static decimal ToDecimal(
			this List<Any> anys, string name)
		{
			return anys.ToDecimal(name, default(decimal));
		}

		public static decimal ToDecimal(
			this List<Any> anys, string name, decimal airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToDecimal(airBag);
		}


		public static Color ToColor(this List<Any> anys, string name)
		{
			return anys.ToColor(name, default(Color));
		}
		public static Color ToColor(
			this List<Any> anys, string name, Color airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToColor(airBag);
		}

		public static Guid ToGuid(this List<Any> anys, string name)
		{
			return anys.ToGuid(name, default(Guid));
		}
		public static Guid ToGuid(
			this List<Any> anys, string name, Guid airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToGuid(airBag);
		}

		public static byte[] ToByteArray(
			this List<Any> anys, string name)
		{
			return anys.ToByteArray(name, new byte[0]);
		}
		public static byte[] ToByteArray(
			this List<Any> anys, string name, byte[] airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToByteArray(airBag);
		}

		public static T ToEnum<T>(this List<Any> anys, string name)
		{
			return anys.ToEnum(name, default(T));
		}
		public static T ToEnum<T>(
			this List<Any> anys, string name, T airBag)
		{
			if(anys.IsNullOrEmpty()) { return airBag; }
			var any = anys.FirstOrDefault(x => x.Name == name);
			if(null == any) { return airBag; }
			return any.ToEnum(airBag);
		}

		public static List<Many> ToManys(
			this List<Any> anys, string group)
		{
			var manys = new List<Many>();
			if(!anys.IsNullOrEmpty()) {
				foreach(var any in anys) {
					manys.Add(any.ToMany(group));
				}
			}
			return manys;
		}

		public static bool Contains(this List<Any> anys, string name)
		{
			if(anys.IsNullOrEmpty()) { return false; }

			return anys.Contains(x => x.Name == name);
		}

		public static bool Exists(this List<Any> anys, string name)
		{
			if(anys.IsNullOrEmpty()) { return false; }

			return anys.Exists(x => x.Name == name);
		}

		public static List<string> GetNames(this List<Any> anys)
		{
			var list = new List<string>();
			if(anys.IsNullOrEmpty()) { return list; }

			foreach(var any in anys) {
				list.Add(any.Name);
			}

			return list;
		}

		public static List<object> GetValues(this List<Any> anys)
		{
			var list = new List<object>();
			if(anys.IsNullOrEmpty()) { return list; }

			foreach(var any in anys) {
				list.Add(any.Name);
			}

			return list;
		}
		#endregion

		#region List<Many>
		public static Many GetMany(
			this List<Many> manys, string group, string name)
		{
			if(manys.IsNullOrEmpty()) { return null; }
			Many many = manys.FirstOrDefault(x =>
				x.Group == group && x.Name == name
			);
			return many;
		}

		public static Any GetAny(
			this List<Many> manys, string group, string name)
		{
			if(manys.IsNullOrEmpty()) { return null; }
			Many many = manys.FirstOrDefault(x =>
				x.Group == group && x.Name == name
			);
			return null == many ? null : many.ToAny();
		}

		public static List<Many> GetManys(
			this List<Many> manys, string group)
		{
			if(manys.IsNullOrEmpty()) { return null; }
			var all = manys.Where(x => x.Group == group);
			return null == all 
				? new List<Many>() 
				: new List<Many>(all);
		}

		public static List<Any> GetAnys(
			this List<Many> manys, string group)
		{
			var all = manys.GetManys(group);
			if(all.IsNullOrEmpty()) { return null; }
			return all.ToAnys();
		}

		// ToAnys
		public static List<Any> ToAnys(this List<Many> manys)
		{
			if(manys.IsNullOrEmpty()) { return new List<Any>(); }
			List<Any> anys = new List<Any>();
			manys.ForEach(x => anys.Add(x.ToAnyWithFullName()));
			return anys;
		}
		public static List<Any> ToAnys(
			this List<Many> manys, string group)
		{
			if(manys.IsNullOrEmpty()) { return new List<Any>(); }
			List<Any> anys = new List<Any>();
			manys.ForEach(x => {
				if(group != x.Group) { return; }
				anys.Add(x.ToAny());
			});
			return anys;
		}

		// ToIntegers
		public static int[] ToIntegers(
			this List<Many> manys, string group, string name)
		{
			return manys.ToIntegers(
				group, name, Symbol.Comma, new int[0]
			);
		}
		public static int[] ToIntegers(
			this List<Many> manys,
			string group,
			string name,
			string separator)
		{
			return manys.ToIntegers(
				group, name, separator, new int[0]
			);
		}
		public static int[] ToIntegers(
			this List<Many> manys,
			string group,
			string name,
			int[] airBag)
		{
			return manys.ToIntegers(
				group, name, Symbol.Comma, airBag
			);
		}
		public static int[] ToIntegers(
			this List<Many> manys,
			string group,
			string name,
			string separator,
			int[] airBag)
		{
			var many = manys.FirstOrDefault(x =>
				x.Group == group && x.Name == name
			);
			if(null == many) { return airBag; }
			return many.ToIntegers(separator, airBag);
		}

		// ToStrings
		public static string[] ToStrings(
			this List<Many> manys, string group, string name)
		{
			return manys.ToStrings(
				group, name, Symbol.Comma, new string[0]
			);
		}
		public static string[] ToStrings(
			this List<Many> manys,
			string group,
			string name,
			string separator)
		{
			return manys.ToStrings(
				group, name, separator, new string[0]
			);
		}
		public static string[] ToStrings(
			this List<Many> manys,
			string group,
			string name,
			string[] airBag)
		{
			return manys.ToStrings(
				group, name, Symbol.Comma, airBag
			);
		}
		public static string[] ToStrings(
			this List<Many> manys,
			string group,
			string name,
			string separator,
			string[] airBag)
		{
			if(manys.IsNullOrEmpty()) { return airBag; }
			var many = manys.FirstOrDefault(x =>
				x.Group == group && x.Name == name
			);
			if(null == many) { return airBag; }
			return many.ToStrings(separator, airBag);
		}

		// ToString
		public static string ToString(
			this List<Many> manys, string group, string name)
		{
			return manys.ToString(group, name, string.Empty);
		}
		public static string ToString(
			this List<Many> manys,
			string group,
			string name,
			string airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToString();
		}

		// ToDateTime
		public static DateTime ToDateTime(
			this List<Many> manys, string group, string name)
		{
			return manys.ToDateTime(group, name, default(DateTime));
		}
		public static DateTime ToDateTime(
			this List<Many> manys,
			string group,
			string name,
			DateTime airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToDateTime();
		}

		// ToBoolean
		public static bool ToBoolean(
			this List<Many> manys, string group, string name)
		{
			return manys.ToBoolean(group, name, default(bool));
		}
		public static bool ToBoolean(
			this List<Many> manys,
			string group,
			string name,
			bool airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToBoolean();
		}

		// ToChar
		public static char ToChar(
			this List<Many> manys, string group, string name)
		{
			return manys.ToChar(group, name, default(char));
		}
		public static char ToChar(
			this List<Many> manys,
			string group,
			string name,
			char airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToChar();
		}

		// ToByte
		public static byte ToByte(
			this List<Many> manys, string group, string name)
		{
			return manys.ToByte(group, name, default(byte));
		}
		public static byte ToByte(
			this List<Many> manys,
			string group,
			string name,
			byte airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToByte();
		}

		// ToShort
		public static short ToShort(
			this List<Many> manys, string group, string name)
		{
			return manys.ToShort(group, name, default(short));
		}
		public static short ToShort(
			this List<Many> manys,
			string group,
			string name,
			short airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToShort();
		}

		// ToInteger
		public static int ToInteger(
			this List<Many> manys, string group, string name)
		{
			return manys.ToInteger(group, name, default(int));
		}
		public static int ToInteger(
			this List<Many> manys,
			string group,
			string name,
			int airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToInteger();
		}

		// ToLong
		public static long ToLong(
			this List<Many> manys, string group, string name)
		{
			return manys.ToLong(group, name, default(long));
		}
		public static long ToLong(
			this List<Many> manys,
			string group,
			string name,
			long airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToLong();
		}

		// ToFloat
		public static float ToFloat(
			this List<Many> manys, string group, string name)
		{
			return manys.ToFloat(group, name, default(float));
		}
		public static float ToFloat(
			this List<Many> manys,
			string group,
			string name,
			float airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToFloat();
		}

		// ToDouble
		public static double ToDouble(
			this List<Many> manys, string group, string name)
		{
			return manys.ToDouble(group, name, default(double));
		}
		public static double ToDouble(
			this List<Many> manys,
			string group,
			string name,
			double airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToDouble();
		}

		// ToDecimal
		public static decimal ToDecimal(
			this List<Many> manys, string group, string name)
		{
			return manys.ToDecimal(group, name, default(decimal));
		}
		public static decimal ToDecimal(
			this List<Many> manys,
			string group,
			string name,
			decimal airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToDecimal();
		}

		// ToColor
		public static Color ToColor(
			this List<Many> manys, string group, string name)
		{
			return manys.ToColor(group, name, default(Color));
		}
		public static Color ToColor(
			this List<Many> manys,
			string group,
			string name,
			Color airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToColor();
		}

		// ToGuid
		public static Guid ToGuid(
			this List<Many> manys, string group, string name)
		{
			return manys.ToGuid(group, name, default(Guid));
		}
		public static Guid ToGuid(
			this List<Many> manys,
			string group,
			string name,
			Guid airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToGuid();
		}

		// ToByteArray
		public static byte[] ToByteArray(
			this List<Many> manys, string group, string name)
		{
			return manys.ToByteArray(group, name, new byte[0]);
		}
		public static byte[] ToByteArray(
			this List<Many> manys,
			string group,
			string name,
			byte[] airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToByteArray();
		}

		// ToEnum
		public static T ToEnum<T>(
			this List<Many> manys, string group, string name)
		{
			return manys.ToEnum(group, name, default(T));
		}
		public static T ToEnum<T>(
			this List<Many> manys,
			string group,
			string name,
			T airBag)
		{
			Many many = manys.GetMany(group, name);
			if(null == many) { return airBag; }
			return many.ToEnum<T>();
		}

		public static List<Many> Add(
			this List<Many> manys, string group, string name, object value)
		{
			if(null == manys) {
				throw new ArgumentNullException("manys");
			}
			if(group.IsNullOrEmpty()) {
				throw new ArgumentNullException("group");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			manys.Add(new Many(group, name, value));
			return manys;
		}

		public static List<Many> SafeAdd(
			this List<Many> manys, string group, string name, object value)
		{
			if(null == manys) {
				throw new ArgumentNullException("manys");
			}
			if(group.IsNullOrEmpty()) {
				throw new ArgumentNullException("group");
			}
			if(name.IsNullOrEmpty()) {
				throw new ArgumentNullException("name");
			}

			manys.SafeAdd(new Many(group, name, value));
			return manys;
		}

		public static List<Many> SafeAdd(
			this List<Many> manys, params Many[] list)
		{
			if(null == manys) {
				throw new ArgumentNullException("manys");
			}
			if(list.IsNullOrEmpty()) { return manys; }

			foreach(var many in list) {
				if(null == many) { continue; }

				if(manys.Contains(many.Group, many.Name)) {
					manys.Remove(many.Group, many.Name);
				}
				manys.Add(many);
			}
			return manys;
		}


		public static List<Many> Remove(
			this List<Many> manys, string group, string name)
		{
			if(manys.IsNullOrEmpty()) { return new List<Many>(); }
			if(group.IsNullOrEmpty()) { return manys; }
			if(name.IsNullOrEmpty()) { return manys; }

			var newList = manys.Where(x => 
				x.Group != group && x.Name != name
			);

			return newList.ToList();
		}

		public static bool Contains(
			this List<Many> manys, string group, string name)
		{
			if(manys.IsNullOrEmpty()) { return false; }

			return manys.Contains(x =>
				x.Group == group && x.Name == name
			);
		}

		public static bool Exists(
			this List<Many> manys, string group, string name)
		{
			if(manys.IsNullOrEmpty()) { return false; }

			return manys.Exists(x => 
				x.Group == group && x.Name == name
			);
		}
		#endregion

		#region Any[]
		public static bool IsNullOrEmpty(this Any[] objs)
		{
			return null == objs || objs.Length == 0;
		}

		public static List<Any> Concat(
			this Any[] anys1, params Any[] anys2)
		{
			List<Any> list = new List<Any>();
			list.SafeAdd(anys1);
			list.SafeAdd(anys2);
			return list;
		}
		#endregion

		#region DataRow
		static public void AssignValue(
			this DataRow row, string name, object val)
		{
			if(null == val) {
				val = DBNull.Value;
			} else {
				if(val is DateTime) {
					if(((DateTime)val).Ticks == 0) {
						val = DBNull.Value;
					}
				}
			}
			row[name] = val;
			if(val is DateTime) {
				if(val != DBNull.Value) {
					row[name] = (DateTime)val;
				} else {
					row[name] = string.Empty;
				}
			}
		}
		#endregion

		#region PropertyInfo
		public static bool HasAttribute<T>(this PropertyInfo info)
			where T : Attribute
		{
			return null != info.GetAttribute<T>();
		}

		public static T GetAttribute<T>(this PropertyInfo info)
		{
			if(null == info) { return default(T); }
			foreach(var x in info.GetCustomAttributes(false)) {
				if(x is T) { return (T)x; }
			}
			return default(T);
		}

		public static string GetDescription(this PropertyInfo info)
		{
			string description = string.Empty;
			if(null == info) { return description; }

			var attr = info.GetAttribute<DescriptionAttribute>();
			if(null != attr) { description = attr.Description; }

			return description;
		}

		public static string GetCategory(this PropertyInfo info)
		{
			string category = string.Empty;
			if(null == info) { return category; }

			var attr = info.GetAttribute<CategoryAttribute>();
			if(null != attr) { category = attr.Category; }

			return category;
		}

		public static object GetDefaultValue(this PropertyInfo info)
		{
			object value = null;
			if(null == info) { return value; }

			var attr = info.GetAttribute<DefaultValueAttribute>();
			if(null != attr) { value = attr.Value; }

			return value;
		}
		#endregion

		#region HttpBrowserCapabilities
		public static List<Any> ToAnys(
			this HttpBrowserCapabilities browser)
		{
			var list = new List<Any>();
			if(null == browser) { return list; }

			list
				.Add("Type", browser.Type)
				.Add("Browser", browser.Browser)
				.Add("Browser", browser.Browser)
				.Add("Version", browser.Version)
				.Add("MajorVersion", browser.MajorVersion)
				.Add("MinorVersion", browser.MinorVersion)
				.Add("Platform", browser.Platform)
				.Add("Beta", browser.Beta)
				.Add("Crawler", browser.Crawler)
				.Add("AOL", browser.AOL)
				.Add("Win16", browser.Win16)
				.Add("Win32", browser.Win32)
				.Add("Frames", browser.Frames)
				.Add("Tables", browser.Tables)
				.Add("Cookies", browser.Cookies)
				.Add("VBScript", browser.VBScript)
				.Add("EcmaScriptVersion", browser.EcmaScriptVersion.ToString())
				.Add("JavaApplets", browser.JavaApplets)
				.Add("ActiveXControls", browser.ActiveXControls)
				.Add("JavaScriptVersion", browser["JavaScriptVersion"]);

			return list;
		}
		#endregion

		#region List<T>
		public static string Join<T>(
			this List<T> list,
			Func<T, string> func)
		{
			return list.Join(
				func, string.Empty, string.Empty, string.Empty
			);
		}
		public static string Join<T>(
			this List<T> list,
			Func<T, string> func,
			string separator)
		{
			return list.Join(
				func, separator, string.Empty, string.Empty
			);
		}
		public static string Join<T>(
			this List<T> list,
			Func<T, string> func,
			string separator,
			string quoter)
		{
			return list.Join(
				func, separator, quoter, quoter
			);
		}
		public static string Join<T>(
			this List<T> list,
			Func<T, string> func,
			string separator,
			string quoter0,
			string quoter9)
		{
			if(list.IsNullOrEmpty()) { return string.Empty; }

			var cols = new List<string>();
			foreach(var one in list) {
				cols.Add(null == func ? string.Empty: func(one));
			}

			return cols.Join(separator, quoter0, quoter9);
		}
		#endregion
	}
}
