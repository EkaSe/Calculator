using System;

namespace Calculator.Tests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Calculator.Logic.Prerequisites.RegisterOperators ();
			TestCalculator.RunTests ();
		}
	}
}
