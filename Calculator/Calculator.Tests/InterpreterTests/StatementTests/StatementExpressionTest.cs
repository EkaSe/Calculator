using System;

namespace Calculator.Tests
{
	public class StatementExpressionTest
	{
		static public void TestExpression (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestExpression (name, input, expectedOutput);
		}

		static public void TestExpression (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Expression " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		static public void Run () {
			TestExpression ("0", "", "");
			TestExpression ("1", "1+2", "3");
			TestExpression ("2", "0.1*10;", "");
			TestExpression ("3", "+12.34 \n  0-3!", "-6");
			TestExpression ("4", "1+(1+2*(3-2)); \n (5.5+2.15) / 0.5 ", "15.3");
			TestExpression ("5", "15-0.2 \n ", "");
			TestExpression ("6", "15-0.2; \n 000", "0");
		}
	}
}

