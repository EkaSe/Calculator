using System;
using System.Text;
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
				string result = Console.ReadLine ();
				int blockStart = result.IndexOf ('{');
				if (blockStart > -1) 
					while (Parser.FindClosing (result, blockStart + 1, '{') < 0) {
						string nextLine = Console.ReadLine ();
						if (Parser.ToLowerCase (nextLine) != "q")
							result = result + "\n" + Console.ReadLine ();
						else 
							result = "q";
					}
				return result;
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

