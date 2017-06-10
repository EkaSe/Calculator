using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	[TestFixture]
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

		[Test]
		[Covers (typeof (Interpreter), nameof (Interpreter.Run))]
		[Covers (typeof (Lambda), "Execute")]
		static public void Run () {
			TestLambda ("0.0: ", 
				"var f = function (arg1, arg2) { return 0; }",
				"User function f is defined");
			TestLambda ("0.1: ", 
				"var f = function (x) { x = x*2; return x; }", 
				"User function f is defined");
			TestLambda ("1.0: ", 
				"var f = function (x) { var y = x*2; return y; }\n f(3)", 
				"6");
			TestLambda ("1.2: ", 
				"var f = function (x) { x = x*2; return x; }\n var y = f(3); y = f(y)", 
				"y = 12");
			TestLambda ("1.3: ", 
				"var f = function (x) { return sqrt(x); }\n var y = f(4)", 
				"y = 2");
			TestLambda ("2.0: ", 
				"function f (x) { return x; }", 
				"User function f is defined");
			TestLambda ("2.1: ", 
				"function f (x) { return x; }; f(-52)", 
				"-52");
		}
	}
}

