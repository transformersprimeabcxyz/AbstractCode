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
using AbstractCode.Ast.Types;
using PrimitiveTypeReferenceBase = AbstractCode.Ast.Types.PrimitiveTypeReference;

namespace AbstractCode.Ast.CSharp.Types
{
    public class PrimitiveTypeReference : PrimitiveTypeReferenceBase
    {
        internal static readonly Dictionary<string, PrimitiveType> TypeMapping = new Dictionary<string, PrimitiveType>()
        {
            {"void", PrimitiveType.Void},
            {"object", PrimitiveType.Object},
            {"bool", PrimitiveType.Boolean},
            {"byte", PrimitiveType.Byte},
            {"ushort", PrimitiveType.UInt16},
            {"uint", PrimitiveType.UInt32},
            {"ulong", PrimitiveType.UInt64},
            {"UIntPtr", PrimitiveType.UIntPtr},
            {"sbyte", PrimitiveType.SByte},
            {"short", PrimitiveType.Int16},
            {"int", PrimitiveType.Int32},
            {"long", PrimitiveType.Int64},
            {"IntPtr", PrimitiveType.IntPtr},
            {"float", PrimitiveType.Single},
            {"double", PrimitiveType.Double},
            {"decimal", PrimitiveType.Decimal},
            {"char", PrimitiveType.Char},
            {"string", PrimitiveType.String},
        };

        public static PrimitiveType TypeFromString(string typeString)
        {
            PrimitiveType type;
            if (!TypeMapping.TryGetValue(typeString, out type))
                throw new ArgumentException("Type is not recognized as a valid C# primitive type.");
            return type;
        }

        public static string TypeToString(PrimitiveType type)
        {
            var pair = TypeMapping.FirstOrDefault(x => x.Value == type);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Primitive type is not supported in C#.");
            return pair.Key;
        }

        public PrimitiveTypeReference()
        {
        }

        public PrimitiveTypeReference(PrimitiveType type)
            : base(type)
        {
        }

        public override PrimitiveType PrimitiveType
        {
            get { return TypeFromString(Identifier.Name); }
            set { Identifier = new Identifier(TypeToString(value), TextRange.Empty); }
        }
    }
}
