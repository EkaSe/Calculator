using System;
using Calculator.Logic;
using static Calculator.Logic.Interpreter;
using static Calculator.Logic.Parser;

namespace Calculator.UI
{
	public class CalculatorUI
	{
		static public void ConsoleCalculator () {

			Func<string> getExpression = () => {
				Console.Write(">> ");
				return Console.ReadLine ();
			};

			Func<string, bool> outputAction = (output) => {
				if (output != "q") {
					Console.WriteLine ("= " + output);
					return false;
				} else
					return true;
			};

			Func<string, double> getValueByAliasUI = (newAlias) => {
				bool isValueReceived = false;
				double value = 0;
				while (!isValueReceived) {
					Console.WriteLine ("Enter value of variable " + newAlias);
					string aliasValueString = Console.ReadLine ();
					int inputIsDouble = 0;
					double aliasValue = StringToDouble (aliasValueString, out inputIsDouble);
					if (inputIsDouble == 1) {
						value = aliasValue;
						isValueReceived = true;
					} else {
						Console.WriteLine ("Invalid input");
					}
				}
				return value;
			};

			Console.WriteLine ("Enter expression for calculation (q to quit)");
			Interpreter.Run (getExpression, outputAction, getValueByAliasUI);
		}
	}
}

