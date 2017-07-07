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
			OutputPrinter.ClearLog (OutputPrinter.ErrorLogPath);
			TestHelper.SubscribeToTests ();
			TestCalculator.RunTests ();

			TestRunner.RunTests (typeof (InterpreterTest).Assembly);
		}
	}
}
