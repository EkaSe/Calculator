using System;

namespace Calculator.Tests
{
	public class StatementEmbeddedTest
	{
		static public void TestEmbedded (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestEmbedded (name, input, expectedOutput);
		}

		static public void TestEmbedded (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Embedded " + name;
			TestCalculator.InterpreterTest (testName, input, expectedOutput);
		}

		static public void Run () {
			TestEmbedded ("0", "{{x=3}; {y = mIn (x, 10) \n}}", "x = 3");

		}
	}
}

