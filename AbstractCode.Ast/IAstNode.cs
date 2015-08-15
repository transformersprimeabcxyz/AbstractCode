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
using AbstractCode.Symbols;

namespace AbstractCode.Ast
{
    public interface IAstNode : IScopeMember
    {
        TextRange Range
        {
            get;
        }

        AstNodeTitle Title
        {
            get;
        }

        IAstNode Parent
        {
            get;
        }

        IList<IAstNode> Children
        {
            get;
        }

        IAstNode FirstChild
        {
            get;
        }

        IAstNode LastChild
        {
            get;
        }

        IAstNode NextSibling
        {
            get;
        }

        IAstNode PreviousSibling
        {
            get;
        }

        IAstNode GetRoot();
        IAstNode GetChildByTitle(AstNodeTitle title);
        IEnumerable<IAstNode> GetChildrenByTitle(AstNodeTitle title);
        IEnumerable<IAstNode> GetChildren(Predicate<IAstNode> predicate);
        IEnumerable<TNode> GetChildren<TNode>() where TNode : IAstNode;

        void AddChild(AstNodeTitle title, IAstNode node);
        void InsertChild(AstNodeTitle title, int index, IAstNode node);
        IAstNode ReplaceWith(IAstNode newItem);
        IAstNode Remove();

        IDictionary<object, object> UserData
        {
            get;
        }
    }
}
