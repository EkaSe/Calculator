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
			FuncAnalyzerReport.Run (typeof(Interpreter));
			FuncAnalyzerXML.Run (typeof(Interpreter));
		}
	}
}
