using System;
using System.Linq;
using System.Xml.Linq;
using System.IO;
using System.Reflection;
using System.Text;

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
			XElement main = new XElement ("report", new XAttribute ("name", "FuncAnReport"));
			main.Add (assembly.GetTypes ()
				.Where ((t) => t.BaseType != typeof(object))
				.Select ((t) => {
					XElement type = new XElement ("class", new XAttribute ("name", t.Name));
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
			doc.Declaration = new XDeclaration("1.0", "utf-8", "true");
			//doc = XDocument.Parse (XmlFormat (doc.ToString ()), LoadOptions.PreserveWhitespace);
			Console.WriteLine (doc);
			doc.Save (LogPath);
		}

		static public string XmlFormat (string doc) {
			StringBuilder indented = new StringBuilder ();
			int tabCount = -1;
			for (int i = 0; i < doc.Length; i++) {
				if (doc [i] == '<') {
					if (i > 0)
						indented.AppendLine();
					if (doc [i + 1] != '/')
						tabCount++;
					else {
						if (tabCount > 0)
							indented.Append ('\t');
						tabCount--;
					}
					for (int j = 0; j < tabCount; j++) {
						indented.Append ('\t');
					}
				}
				indented.Append (doc [i]);
				if (doc [i] == '>') {
					if (i < doc.Length - 1 && doc [i + 1] != '<') {
						indented.AppendLine ();
						for (int j = 0; j < tabCount + 1; j++) {
							indented.Append ('\t');
						}
					}
				}
			}
			return indented.ToString ();
		}
	}
}

