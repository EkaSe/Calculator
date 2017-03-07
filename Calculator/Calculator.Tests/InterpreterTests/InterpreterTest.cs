using System;
using Calculator.Logic;
using static Calculator.Logic.Parser;

namespace Calculator.Tests
{
	public class InterpreterTest
	{
		static public void Run (string testName, string[] input, string expectedOutput)
		{
			int expressionIndex = 0;
			string result = null;

			Func<string> getExpression = () => {
				if (expressionIndex < input.Length)
					return input [expressionIndex++];
				else return "q";
			};

			Func<string, bool> outputAction = (output) => {
				if (expressionIndex < input.Length && output != "q")
					return false;
				else {
					result = output;
					return true;
				}
			};

			Interpreter.Run (getExpression, outputAction, true);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + testName + " passed");
			else {
				Console.WriteLine ("Test " + testName + " failed: " + result);
				TestCalculator.FailedTestsCount++;
			}
		}
	}

	public class ParserTests {
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
				TestCalculator.FailedTestsCount++;
			}
		}

		static public void DoubleToStringTest (double number, string expectedResult) {
			string result = DoubleToString (number);
			if (result == expectedResult)
				Console.WriteLine ("Test: Double to string conversion of " + number + " passed");
			else {
				Console.WriteLine ("Test: Double to string conversion of " + number + " failed");
				TestCalculator.FailedTestsCount++;
			}
		}
	}
}

