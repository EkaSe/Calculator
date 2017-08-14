using System;
using System.IO;

namespace Calculator.Tests
{
	public class TestReporter
	{
		public TestReporter ()
		{
		}

		static public void MessageReceived (object sender, string message) {
			string line = message.Contains ("[fail]") ? " * **" + message + "**" : " * " + message;
			using (StreamWriter sw = File.AppendText (ReportPath)) {
				sw.WriteLine (line);
			}
		}

		static public void ClearReport () {
			if (File.Exists (ReportPath))
				File.Delete (ReportPath);
		}

		public static string ReportPath {
			get {
				if (reportPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					reportPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator/Calculator")) + @"Calculator/Calculator/TestReport.md";
				}
				return reportPath;
			}
		}
		static string reportPath;
	}
}

