using System;

namespace MyLibrary
{
	public interface IMyEnumerator<T>
	{
<<<<<<< HEAD
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
=======
		T Current {get;} //returns the current element of enumerating collection
		bool HasNext {get;}//returns True, if there are remain some other elements in enumerating collection, that can be enumerated
		void Next();//change ** Current** value to the next not enumerated yet element of collection
		void Reset(); //reset **Current
	}
}

>>>>>>> bd4e2d04adff13eae70bd0eb0e7ba7b2dc83eaad
