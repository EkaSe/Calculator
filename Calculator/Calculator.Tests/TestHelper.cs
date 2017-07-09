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

		/// <param name="keySelector">Function that gets key for the dictionary from the given source element. Usage: el => el.Name</param>
		/// <param name="onNewKeyValueSelector">Function that gets value for the dictionary in a case when corresponding key not yet exists in the dictionary. Usage: el => el.Value</param>
		/// <param name="onKeyExistsValueSelector">Function that gets value for the dictionary in a case when corresponding key is already exists in the dictionary. Usage: (el, oldValue) => oldValue + el.Value</param>
		public static Dictionary<TKey, TValue> ToDictionarySafe<TItem, TKey, TValue>(
			this IEnumerable<TItem> source,
			Func<TItem, TKey> keySelector,
			Func<TItem, TValue> onNewKeyValueSelector,
			Func<TItem, TValue, TValue> onKeyExistsValueSelector) {
			Dictionary<TKey, TValue> result = new Dictionary<TKey, TValue> ();
			foreach (var item in source) {
				TKey key = keySelector (item);
				if (result.ContainsKey (key))
					result [key] = onKeyExistsValueSelector (item, result [key]);
				else
					result.Add (key, onNewKeyValueSelector (item));
			}
			return result;
		}
	}

	public class TestAttribute : Attribute {}

	public class TestFixtureAttribute : Attribute {}

	[AttributeUsage(AttributeTargets.Method, Inherited = false, AllowMultiple = true)]
	public class CoversAttribute : Attribute {
		public MethodInfo Method { get; private set; }

		public CoversAttribute (Type type, string methodName, Type[] args = null) {
			if (args == null)
				Method = type.GetMethod (methodName);
			else
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
			var enumCoveredMethods = assembly.GetTypes ()
				.SelectMany ((testClass) => testClass.GetMethods ()
					.SelectMany ((mInfo) => Attribute.GetCustomAttributes (mInfo)
						.OfType<CoversAttribute> ()
						.Where ((attribute) => ((CoversAttribute)attribute).Method != null)
						.ToDictionarySafe <Attribute, MethodInfo, List<MethodInfo>> (
							(attribute) => ((CoversAttribute)attribute).Method,
							(attribute) => {
								List<MethodInfo> mInfoList = new List<MethodInfo> ();
								mInfoList.Add (mInfo);
								return mInfoList;
							},
							(attribute, mInfoList) => {
								mInfoList.Add (mInfo);
								return mInfoList;
							}
						)));
			//exception: Specified cast is not valid
			CoveredMethods = (Dictionary <MethodInfo, List<MethodInfo>>) enumCoveredMethods;

			TestedMethodsCount = CoveredMethods.Select ((method) => method.Key.DeclaringType)
				.Distinct ()
				.ToDictionary ((type) => type, (type) => 
					CoveredMethods.Count ((method1) => method1.Key.DeclaringType == type));
		}
	}
}