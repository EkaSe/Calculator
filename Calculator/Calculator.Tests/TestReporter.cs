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
			string line;
			if (message.Contains ("[fail]"))
				line = "**" + message + "**";
			else
				line = message;
			if (!File.Exists (ReportPath)) {
				using (StreamWriter sw = File.CreateText(ReportPath)) 
				{
					sw.WriteLine(line);
				}
			} else {
				using (StreamWriter sw = File.AppendText(ReportPath)) {
					sw.WriteLine(line);
				}
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

