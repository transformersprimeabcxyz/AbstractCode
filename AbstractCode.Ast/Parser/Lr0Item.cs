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
using System.Collections;
using System.Collections.Generic;
using AbstractCode.Ast.Expressions;

namespace AbstractCode.Ast.Parser
{
    public class Lr0Item
    {
        public Lr0Item(int id, GrammarReduction reduction, int index)
        {
            if (reduction == null)
                throw new ArgumentNullException(nameof(reduction));
            Id = id;
            Reduction = reduction;
            Index = index;
        }

        public GrammarReduction Reduction
        {
            get;
        }

        public GrammarElement Element
        {
            get { return IsFinalizer ? null : Reduction.Sequence[Index]; }
        }

        public Lr0Item NextItem
        {
            get { return IsFinalizer ? null : Reduction.Lr0Items[Index + 1]; }
        }
        
        public int Id
        {
            get;
        }

        public int Index
        {
            get;
        }

        public bool IsInitializer
        {
            get { return Index == 0; }
        }

        public bool IsFinalizer
        {
            get { return Index == Reduction.Lr0Items.Count - 1; }
        }

        public override string ToString()
        {
            return Reduction.GetDottedString(Index);
        }
    }
}