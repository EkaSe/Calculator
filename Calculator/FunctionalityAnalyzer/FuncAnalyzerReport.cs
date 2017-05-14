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
		static public void Run (Type childClassType) {
			Assembly assembly = childClassType.Assembly;
			string report = GetReportSelectMany (assembly);
			PrintReport (report);
			//Console.WriteLine (GetReportSelectMany (assembly));
			//Console.WriteLine (GetReportLinqStringBuilder (assembly));
			//Console.WriteLine (GetReportLinqString (assembly));
			//Console.WriteLine (GetReportForeach (assembly));
		}

		public static string LogPath {
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
		static bool clearLog = true;

		static bool PrintReport (string report) {
			try {
				if (clearLog && File.Exists (LogPath))
					File.Delete (LogPath);
				if (!File.Exists (LogPath)) {
					using (StreamWriter sw = File.CreateText (LogPath)) {
						sw.WriteLine (report);
					}
				} else {
					using (StreamWriter sw = File.AppendText (LogPath)) {
						sw.WriteLine (report);
					}
				}
				return true;
			} catch (Exception e) {
				Console.WriteLine (e);
				Console.WriteLine (LogPath);
				return false;
			}
		}

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
		}

		static string GetReportLinqStringBuilder (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((t) => t.BaseType != typeof (object))
				.Aggregate (new StringBuilder (), (report, nextClass) => {
					report.AppendLine (nextClass.Name);
					var fields = nextClass.GetFields ()
						.Where ((field) => field.DeclaringType != typeof (object));
					if (fields.Count () > 0)
						fields.Aggregate (report.AppendLine ("  Fields:"),
							(fieldReport, field) => fieldReport.AppendLine ("\t" + field.ToString ()));
					var methods = nextClass.GetMethods ()
						.Where ((method) => method.DeclaringType != typeof (object));
					if (methods.Count () > 0)
						methods.Aggregate (report.AppendLine ("  Methods:"), 
							(methodReport, method) => methodReport.AppendLine ("\t" + method.ToString ()));
					return report;
				}).ToString ();
		}
	}
}

