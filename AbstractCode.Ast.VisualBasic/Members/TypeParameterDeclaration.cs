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
using TypeParameterDeclarationBase = AbstractCode.Ast.Members.TypeParameterDeclaration;

namespace AbstractCode.Ast.VisualBasic.Members
{
    public class TypeParameterDeclaration : TypeParameterDeclarationBase
    {
        internal static readonly Dictionary<string, TypeParameterVariance> VarianceMapping =
            new Dictionary<string, TypeParameterVariance>(StringComparer.OrdinalIgnoreCase)
            {
                { "Out", TypeParameterVariance.Covariant },
                { "In", TypeParameterVariance.Contravariant },
            };

        public static TypeParameterVariance OperatorFromString(string varianceString)
        {
            TypeParameterVariance variance;
            if (!VarianceMapping.TryGetValue(varianceString, out variance))
                throw new ArgumentException("Code is not recognized as a valid Visual Basic variance.");
            return variance;
        }

        public static string OperatorToString(TypeParameterVariance variance)
        {
            var pair = VarianceMapping.FirstOrDefault(x => x.Value == variance);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variance is not supported in Visual Basic.");
            return pair.Key;
        }

        public TypeParameterDeclaration()
        {
        }

        public TypeParameterDeclaration(Identifier identifier)
            : base(identifier, TypeParameterVariance.Invariant)
        {
        }

        public TypeParameterDeclaration(Identifier identifier, TypeParameterVariance variance)
            : base(identifier, variance)
        {
        }

        public TypeParameterDeclaration(string identifier)
            : base(new Identifier(identifier), TypeParameterVariance.Invariant)
        {
        }

        public TypeParameterDeclaration(string identifier, TypeParameterVariance variance)
            : base(new Identifier(identifier), variance)
        {
        }

        public override TypeParameterVariance Variance
        {
            get { return base.Variance; }
            set { base.Variance = value; }
        }
    }
}