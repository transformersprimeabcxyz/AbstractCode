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

namespace AbstractCode.Ast.Members
{
    public class ModifierElement : AstNode
    {
        public ModifierElement()
        {

        }

        public ModifierElement(Modifier modifier)
        {
            Modifier = modifier;
        }
        
        public ModifierElement(string value, TextRange textRange)
        {
            Value = value;
            Range = textRange;
        }

        public virtual Modifier Modifier
        {
            get;
            set;
        }

        public string Value
        {
            get;
            protected set;
        }

        public bool IsLanguageSpecific
        {
            get { return Modifier.HasLanguageSpecificModifiers(); }
        }

        public override string ToString()
        {
            return string.Format("{0} ({1})", GetType().Name, Value);
        }

        public override bool Match(AstNode other)
        {
            var element = other as ModifierElement;
            return element != null && Modifier == element.Modifier;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitModifierElement(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitModifierElement(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitModifierElement(this, data);
        }
    }

    [Flags]
    public enum Modifier : uint
    {
        None                    = 0x00000000,

        VisibilityMask          = 0x00000007,
        Private                 = 0x00000001,
        Internal                = 0x00000002,
        Protected               = 0x00000003,
        Public                  = 0x00000004,

        Static                  = 0x00000010,
        Sealed                  = 0x00000020,
        Virtual                 = 0x00000040,
        Abstract                = 0x00000080,
        Override                = 0x00000100,
        Shadow                  = 0x00000200,
        Partial                 = 0x00000400,
        Const                   = 0x00000800,
        ReadOnly                = 0x00001000,
        Extern                  = 0x00002000,
        Async                   = 0x00004000,

        SharedMask              = 0x0000FFFF,
        LanguageSpecificMask    = 0xFFFF0000,
    }

    public static class ModifierExtensions
    {
        public static bool HasMaskedFlag(this Modifier modifier, Modifier mask, Modifier flag)
        {
            return (modifier & mask) == flag;
        }

        public static bool HasLanguageSpecificModifiers(this Modifier modifier)
        {
            return (modifier & Modifier.SharedMask) != modifier;
        }
    }
}
