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

			Console.WriteLine ("Enter expression for calculation (q to quit)");
			Interpreter.Run (getExpression, outputAction);
		}
	}
}

