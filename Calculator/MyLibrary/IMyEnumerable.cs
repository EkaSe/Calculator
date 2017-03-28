using System;

namespace MyLibrary
{
	public interface IMyEnumerable<T>
	{
		IMyEnumerator<T> Enumerator {get;} //returns the enumerator for this collectio
	}
}

/*

	Create nested class MyListEnumerator, implemented IMyEnumerator for the instance of MyList collection, given in constructor parameter

		Implement in the MyList class IMyEnumerable interface

		Do the same for MyStack and MyLinkedList classes
*/
