using System;

namespace MyLibrary
{
	public interface IMyEnumerator<T>
	{
		/// <summary>
		/// returns the current element of enumerating collection
		/// </summary>
		T Current {get;}

		/// <summary>
		/// returns True, if there are remain some other elements in enumerating collection, that can be enumerated
		/// </summary>
		bool HasNext {get;}

		/// <summary>
		/// change ** Current** value to the next not enumerated yet element of collection
		/// </summary>
		void Next();

		/// <summary>
		/// Reset current instance.
		/// </summary>
		void Reset(); 
	}
}
