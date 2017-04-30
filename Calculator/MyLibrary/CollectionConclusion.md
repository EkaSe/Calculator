### Collections
 
* **Array** is an ordered set of objects of one predefined type. Quantity of elements is constant since array initialization. Elements can be accessed by index.
* **List** is an array of variable length.
* **Stack** is variable size array. Elements can be accessed in reverse order to adding to collection.
* **Dictionary** is a collection that stores pairs of strictly typed elements, one element of pair called key, another called value. Key element is used for accessing the value. Hash table. unique keys

### Interfaces IEnumerable and IEnumerator
Interfaces `IEnumerator<T>` and `IEnumerable<T>` of System.Collections.Generic namespace allow simple iteration over collection.
#### IEnumerable
The only method GetEnumerator() returns the enumerator for this collection.
#### IEnumerator
| Method | Description |
| --- | --- |
| T Current {get;} | returns current element of enumerated collection |
| bool MoveNext () | returns True, if there are remain some other elements in enumerated collection and advances enumerator to this next element |
| void Reset() | sets the enumerator to its initial position which is **before** the first element of the collection |
| void Dispose() | performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources |
 
foreach statement can enumerate object of any class, which implements IEnumerable
Collection initialization can initialize object of any class, which implements IEnumerable and contains method Add(item)
yield statement requires that the return value of the method which contains this statement is IEnumerable - we will consider that statement after completion of this task in the Extension class issue.
 
To avoid making same querying methods in each collection type, needs to create class MyEnumarebleExtension, which contains following methods:
 
public static IMyEnumerable< T> Where< T>(IMyEnumerable< T> collection, Func<T, bool> predicate)
public static IMyEnumerable< T2> Select<T1, T2>(IMyEnumerable< T1> collection, Func<T1, T2> select) {...}
public static T[] ToArray(IMyEnumerable< T> collection)
public static MyList< T> ToList< T>(IMyEnumerable< T> collection)
public static MyDictionary <K, V> ToDictionary<K, V>(IMyEnumerable< T> collection, Func<T, K> keySelector, Func<T, V> valueSelector) - creates from given T item a pair of key and value, getting them by applying corresponding selectors to the given item, and then populating adding them to a new MyDictionary instance.
public static T FirstOrDefault< T>(IMyEnumerable< T> collection)
public static T FirstOrDefault< T>(IMyEnumerable< T> collection, Func<T, bool> predicate)
Cover methods with tests, that demostraits different cases of their usage
 
UPD: all methods should be static
 
 Use this modifier in all public methods of MyEnumerableExtension class.
 Rewrite corresponding tests in fluent-style
 
 
 Enumerating-on-demand instead of storing results
