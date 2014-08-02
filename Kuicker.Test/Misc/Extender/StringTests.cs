using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using Xunit.Extensions;

namespace Kuicker.Test
{
	public class StringTests
	{
		[Fact]
		public void IsNullOrEmpty()
		{
			string str = null;
			Assert.True(str.IsNullOrEmpty());
			Assert.True(string.Empty.IsNullOrEmpty());
			Assert.True("".IsNullOrEmpty());

			Assert.False(" ".IsNullOrEmpty());
			Assert.False("	".IsNullOrEmpty());
			Assert.False(Environment.NewLine.IsNullOrEmpty());
			Assert.False((" " + Environment.NewLine + "	").IsNullOrEmpty());
			Assert.False((" " + Environment.NewLine + "	a").IsNullOrEmpty());
			Assert.False("a".IsNullOrEmpty());
			Assert.False("1".IsNullOrEmpty());
			Assert.False("Here are a few words.".IsNullOrEmpty());
		}

		[Fact]
		public void IsNullOrWhiteSpace()
		{
			string str = null;
			Assert.True(str.IsNullOrWhiteSpace());
			Assert.True(string.Empty.IsNullOrWhiteSpace());
			Assert.True("".IsNullOrWhiteSpace());

			Assert.True(" ".IsNullOrWhiteSpace());
			Assert.True("	".IsNullOrWhiteSpace());
			Assert.True(Environment.NewLine.IsNullOrWhiteSpace());
			Assert.True((" " + Environment.NewLine + "	").IsNullOrWhiteSpace());
			Assert.False((" " + Environment.NewLine + "	a").IsNullOrWhiteSpace());
			Assert.False("a".IsNullOrWhiteSpace());
			Assert.False("1".IsNullOrWhiteSpace());
			Assert.False("Here are a few words.".IsNullOrWhiteSpace());
		}

		[Theory]
		[InlineData("ab c def gh ij	k", 5, "ab c ")]
		[InlineData(" ab c def gh i jk", 5, " ab c")]
		[InlineData("	ab c def gh ijk	", 5, "	ab c")]
		[InlineData(" 	 ab c def gh ij k	 ", 5, " 	 ab")]
		[InlineData(" ab \r\n c def gh i\r\nj k	", 5, " ab \r")]
		public void Left(string input, int length, string result)
		{
			string left = input.Left(length);
			//Debug.WriteLine(string.Format(
			//	"(Left, result) = (\"{0}\", \"{1}\")",
			//	left,
			//	result
			//));

			Assert.True(left == result);
		}

		[Theory]
		[InlineData("ab c def gh ij	k", 5, " ij	k")]
		[InlineData(" ab c def gh i jk", 5, " i jk")]
		[InlineData("	ab c def gh ijk	", 5, " ijk	")]
		[InlineData(" 	 ab c def gh ij k	 ", 5, "j k	 ")]
		[InlineData(" ab \r\n c def gh i\r\nj k	", 5, "\nj k	")]
		public void Right(string input, int length, string result)
		{
			string right = input.Right(length);
			Debug.WriteLine(string.Format(
				"(Right, result) = (\"{0}\", \"{1}\")",
				right,
				result
			));

			Assert.True(right == result);
		}

		[Theory]
		[PropertyData("SplitPattern_Data")]
		public void SplitPattern(
			string input, 
			string pattern, 
			RegexOptions options, 
			string[] expect)
		{
			var result = input.SplitAndTrim(pattern, options);
			Debug.WriteLine(string.Format(
				"(result, expect)=(\"{0}\", \"{1}\")",
				result.Join(","),
				expect.Join(",")
			));

			Assert.True(result.SequenceEqual(expect));
		}


		public static IEnumerable<object[]> SplitPattern_Data
		{
			get
			{
				return new[] {
					new object[]{
						"ab c  ,def " + Environment.NewLine + "gh ij	k ",
						"\\s",
						RegexOptions.IgnoreCase,
						new[]{
							"ab",
							"c",
							"",
							",def",
							"",
							"",
							"gh",
							"ij",
							"k",
							"",
						}
					},
				};
			}
		}


		[Theory]
		[PropertyData("SplitSymbol_Data")]
		public void SplitSymbol(
			string input,
			string[] symbols, 
			string[] expect)
		{
			var result = input.SplitAndTrim(symbols);
			Debug.WriteLine(string.Format(
				"(result, expect)=(\"{0}\", \"{1}\")",
				result.Join(","),
				expect.Join(",")
			));

			Assert.True(result.SequenceEqual(expect));
		}
		public static IEnumerable<object[]> SplitSymbol_Data
		{
			get
			{
				return new[] {
					new object[]{
						"ab c  ,def " + Environment.NewLine + "gh ij	k ",
						new[]{
							" ",
							",",
						},
						new[]{
							"ab",
							"c",
							"",
							"",
							"def",
							"gh",
							// Environment.NewLine + "gh", // Trim
							"ij	k",
							"",
						}
					},
				};
			}
		}

		// TODO
		[Theory]
		[InlineData()]
		public void ToBytes(
			string input,
			Encoding encoding,
			byte[] expect)
		{
			var result = input.ToBytes(encoding);

			Assert.True(result.SequenceEqual(expect));
		}

		[Fact]
		public void ToStream()
		{
			// TODO
		}

		[Fact]
		public void ToBase64()
		{
			// TODO
		}

		[Fact]
		public void FromBase64()
		{
			// TODO
		}

		[Fact]
		public void Repeat()
		{
			// TODO
		}

		[Fact]
		public void Encode()
		{
			// TODO
		}

		[Fact]
		public void HexToBytes()
		{
			// TODO
		}

		[Fact]
		public void AirBag()
		{
			// TODO
		}

		[Fact]
		public void ToDateTime()
		{
			// TODO
		}

		[Fact]
		public void ToBoolean()
		{
			// TODO
		}

		[Fact]
		public void ToChar()
		{
			// TODO
		}

		[Fact]
		public void ToByte()
		{
			// TODO
		}

		[Fact]
		public void ToShort()
		{
			// TODO
		}

		[Fact]
		public void ToInt()
		{
			// TODO
		}

		[Fact]
		public void ToLong()
		{
			// TODO
		}

		[Fact]
		public void ToFloat()
		{
			// TODO
		}

		[Fact]
		public void ToDouble()
		{
			// TODO
		}

		[Fact]
		public void ToDecimal()
		{
			// TODO
		}

		[Fact]
		public void ToEnum()
		{
			// TODO
		}

		[Fact]
		public void ToColor()
		{
			// TODO
		}

		[Fact]
		public void ToGuid()
		{
			// TODO
		}

		[Fact]
		public void In()
		{
			// TODO
		}

		[Fact]
		public void StartsX()
		{
			// TODO
		}

		[Fact]
		public void EndsX()
		{
			// TODO
		}

		[Fact]
		public void IsMatch()
		{
			// TODO
		}

		[Fact]
		public void TrimStart()
		{
			// TODO
		}

		[Fact]
		public void TrimEnd()
		{
			// TODO
		}

		[Fact]
		public void AppendPrefix()
		{
			// TODO
		}

		[Fact]
		public void AppendSuffix()
		{
			// TODO
		}

		[Fact]
		public void AppendLineFirst()
		{
			// TODO
		}

		[Fact]
		public void AppendLineLast()
		{
			// TODO
		}

		[Fact]
		public void AppendLine()
		{
			// TODO
		}

		[Fact]
		public void ToUnicodeEntity()
		{
			// TODO
		}

		[Fact]
		public void ToWords()
		{
			// TODO
		}

		[Fact]
		public void AllUpper()
		{
			// TODO
		}

		[Fact]
		public void AllLower()
		{
			// TODO
		}

		[Fact]
		public void Remove()
		{
			// TODO
		}

		[Fact]
		public void ToUpperCasing()
		{
			// TODO
		}

		[Fact]
		public void ToLowerCasing()
		{
			// TODO
		}

		[Fact]
		public void ToCompactUpperCasing()
		{
			// TODO
		}

		[Fact]
		public void ToCompactLowerCasing()
		{
			// TODO
		}

		[Fact]
		public void ToPascalCasing()
		{
			// TODO
		}

		[Fact]
		public void ToCamelCasing()
		{
			// TODO
		}

		[Fact]
		public void IsDateTime()
		{
			// TODO
		}

		[Fact]
		public void IsBoolean()
		{
			// TODO
		}

		[Fact]
		public void IsChar()
		{
			// TODO
		}

		[Fact]
		public void IsByte()
		{
			// TODO
		}

		[Fact]
		public void IsShort()
		{
			// TODO
		}

		[Fact]
		public void IsInteger()
		{
			// TODO
		}

		[Fact]
		public void IsLong()
		{
			// TODO
		}

		[Fact]
		public void IsFloat()
		{
			// TODO
		}

		[Fact]
		public void IsDouble()
		{
			// TODO
		}

		[Fact]
		public void IsDecimal()
		{
			// TODO
		}

		[Fact]
		public void IsTaiwanIdNumber()
		{
			// TODO
		}

		[Fact]
		public void IsEmail()
		{
			// TODO
		}

		[Fact]
		public void IsIp()
		{
			// TODO
		}

		[Fact]
		public void IsLoopBackIp()
		{
			// TODO
		}

		[Fact]
		public void IpToLong()
		{
			// TODO
		}

		[Fact]
		public void IsPrivateIp()
		{
			// TODO
		}

		[Fact]
		public void IsNullIp()
		{
			// TODO
		}

		[Fact]
		public void IsAlphabet()
		{
			// TODO
		}

		[Fact]
		public void IsNumeric()
		{
			// TODO
		}

		[Fact]
		public void IsAlphaNumeric()
		{
			// TODO
		}

		[Fact]
		public void IsUri()
		{
			// TODO
		}

		[Fact]
		public void Like()
		{
			// TODO
		}
	}
}
