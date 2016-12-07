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

		static public void CalculatorTest (string input, double[] aliasValuesArray, string expectedOutput)
		{

			int aliasIndex = 0;

			Func<string, double> getValueByAliasTest = (newAlias) => {
				double value = aliasValuesArray [aliasIndex];
				aliasIndex++;
				return value;
			};

			string result = ProcessExpression (input, getValueByAliasTest);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " passed");
			else {
				Console.WriteLine ("Test " + input + " = " + expectedOutput + " failed");
				FailedTestsCount++;
			}
		}

		static public void InterpreterTest (string testName, string[] input, double[] aliasValuesArray, string expectedOutput)
		{

			int aliasIndex = 0;
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

			Func<string, double> getValueByAliasTest = (newAlias) => {
				double value = aliasValuesArray [aliasIndex];
				aliasIndex++;
				return value;
			};

			Interpreter.Run (getExpression, outputAction, getValueByAliasTest);
			if (result == expectedOutput)
				Console.WriteLine ("Test " + testName + " passed");
			else {
				Console.WriteLine ("Test " + testName + " failed");
				FailedTestsCount++;
			}
		}

		static public void FindOperandTest (string input, int startPosition, double[] aliasValuesArray, int expectedPosition, double expectedResult)
		{
			int aliasIndex = 0;
			Func<string, double> getValueByAliasTest = (newAlias) => {
				double value = aliasValuesArray [aliasIndex];
				aliasIndex++;
				return value;
			};

			double result;
			int foundPosition = FindOperand (input, startPosition, out result, getValueByAliasTest);
			if (foundPosition == expectedPosition && result == expectedResult)
				Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
					+ " is " + expectedResult + " passed");
			else {
				Console.WriteLine ("Test: First operand in " + input + " with start posiion " + startPosition 
				+ " is " + expectedResult + " failed");
				FailedTestsCount++;
			}
		}

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
			DoubleToStringTest (-5.1540, "-5.154");
			DoubleToStringTest (-125, "-125");
			DoubleToStringTest (1234567890.123456, "1234567890.123456");

			Console.WriteLine ();
			double[] aliasTestValues = new double[] { }; 
			CalculatorTest ("12", aliasTestValues, "12");
			CalculatorTest ("-12", aliasTestValues, "-12");
			CalculatorTest ("+12.34", aliasTestValues, "12.34");
			CalculatorTest ("2+3", aliasTestValues, "5");
			CalculatorTest ("2-2", aliasTestValues, "0");
			CalculatorTest ("10-5", aliasTestValues, "5");
			CalculatorTest ("7*8", aliasTestValues, "56");
			CalculatorTest ("32/8", aliasTestValues, "4");
			CalculatorTest ("2+8/2", aliasTestValues, "6");
			CalculatorTest ("(2+2)*2", aliasTestValues, "8");
			CalculatorTest ("1+(1+2*(3-2))", aliasTestValues, "4");
			CalculatorTest ("1 * 3 * 4", aliasTestValues, "12");
			CalculatorTest ("10/7*14", aliasTestValues, "20");
			//CalculatorTest ("3^2^2", aliasTestValues, "81");
			CalculatorTest ("-7+3", aliasTestValues, "-4");
			CalculatorTest ("5.5+2.15", aliasTestValues, "7.65");
			//CalculatorTest ("4^(1/2)", aliasTestValues, "2");
			//CalculatorTest ("0.125^(-1/3)", aliasTestValues, "2");
			aliasTestValues = new double[] {1.5, -0.5};
			CalculatorTest ("2*(x+3)-x/y", aliasTestValues, "12");

			Console.WriteLine ();
			aliasTestValues = new double[] { }; 
			string[] expressionSet = new string[] {"x=3","x-1"};
			InterpreterTest ("Interpreter 1:", expressionSet, aliasTestValues, "2");
			expressionSet = new string[] {"x = x - 3"};
			InterpreterTest ("Interpreter 2: x = x -3 ", expressionSet, aliasTestValues, "Invalid expression: Cannot assign value to x");
			expressionSet = new string[] {"/"};
			InterpreterTest ("Interpreter 3: no operand ", expressionSet, aliasTestValues, "Invalid expression: no operand found");
			expressionSet = new string[] {"a = 2*(x+3)-x/y"};
			aliasTestValues = new double[] {1.5, -0.5};
			InterpreterTest ("Interpreter 4:", expressionSet, aliasTestValues, "a = 12");

			Console.WriteLine ();
			FindOperandTest ("5", 0, aliasTestValues, 0, 5);
			FindOperandTest ("10", 0, aliasTestValues, 1, 10);
			FindOperandTest ("05", 1, aliasTestValues, 1, 5);
			FindOperandTest ("0.1", 0, aliasTestValues, 2, 0.1);
			FindOperandTest ("+50-5", 3, aliasTestValues, 4, -5);
			FindOperandTest ("(17+2)*2", 0, aliasTestValues, 5, 19);
			FindOperandTest ("-10.258", 0, aliasTestValues, 6, -10.258);
			FindOperandTest ("15.8 * (-8.4 / 2.1)", 6, aliasTestValues, 18, -4);
			Console.WriteLine ();
			FindOperatorTest ("4 + 5", 0, 2, OperatorCode.plus);
			FindOperatorTest ("10", 0, -1, OperatorCode.unknown);
			FindOperatorTest ("-50*3", 0, 0, OperatorCode.minus);
			FindOperatorTest ("-50*3", 1, 3, OperatorCode.multiply);
			FindOperatorTest ("*-+/*", 3, 3, OperatorCode.divide);
			FindOperatorTest ("8!", 1, 1, OperatorCode.factorial);
			FindOperatorTest ("2.05^2", 1, 4, OperatorCode.degree);

			MyCollectionTest.MyListTest ();

			Console.WriteLine ("\r\n" + FailedTestsCount + " tests failed");
		}
	}
}

