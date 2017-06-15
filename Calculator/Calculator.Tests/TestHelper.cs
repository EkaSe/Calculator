using System;
using System.Text;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

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

			OutputPrinter.ClearLog (TestHelper.TestLogPath);
			TestHelper.MessageReceived += OutputPrinter.MessageReceived;
			CoversAttribute.ErrorReceived += OutputPrinter.ErrorReceived;
		}

		static void Test_OutputMessage (object sender, string message) {
			MessageReceivedEventArgs args = new MessageReceivedEventArgs { Message = message, LogPath = TestLogPath };
			OnMessageReceived (args);
		}

		public static string TestLogPath {
			get {
				if (testLogPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					testLogPath = currentPath.Substring (0, currentPath.IndexOf (@"Calculator/Calculator")) + 
						@"Calculator/Calculator/Calculator.Tests/TestLog.txt";
				}
				return testLogPath;
			}
		}
		static string testLogPath;
	}

	public class TestAttribute : Attribute {}

	public class TestFixtureAttribute : Attribute {}

	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class CoversAttribute : Attribute {
		public MethodInfo Method { get; private set; }

		public CoversAttribute (Type type, string methodName) {
			Method = type.GetMethod (methodName);
			if (Method == null)
				OnErrorReceived (type.ToString () + "." + methodName + " not found");
		}

		public CoversAttribute (Type type, string methodName, Type[] args) {
			Method = type.GetMethod (methodName, args);
			if (Method == null)
				OnErrorReceived (type.ToString () + "." + methodName + " not found");
		}

		static public event EventHandler<string> ErrorReceived;

		static public void OnErrorReceived(string args)
		{
			EventHandler<string> handler = ErrorReceived;
			if (handler != null)
			{
				handler(null, args);
			}
		}
	}

	public class TestCoverage {
		public Dictionary <MethodInfo, List<MethodInfo>> CoveredMethods = new Dictionary <MethodInfo, List<MethodInfo>> ();
		public Dictionary <Type, int> TestedMethodsCount  = new Dictionary<Type, int> ();

		public TestCoverage (Assembly assembly) {
			var methodList = assembly.GetTypes ()
				.SelectMany ((testClass) => testClass.GetMethods ()
					.SelectMany ((mInfo) => Attribute.GetCustomAttributes (mInfo)
						.Where ((attribute) => attribute.GetType () == typeof(CoversAttribute))
						.Where ((attribute) => ((CoversAttribute)attribute).Method != null)
						.Select ((attribute) => {
							MethodInfo coveredMethod = ((CoversAttribute)attribute).Method;
							if (!CoveredMethods.ContainsKey (coveredMethod))
								CoveredMethods.Add (coveredMethod, new List<MethodInfo> ());
							CoveredMethods [coveredMethod].Add (mInfo);
							return coveredMethod;
			})));			
			methodList.GetEnumerator ();

			TestedMethodsCount = methodList.Select ((method) => method.DeclaringType)
				.Distinct ()
				.ToDictionary ((type) => type, (type) => 
					CoveredMethods.Count ((method1) => method1.Key.DeclaringType == type));
			TestedMethodsCount.GetEnumerator();
		}
	}
}

