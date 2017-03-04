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
			SingleTest ("", "Expression");
			SingleTest ("1+2", "Expression");
			SingleTest ("x=1", "Assignment");
			SingleTest ("x=x", "Assignment");
			SingleTest ("{1+2,2/3,x=0}", "Block");
			SingleTest ("UF()=>x=1", "Lambda");
			SingleTest ("x=unknown", "Invalid statement");
		}
	}
}

