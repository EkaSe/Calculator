using System;

namespace Calculator
{
	public class TestCalculator
	{
		static public void CalculatorTest (string input, string expectedOutput)
		{
			string result = Calculator.ProcessExpression (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " passed");
			else Console.WriteLine ("Test " + input + " = " + expectedOutput + " failed");
		}

		static public void FindOperandTest (string input, double expectedOutput)
		{
			double result = Calculator.FindOperand (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test: First operand in " + input + " is " + expectedOutput + " passed");
			else Console.WriteLine ("Test: First operand in " + input + " is " + expectedOutput + " failed");
		}

		static public void FindOperatorTest (string input, int expectedOutput)
		{
			int result = Calculator.FindOperator (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test: First operator in " + input + " is " + expectedOutput + " passed");
			else Console.WriteLine ("Test: First operator in " + input + " is " + expectedOutput + " failed");
		}

		static public void RunTests ()
		{
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

			FindOperandTest ("5", 5);
			FindOperandTest ("10", 10);
			FindOperandTest ("05", 5);
			FindOperandTest ("0.1", 0.1);
			FindOperandTest ("+50-5", 50);
			FindOperandTest ("(17+2)*2", 17);
			FindOperandTest ("-10.258", 10.258);

			FindOperatorTest ("4 + 5", (int) Calculator.OperatorCode.plus);
			FindOperatorTest ("10", -1);
			FindOperatorTest ("-50*3", (int) Calculator.OperatorCode.minus);
			FindOperatorTest ("(5/3", (int) Calculator.OperatorCode.divide);
			FindOperatorTest ("*-+/", (int) Calculator.OperatorCode.multiply);
			FindOperatorTest ("8!", (int) Calculator.OperatorCode.factorial);
			FindOperatorTest ("2.05^2", (int) Calculator.OperatorCode.degree);
		}
	}
}

