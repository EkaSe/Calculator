using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	[TestFixture]
	public class StatementBlockTest
	{
		static public void TestBlock (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestBlock (name, input, expectedOutput);
		}

		static public void TestBlock (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Block " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		[Test]
		[Covers (typeof (Interpreter), nameof (Interpreter.Run), 
			new Type[] { typeof (Func<string>), typeof (Func<string, bool>), typeof (bool)})]
		[Covers (typeof (Block), "Execute")]
		static public void Run () {
			TestBlock ("0", "{1; 1 + 2; -5*(0.5/0.1) }", "1, 3, -25");
			TestBlock ("1", "{x = 1 \n y = 2; z = x + y }", "x = 1, y = 2, z = 3");
			TestBlock ("2", "{x = 1 \n ;; x = x-1}", "x = 1, x = 0");
			TestBlock ("3", "{x = 1 \n x = x-1};", "");
		}
	}
}

