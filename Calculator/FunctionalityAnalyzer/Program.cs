using System;
using Calculator.Logic;

namespace FunctionalityAnalyzer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			FuncAnalyzerReport.Run (typeof(Interpreter));
		}
	}
}
