using System;
using System.IO;
using MyLibrary;

namespace Calculator.Tests
{
	public class OutputPrinter
	{
		static public void ClearLog (string logPath) {
			if (File.Exists (logPath))
				File.Delete (logPath);
		}

		static public bool PrintTxt (MessageReceivedEventArgs args) {
			try {
				if (!File.Exists (args.LogPath)) {
					using (StreamWriter sw = File.CreateText(args.LogPath)) 
					{
						sw.WriteLine(args.Message);
					}
				} else {
					using (StreamWriter sw = File.AppendText(args.LogPath)) {
						sw.WriteLine(args.Message);
					}
				}
				return true;
			} catch (Exception e) {
				PrintError ("Couldn't write to log " + args.LogPath);
				PrintError (e.Message);
				return false;
			}
		}

		static public bool PrintError (string errorMessage) {
			try {
				if (!File.Exists (ErrorLogPath)) {
					using (StreamWriter sw = File.CreateText(ErrorLogPath)) 
					{
						sw.WriteLine(errorMessage);
					}
				} else {
					using (StreamWriter sw = File.AppendText(ErrorLogPath)) {
						sw.WriteLine(errorMessage);
					}
				}
				return true;
			} catch (Exception e) { 
				ErrorToConsole ("Couldn't write to error log ");
				ErrorToConsole ("Error message: ");
				ErrorToConsole (errorMessage);
				return false; 
			}
		}

		static public void ErrorToConsole (string errorMessage) {
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine (errorMessage);
			Console.ForegroundColor = ConsoleColor.White;
		}

		static public void MessageReceived (object sender, MessageReceivedEventArgs args) {
			PrintTxt (args);
		}

		static public void ErrorReceived (object sender, string errorMessage) {
			PrintError (errorMessage);
		}

		public static string ErrorLogPath {
			get {
				if (errorLogPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					errorLogPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator/Calculator")) + @"Calculator/Calculator/ErrorLog.txt";
				}
				return errorLogPath;
			}
		}
		static string errorLogPath;
	}

	public class MessageReceivedEventArgs : EventArgs {
		public string Message { get; set; }
		public string LogPath { get; set; }
	}
}

