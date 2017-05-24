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
				Console.WriteLine (e);
				Console.WriteLine (args.LogPath);
				return false;
			}
		}

		static public void MessageReceived (object sender, MessageReceivedEventArgs args) {
			PrintTxt (args);
		}
	}

	public class MessageReceivedEventArgs : EventArgs {
		public string Message { get; set; }
		public string LogPath { get; set; }
	}
}

