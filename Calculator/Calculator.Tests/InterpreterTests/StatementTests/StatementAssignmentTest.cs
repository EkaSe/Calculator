using System;

namespace Calculator.Tests
{
	public class StatementAssignmentTest
	{
		static public void TestAssignment (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestAssignment (name, input, expectedOutput);
		}

		static public void TestAssignment (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Assignment " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		static public void Run () {
			TestAssignment ("0", "var x=1+2", "x = 3");
			TestAssignment ("1", new string[] {"var x=3","x-1"}, "2");
			TestAssignment ("2", "var x=3; x=x-1", "x = 2");
			TestAssignment ("3", "x = 1", "Invalid statement");
			TestAssignment ("4", "var x = x = 1", "Invalid statement");

		}
	}
}

