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
using System.Linq;
using AbstractCode.Ast.Members;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Ast
{
    public class CompilationUnit : AstNode, IScopeProvider, IUsingDeclarationProvider, INamespaceDefinitionProvider, ITypeDefinitionProvider
    {
        private sealed class CompilationUnitScope : AbstractScope<CompilationUnit>
        {
            public CompilationUnitScope(CompilationUnit unit)
                : base(unit)
            {
            }

            public override ResolveResult ResolveIdentifier(string identifier)
            {
                var types = new HashSet<TypeDefinition>();

                foreach (var directive in Container.UsingDirectives)
                {
                    var namespaceDirective = directive as UsingNamespaceDirective;
                    if (namespaceDirective != null)
                    {
                        var namespaceResolveResult =
                            base.ResolveIdentifier(namespaceDirective.NamespaceIdentifier.Name) as
                                NamespaceResolveResult;
                        if (namespaceResolveResult != null)
                        {
                            if (namespaceDirective.NamespaceIdentifier.Name == identifier)
                                return namespaceResolveResult;

                            foreach (var definition in namespaceResolveResult.ResolvedDefinitions)
                            {
                                var result = definition.GetScope().ResolveIdentifier(identifier) as MemberResolveResult;
                                var type = result?.Member as TypeDefinition;
                                if (type != null)
                                    types.Add(type);
                            }
                        }
                    }
                    else
                    {
                        var aliasDirective = directive as UsingAliasDirective;
                        if (aliasDirective != null && aliasDirective.AliasIdentifier.Name == identifier)
                        {
                            var declaringProvider = GetDeclaringProvider();
                            if (declaringProvider != null)
                            {
                                var result = aliasDirective.TypeImport.Resolve(declaringProvider.GetScope()) 
                                    as MemberResolveResult;
                                var type = result?.Member as TypeDefinition;
                                if (type != null)
                                    types.Add(type);
                            }
                        }
                    }
                }

                if (types.Count > 1)
                    return new AmbiguousMemberResolveResult(types);
                if (types.Count == 1)
                    return new MemberResolveResult(types.First());

                return base.ResolveIdentifier(identifier);
            }
        }

        private CompilationUnitScope _scope;

        public CompilationUnit()
        {
            UsingDirectives = new AstNodeCollection<UsingDirective>(this, AstNodeTitles.UsingDirective);
            Namespaces = new AstNodeCollection<NamespaceDeclaration>(this, AstNodeTitles.NamespaceDeclaration);
            Types = new AstNodeCollection<TypeDeclaration>(this, AstNodeTitles.TypeDeclaration);
            Errors = new List<SyntaxError>();
        }

        public AstNodeCollection<NamespaceDeclaration> Namespaces
        {
            get;
            private set;
        }

        public AstNodeCollection<TypeDeclaration> Types
        {
            get;
            private set;
        }

        public IList<SyntaxError> Errors
        {
            get;
            private set;
        }

        public AssemblyDefinition Assembly
        {
            get;
            internal set;
        }

        public AstNodeCollection<UsingDirective> UsingDirectives
        {
            get;
            private set;
        }

        public IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
        {
            foreach (var @namespace in GetChildren<NamespaceDeclaration>())
                yield return @namespace.GetDefinition();
        }

        public IEnumerable<TypeDefinition> GetTypeDefinitions()
        {
            foreach (var type in GetChildren<TypeDeclaration>())
                yield return type.GetDefinition();
        }

        public override bool Match(AstNode other)
        {
            var unit = other as CompilationUnit;
            return unit != null
                   && UsingDirectives.Match(unit.UsingDirectives)
                   && Namespaces.Match(unit.Namespaces)
                   && Types.Match(unit.Types);
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitCompilationUnit(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
            return visitor.VisitCompilationUnit(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
            return visitor.VisitCompilationUnit(this, data);
        }

        public override IScope GetDeclaringScope()
        {
            return Assembly?.GetScope();
        }

        public IScope GetScope()
        {
            return _scope ?? (_scope = new CompilationUnitScope(this));
        }
    }
}
