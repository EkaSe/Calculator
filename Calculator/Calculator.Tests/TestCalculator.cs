using System;
using Calculator.Logic;
using MyLibrary;
using static Calculator.Logic.Interpreter;
using static Calculator.Logic.Parser;

namespace Calculator.Tests
{
	public class TestCalculator
	{
		public static int FailedTestsCount = 0;

		static public Func <string, bool> OutputFunc = (string output) => {
			Console.WriteLine (output);
			return true;
		};

		static public void RunTests ()
		{
			TestHelper.SetOutputAction ();
			CalculatorTest.Run ();
			StatementSearcherTest.Run ();
			StatementExpressionTest.Run ();
			StatementAssignmentTest.Run ();
			StatementBlockTest.Run ();
			StatementLambdaTest.Run ();
			//StatementEmbeddedTest.Run ();
			//Console.WriteLine ();
			//StatementInvalidTest.Run ();
			//Console.WriteLine ();

			OutputFunc ("\r\n" + FailedTestsCount + " tests failed\n");

			MyCollectionTest.Run ();
		}
	}
}