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
using System.Linq;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Collections.Generic;
using AbstractCode.Symbols;

namespace AbstractCode.Ast
{
    /// <summary>
    /// When derived from this class, represents a node in an abstract syntax tree.
    /// </summary>
    public abstract class AstNode : IAstNode, IVisitable
    {
        internal static readonly AstNodeTitle<AstNode> RootTitle = new AstNodeTitle<AstNode>("Root");

        public event EventHandler ParentChanged;

        private readonly EventBasedCollection<AstNode> _children = new EventBasedCollection<AstNode>();
        private TextRange _range;
        private AstNode _parent;

        protected AstNode()
        {
            _children.InsertingItem += ChildrenOnInsertingItem;
            _children.InsertedItem += ChildrenOnInsertedItem;
            _children.RemovedItem += ChildrenOnRemovedItem;
            Title = RootTitle;
            UserData = new Dictionary<object, object>();
        }
        
        public TextRange Range
        {
            get
            {
                if (_range != TextRange.Empty)
                    return _range;

                if (FirstChild != null && LastChild != null)
                    return new TextRange(FirstChild.Range.Start, LastChild.Range.End);
                return TextRange.Empty;
            }
            set
            {
                _range = value;
            }
        }

        public AstNodeTitle Title
        {
            get;
            set;
        }

        IAstNode IAstNode.Parent
        {
            get { return Parent; }
        }

        public AstNode Parent
        {
            get { return _parent; }
            private set
            {
                if (_parent != value)
                {
                    _parent = value;
                    OnParentChanged();
                }
            }
        }

        IList<IAstNode> IAstNode.Children
        {
            get { return _children.Cast<IAstNode>().ToList().AsReadOnly(); }
        }

        public IList<AstNode> Children
        {
            get { return _children.AsReadOnly(); }
        }

        IAstNode IAstNode.FirstChild
        {
            get { return FirstChild; }
        }

        public AstNode FirstChild
        {
            get { return _children.Count > 0 ? Children[0] : null; }
        }

        IAstNode IAstNode.LastChild
        {
            get { return LastChild; }
        }

        public AstNode LastChild
        {
            get { return _children.Count > 0 ? Children[_children.Count - 1] : null; }
        }

        IAstNode IAstNode.NextSibling
        {
            get { return NextSibling; }
        }

        public AstNode NextSibling
        {
            get;
            private set;
        }

        IAstNode IAstNode.PreviousSibling
        {
            get { return PreviousSibling; }
        }

        public AstNode PreviousSibling
        {
            get;
            private set;
        }

        IAstNode IAstNode.GetRoot()
        {
            return GetRoot();
        }

        public AstNode GetRoot()
        {
            var root = this;
            while (root.Parent != null)
                root = root.Parent;
            return root;
        }

        IAstNode IAstNode.GetChildByTitle(AstNodeTitle title)
        {
            return _children.FirstOrDefault(x => x.Title == title);
        }

        IEnumerable<IAstNode> IAstNode.GetChildrenByTitle(AstNodeTitle title)
        {
            return GetChildren(x => x.Title == title);
        }

        void IAstNode.AddChild(AstNodeTitle title, IAstNode node)
        {
            AddChild(title, (AstNode)node);
        }

        public void AddChild(AstNodeTitle title, AstNode node)
        {
            node.Title = title;
            _children.Add(node);
        }

        void IAstNode.InsertChild(AstNodeTitle title, int index, IAstNode node)
        {
            InsertChild(title, index, (AstNode)node);
        }

        public void InsertChild(AstNodeTitle title, int index, AstNode node)
        {
            node.Title = title;
            _children.Insert(index, node);
        }

        IAstNode IAstNode.ReplaceWith(IAstNode newItem)
        {
            return ReplaceWith((AstNode)newItem);
        }

        public AstNode ReplaceWith(AstNode newItem)
        {
            var parent = Parent;
            if (parent == null)
                throw new ArgumentException("Cannot replace root nodes.");

            newItem.Title = this.Title;
            int index = parent._children.IndexOf(this);
            parent._children.RemoveAt(index);
            parent._children.Insert(index, newItem);

            return newItem;
        }

        IAstNode IAstNode.Remove()
        {
            return Remove();
        }

        public AstNode Remove()
        {
            Parent?._children.Remove(this);
            return this;
        }

        public void ClearChildren()
        {
            _children.Clear();
        }
        
        public IDictionary<object, object> UserData
        {
            get;
        } 

        public IEnumerable<IAstNode> GetChildren(Predicate<IAstNode> predicate)
        {
            return (from child in Children
                    where predicate(child)
                    select child).ToArray();
        }

        public IEnumerable<TNode> GetChildren<TNode>() where TNode : IAstNode
        {
            return GetChildren(x => x is TNode).Cast<TNode>();
        }

        public IEnumerable<TNode> GetChildrenByTitle<TNode>(AstNodeTitle<TNode> title) where TNode : AstNode
        {
            return GetChildren(x => x.Title == title).Cast<TNode>();
        }

        public TNode GetChildByTitle<TNode>(AstNodeTitle<TNode> title) where TNode : AstNode
        {
            return _children.FirstOrDefault(x => x.Title == title) as TNode;
        }

        protected void SetChildByTitle<TNode>(AstNodeTitle<TNode> title, AstNode node) where TNode : AstNode
        {
            var child = GetChildByTitle(title);

            if (child == null)
            {
                if (node != null)
                {
                    AddChild(title, node);
                    node.Title = title;
                }
            }
            else
            {
                if (node != null)
                {
                    child.ReplaceWith(node);
                    node.Title = title;
                }
                else
                    child.Remove();
            }
            
        }
        
        public AstNode MoveTo(AstNode newParent)
        {
            this.Remove();
            newParent.AddChild(this.Title, this);
            return this;
        }

        public void MoveChildrenTo(AstNode newParent)
        {
            while (Children.Count > 0)
                Children[0].MoveTo(newParent);
        }

        public TNode GetNodeContainer<TNode>()
            where TNode : AstNode
        {
            var container = Parent;
            while (container != null)
            {
                if (container is TNode)
                    break;
                container = container.Parent;
            }
            return container as TNode;
        }

        public AstNode GetMemberContainer()
        {
            var container = Parent;
            while (container != null)
            {
                if (container is MemberDeclaration || 
                    container is NamespaceDeclaration ||
                    container is CompilationUnit)
                    break;
                container = container.Parent;
            }
            return container;
        }

        public AstNode GetNodeAtLocation(TextLocation location)
        {
            if (!Range.Contains(location))
                return null;

            foreach (var child in _children)
            {
                var node = child.GetNodeAtLocation(location);
                if (node != null)
                    return node;
            }

            return this;
        }
        
        protected virtual void OnParentChanged()
        {
            ParentChanged?.Invoke(this, EventArgs.Empty);
        }

        private void ChildrenOnInsertingItem(object sender, CollectionChangingEventArgs<AstNode> e)
        {
            if (e.TargetObject.Title == null)
                throw new ArgumentNullException("Role");
            if (e.TargetObject.Parent != null)
                throw new InvalidOperationException("Child is already added to another tree.");
        }

        private void ChildrenOnInsertedItem(object sender, CollectionChangedEventArgs<AstNode> e)
        {
            var child = e.TargetObject;
            child.Parent = this;

            if (e.TargetIndex < _children.Count - 1)
            {
                child.NextSibling = Children[e.TargetIndex + 1];
                child.NextSibling.PreviousSibling = child;
            }
            else
                child.NextSibling = null;

            if (e.TargetIndex > 0)
            {
                child.PreviousSibling = Children[e.TargetIndex - 1];
                child.PreviousSibling.NextSibling = child;
            }
            else
                child.PreviousSibling = null;

        }
        
        private void ChildrenOnRemovedItem(object sender, CollectionChangedEventArgs<AstNode> e)
        {
            var child = e.TargetObject;

            if (child.Parent == this)
            {
                if (child.PreviousSibling != null)
                {
                    child.PreviousSibling.NextSibling = child.NextSibling;
                }
                if (child.NextSibling != null)
                {
                    child.NextSibling.PreviousSibling = child.PreviousSibling;
                }

                child.Parent = null;
                child.NextSibling = null;
                child.PreviousSibling = null;
            }
        }

        public virtual IScope GetDeclaringScope()
        {
            AstNode current = this.Parent;
            while (current != null)
            {
                var scopeProvider = current as IScopeProvider;
                if (scopeProvider != null)
                    return scopeProvider.GetScope();
                current = current.Parent;
            }
            return null;
        }

        public override string ToString()
        {
            return GetType().Name;
        }

        public abstract bool Match(AstNode other);

        public abstract void AcceptVisitor(IAstVisitor visitor);

        public abstract TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor);

        public abstract TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data);

    }

    public static class AstNodeExtensions
    {
        public static bool MatchOrNull(this AstNode self, AstNode other)
        {
            return self?.Match(other) ?? other == null;
        }
    }
}
