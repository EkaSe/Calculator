using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	public class StatementSearcherTest
	{
		static public void SingleTest (string name, string input, string expectedOutput) {
			string result;
			try {
				result = StatementSearcher.Run (input).GetType ().ToString ();
			} catch (Exception e) {
				result = e.Message;
			}
			string testName = "Statement Searcher Test: " + name;
			if (result == expectedOutput)
				Console.WriteLine (testName + " passed");
			else {
				Console.WriteLine (testName + " failed: " + result);
				TestCalculator.FailedTestsCount++;
			}
		}

		static public void SingleTest (string input, string expectedOutput) {
			string name = expectedOutput.ToString ();
			SingleTest (name, input, expectedOutput);
		}

		static public void Run () {
			SingleTest ("", typeof (Expression).ToString ());
			SingleTest ("1+2", typeof (Expression).ToString ());
			SingleTest ("x=1", typeof (Assignment).ToString ());
			SingleTest ("x=x", typeof (Assignment).ToString ());
			SingleTest ("{1+2,2/3,x=0}", typeof (Block).ToString ());
			SingleTest ("UF()=>x=1", typeof (Lambda).ToString ());
			SingleTest ("x=unknown", "Invalid statement");
		}
	}
}

