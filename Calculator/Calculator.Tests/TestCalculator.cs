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

		static public void RunTests ()
		{
			CalculatorTest.Run ();
			StatementSearcherTest.Run ();
			StatementExpressionTest.Run ();
			StatementAssignmentTest.Run ();
			StatementBlockTest.Run ();
			StatementLambdaTest.Run ();
			StatementEmbeddedTest.Run ();
			StatementInvalidTest.Run ();

			Console.WriteLine ("\r\n" + FailedTestsCount + " tests failed");
		}
	}
}