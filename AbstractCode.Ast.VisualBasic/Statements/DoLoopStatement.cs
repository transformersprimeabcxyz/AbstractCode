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
using DoLoopStatementBase = AbstractCode.Ast.Statements.DoLoopStatement;

namespace AbstractCode.Ast.VisualBasic.Statements
{
    public class DoLoopStatement : DoLoopStatementBase
    {
        public static readonly AstNodeTitle<AstToken> ConditionVariantElementTitle =
            new AstNodeTitle<AstToken>("ConditionVariantElement");

        internal static readonly Dictionary<string, DoLoopConditionVariant> VariantMapping =
            new Dictionary<string, DoLoopConditionVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "until", DoLoopConditionVariant.Until },
                { "while", DoLoopConditionVariant.While },
            };

        public static DoLoopConditionVariant VariantFromString(string variantString)
        {
            DoLoopConditionVariant variant;
            if (!VariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException("Variant is not recognized as a valid Visual Basic do loop variant.");
            return variant;
        }

        public static string VariantToString(DoLoopConditionVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public DoLoopStatement()
        {
        }

        public DoLoopStatement(Expression condition)
            : base(condition)
        {
        }

        public AstToken ConditionVariantElement
        {
            get { return GetChildByTitle(ConditionVariantElementTitle); }
            set { SetChildByTitle(ConditionVariantElementTitle, value); }
        }

        public DoLoopConditionVariant ConditionVariant
        {
            get;
            set;
        }

        public DoLoopVariant LoopVariant
        {
            get;
            set;
        }
    }

    public enum DoLoopConditionVariant
    {
        Until,
        While,
    }

    public enum DoLoopVariant
    {
        ConditionFirst,
        ConditionLast,
    }
}