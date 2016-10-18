using System;

namespace Calculator
{
	public class TestCalculator
	{
		static public void Execute (string input, string expectedOutput)
		{
			string result = Calculator.ProcessExpression (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " passed");
			else Console.WriteLine ("Test " + input + " = " + expectedOutput + " failed");
		}

		static public void RunTests ()
		{
			Execute ("12", "12");
			Execute ("-12", "-12");
			Execute ("+12.34", "12.34");
			Execute ("2+3", "5");
			Execute ("10-5", "5");
			Execute ("7*8", "56");
			Execute ("32/8", "4");
			Execute ("2+8/2", "6");
			Execute ("(2+2)*2", "8");
			Execute ("1+(1+2*(3-2))", "4");
			Execute ("1 * 3 * 4", "12");
			Execute ("10/7*14", "20");
			Execute ("3^2^2", "81");
			Execute ("-7+3", "-4");
			Execute ("5.5+2.15", "7.65");
			Execute ("4^(1/2)", "2");
			Execute ("0.125^(-1/3)", "2");
		}
	}
}

