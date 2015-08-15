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

using System.Linq;
using AbstractCode.Ast.Statements;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast
{
    public interface ICodeBlockAstNode : IAstNode, IScopeProvider
    {
    }

    public sealed class SimpleCodeBlockScope : AbstractScope<ICodeBlockAstNode>
    {
        public SimpleCodeBlockScope(ICodeBlockAstNode container)
            : base(container)
        {
        }

        public override ResolveResult ResolveIdentifier(string identifier)
        {
            var variable = (from declaration in Container.GetChildren<VariableDeclarationStatement>()
                from definition in declaration.GetVariableDefinitions()
                where definition.Name == identifier
                select definition).FirstOrDefault();

            return variable != null 
                ? new LocalResolveResult(variable) 
                : base.ResolveIdentifier(identifier);
        }
    }
}
