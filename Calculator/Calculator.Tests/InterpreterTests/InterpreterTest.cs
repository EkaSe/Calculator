﻿using System;
using MyLibrary;
using Calculator.Logic;
using static Calculator.Logic.Parser;

namespace Calculator.Tests
{
	public class InterpreterTest
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

		static public void Run (string testName, string[] input, string expectedOutput) {
			string result = RunInterpreter (input);

			string testResult;
			if (result == expectedOutput)
				testResult = "Test " + testName + " passed";
			else {
				testResult = "Test " + testName + " failed: " + result;
				TestCalculator.FailedTestsCount++;
			}

			OnOutputMessage (testResult);
		}

		static public string RunInterpreter (string[] input) {
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
			return result;
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

