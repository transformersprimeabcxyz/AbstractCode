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

using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Expressions
{
    public class PrimitiveExpression : Expression
    {
        public PrimitiveExpression(object value)
            :this(value, string.Empty)
        {
        }

        public PrimitiveExpression(object value, string literalValue)
            : this(value, literalValue, TextRange.Empty)
        {
        }

        public PrimitiveExpression(object value, string literalValue, TextRange range)
        {
            Value = value;
            Range = range;
            LiteralValue = literalValue;
        }

        public object Value
        {
            get;
        }

        public string LiteralValue
        {
            get;
        }

        public override string ToString()
        {
            return string.Format("{0} (Value = {1}, LiteralValue = {2})", GetType().Name, Value ?? "null", LiteralValue);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as PrimitiveExpression;
            if (expression == null)
                return false;

            if (Value == null)
                return expression.Value == null;
            return Value.Equals(expression.Value);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitPrimitiveExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitPrimitiveExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitPrimitiveExpression(this, data);
        }
        
        public override ResolveResult Resolve(IScope scope)
        {
            // TODO: handle null value.

            var systemNamespace = scope.ResolveIdentifier("System") as NamespaceResolveResult;
            if (systemNamespace != null)
            {
                var @namespaceScope = systemNamespace.ScopeProvider.GetScope();
                var typeResult = @namespaceScope.ResolveIdentifier(Value.GetType().Name) as MemberResolveResult;
                var typeDefinition = typeResult?.Member as TypeDefinition;
                if (typeDefinition != null)
                    return new ConstantResolveResult(Value, typeDefinition);
            }

            return new ConstantResolveResult(Value, null);
        }
    }
}
