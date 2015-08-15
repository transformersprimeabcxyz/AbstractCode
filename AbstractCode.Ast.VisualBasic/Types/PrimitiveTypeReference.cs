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
using AbstractCode.Ast.Types;
using PrimitiveTypeReferenceBase = AbstractCode.Ast.Types.PrimitiveTypeReference;

namespace AbstractCode.Ast.VisualBasic.Types
{
    public class PrimitiveTypeReference : PrimitiveTypeReferenceBase
    {
        internal static readonly Dictionary<string, PrimitiveType> TypeMapping =
            new Dictionary<string, PrimitiveType>(StringComparer.OrdinalIgnoreCase)
            {
                { "Void", PrimitiveType.Void },
                { "Object", PrimitiveType.Object },
                { "Boolean", PrimitiveType.Boolean },
                { "Byte", PrimitiveType.Byte },
                { "UShort", PrimitiveType.UInt16 },
                { "UInteger", PrimitiveType.UInt32 },
                { "ULong", PrimitiveType.UInt64 },
                { "UIntPtr", PrimitiveType.UIntPtr },
                { "SByte", PrimitiveType.SByte },
                { "Short", PrimitiveType.Int16 },
                { "Integer", PrimitiveType.Int32 },
                { "Long", PrimitiveType.Int64 },
                { "IntPtr", PrimitiveType.IntPtr },
                { "Single", PrimitiveType.Single },
                { "Double", PrimitiveType.Double },
                { "Decimal", PrimitiveType.Decimal },
                { "Char", PrimitiveType.Char },
                { "String", PrimitiveType.String },
            };

        public static PrimitiveType TypeFromString(string typeString)
        {
            PrimitiveType type;
            if (!TypeMapping.TryGetValue(typeString, out type))
                throw new ArgumentException("Type is not recognized as a valid Visual Basic primitive type.");
            return type;
        }

        public static string TypeToString(PrimitiveType type)
        {
            var pair = TypeMapping.FirstOrDefault(x => x.Value == type);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Primitive type is not supported in Visual Basic.");
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