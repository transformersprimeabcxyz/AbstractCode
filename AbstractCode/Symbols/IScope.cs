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
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.Symbols
{
    /// <summary>
    /// Provides methods for defining a resolution scope.
    /// </summary>
    public interface IScope
    {
        /// <summary>
        /// Gets the resolution scope provider that declares this resolution scope.
        /// </summary>
        /// <returns>The declaring resolution scope provider.</returns>
        IScopeProvider GetDeclaringProvider();

        /// <summary>
        /// Resolves an identifier to an actual definition.
        /// </summary>
        /// <param name="identifier">The identifier to resolve.</param>
        /// <returns>The result of the resolution process.</returns>
        ResolveResult ResolveIdentifier(string identifier);
    }

    /// <summary>
    /// Represents a basic resolution scope in a type system.
    /// </summary>
    public abstract class AbstractScope : IScope
    {
        protected AbstractScope(IScopeProvider container)
        {
            Container = container;
        }

        /// <summary>
        /// Gets the provider that defines this resolution scope.
        /// </summary>
        protected IScopeProvider Container
        {
            get;
        }

        public IScopeProvider GetDeclaringProvider()
        {
            return Container;
        }

        public virtual ResolveResult ResolveIdentifier(string identifier)
        {
            var parent = Container.GetDeclaringScope();
            if (parent != null)
                return parent.ResolveIdentifier(identifier);

            return new UnknownIdentifierResolveResult(identifier);
        }
    }

    /// <summary>
    /// Represents a basic resolution scope in a type system, using a specific type of a resolution scope provider.
    /// </summary>
    /// <typeparam name="TContainer">The type of the container that defines the scope.</typeparam>
    public abstract class AbstractScope<TContainer> : AbstractScope
        where TContainer : IScopeProvider
    {
        protected AbstractScope(TContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Gets the strongly-typed provider that defines this resolution scope.
        /// </summary>
        protected new TContainer Container
        {
            get { return (TContainer)base.Container; }
        }
    }

    /// <summary>
    /// Provides methods for representing a member in a resolution scope.
    /// </summary>
    public interface IScopeMember
    {
        IScope GetDeclaringScope();
    }
    
    /// <summary>
    /// Provides methods for representing a scope member that defines a new resolution scope.
    /// </summary>
    public interface IScopeProvider : IScopeMember
    {
        IScope GetScope();
    }
}