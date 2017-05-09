using System;
using System.Linq;
using System.Reflection;
using Calculator.Logic;
using Calculator.Tests;
using System.Text;

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
