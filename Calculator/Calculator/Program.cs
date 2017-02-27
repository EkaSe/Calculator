using System;

namespace Calculator.UI
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Calculator.Logic.Prerequisites.RegisterOperators ();
			Calculator.Logic.Prerequisites.RegisterBIFs ();
			Calculator.Logic.Prerequisites.RegisterStatements ();
			CalculatorUI.ConsoleCalculator ();
		}
	}
}
