using System;
using Calculator.Logic;
using Calculator.Tests;

namespace FunctionalityAnalyzer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FuncAnalyzerReport.MessageReceived += OutputPrinter.MessageReceived;
			OutputPrinter.ClearLog (FuncAnalyzerReport.FuncAnLogPath);
			OutputPrinter.ClearLog (OutputPrinter.ErrorLogPath);
			TestHelper.SubscribeToTests ();
			FuncAnalyzerReport.Run (typeof(Interpreter), typeof (InterpreterTest));
			FuncAnalyzerXML.Run (typeof(Interpreter));
		}
	}
}
