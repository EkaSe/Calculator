using System;
using System.IO;
using MyLibrary;

namespace Calculator.Tests
{
	public class OutputPrinter
	{
		public static string TxtLogPath {
			get {
				if (txtLogPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					txtLogPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator.Tests")) + @"Calculator.Tests/TestLog.txt";
				}
				return txtLogPath;
			}
		}
		static string txtLogPath;

		static public void ClearLog () {
			if (File.Exists (TxtLogPath))
				File.Delete (TxtLogPath);
		}

		static public bool PrintTxt (string message) {
			try {
				if (!File.Exists (TxtLogPath)) {
					using (StreamWriter sw = File.CreateText(TxtLogPath)) 
					{
						sw.WriteLine(message);
					}
				} else {
					using (StreamWriter sw = File.AppendText(TxtLogPath)) {
						sw.WriteLine(message);
					}
				}
				return true;
			} catch (Exception e) {
				Console.WriteLine (e);
				Console.WriteLine (TxtLogPath);
				return false;
			}
		}

		static public void MessageReceived (object sender, string message) {
			PrintTxt (message);
		}
	}	
}

