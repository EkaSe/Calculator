using System;

namespace Calculator.UI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Calculator.Logic.Prerequisites.RegisterOperators ();
			CalculatorUI.ConsoleCalculator ();
		}
	}
}
