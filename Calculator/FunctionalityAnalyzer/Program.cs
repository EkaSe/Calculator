using System;
using System.Reflection;
using Calculator.Logic;
using System.Text;

namespace FunctionalityAnalyzer
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			StringBuilder result = new StringBuilder ();
			Assembly calculatorLogic = typeof(Interpreter).Assembly;
			Console.WriteLine (calculatorLogic.GetTypes ());
			foreach (Type t in calculatorLogic.GetTypes()) {
				result.AppendLine (t.Name);
				result.AppendLine ("  Fields:");
				foreach (FieldInfo field in t.GetFields()) {
					result.AppendLine ("\t" + field);
				}
				result.AppendLine ("  Methods:");
				foreach (MethodInfo method in t.GetMethods()) {
					result.AppendLine ("\t" + method);
				}
			}
			Console.Write (result.ToString ());
		}
	}
}
