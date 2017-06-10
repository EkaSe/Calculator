using System;

namespace Calculator.Tests
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			Calculator.Logic.Prerequisites.RegisterOperators ();
			Calculator.Logic.Prerequisites.RegisterBIFs ();
			Calculator.Logic.Prerequisites.RegisterStatements ();
			TestHelper.SubscribeToTests ();
			OutputPrinter.ClearLog (TestHelper.TestLogPath);
			TestCalculator.RunTests ();

			TestCoverage.Check ();
		}
	}
}
