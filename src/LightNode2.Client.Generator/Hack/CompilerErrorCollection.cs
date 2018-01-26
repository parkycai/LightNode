using System.Collections;
using System.Reflection;

namespace System.CodeDom.Compiler
{
    //
    // Summary:
    //     Represents a collection of System.CodeDom.Compiler.CompilerError objects.
    public class CompilerErrorCollection : CollectionBase
    {
        //
        // Summary:
        //     Initializes a new instance of the System.CodeDom.Compiler.CompilerErrorCollection
        //     class.
        public CompilerErrorCollection() { }
        //
        // Summary:
        //     Initializes a new instance of the System.CodeDom.Compiler.CompilerErrorCollection
        //     class that contains the contents of the specified System.CodeDom.Compiler.CompilerErrorCollection.
        //
        // Parameters:
        //   value:
        //     A System.CodeDom.Compiler.CompilerErrorCollection object with which to initialize
        //     the collection.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public CompilerErrorCollection(CompilerErrorCollection value) { }
        //
        // Summary:
        //     Initializes a new instance of System.CodeDom.Compiler.CompilerErrorCollection
        //     that contains the specified array of System.CodeDom.Compiler.CompilerError objects.
        //
        // Parameters:
        //   value:
        //     An array of System.CodeDom.Compiler.CompilerError objects to initialize the collection
        //     with.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public CompilerErrorCollection(CompilerError[] value) { }

        //
        // Summary:
        //     Gets or sets the System.CodeDom.Compiler.CompilerError at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index of the entry to locate in the collection.
        //
        // Returns:
        //     A System.CodeDom.Compiler.CompilerError at each valid index.
        //
        // Exceptions:
        //   T:System.ArgumentOutOfRangeException:
        //     The index value indicated by the index parameter is outside the valid range of
        //     indexes for the collection.
        public CompilerError this[int index] { get { return new CompilerError(); } set { } }

        //
        // Summary:
        //     Gets a value that indicates whether the collection contains errors.
        //
        // Returns:
        //     true if the collection contains errors; otherwise, false.
        public bool HasErrors { get; }
        //
        // Summary:
        //     Gets a value that indicates whether the collection contains warnings.
        //
        // Returns:
        //     true if the collection contains warnings; otherwise, false.
        public bool HasWarnings { get; }

        //
        // Summary:
        //     Adds the specified System.CodeDom.Compiler.CompilerError object to the error
        //     collection.
        //
        // Parameters:
        //   value:
        //     The System.CodeDom.Compiler.CompilerError object to add.
        //
        // Returns:
        //     The index at which the new element was inserted.
        public int Add(CompilerError value) => 0;
        //
        // Summary:
        //     Copies the elements of an array to the end of the error collection.
        //
        // Parameters:
        //   value:
        //     An array of type System.CodeDom.Compiler.CompilerError that contains the objects
        //     to add to the collection.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public void AddRange(CompilerError[] value) { }
        //
        // Summary:
        //     Adds the contents of the specified compiler error collection to the end of the
        //     error collection.
        //
        // Parameters:
        //   value:
        //     A System.CodeDom.Compiler.CompilerErrorCollection object that contains the objects
        //     to add to the collection.
        //
        // Exceptions:
        //   T:System.ArgumentNullException:
        //     value is null.
        public void AddRange(CompilerErrorCollection value) { }
        //
        // Summary:
        //     Gets a value that indicates whether the collection contains the specified System.CodeDom.Compiler.CompilerError
        //     object.
        //
        // Parameters:
        //   value:
        //     The System.CodeDom.Compiler.CompilerError to locate.
        //
        // Returns:
        //     true if the System.CodeDom.Compiler.CompilerError is contained in the collection;
        //     otherwise, false.
        public bool Contains(CompilerError value) => true;
        //
        // Summary:
        //     Copies the collection values to a one-dimensional System.Array instance at the
        //     specified index.
        //
        // Parameters:
        //   array:
        //     The one-dimensional System.Array that is the destination of the values copied
        //     from System.CodeDom.Compiler.CompilerErrorCollection.
        //
        //   index:
        //     The index in the array at which to start copying.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The array indicated by the array parameter is multidimensional.-or- The number
        //     of elements in the System.CodeDom.Compiler.CompilerErrorCollection is greater
        //     than the available space between the index value of the arrayIndex parameter
        //     in the array indicated by the array parameter and the end of the array indicated
        //     by the array parameter.
        //
        //   T:System.ArgumentNullException:
        //     The array parameter is null.
        //
        //   T:System.ArgumentOutOfRangeException:
        //     The index parameter is less than the lowbound of the array indicated by the array
        //     parameter.
        public void CopyTo(CompilerError[] array, int index) { }
        //
        // Summary:
        //     Gets the index of the specified System.CodeDom.Compiler.CompilerError object
        //     in the collection, if it exists in the collection.
        //
        // Parameters:
        //   value:
        //     The System.CodeDom.Compiler.CompilerError to locate.
        //
        // Returns:
        //     The index of the specified System.CodeDom.Compiler.CompilerError in the System.CodeDom.Compiler.CompilerErrorCollection,
        //     if found; otherwise, -1.
        public int IndexOf(CompilerError value) => 0;
        //
        // Summary:
        //     Inserts the specified System.CodeDom.Compiler.CompilerError into the collection
        //     at the specified index.
        //
        // Parameters:
        //   index:
        //     The zero-based index where the compiler error should be inserted.
        //
        //   value:
        //     The System.CodeDom.Compiler.CompilerError to insert.
        public void Insert(int index, CompilerError value) { }
        //
        // Summary:
        //     Removes a specific System.CodeDom.Compiler.CompilerError from the collection.
        //
        // Parameters:
        //   value:
        //     The System.CodeDom.Compiler.CompilerError to remove from the System.CodeDom.Compiler.CompilerErrorCollection.
        //
        // Exceptions:
        //   T:System.ArgumentException:
        //     The specified object is not found in the collection.
        public void Remove(CompilerError value) { }
    }
}