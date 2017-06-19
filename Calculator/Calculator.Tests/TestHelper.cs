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
			TestCoverage.ErrorReceived += OutputPrinter.ErrorReceived;
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

	public class TestFixtureAttribute : Attribute {
		public Type Class { get; private set; }

		public TestFixtureAttribute (Type type) {
			Class = type;
		}

		public TestFixtureAttribute () {
			Class = null;
		}
	}

	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class CoversAttribute : Attribute {
		public string MethodName = "";
		public Type Class = null;
		public Type[] Args = null;

		public CoversAttribute (Type type, string methodName) {
			Class = type;
			MethodName = methodName;
		}

		public CoversAttribute (Type type, string methodName, Type[] args) {
			Class = type;
			MethodName = methodName;
			Args = args;
		}

		public CoversAttribute (string methodName) {
			MethodName = methodName;
		}

		public CoversAttribute (string methodName, Type[] args) {
			MethodName = methodName;
			Args = args;
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
						.Select ((attribute) => {
							MethodInfo coveredMethod = GetTestMethod (
								(TestFixtureAttribute) testClass.GetCustomAttribute (typeof (TestFixtureAttribute)),
								(CoversAttribute)attribute);
							if (coveredMethod != null) {
								if (!CoveredMethods.ContainsKey (coveredMethod))
									CoveredMethods.Add (coveredMethod, new List<MethodInfo> ());
								CoveredMethods [coveredMethod].Add (mInfo);
							}
							return coveredMethod;
						})
						.Where ((method) => method != null)));			
			methodList.GetEnumerator ();

			TestedMethodsCount = methodList.Select ((method) => method.DeclaringType)
				.Distinct ()
				.ToDictionary ((type) => type, (type) => 
					CoveredMethods.Count ((method1) => method1.Key.DeclaringType == type));
			TestedMethodsCount.GetEnumerator();
		}

		private MethodInfo GetTestMethod (TestFixtureAttribute testFixture, CoversAttribute covers) {
			Type type = covers.Class ?? testFixture.Class;
			MethodInfo method;
			if (covers.Args != null)
				method = type.GetMethod (covers.MethodName, covers.Args);
			else
				method = type.GetMethod (covers.MethodName);
			if (method == null)
				OnErrorReceived (type.ToString () + "." + covers.MethodName + " not found");
			return method;
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
}

