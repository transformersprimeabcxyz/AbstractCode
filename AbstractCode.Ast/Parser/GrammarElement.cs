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

namespace AbstractCode.Ast.Parser
{
    public delegate AstNode CreateAstNodeDelegate(ParserNode node);

    public abstract class GrammarElement
    {
        protected GrammarElement(string name)
        {
            Name = name;
        }

        public string Name
        {
            get;
        }

        public IEnumerable<TokenGrammarElement> GetFirstItems()
        {
            return GetFirstItems(new List<GrammarElement>());
        }

        public abstract IEnumerable<TokenGrammarElement> GetFirstItems(IList<GrammarElement> alreadyTraversedItems);

        public override string ToString()
        {
            return Name;
        }

        public static GrammarExpression operator +(GrammarElement a, GrammarElement b)
        {
            var newExpression = new GrammarExpression();
            newExpression.Switch.Clear();

            var expressionA = a as GrammarExpression ?? new GrammarExpression(a);

            newExpression.Switch.AddRange(expressionA.Switch);
            newExpression.Switch[newExpression.Switch.Count - 1].Add(b);
            return newExpression;
        }

        public static GrammarExpression operator |(GrammarElement a, GrammarElement b)
        {
            var newExpression = new GrammarExpression();
            newExpression.Switch.Clear();

            var expressionA = a as GrammarExpression ?? new GrammarExpression(a);
            var expressionB = b as GrammarExpression ?? new GrammarExpression(b);

            newExpression.Switch.AddRange(expressionA.Switch);
            newExpression.Switch.AddRange(expressionB.Switch);

            return newExpression;
        }
    }

    public class GrammarElementSwitch : List<GrammarElementSequence>
    {
        public override string ToString()
        {
            return string.Join("|", this.Select(x =>
            {
                if (x.Count > 1)
                    return '(' + x.ToString() + ')';
                return x.ToString();
            }));
        }
    }

    public class GrammarElementSequence : List<GrammarElement>
    {
        public override string ToString()
        {
            return string.Join("+", this);
        }
    }

    public class TokenGrammarElement : GrammarElement
    {
        public TokenGrammarElement(string name)
            : base(name)
        {
        }

        public override IEnumerable<TokenGrammarElement> GetFirstItems(IList<GrammarElement> alreadyTraversedItems)
        {
            if (alreadyTraversedItems.Contains(this))
                yield break;
            yield return this;
        }

        public CustomActionGrammarElement WithAction(CustomActionDelegate action)
        {
            return new CustomActionGrammarElement(this, action);
        }

    }

    public delegate void CustomActionDelegate(ParserContext context);

    public class CustomActionGrammarElement : TokenGrammarElement
    {
        public CustomActionGrammarElement(TokenGrammarElement baseElement, CustomActionDelegate action)
            : base(baseElement.Name)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));
            BaseElement = baseElement;
            Action = action;
        }

        public TokenGrammarElement BaseElement
        {
            get;
        }

        public CustomActionDelegate Action
        {
            get;
        }

        public override IEnumerable<TokenGrammarElement> GetFirstItems(IList<GrammarElement> alreadyTraversedItems)
        {
            return BaseElement.GetFirstItems(alreadyTraversedItems);
        }
    }

}