using System;
using System.IO;
using System.Reflection;

namespace Calculator.Tests
{
	public static class TestHelper
	{
		public static string LogPath {
			get {
				if (logPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					logPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator.Tests")) + @"Calculator.Tests/TestLog.txt";
				}
				return logPath;
			}
		}
		static string logPath;

		static bool OutputAction (string output) {
			try {
				if (!File.Exists (LogPath)) {
					//File.CreateText (logPath);
					using (StreamWriter sw = File.CreateText(LogPath)) 
					{
						sw.WriteLine(output);
					}
				}
				using (StreamWriter sw = File.AppendText(LogPath)) 
				{
					sw.WriteLine(output);
				}
				return true;
			} catch (Exception e) {
				Console.WriteLine (e);
				Console.WriteLine (LogPath);
				return false;
			}
		}

		static bool clearLog = true;
		public static void SetOutputAction () {
			if (clearLog && File.Exists (LogPath))
				File.Delete (LogPath);
			InterpreterTest.OutputFunc = (string output) => OutputAction (output);
			StatementSearcherTest.OutputFunc = (string output) => OutputAction (output);
			MyLibrary.MyCollectionTest.OutputFunc = (string output) => OutputAction (output);
			MyLibrary.MyEnumerableExtensionTest.OutputFunc = (string output) => OutputAction (output);
			TestCalculator.OutputFunc = (string output) => OutputAction (output);
		}
	}
}

