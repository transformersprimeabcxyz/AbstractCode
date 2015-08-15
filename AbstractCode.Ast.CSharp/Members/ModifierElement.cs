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
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast.Members;
using ModifierElementBase = AbstractCode.Ast.Members.ModifierElement;

namespace AbstractCode.Ast.CSharp.Members
{
    public class ModifierElement : ModifierElementBase
    {
        public ModifierElement()
        {

        }

        public ModifierElement(Modifier modifier)
            : base(modifier)
        {
        }

        internal ModifierElement(string value, TextRange textRange)
            : base(value, textRange)
        {
        }

        public override Modifier Modifier
        {
            get;
            set;
        }

        public CSharpModifier CSharpModifier
        {
            get { return (CSharpModifier)Modifier; }
            set { Modifier = (Modifier)value; }
        }
    }

    [Flags]
    public enum CSharpModifier : uint
    {
        Unsafe = 0x00010000,
    }
}
