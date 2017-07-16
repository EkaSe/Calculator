using System;
using Calculator.Logic;

namespace Calculator.Tests
{
	[TestFixture (typeof (Interpreter))]
	public class CalculatorTest
	{
		static public void SingleTest (string input, string expectedOutput)
		{
			string testName = input;
			string [] expressionSet = new string [] {input};
			InterpreterTest.Run (testName, expressionSet, expectedOutput);
		}

		[Test]
		[Covers (nameof (Interpreter.Run), 
			new Type[] { typeof (Func<string>), typeof (Func<string, bool>), typeof (bool)})]
		static public void Run () {
			SingleTest ("12", "12");
			SingleTest ("-12", "-12");
			SingleTest ("+12.34", "12.34");
			SingleTest ("2+3", "5");
			SingleTest ("2-2", "0");
			SingleTest ("10-5", "5");
			SingleTest ("7*8", "56");
			SingleTest ("32/8", "4");
			SingleTest ("2+8/2", "6");
			SingleTest ("(2+2)*2", "8");
			SingleTest ("1+(1+2*(3-2))", "4");
			SingleTest ("1 * 3 * 4", "12");
			SingleTest ("10/7*14", "20");
			SingleTest ("5!", "120");
			SingleTest ("0.5!", "Invalid expression: Factorial is defined only for non-negative integers");
			SingleTest ("-7+3", "-4");
			SingleTest ("5.5+2.15", "7.65");
			//CalculatorTest ("4^(1/2)", "2");
			//CalculatorTest ("0.125^(-1/3)", "2");
		}

		[Test]
		[Covers (nameof (Interpreter.Run), 
			new Type[] { typeof (Func<string>), typeof (Func<string, bool>), typeof (bool)})]
		[TestCase ("12", "12")]
		[TestCase ("-12", "-12")]
		[TestCase ("+12.34", "12.34")]
		[TestCase ("2+3", "5")]
		[TestCase ("2-2", "0")]
		[TestCase ("10-5", "5")]
		[TestCase ("7*8", "56")]
		[TestCase ("32/8", "4")]
		[TestCase ("2+8/2", "6")]
		[TestCase ("(2+2)*2", "8")]
		[TestCase ("1+(1+2*(3-2))", "4")]
		[TestCase ("1 * 3 * 4", "12")]
		[TestCase ("10/7*14", "20")]
		[TestCase ("5!", "120")]
		[TestCase ("0.5!", "Invalid expression: Factorial is defined only for non-negative integers")]
		[TestCase ("-7+3", "-4")]
		[TestCase ("5.5+2.15", "7.65")]
		public static void ShouldCalculateSingleExpression (string input, string expectedOutput) {
			string [] expressionSet = new string [] {input};
			string result = InterpreterTest.RunInterpreter (expressionSet);
			result.ShouldBeEqual (expectedOutput);
		}
	}
}

