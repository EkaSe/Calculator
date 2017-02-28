using System;
using Calculator.Logic;
using MyLibrary;
using static Calculator.Logic.Interpreter;
using static Calculator.Logic.Parser;

namespace Calculator
{
	public class TestCalculator
	{
		static int FailedTestsCount = 0;

		static public void CalculatorTest (string input, string expectedOutput)
		{
			string testName = input;
			string [] expressionSet = new string [] {input};
			InterpreterTest (testName, expressionSet, expectedOutput);
		}

		static public void InterpreterTest (string testName, string[] input, string expectedOutput)
		{
			int expressionIndex = 0;
			string result = null;

			Func<string> getExpression = () => {
				return input [expressionIndex++];
			};

			Func<string, bool> outputAction = (output) => {
				if (expressionIndex < input.Length && output != "q")
					return false;
				else {
					result = output;
					return true;
				}
			};

			Interpreter.Run (getExpression, outputAction);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + testName + " passed");
			else {
				Console.WriteLine ("Test " + testName + " failed: " + result);
				FailedTestsCount++;
			}
		}

		static public void FindOperandTest (string input, int startPosition, int expectedPosition, double expectedResult)
		{
			Token operand = null;
			int foundPosition = OperandSearch.Run (input, startPosition, out operand);
			double result = ((Operand)operand).Value;
			if (foundPosition == expectedPosition && result == expectedResult)
				Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " passed");
			else {
				Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " failed");
				FailedTestsCount++;
			}
		}

		/*
		static public void FindOperatorTest (string input, int startPosition, int expectedPosition, OperatorCode expectedResult)
		{
			OperatorCode result = OperatorCode.plus;
			int foundPosition = FindOperator (input, startPosition, out result);
			if (foundPosition == expectedPosition && result == expectedResult)
				Console.WriteLine ("Test: First operator in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " passed");
			else {
				Console.WriteLine ("Test: First operator in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " failed");
				FailedTestsCount++;
			}
		}
		*/

		static public void DoubleToStringTest (double number, string expectedResult) {
			string result = DoubleToString (number);
			if (result == expectedResult)
				Console.WriteLine ("Test: Double to string conversion of " + number + " passed");
			else {
				Console.WriteLine ("Test: Double to string conversion of " + number + " failed");
				FailedTestsCount++;
			}
		}

		static public void RunTests ()
		{
			CalculatorTest ("12", "12");
			CalculatorTest ("-12", "-12");
			CalculatorTest ("+12.34", "12.34");
			CalculatorTest ("2+3", "5");
			CalculatorTest ("2-2", "0");
			CalculatorTest ("10-5", "5");
			CalculatorTest ("7*8", "56");
			CalculatorTest ("32/8", "4");
			CalculatorTest ("2+8/2", "6");
			CalculatorTest ("(2+2)*2", "8");
			CalculatorTest ("1+(1+2*(3-2))", "4");
			CalculatorTest ("1 * 3 * 4", "12");
			CalculatorTest ("10/7*14", "20");
			CalculatorTest ("5!", "120");
			/*?*/CalculatorTest ("0.5!", "");
			CalculatorTest ("-7+3", "-4");
			CalculatorTest ("5.5+2.15", "7.65");
			//CalculatorTest ("4^(1/2)", "2");
			//CalculatorTest ("0.125^(-1/3)", "2");

			Console.WriteLine ();
			string[] expressionSet;

			expressionSet = new string[] {""};
			InterpreterTest ("Statement 0.0", expressionSet, "");
			expressionSet = new string[] {"1+2"};
			InterpreterTest ("Statement 0.1", expressionSet, "3");
			expressionSet = new string[] {"1+2;"};
			InterpreterTest ("Statement 0.2", expressionSet, "");
			/*?*/expressionSet = new string[] {"-12; 0.1*10"};
			InterpreterTest ("Statement 0.3", expressionSet, "1");
			/*?*/expressionSet = new string[] {"+12.34 \n  0-3!"};
			InterpreterTest ("Statement 0.4", expressionSet, "-6");
			/*?*/expressionSet = new string[] {"1+(1+2*(3-2)); \n (5.5+2.15) / 0.5 "};
			InterpreterTest ("Statement 0.5", expressionSet, "15.3");
			/*?*/expressionSet = new string[] {"15-0.2 \n "};
			InterpreterTest ("Statement 0.6", expressionSet, "");
			/*?*/expressionSet = new string[] {"15-0.2; \n 000"};
			InterpreterTest ("Statement 0.7", expressionSet, "0");

			expressionSet = new string[] {"var=1+2"};
			InterpreterTest ("Statement 1.0: Assignment", expressionSet, "var = 3");
			expressionSet = new string[] {"x=3","x-1"};
			InterpreterTest ("Statement 1.1: Assignment", expressionSet, "2");
			expressionSet = new string[] {"x=3; x=x-1"};
			InterpreterTest ("Statement 1.2: Assignment", expressionSet, "x = 2");

			expressionSet = new string[] {"{x = 1; y = 2; z = x + y }"};
			InterpreterTest ("Statement 2.0: Block", expressionSet, "x = 1, y = 2, z = 3");
			/*?*/expressionSet = new string[] {"{x = 1 \n y = 2; z = x + y; }"};
			InterpreterTest ("Statement 2.1: Block", expressionSet, "x = 1, y = 2, z = 3,");

			expressionSet = new string[] {"UF () => x=1+2"};
			InterpreterTest ("Statement 3.0: Lambda", expressionSet, "User function UF is defined");
			expressionSet = new string[] {"UF () => {x=1+2; x = x-1}"};
			InterpreterTest ("Statement 3.1: Lambda", expressionSet, "User function UF is defined");

			expressionSet = new string[] {""};
			InterpreterTest ("Statement 4.0: Incorrect input", expressionSet, "");

			expressionSet = new string[] {"{{x=3}; {y = mIn (x, 10) \n}}"};
			InterpreterTest ("Statement 5.0: Nested", expressionSet, "x = 3");




			expressionSet = new string[] {"x=3","x-1"};
			InterpreterTest ("Interpreter 1:", expressionSet, "2");
			expressionSet = new string[] {"x = x - 3"};
			InterpreterTest ("Interpreter 2: x = x -3 ", expressionSet, "Invalid expression: Cannot assign value to x");
			expressionSet = new string[] {"/"};
			InterpreterTest ("Interpreter 3: no operand ", expressionSet, "Invalid expression: no operand found");
			expressionSet = new string[] {"x = 3 / 2","y=x/(-3)","a = 2*(x+3)-x/y"};
			InterpreterTest ("Interpreter 4:", expressionSet, "a = 12");
			expressionSet = new string[] {"x1=3","x1=x1-1"};
			InterpreterTest ("Interpreter 5:", expressionSet, "x1 = 2");

			expressionSet = new string[] {"x=3","y = mIn (x, 10)"};
			InterpreterTest ("Interpreter 6:", expressionSet, "y = 3");
			expressionSet = new string[] {"x= - sqrt (9)","max (-x, max (1, 2))"};
			InterpreterTest ("Interpreter 7:", expressionSet, "3");

			expressionSet = new string[] {"UF {x=3 \n y = mIn (x, 10)}", "UF", "y"};
			InterpreterTest ("Interpreter 8: Lambda", expressionSet, "3");

			/*Console.WriteLine ();
			Expression testTree = new ExpressionBuilder ("2+3").ToExpression ();
			Console.Write (testTree.Draw ());
			testTree = new ExpressionBuilder ("1+(2+3*(4-5))").ToExpression ();
			Console.Write (testTree.Draw ());
			testTree = new ExpressionBuilder ("min (6!, 2)").ToExpression ();
			Console.Write (testTree.Draw ());
			testTree = new ExpressionBuilder ("max (1+(2+3*(4-5)), 6!, sqrt (-8))").ToExpression ();
			Console.Write (testTree.Draw ());*/

			//MyCollectionTest.MyListTest ();

			Console.WriteLine ("\r\n" + FailedTestsCount + " tests failed");
		}
	}
}