using System;

namespace Calculator.Tests
{
	public class StatementInvalidTest
	{
		static public void TestInvalidInput (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestInvalidInput (name, input, expectedOutput);
		}

		static public void TestInvalidInput (string name, string[] input, string expectedOutput) {
			string testName = "Statement_InvalidInput " + name;
			TestCalculator.InterpreterTest (testName, input, expectedOutput);
		}

		static public void Run () {
			TestInvalidInput ("0", "", "");

		}
	}
}

