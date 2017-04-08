using System;

namespace MyLibrary
{
	public interface IMyEnumerable<T>
	{
		IMyEnumerator<T> Enumerator {get;} //returns the enumerator for this collectio
	}
}
