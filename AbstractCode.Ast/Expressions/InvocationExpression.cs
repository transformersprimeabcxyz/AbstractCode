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
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast.Expressions
{
    public class InvocationExpression : Expression
    {
        public InvocationExpression()
        {
            Arguments = new AstNodeCollection<Expression>(this, AstNodeTitles.Argument);
        }
        
        public InvocationExpression(Expression target, params Expression[] arguments)
            : this(target, (IEnumerable<Expression>)arguments)
        {

        }

        public InvocationExpression(Expression target, IEnumerable<Expression> arguments)
            : this()
        {
            Target = target;
            Arguments.AddRange(arguments);
        }

        public Expression Target
        {
            get { return GetChildByTitle(AstNodeTitles.TargetExpression); }
            set { SetChildByTitle(AstNodeTitles.TargetExpression, value); }
        }

        public AstToken LeftParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.LeftParenthese); }
            set { SetChildByTitle(AstNodeTitles.LeftParenthese, value); }
        }

        public AstNodeCollection<Expression> Arguments
        {
            get;
            private set;
        }
        
        public AstToken RightParenthese
        {
            get { return GetChildByTitle(AstNodeTitles.RightParenthese); }
            set { SetChildByTitle(AstNodeTitles.RightParenthese, value); }
        }
                
        public override string ToString()
        {
            return string.Format("{0} (Target = {1})", GetType().Name, Target);
        }

        public override bool Match(AstNode other)
        {
            var expression = other as InvocationExpression;
            return expression != null
                   && Target.MatchOrNull(expression.Target)
                   && Arguments.Match(expression.Arguments);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitInvocationExpression(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitInvocationExpression(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitInvocationExpression(this, data);
        }
        
        public override ResolveResult Resolve(IScope scope)
        {
            var resolvableTarget = Target as IResolvable;

            if (resolvableTarget != null)
            {
                var resolvedTarget = (MemberResolveResult)resolvableTarget.Resolve(scope);
                var resolvedArguments = ResolveArguments(scope).ToArray();
                var member = resolvedTarget.Member;

                var argumentTypes = new TypeDefinition[resolvedArguments.Length];
                for (int i = 0; i < resolvedArguments.Length; i++)
                {
                    var argumentType = resolvedArguments[i].ScopeProvider as TypeDefinition;
                    if (argumentType == null)
                        return ErrorResolveResult.Instance;
                    argumentTypes[i] = argumentType;
                }
                
                var matches = (from candidate in resolvedTarget.GetMembers()
                               let method = (MethodDefinition)candidate
                               where method.MatchesSignature(argumentTypes)
                               select method).ToArray();
                
                if (matches.Length == 0)
                    return ErrorResolveResult.Instance;

                if (matches.Length == 1)
                    member = matches[0];
                
                return new InvocationResolveResult(member, resolvedArguments);
            }

            return null;
        }

        private IEnumerable<ResolveResult> ResolveArguments(IScope scope)
        {
            return Arguments.Select(argument => argument.Resolve(scope));
        }
    }
}
