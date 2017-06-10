using System;
using System.Reflection;
using Calculator.Logic;
using MyLibrary;

namespace Calculator.Tests
{
	[TestFixture]
	public class StatementSearcherTest
	{
		static void OnOutputMessage(string message)
		{
			EventHandler<string> handler = OutputMessage;
			if (handler != null)
			{
				handler(null, message);
			}
		}

		static public event EventHandler<string> OutputMessage;

		static public void SingleTest (string name, string input, string expectedOutput) {
			string result;
			try {
				result = StatementSearcher.Run (input).GetType ().ToString ();
			} catch (Exception e) {
				result = e.Message;
			}
			string testName = "Statement Searcher Test: " + name;
			string testResult;
			if (result == expectedOutput)
				testResult = testName + " passed";
			else {
				testResult = testName + " failed: " + result;
				TestCalculator.FailedTestsCount++;
			}
			OnOutputMessage (testResult);
		}

		static public void SingleTest (string input, string expectedOutput) {
			string name = expectedOutput.ToString ();
			SingleTest (name, input, expectedOutput);
		}

		[Test]
		[Covers (typeof (StatementSearcher), nameof (StatementSearcher.Run))]
		static public void Run () {
			SingleTest ("1+2", typeof (Expression).ToString ());
			SingleTest ("xy12_x=1", typeof (Assignment).ToString ());
			SingleTest ("x=1+sqrt(4)", typeof (Assignment).ToString ());
			SingleTest ("x=1+x", typeof (Assignment).ToString ());
			SingleTest ("var x", typeof (Declaration).ToString ());
			SingleTest ("var x=1", typeof (Assignment).ToString ());
			SingleTest ("{1+2;2/3;0}", typeof (Block).ToString ());
			SingleTest ("{1+2;2/3;x = 0;}", typeof (Block).ToString ());
			SingleTest ("var f = function () { return 0; }", typeof (Lambda).ToString ());
			SingleTest ("function f (arg) { return arg+1; }", typeof (Lambda).ToString ());
		}
	}
}

