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
			TestLambda ("0: define", "UF () => x=1+2", "User function UF is defined");
			TestLambda ("1: define as block", "UF () => {x=1+2; x = x-1}", "User function UF is defined");
			TestLambda ("2: call", "UF () => x=1+2; UF ()", "x = 3"); 
			TestLambda ("3: call twice", "x=1; UF () => x=x+2; UF (); UF ()", "x = 5"); 


		}
	}
}

