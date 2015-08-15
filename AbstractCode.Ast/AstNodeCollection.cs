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

using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AbstractCode.Ast
{
    public class AstNodeCollection<TNode> : ICollection<TNode> where TNode : AstNode
    {
        public AstNodeCollection(AstNode container, AstNodeTitle<TNode> itemTitle)
        {
            Container = container;
            ItemTitle = itemTitle;
        }

        public AstNode Container
        {
            get;
        }

        public AstNodeTitle<TNode> ItemTitle
        {
            get;
        }

        public void AddRange(IEnumerable<TNode> nodes)
        {
            foreach (var node in nodes)
                Add(node);
        }

        public void Add(TNode item)
        {
            Container.AddChild(ItemTitle, item);
        }

        public void Clear()
        {
            foreach (var element in this)
                element.Remove();
        }

        public bool Contains(TNode item)
        {
            return item != null && item.Parent == Container;
        }

        public void CopyTo(TNode[] array, int arrayIndex)
        {
            foreach (var item in this)
                array[arrayIndex++] = item;
        }

        public int Count
        {
            get { return Container.GetChildrenByTitle(ItemTitle).Count(); }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TNode item)
        {
            if (Contains(item))
            {
                item.Remove();
                return true;
            }
            return false;
        }

        public IEnumerator<TNode> GetEnumerator()
        {
            return Container.GetChildrenByTitle(ItemTitle).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void ReplaceWith(IEnumerable<TNode> nodes)
        {
            nodes = nodes.ToList();
            this.Clear();
            foreach (var node in nodes)
                this.Add((TNode)node.Remove());
        }

        public bool Match(IEnumerable<AstNode> nodes)
        {
            using (var otherEnumerator = nodes.GetEnumerator())
            {
                using (var thisEnumerator = GetEnumerator())
                {
                    while (thisEnumerator.MoveNext())
                    {
                        if (!otherEnumerator.MoveNext() || !thisEnumerator.Current.MatchOrNull(otherEnumerator.Current))
                            return false;
                    }

                    if (otherEnumerator.MoveNext())
                        return false;
                }
            }
            return true;
        }
    }
}
