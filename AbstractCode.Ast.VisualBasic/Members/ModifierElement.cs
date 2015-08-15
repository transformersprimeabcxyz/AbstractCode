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
using AbstractCode.Ast.Members;
using ModifierElementBase = AbstractCode.Ast.Members.ModifierElement;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class ModifierElement : ModifierElementBase
    {
        internal static readonly Dictionary<string, Modifier> ModifierMapping =
            new Dictionary<string, Modifier>(StringComparer.OrdinalIgnoreCase)
            {
                { "Private", Modifier.Private },
                { "Friend", Modifier.Internal },
                { "Protected", Modifier.Protected },
                { "Public", Modifier.Public },
                { "Shared", Modifier.Static },
                { "Overridable", Modifier.Virtual },
                { "MustOverride", Modifier.Abstract },
                { "Overrides", Modifier.Override },
                { "NotOverridable", Modifier.Sealed },
                { "Shadows", Modifier.Shadow },
                { "Partial", Modifier.Partial },
                { "Const", Modifier.Const },
                { "Readonly", Modifier.ReadOnly },
                { "Async", Modifier.Async },
                { "WriteOnly", (Modifier)VisualBasicModifier.WriteOnly },
                { "Iterator", (Modifier)VisualBasicModifier.Iterator },
                { "WithEvents", (Modifier)VisualBasicModifier.WithEvents },
            };

        public static Modifier ModifierFromString(string operatorString)
        {
            Modifier @operator;
            if (!ModifierMapping.TryGetValue(operatorString, out @operator))
                throw new ArgumentException("Code is not recognized as a valid Visual Basic modifier.");
            return @operator;
        }

        public static string ModifierToString(Modifier @operator)
        {
            var pair = ModifierMapping.FirstOrDefault(x => x.Value == @operator);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Modifier is not supported in Visual Basic.");
            return pair.Key;
        }

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
            get { return ModifierFromString(Value); }
            set { Value = ModifierToString(value); }
        }

        public VisualBasicModifier VisualBasicModifier
        {
            get { return (VisualBasicModifier)Modifier; }
            set { Modifier = (Modifier)value; }
        }
    }

    [Flags]
    public enum VisualBasicModifier
    {
        WriteOnly = 0x00010000,
        Iterator = 0x00020000,
        WithEvents = 0x00040000,
    }
}