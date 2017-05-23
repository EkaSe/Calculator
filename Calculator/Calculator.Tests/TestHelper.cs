using System;
using System.IO;
using System.Reflection;
using MyLibrary;

namespace Calculator.Tests
{
	public static class TestHelper
	{
		static public event EventHandler<string> MessageReceived;

		static public void OnMessageReceived(string message)
		{
			EventHandler<string> handler = MessageReceived;
			if (handler != null)
			{
				handler(null, message);
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
			OnMessageReceived (message);
		}
	}
}

