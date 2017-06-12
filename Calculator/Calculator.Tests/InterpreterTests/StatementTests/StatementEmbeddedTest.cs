using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	[TestFixture]
	public class StatementEmbeddedTest
	{
		static public void TestEmbedded (string name, string singleInput, string expectedOutput) {
			string[] input = new string[] { singleInput };
			TestEmbedded (name, input, expectedOutput);
		}

		static public void TestEmbedded (string name, string[] input, string expectedOutput) {
			string testName = "Statement_Embedded " + name;
			InterpreterTest.Run (testName, input, expectedOutput);
		}

		[Test]
		[Covers (typeof (Interpreter), nameof (Interpreter.Run), 
			new Type[] { typeof (Func<string>), typeof (Func<string, bool>), typeof (bool)})]
		[Covers (typeof (Block), "Execute")]
		static public void Run () {
			TestEmbedded ("0: Block in Block", "{{x=3}; {y = mIn (x, 10) \n}}", "x = 3, y = 3");

		}
	}
}

