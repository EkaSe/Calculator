using System;
using System.Collections.Generic;

namespace MyLibrary
{
	public interface IMyObserver
	{
		void HandleEvent (string message);
	}

	public class StaticObservable {

		List <IMyObserver> observers = new List<IMyObserver> ();
		public void AddObserver (IMyObserver observer) {
			observers.Add (observer);
		}
		public void RemoveObserver (IMyObserver observer) {
			observers.Remove (observer);
		}
		public void NotifyObservers (string message) {
			foreach (var observer in observers) {
				observer.HandleEvent (message);
			}
		}
	}
}

