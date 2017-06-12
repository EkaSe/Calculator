using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using Calculator.Logic;
using Calculator.Tests;
using System.Text;
using System.IO;

namespace FunctionalityAnalyzer
{
	public static class FuncAnalyzerReport
	{
		static TestCoverage testCoverage;

		static public void Run (Type childClassType, Type testClassType) {
			testCoverage = new TestCoverage (testClassType.Assembly);
			Assembly assembly = childClassType.Assembly;
			string report = GetReportLinqStringBuilder (assembly);
			PrintReport (report);
			//Console.WriteLine (GetReportSelectMany (assembly));
			Console.WriteLine (report);
			//Console.WriteLine (GetReportLinqString (assembly));
			//Console.WriteLine (GetReportForeach (assembly));
		}

		public static string FuncAnLogPath {
			get {
				if (logPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					logPath = currentPath.Substring (0, currentPath.IndexOf (@"FunctionalityAnalyzer")) 
						+ @"FunctionalityAnalyzer/FuncAnLog.txt";
				}
				return logPath;
			}
		}
		static string logPath;

		static public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		static public void OnMessageReceived(MessageReceivedEventArgs args)
		{
			EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
			if (handler != null)
			{
				handler(null, args);
			}
		}

		static public void PrintReport (string report) {
			MessageReceivedEventArgs args = new MessageReceivedEventArgs { Message = report, LogPath = FuncAnLogPath };
			OnMessageReceived (args);
		}
		/*
		static string GetReportForeach (Assembly assembly) {
			StringBuilder report = new StringBuilder ();
			foreach (Type t in assembly.GetTypes()) {
				if (t.BaseType != typeof (object)) {
					report.AppendLine (t.Name);
					FieldInfo[] fields = t.GetFields();
					if (fields.Length > 0) {
						report.AppendLine ("  Fields:");
						foreach (FieldInfo field in fields) {
							if (field.DeclaringType != typeof(object))
								report.AppendLine ("\t" + field);
						}
					}
					MethodInfo[] methods = t.GetMethods();
					if (methods.Length > 0)
					report.AppendLine ("  Methods:");
					foreach (MethodInfo method in methods) {
						if (method.DeclaringType != typeof (object))
							report.AppendLine ("\t" + method);
					}
				}
			}
			return report.ToString ();
		}

		static string GetReportLinqString (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((t) => t.BaseType != typeof (object))
				.Select ((Type t) => t.Name 
					+ t.GetFields ()
						.Where ((field) => field.DeclaringType != typeof (object))
						.Aggregate ("",
							(startValue, field) => startValue + "\n\t" + field.ToString (),
							(result) => {if (result != "") return "\n  Fields:" + result; else return "";})
					+ t.GetMethods ()
						.Where ((method) => method.DeclaringType != typeof (object))
						.Aggregate ("", 
						(startValue, method) => startValue + "\n\t" + method.ToString (),
						(result) => {if (result != "") return "\n  Methods:" + result + "\n"; else return "";}))
				.Aggregate ((report, nextClass) => report + nextClass);
		}

		static string GetReportSelectMany (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((t) => t.BaseType != typeof (object))
				.SelectMany ((t) => (new string[] {t.Name + "\n"}
					.Concat (t.GetFields ()
						.Where ((field) => field.DeclaringType != typeof (object))
						.Select ((field) => "\t" + field.ToString () + "\n"))
					.Concat (t.GetMethods ()
						.Where ((method) => method.DeclaringType != typeof (object))
						.Select ((method) => "\t" + method.ToString () + "\n"))))
				.Aggregate ((report, nextClass) => report + nextClass);
		}*/

		static string GetReportLinqStringBuilder (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((type) => !type.Name.Contains("AnonStorey"))
				.Aggregate (new StringBuilder (), (report, nextClass) => {
					report.AppendLine (nextClass.Name);
						var fields = nextClass.GetFields (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
					if (fields.Count () > 0)
						fields.Aggregate (report.AppendLine ("  Fields:"),
							(fieldReport, field) => fieldReport.AppendLine ("\t" + field.ToString ()));
						var methods = nextClass.GetMethods (BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Static | BindingFlags.Instance);
					if (methods.Count () > 0) {
						methods.Aggregate (report.AppendLine ("  Methods:"), 
							(methodReport, method) => methodReport.AppendLine ("\t" + method.ToString ()));
						double testPercentage = 0;
						if (testCoverage.TestedMethodsCount.ContainsKey (nextClass)) 
							testPercentage = testCoverage.TestedMethodsCount [nextClass] * 100.0 / methods.Count ();
						report.AppendLine ("  Test Coverage: " + testPercentage + "%");
					}
					return report;
				}).ToString ();
		}
	}
}

