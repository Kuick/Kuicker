// Symbol.cs
//
// Copyright (c) Chung, Chun-Yi. All rights reserved.
// kevin@kuicker.org

using System;

namespace Kuicker
{
	public class Symbol
	{
		public static string[] All = new[]{
			Space, At, Asterisk, Ellipsis, Tilde, Percent, 
			Pound , GreaterThan, LessThan, Exclamation,
			Plus ,Dollar , Caret, Ampersand, OpenParenthesis,
			CloseParenthesis, OpenBracket, CloseBracket,
			OpenBrace, CloseBrace, Colon, Semicolon,
			Comma, Quotation, Quoter, BackQuoter, Slash,
			BackwardSlash, Equal, Period, Minus, UnderScore,
			Tab, NewLine, CarriageReturn, Question, Pipe
		};

		public const string Space = " ";
		public const string At = "@";
		public const string Asterisk = "*";
		public const string Ellipsis = "…";
		public const string Tilde = "~";
		public const string Percent = "%";
		public const string Pound = "#";
		public const string GreaterThan = ">";
		public const string LessThan = "<";
		public const string Exclamation = "!";
		public const string Plus = "+";
		public const string Dollar = "$";
		public const string Caret = "^";
		public const string Ampersand = "&";
		public const string OpenParenthesis = "(";
		public const string CloseParenthesis = ")";
		public const string OpenBracket = "[";
		public const string CloseBracket = "]";
		public const string OpenBrace = "{";
		public const string CloseBrace = "}";
		public const string Colon = ":";
		public const string Semicolon = ";";
		public const string Comma = ",";
		public const string Quotation = "\"";
		public const string Quoter = "'";
		public const string BackQuoter = "`";
		public const string Slash = "/";
		public const string BackwardSlash = "\\";
		public const string Equal = "=";
		public const string Period = ".";
		public const string Minus = "-";
		public const string UnderScore = "_";
		public const string Tab = "\t";
		public const string NewLine = "\n";
		public const string CarriageReturn = "\r";
		public const string Question = "?";
		public const string Pipe = "|";

		public static class Char
		{
			public static char[] All = new[] {
				Space, At, Asterisk, Ellipsis, Tilde, Percent, 
				Pound , GreaterThan, LessThan, Exclamation,
				Plus ,Dollar , Caret, Ampersand, OpenParenthesis,
				CloseParenthesis, OpenBracket, CloseBracket,
				OpenBrace, CloseBrace, Colon, Semicolon,
				Comma, Quotation, Quoter, BackQuoter, Slash,
				BackwardSlash, Equal, Period, Minus, UnderScore,
				Tab, NewLine, CarriageReturn, Question, Pipe
			};

			public const char Space = ' ';
			public const char At = '@';
			public const char Asterisk = '*';
			public const char Ellipsis = '…';
			public const char Tilde = '~';
			public const char Percent = '%';
			public const char Pound = '#';
			public const char GreaterThan = '>';
			public const char LessThan = '<';
			public const char Exclamation = '!';
			public const char Plus = '+';
			public const char Dollar = '$';
			public const char Caret = '^';
			public const char Ampersand = '&';
			public const char OpenParenthesis = '(';
			public const char CloseParenthesis = ')';
			public const char OpenBracket = '[';
			public const char CloseBracket = ']';
			public const char OpenBrace = '{';
			public const char CloseBrace = '}';
			public const char Colon = ':';
			public const char Semicolon = ';';
			public const char Comma = ',';
			public const char Quotation = '"';
			public const char Quoter = '\'';
			public const char BackQuoter = '`';
			public const char Slash = '/';
			public const char BackwardSlash = '\\';
			public const char Equal = '=';
			public const char Period = '.';
			public const char Minus = '-';
			public const char UnderScore = '_';
			public const char Tab = '\t';
			public const char NewLine = '\n';
			public const char CarriageReturn = '\r';
			public const char Question = '?';
			public const char Pipe = '|';
		}
	}
}
