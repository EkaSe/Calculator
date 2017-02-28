using System;

namespace Calculator.Tests
{
	public class StatementBlockTest
	{
		static public void TestBlock (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestBlock (name, input, expectedOutput);
		}

		static public void TestBlock (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Block " + name;
			TestCalculator.InterpreterTest (testName, input, expectedOutput);
		}

		static public void Run () {
			TestBlock ("0", "", "");
			/*
			expressionSet = new string[] {"{x = 1; y = 2; z = x + y }"};
			InterpreterTest ("Statement 2.0: Block", expressionSet, "x = 1, y = 2, z = 3");
			expressionSet = new string[] {"{x = 1 \n y = 2; z = x + y; }"};
			InterpreterTest ("Statement 2.1: Block", expressionSet, "x = 1, y = 2, z = 3,");*/

		}
	}
}

