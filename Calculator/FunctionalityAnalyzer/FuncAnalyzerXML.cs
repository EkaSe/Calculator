using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Reflection;

namespace FunctionalityAnalyzer
{
	public class FuncAnalyzerXML
	{
		static public void Run (Type childClassType) {
			Assembly assembly = childClassType.Assembly;
			GetReport (assembly);
			//GetReport (report);
			//Console.WriteLine (GetReport (assembly));
		}

		public static string LogPath {
			get {
				if (logPath == null) {
					string currentPath = Directory.GetCurrentDirectory ();
					logPath = currentPath.Substring (0, currentPath.IndexOf (@"FunctionalityAnalyzer")) 
						+ @"FunctionalityAnalyzer/FuncAnLog.xml";
				}
				return logPath;
			}
		}
		static string logPath;

		static void GetReport (Assembly assembly) {
			XDocument doc = new XDocument ();
			XElement main = new XElement ("report", "FuncAnReport");
			main.Add (assembly.GetTypes ()
				.Where ((t) => t.BaseType != typeof(object))
				.Select ((t) => {
					XElement type = new XElement ("class", t.Name);
					type.Add (t.GetFields ()
						.Where ((field) => field.DeclaringType != typeof(object))
						.Select ((FieldInfo field) => new XElement ("field", field.ToString ())));
					type.Add (t.GetMethods ()
						.Where ((method) => method.DeclaringType != typeof(object))
						.Select ((MethodInfo method) => new XElement ("method", method.ToString ())));
				return type;
				})
			);
			doc.Add (main);
			doc.Save (LogPath);
		}
	}
}

