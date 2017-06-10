using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	[TestFixture]
	public class StatementInvalidTest
	{
		static public void TestInvalidInput (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestInvalidInput (name, input, expectedOutput);
		}

		static public void TestInvalidInput (string name, string[] input, string expectedOutput) {
			string testName = "Statement_InvalidInput " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		[Test]
		[Covers (typeof (Interpreter), nameof (Interpreter.Run))]
		[Covers (typeof (Statement), "Execute")]
		static public void Run () {
			TestInvalidInput ("0", "=>1", "Invalid statement");
			TestInvalidInput ("1", "x=", "Invalid statement");
			TestInvalidInput ("2", "x={1,2}", "Invalid statement");
		}
	}
}

