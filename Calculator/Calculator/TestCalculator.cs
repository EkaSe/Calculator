using System;

namespace Calculator
{
	public class TestCalculator
	{
		static public void Execute (string input, string expectedOutput)
		{
			string result = Calculator.Calculate (input);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " passed");
			else Console.WriteLine ("Test " + input + " = " + expectedOutput + " failed");
		}

		static public void RunTests ()
		{
			Execute ("2+3", "5");
		}
	}
}

