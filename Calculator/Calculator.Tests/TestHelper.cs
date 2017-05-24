using System;
using System.IO;
using System.Reflection;
using MyLibrary;

namespace Calculator.Tests
{
	public static class TestHelper
	{
		static public event EventHandler<MessageReceivedEventArgs> MessageReceived;

		static public void OnMessageReceived(MessageReceivedEventArgs args)
		{
			EventHandler<MessageReceivedEventArgs> handler = MessageReceived;
			if (handler != null)
			{
				handler(null, args);
			}
		}

		static public void SubscribeToTests () {
			TestCalculator.OutputMessage += Test_OutputMessage;
			InterpreterTest.OutputMessage += Test_OutputMessage;
			StatementSearcherTest.OutputMessage += Test_OutputMessage;

			TestHelper.MessageReceived += OutputPrinter.MessageReceived;
		}

		static void Test_OutputMessage (object sender, string message)
		{
			MessageReceivedEventArgs args = new MessageReceivedEventArgs { Message = message, LogPath = TestLogPath };
			OnMessageReceived (args);
		}

		public static string TestLogPath {
			get {
				if (testLogPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					testLogPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator.Tests")) + @"Calculator.Tests/TestLog.txt";
				}
				return testLogPath;
			}
		}
		static string testLogPath;
	}
}

