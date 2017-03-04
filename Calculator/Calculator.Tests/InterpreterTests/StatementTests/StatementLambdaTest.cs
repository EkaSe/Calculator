using System;

namespace Calculator.Tests
{
	public class StatementLambdaTest
	{
		static public void TestLambda (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestLambda (name, input, expectedOutput);
		}

		static public void TestLambda (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Lambda " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		static public void Run () {
			TestLambda ("0", "UF () => x=1+2", "User function UF is defined");
			TestLambda ("1", "UF () => {x=1+2; x = x-1}", "User function UF is defined");


		}
	}
}

