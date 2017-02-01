using System;
using System.Collections.Generic;
using System.Text;
using MyLibrary;

namespace Calculator.Logic
{
	public class Parser
	{
		static public int CharToDigit (char symbol) {
			int code = (int) symbol;
			int digit;
			if (code >= (int) '0' && code <= (int) '9')
				digit = code - (int) '0';
			else
				digit = -1;
			return digit;
		}

		static public string DoubleToString (double number) {
			StringBuilder result = new StringBuilder ();
			bool isNegative = false;
			bool isInt = true;
			if (number < 0) {
				isNegative = true;
				number = - number;
			}

			int integralPart = (int) number;
			int mantissa = (int) (long)(number * 1E9 - integralPart * 1E9);

			int counter = 20;
			if (mantissa != 0) {
				result.Insert (0, '.');
				isInt = false;
			}

			while ((int) mantissa * counter != 0) {
				int digit = mantissa % 10;
				result.Insert (1, (char) (digit + (int) '0'));
				mantissa = (mantissa- digit) / 10;
				counter--;
			}

			counter = 20;
			if (integralPart == 0)
				result.Insert (0, '0');
			while (integralPart * counter != 0) {
				int digit = integralPart % 10;
				result.Insert (0, (char) (digit + (int) '0'));
				integralPart = (integralPart - digit) / 10;
				counter--;
			}

			if (isNegative) 
				result.Insert (0, '-');

			if (!isInt) {
				while (result [result.Length - 1] == '0') {
					result.Remove (result.Length - 1, 1);
				}
			}

			return result.ToString ();
		}

		static public char ToLowerCase (char symbol) {
			int code = (int)symbol;
			if ((int)'A' <= code && code <= (int)'Z') 
				return (char) (code - (int) 'A' + (int) 'a');
			else
				return symbol;
		}

		static public string ToLowerCase (string input) {
			StringBuilder result = new StringBuilder ();
			for (int i = 0; i < input.Length; i++) {
				result.Append (ToLowerCase (input [i]));
			}
			return result.ToString ();
		}

		static public double StringToDouble (string input, out int outputCode) {
			double result = 0;
			outputCode = 1;
			bool isNegative = false;
			if (input == "") {
				outputCode = 0;
				return 0;
			}
			char symbol = input [0];
			if (symbol == '-') 
				isNegative = true;
			double mantissaLength = 1;
			for (int i = 0; i < input.Length; i++) {
				if (isNegative && i == 0)
					i++;
				symbol = input [i];
				int digit = CharToDigit (symbol);
				if (digit >= 0) {
					if (mantissaLength == 1)
						result = result * 10 + (double) digit;
					else {
						result = result + (double) digit * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					if ((symbol == '.' || symbol == ',') && mantissaLength == 1)
						mantissaLength = 0.1;
					else if (symbol != ' ') 
						outputCode = -1;
				}
			}
			if (isNegative)
				result = -result;
			return result;
		}

		static public double StringToDouble (string input) {
			int outputCodeUnused = 0;
			double result = StringToDouble (input, out outputCodeUnused);
			return result;
		}

		static public string SkipSpaces (string input) {
			StringBuilder result = new StringBuilder ();
			for (int i = 0; i < input.Length; i++) {
				if (input [i] != ' ')
					result.Append (input [i]);
			}
			return result.ToString ();
		}

		static public bool IsLetter (char symbol) {
			bool result = false;
			int code = (int)symbol;
			if ((int)'A' <= code && code <= (int)'Z' || (int)'a' <= code && code <= (int)'z')
				result = true;
			return result;
		}

		static public bool IsIdentifierChar (char symbol, bool isFirstChar) {
			bool result = false;
			if (CharToDigit (symbol) != -1 && !isFirstChar)
				result = true;
			if (IsLetter (symbol) || symbol == '_')
				result = true;
			return result;
		}

		static public int FindNumber (string input, int startPosition, out double number) {
			//reads double from start position
			number = 0;
			double currentDecimal = -1;
			double mantissaLength = 1;
			bool operandEnd = false;
			int i = startPosition;
			int endPosition = -1;
			while (!operandEnd) {
				char currentSymbol = input [i];
				int currentDigit = Parser.CharToDigit (currentSymbol);
				if (currentDigit >= 0) {
					currentDecimal = (double) currentDigit;
					if (mantissaLength == 1)
						number = number * 10 + currentDecimal;
					else {
						number = number + currentDecimal * mantissaLength;
						mantissaLength *= 0.1;
					}
				} else {
					if (currentSymbol == '.')
						mantissaLength = 0.1;
					else if (currentDecimal >= 0) {
						operandEnd = true;
						endPosition = i - 1;
					}
				}
				i++;
				if (i == input.Length && !operandEnd) {
					operandEnd = true;
					if (currentDecimal >= 0)
						endPosition = input.Length - 1;
				}
			}
			return endPosition;
		}

		static public int FindName (string input, int startPosition, out string name) {
			//reads alias from start position, else returns -1
			int endPosition = -1;
			name = null;
			StringBuilder alias = new StringBuilder ();
			bool aliasEnd = false;
			int i = startPosition;
			char currentSymbol = input [i];
			if (!IsIdentifierChar (currentSymbol, true))
				return -1;
			while (!aliasEnd) {
				if (i == input.Length)
					aliasEnd = true;
				else {
					currentSymbol = input [i];
					if (IsIdentifierChar (currentSymbol, false)) {
						alias.Append (currentSymbol);
						endPosition = i;
					} else
						aliasEnd = true;
				}
				i++;
			}
			name = alias.ToString ();
			return endPosition;
		}

		static public int FindClosing (string input, int startPosition, out string substring) {
			return FindClosing (input, startPosition, out substring, '(');
		}

		static public int FindClosing (string input, int startPosition, out string substring, char open) {
			char close;
			switch (open) {
			case '(':
				close = ')';
				break;
			case '{':
				close = '}';
				break;
			case '[':
				close = ']';
				break;
			case '<':
				close = '>';
				break;
			default:
				throw new Exception ("No such parenthesis type is defined");
			}
			int parenthesisCount = 1;
			int j = startPosition;
			substring = "";
			do {
				j++;
				if (j == input.Length)
					return -1;
				if (input [j] == close)
					parenthesisCount--;
				if (input [j] == open)
					parenthesisCount++;
				if (parenthesisCount < 0)
					return -1;
			} while (parenthesisCount != 0);
			substring = input.Substring (startPosition + 1, j - startPosition - 1);
			return j;
		}

		static public int[] TextPlateSize (string plate) {
			int x = 0;
			int y = 0;
			if (plate.Length == 0)
				return new int[] {0, 0};
			if (plate [plate.Length - 1] == '\n')
				plate = plate.Substring (0, plate.Length - 1);
			int lineStart = -1;
			do {
				int lineEnd = plate.IndexOf ("\n", lineStart + 1);
				if (lineEnd < 0)
					lineEnd = plate.Length;
				int lineLength = lineEnd - lineStart - 1;
				if (lineLength > x)
					x = lineLength;
				y++;
				lineStart = plate.IndexOf ("\n", lineStart + 1);
			} while (lineStart >= 0);
			return new int[] { x, y };
		}
			
		static public string AdjustPlate (string plate, int x, int y) {
			StringBuilder result = new StringBuilder ();
			int oldY = 0;
			int lineStart = -1;
			do {
				int lineEnd = plate.IndexOf ("\n", lineStart + 1);
				if (lineEnd < 0)
					lineEnd = plate.Length;
				int lineLength = lineEnd - lineStart - 1;
				result.Append (plate.Substring (lineStart + 1, lineLength));
				for (int i = lineLength; i < x; i++) {
					result.Append (" ");
				}
				oldY++;
				result.AppendLine();
				lineStart = plate.IndexOf ("\n", lineStart + 1);
			} while (lineStart >= 0);
			for (int i = oldY; i < y; i++) {
				for (int j = 0; j < x; j++) {
					result.Append (" ");
				}
				result.AppendLine ();
			}
			return result.ToString();
		}

		static public string AdjustPlate (string plate) {
			int[] size = TextPlateSize (plate);
			return AdjustPlate (plate, size [0], size [1]);
		}

		static public string CenterString (string title, int width) {
			StringBuilder result = new StringBuilder ();
			int length = title.Length;
			for (int i = 0; i < (width - length) / 2; i++) {
				result.Append (" ");
			}
			result.Append (title);
			for (int i = result.Length; i < width; i++) {
				result.Append (" ");
			}
			return result.ToString();
		}

		static public string ConcatPlates (string text1, string text2) {
			StringBuilder result = new StringBuilder ();
			int[] size1 = TextPlateSize (text1);
			int[] size2 = TextPlateSize (text2);
			int y = Math.Max (size1 [1], size2 [1]);
			string plate1 = AdjustPlate (text1, size1 [0], y);
			string plate2 = AdjustPlate (text2, size2 [0], y);
			if (size1 [0] == 0)
				return plate2;
			if (size2 [0] == 0)
				return plate1;
			for (int i = 0; i < y; i++) {
				result.Append (plate1.Substring (i * (size1[0] + 1), size1[0]));
				result.Append (" ");
				result.AppendLine (plate2.Substring (i * (size2[0] + 1), size2[0]));
			}
			return result.ToString();
		}
	}
}