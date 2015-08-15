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
using System.Collections.Generic;
using System.Text;

namespace AbstractCode.Ast.Parser
{
    public class GrammarReduction
    {
        public GrammarReduction(GrammarDefinition product, GrammarElementSequence sequence)
        {
            Product = product;
            Sequence = sequence;
            Lr0Items = new List<Lr0Item>();
        }

        public GrammarDefinition Product
        {
            get;
        }

        public GrammarElementSequence Sequence
        {
            get;
        }

        public IList<Lr0Item> Lr0Items
        {
            get;
        }

        public override string ToString()
        {
            return GetDottedString(-1);
        }

        public string GetDottedString(int index)
        {
            const string arrow = " \u2192 ";
            const char bullet = '\u2219';

            var builder = new StringBuilder();

            builder.Append(Product.Name);
            builder.Append(arrow);

            for (int i = 0; i < Sequence.Count; i++)
            {
                if (index == i)
                    builder.Append(bullet);

                builder.Append(Sequence[i].Name);

                if (i < Sequence.Count - 1)
                    builder.Append(' ');
            }

            if (index == Sequence.Count)
                builder.Append(bullet);

            return builder.ToString();
        }
    }
}