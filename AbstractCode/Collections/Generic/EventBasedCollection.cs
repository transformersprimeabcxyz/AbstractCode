// This file is part of AbstractCode.
// 
// AbstractCode is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// AbstractCode is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Lesser General Public License for more details.
// 
// You should have received a copy of the GNU Lesser General Public License
// along with AbstractCode.  If not, see <http://www.gnu.org/licenses/>.
// 
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace AbstractCode.Collections.Generic
{
    /// <summary>
    ///     Represents a strongly typed event-driven collection of objects that can be accessed by an index, and provides events
    ///     which occur after modifying the collection.
    /// </summary>
    /// <typeparam name="T">The type of the elements in the collection.</typeparam>
    public class EventBasedCollection<T> : ICollection<T>, IList<T>, IEnumerable<T>
    {
        public event CollectionChangingEventHandler<T> InsertingItem;
        public event CollectionChangeEventHandler<T> InsertedItem;
        public event CollectionChangingEventHandler<T> RemovingItem;
        public event CollectionChangeEventHandler<T> RemovedItem;
        public event EventHandler ClearedCollection;

        private readonly List<T> _collection;

        public EventBasedCollection()
        {
            _collection = new List<T>();
        }

        /// <inheritdoc />
        public void Add(T item)
        {
            Insert(Count, item);
        }

        public void AddRange(IEnumerable<T> collection)
        {
            foreach (var item in collection)
                Add(item);
        }

        /// <inheritdoc />
        public void Clear()
        {
            while (Count != 0)
                Remove(this[0]);

            if (ClearedCollection != null)
                ClearedCollection(this, EventArgs.Empty);
        }

        /// <inheritdoc />
        public bool Contains(T item)
        {
            return _collection.Contains(item);
        }

        /// <inheritdoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _collection.CopyTo(array, arrayIndex);
        }

        /// <inheritdoc />
        public int Count
        {
            get { return _collection.Count; }
        }

        /// <inheritdoc />
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <inheritdoc />
        public bool Remove(T item)
        {
            int index = IndexOf(item);
            if (index == -1)
                return false;

            RemoveAt(index);

            return true;
        }

        /// <inheritdoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc />
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        /// <inheritdoc />
        public int IndexOf(T item)
        {
            return _collection.IndexOf(item);
        }

        /// <inheritdoc />
        public void Insert(int index, T item)
        {
            var eventArgs = new CollectionChangingEventArgs<T>(item, index);
            OnInsertingItem(eventArgs);
            if (!eventArgs.Cancel)
            {
                _collection.Insert(index, item);
                OnInsertedItem(new CollectionChangedEventArgs<T>(item, index));
            }
        }

        /// <inheritdoc />
        public void RemoveAt(int index)
        {
            T item = _collection[index];
            var eventArgs = new CollectionChangingEventArgs<T>(item, index);
            OnRemovingItem(eventArgs);
            if (!eventArgs.Cancel)
            {
                _collection.RemoveAt(index);
                OnRemovedItem(new CollectionChangedEventArgs<T>(item, index));
            }
        }

        /// <inheritdoc />
        public T this[int index]
        {
            get { return _collection[index]; }
            set { _collection[index] = value; }
        }

        /// <summary>
        ///     Copies the collection elements to a read-only collection.
        /// </summary>
        /// <returns>
        ///     An instance of a <see cref="System.Collections.ObjectModel.ReadOnlyCollection{T}" /> holding the same elements
        ///     as the source.
        /// </returns>
        public ReadOnlyCollection<T> AsReadOnly()
        {
            return _collection.AsReadOnly();
        }

        protected virtual void OnInsertingItem(CollectionChangingEventArgs<T> e)
        {
            if (InsertingItem != null)
                InsertingItem(this, e);
        }

        protected virtual void OnInsertedItem(CollectionChangedEventArgs<T> e)
        {
            if (InsertedItem != null)
                InsertedItem(this, e);
        }

        protected virtual void OnRemovingItem(CollectionChangingEventArgs<T> e)
        {
            if (RemovingItem != null)
                RemovingItem(this, e);
        }

        protected virtual void OnRemovedItem(CollectionChangedEventArgs<T> e)
        {
            if (RemovedItem != null)
                RemovedItem(this, e);
        }
    }
}