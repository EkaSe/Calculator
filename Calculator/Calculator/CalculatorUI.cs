using System;
using static Calculator.Logic.Calculation;
using static Calculator.Logic.Parser;

namespace Calculator.UI
{
	public class CalculatorUI
	{
		static public void ConsoleCalculator () {

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

			Console.WriteLine ("Enter expression for calculation");
			string input = Console.ReadLine ();
			string result = ProcessExpression (input, getValueByAliasUI);
			Console.WriteLine ("= " + result);
		}
	}
}

