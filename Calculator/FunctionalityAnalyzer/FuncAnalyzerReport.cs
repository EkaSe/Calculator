using System;
using System.Linq;
using System.Reflection;
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
			string report = GetReportLinqStringBuilder (assembly);
			PrintReport (report);
			Console.WriteLine (GetReportLinqString (assembly));
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
				if (t.BaseType != null) {
					report.AppendLine (t.Name);
					report.AppendLine ("  Fields:");
					foreach (FieldInfo field in t.GetFields()) {
						if (field.DeclaringType != typeof (object))
							report.AppendLine ("\t" + field);
					}
					report.AppendLine ("  Methods:");
					foreach (MethodInfo method in t.GetMethods()) {
						if (method.DeclaringType != typeof (object))
							report.AppendLine ("\t" + method);
					}
				}
			}
			return report.ToString ();
		}

		static string GetReportLinqString (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((t) => t.BaseType != null)
				.Select ((Type t) => t.Name 
					+ t.GetFields ()
					   .Where ((field) => field.DeclaringType != typeof (object))
					   .Aggregate ("\n  Fields:",(string startValue, FieldInfo field) => startValue + "\n\t" + field.ToString ())
					+ t.GetMethods ()
					   .Where ((method) => method.DeclaringType != typeof (object))
					   .Aggregate ("\n  Methods:", 
						(string startValue, MethodInfo method) => startValue + "\n\t" + method.ToString ()) + "\n")
				.Aggregate ((report, nextClass) => report + nextClass);
		}

		static string GetReportLinqStringBuilder (Assembly assembly) {
			return assembly.GetTypes ()
				.Where ((t) => t.BaseType != null)
				.Aggregate (new StringBuilder (), (report, nextClass) => {
					nextClass.GetFields ()
						.Where ((field) => field.DeclaringType != typeof (object))
						.Aggregate (report.AppendLine (nextClass.Name).AppendLine ("  Fields:"),
							(fieldReport, field) => fieldReport.AppendLine ("\t" + field.ToString ()));
					nextClass.GetMethods ()
						.Where ((method) => method.DeclaringType != typeof (object))
						.Aggregate (report.AppendLine ("  Methods:"), 
							(methodReport, method) => methodReport.AppendLine ("\t" + method.ToString ()));
					return report;
				}).ToString ();
		}
	}
}

