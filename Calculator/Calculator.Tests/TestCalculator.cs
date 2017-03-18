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
			Console.WriteLine ();
			StatementSearcherTest.Run ();
			Console.WriteLine ();
			StatementExpressionTest.Run ();
			Console.WriteLine ();
			StatementAssignmentTest.Run ();
			Console.WriteLine ();
			StatementBlockTest.Run ();
			Console.WriteLine ();
			StatementLambdaTest.Run ();
			Console.WriteLine ();
			//StatementEmbeddedTest.Run ();
			//Console.WriteLine ();
			//StatementInvalidTest.Run ();
			//Console.WriteLine ();

			Console.WriteLine ("\r\n" + FailedTestsCount + " tests failed");
		}
	}
}