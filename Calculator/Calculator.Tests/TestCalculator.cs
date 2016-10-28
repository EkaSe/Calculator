using System;
using static Calculator.Logic.Calculation;

namespace Calculator
{
	public class TestCalculator
	{
		static public void CalculatorTest (string input, string expectedOutput)
		{
			string result = ProcessExpression (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " passed");
			else Console.WriteLine ("Test " + input + " = " + expectedOutput + " failed");
		}

		static public void FindOperandTest (string input, int startPosition, int expectedPosition, double expectedResult)
		{
			double result;
			int foundPosition = FindOperand (input, startPosition, out result);
			if ((foundPosition == expectedPosition) && (result == expectedResult))
				Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " passed");
			else Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
				+ " is " + expectedResult + " failed");
		}

		static public void FindOperatorTest (string input, int startPosition, int expectedPosition, OperatorCode expectedResult)
		{
			OperatorCode result = OperatorCode.plus;
			int foundPosition = FindOperator (input, startPosition, out result);
			if ((foundPosition == expectedPosition) && (result == expectedResult))
				Console.WriteLine ("Test: First operator in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " passed");
			else Console.WriteLine ("Test: First operator in " + input + " with start posiion " + startPosition 
				+ " is " + expectedResult + " failed");
		}

		static public void DoubleToStringTest (double number, string expectedResult) {
			string result = DoubleToString (number);
			if (result == expectedResult)
				Console.WriteLine ("Test: Double to string conversion of " + number + " passed");
			else Console.WriteLine ("Test: Double to string conversion of " + number + " failed");
		}

		static public void RunTests ()
		{
			DoubleToStringTest (-5.1540, "-5.154");
			DoubleToStringTest (-125, "-125");
			DoubleToStringTest (1234567890.123456, "1234567890.123456");

			Console.WriteLine ();
			CalculatorTest ("12", "12");
			CalculatorTest ("-12", "-12");
			CalculatorTest ("+12.34", "12.34");
			CalculatorTest ("2+3", "5");
			CalculatorTest ("10-5", "5");
			CalculatorTest ("7*8", "56");
			CalculatorTest ("32/8", "4");
			CalculatorTest ("2+8/2", "6");
			CalculatorTest ("(2+2)*2", "8");
			CalculatorTest ("1+(1+2*(3-2))", "4");
			CalculatorTest ("1 * 3 * 4", "12");
			CalculatorTest ("10/7*14", "20");
			CalculatorTest ("3^2^2", "81");
			CalculatorTest ("-7+3", "-4");
			CalculatorTest ("5.5+2.15", "7.65");
			CalculatorTest ("4^(1/2)", "2");
			CalculatorTest ("0.125^(-1/3)", "2");
			Console.WriteLine ();
			FindOperandTest ("5", 0, 0, 5);
			FindOperandTest ("10", 0, 1, 10);
			FindOperandTest ("05", 1, 1, 5);
			FindOperandTest ("0.1", 0, 2, 0.1);
			FindOperandTest ("+50-5", 3, 4, -5);
			FindOperandTest ("(17+2)*2", 0, 5, 19);
			FindOperandTest ("-10.258", 0, 6, -10.258);
			FindOperandTest ("15.8 * (-8.4 / 2.1)", 6, 18, -4);
			Console.WriteLine ();
			FindOperatorTest ("4 + 5", 0, 2, OperatorCode.plus);
			FindOperatorTest ("10", 0, -1, OperatorCode.unknown);
			FindOperatorTest ("-50*3", 0, 0, OperatorCode.minus);
			FindOperatorTest ("-50*3", 1, 3, OperatorCode.multiply);
			FindOperatorTest ("*-+/*", 3, 3, OperatorCode.divide);
			FindOperatorTest ("8!", 1, 1, OperatorCode.factorial);
			FindOperatorTest ("2.05^2", 1, 4, OperatorCode.degree);
		}
	}
}

