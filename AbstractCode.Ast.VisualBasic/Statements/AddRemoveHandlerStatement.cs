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
using AbstractCode.Ast.Statements;
using AddRemoveHandlerStatementBase = AbstractCode.Ast.Statements.AddRemoveHandlerStatement;

namespace AbstractCode.Ast.VisualBasic.Statements
{
    public class AddRemoveHandlerStatement : AddRemoveHandlerStatementBase
    {
        internal static readonly Dictionary<string, AddRemoveHandlerVariant> VariantMapping =
            new Dictionary<string, AddRemoveHandlerVariant>(StringComparer.OrdinalIgnoreCase)
            {
                { "AddHandler", AddRemoveHandlerVariant.Add },
                { "RemoveHandler", AddRemoveHandlerVariant.Remove },
            };

        public static AddRemoveHandlerVariant VariantFromString(string variantString)
        {
            AddRemoveHandlerVariant variant;
            if (!VariantMapping.TryGetValue(variantString, out variant))
                throw new ArgumentException(
                    "Variant is not recognized as a valid Visual Basic add or remove handler variant.");
            return variant;
        }

        public static string VariantToString(AddRemoveHandlerVariant variant)
        {
            var pair = VariantMapping.FirstOrDefault(x => x.Value == variant);
            if (string.IsNullOrEmpty(pair.Key))
                throw new ArgumentException("Variant is not supported in Visual Basic.");
            return pair.Key;
        }

        public AddRemoveHandlerStatement()
        {
        }

        public AddRemoveHandlerStatement(Expression eventExpression, Expression delegateExpression,
            AddRemoveHandlerVariant variant)
            : base(eventExpression, delegateExpression, variant)
        {
        }

        public AstToken CommaElement
        {
            get { return GetChildByTitle(AstNodeTitles.ElementSeparator); }
            set { SetChildByTitle(AstNodeTitles.ElementSeparator, value); }
        }

        public override AddRemoveHandlerVariant Variant
        {
            get;
            set;
        }
    }
}