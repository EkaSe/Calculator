using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
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

		static void Test_OutputMessage (object sender, string message) {
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

	public class TestAttribute : Attribute {}

	public class TestFixtureAttribute : Attribute {}

	[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
	public class CoversAttribute : Attribute {
		public Method Method;

		public CoversAttribute (Type classType, string methodName) {
			Method.Class = classType;
			Method.Name = methodName;
		}
	}

	public struct Method {
		public Type Class;
		public string Name;

		public Method (Type className, string methodName) {
			Class = className;
			Name = methodName;
		}

		public override string ToString () {
			return Class.Name + "." + Name;
		}
	}

	public static class TestCoverage {
		public static Dictionary <Method, List<Method>> CoveredMethods = new Dictionary <Method, List<Method>> ();

		public static void Check () {
			Assembly assembly = typeof (InterpreterTest).Assembly;
			foreach (Type testClass in assembly.GetTypes()) {
				foreach (MethodInfo mInfo in testClass.GetMethods()) {
					foreach (Attribute attr in Attribute.GetCustomAttributes (mInfo)) {
						if (attr.GetType () == typeof(CoversAttribute)) {
							Method coveredMethod = ((CoversAttribute)attr).Method;
							if (!CoveredMethods.ContainsKey (coveredMethod))
								CoveredMethods.Add (coveredMethod, new List<Method> ());
							CoveredMethods [coveredMethod].Add (new Method (testClass, mInfo.Name));
							Console.WriteLine (testClass.Name + "." + mInfo.Name + " covers " + coveredMethod.ToString ());
						}
					}
				}
			}
		}
	}
}

