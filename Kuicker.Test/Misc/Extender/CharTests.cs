using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Kuicker.Test
{
	public class CharTests
	{
		public static IEnumerable<char> Chars
		{
			get
			{
				return new char[] {
					default(char),
					char.MinValue,
					char.MaxValue,
					'a',
					'8',
					'/',
					'#',
					'_',
					'-',
					'+',
					'.',
					' ',
					'	',
					'\r',
					'\n',
					'K',
				};
			}
		}

		[Fact]
		public void IsUpper()
		{
			Assert.False(default(char).IsUpper());
			Assert.False(char.MinValue.IsUpper());
			Assert.False(char.MaxValue.IsUpper());
			Assert.False('a'.IsUpper());
			Assert.False('8'.IsUpper());
			Assert.False('/'.IsUpper());
			Assert.False('#'.IsUpper());
			Assert.False('_'.IsUpper());
			Assert.False('-'.IsUpper());
			Assert.False('+'.IsUpper());
			Assert.False('.'.IsUpper());
			Assert.False(' '.IsUpper());
			Assert.False('	'.IsUpper());
			Assert.False('\r'.IsUpper());
			Assert.False('\n'.IsUpper());
			Assert.True('K'.IsUpper());
		}

		[Fact]
		public void IsLower()
		{
			Assert.False(default(char).IsLower());
			Assert.False(char.MinValue.IsLower());
			Assert.False(char.MaxValue.IsLower());
			Assert.True('a'.IsLower());
			Assert.False('8'.IsLower());
			Assert.False('/'.IsLower());
			Assert.False('#'.IsLower());
			Assert.False('_'.IsLower());
			Assert.False('-'.IsLower());
			Assert.False('+'.IsLower());
			Assert.False('.'.IsLower());
			Assert.False(' '.IsLower());
			Assert.False('	'.IsLower());
			Assert.False('\r'.IsLower());
			Assert.False('\n'.IsLower());
			Assert.False('K'.IsLower());
		}

		[Fact]
		public void IsDigit()
		{
			Assert.False(default(char).IsDigit());
			Assert.False(char.MinValue.IsDigit());
			Assert.False(char.MaxValue.IsDigit());
			Assert.False('a'.IsDigit());
			Assert.True('8'.IsDigit());
			Assert.False('/'.IsDigit());
			Assert.False('#'.IsDigit());
			Assert.False('_'.IsDigit());
			Assert.False('-'.IsDigit());
			Assert.False('+'.IsDigit());
			Assert.False('.'.IsDigit());
			Assert.False(' '.IsDigit());
			Assert.False('	'.IsDigit());
			Assert.False('\r'.IsDigit());
			Assert.False('\n'.IsDigit());
			Assert.False('K'.IsDigit());
		}

		[Fact]
		public void IsLetter()
		{
			Assert.False(default(char).IsLetter());
			Assert.False(char.MinValue.IsLetter());
			Assert.False(char.MaxValue.IsLetter());
			Assert.True('a'.IsLetter());
			Assert.False('8'.IsLetter());
			Assert.False('/'.IsLetter());
			Assert.False('#'.IsLetter());
			Assert.False('_'.IsLetter());
			Assert.False('-'.IsLetter());
			Assert.False('+'.IsLetter());
			Assert.False('.'.IsLetter());
			Assert.False(' '.IsLetter());
			Assert.False('	'.IsLetter());
			Assert.False('\r'.IsLetter());
			Assert.False('\n'.IsLetter());
			Assert.True('K'.IsLetter());
		}

		[Fact]
		public void IsLetterOrDigit()
		{
			Assert.False(default(char).IsLetterOrDigit());
			Assert.False(char.MinValue.IsLetterOrDigit());
			Assert.False(char.MaxValue.IsLetterOrDigit());
			Assert.True('a'.IsLetterOrDigit());
			Assert.True('8'.IsLetterOrDigit());
			Assert.False('/'.IsLetterOrDigit());
			Assert.False('#'.IsLetterOrDigit());
			Assert.False('_'.IsLetterOrDigit());
			Assert.False('-'.IsLetterOrDigit());
			Assert.False('+'.IsLetterOrDigit());
			Assert.False('.'.IsLetterOrDigit());
			Assert.False(' '.IsLetterOrDigit());
			Assert.False('	'.IsLetterOrDigit());
			Assert.False('\r'.IsLetterOrDigit());
			Assert.False('\n'.IsLetterOrDigit());
			Assert.True('K'.IsLetterOrDigit());
		}

		[Fact]
		public void IsNumber()
		{
			Assert.False(default(char).IsNumber());
			Assert.False(char.MinValue.IsNumber());
			Assert.False(char.MaxValue.IsNumber());
			Assert.False('a'.IsNumber());
			Assert.True('8'.IsNumber());
			Assert.False('/'.IsNumber());
			Assert.False('#'.IsNumber());
			Assert.False('_'.IsNumber());
			Assert.False('-'.IsNumber());
			Assert.False('+'.IsNumber());
			Assert.False('.'.IsNumber());
			Assert.False(' '.IsNumber());
			Assert.False('	'.IsLower());
			Assert.False('\r'.IsLower());
			Assert.False('\n'.IsLower());
			Assert.False('K'.IsLower());
		}

		[Fact]
		public void IsWhiteSpace()
		{
			Assert.False(default(char).IsWhiteSpace());
			Assert.False(char.MinValue.IsWhiteSpace());
			Assert.False(char.MaxValue.IsWhiteSpace());
			Assert.False('a'.IsWhiteSpace());
			Assert.False('8'.IsWhiteSpace());
			Assert.False('/'.IsWhiteSpace());
			Assert.False('#'.IsWhiteSpace());
			Assert.False('_'.IsWhiteSpace());
			Assert.False('-'.IsWhiteSpace());
			Assert.False('+'.IsWhiteSpace());
			Assert.False('.'.IsWhiteSpace());
			Assert.True(' '.IsWhiteSpace());
			Assert.True('	'.IsWhiteSpace());
			Assert.True('\r'.IsWhiteSpace());
			Assert.True('\n'.IsWhiteSpace());
			Assert.False('K'.IsWhiteSpace());
		}

		[Fact]
		public void IsUnderScore()
		{
			Assert.False(default(char).IsUnderScore());
			Assert.False(char.MinValue.IsUnderScore());
			Assert.False(char.MaxValue.IsUnderScore());
			Assert.False('a'.IsUnderScore());
			Assert.False('8'.IsUnderScore());
			Assert.False('/'.IsUnderScore());
			Assert.False('#'.IsUnderScore());
			Assert.True('_'.IsUnderScore());
			Assert.False('-'.IsUnderScore());
			Assert.False('+'.IsUnderScore());
			Assert.False('.'.IsUnderScore());
			Assert.False(' '.IsUnderScore());
			Assert.False('	'.IsUnderScore());
			Assert.False('\r'.IsUnderScore());
			Assert.False('\n'.IsUnderScore());
			Assert.False('K'.IsUnderScore());
		}
	}
}
