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

namespace AbstractCode.Collections.Generic
{
    public delegate void CollectionChangeEventHandler<T>(object sender, CollectionChangedEventArgs<T> e);

    public class CollectionChangedEventArgs<T> : EventArgs
    {
        public CollectionChangedEventArgs(T obj, int index)
        {
            TargetObject = obj;
            TargetIndex = index;
        }

        public T TargetObject
        {
            get;
            private set;
        }

        public int TargetIndex
        {
            get;
            private set;
        }
    }
}