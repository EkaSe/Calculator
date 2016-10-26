using System;
using static Calculator.Logic.Calculation;

namespace Calculator.UI
{
	public class CalculatorUI
	{
		static public void ConsoleCalculator () {
			Console.WriteLine ("Enter expression for calculation");
			string input = Console.ReadLine ();
			string result = ProcessExpression (input);
			Console.WriteLine ("= " + result);
		}
	}
}

