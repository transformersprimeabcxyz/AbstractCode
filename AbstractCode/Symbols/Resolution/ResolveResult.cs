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
namespace AbstractCode.Symbols.Resolution
{
    /// <summary>
    /// Represents a result of a resolution process.
    /// </summary>
    public class ResolveResult
    {
        protected ResolveResult()
        {
        }

        public ResolveResult(IScopeProvider scopeProvider)
        {
            ScopeProvider = scopeProvider;
        }

        /// <summary>
        /// Gets a value indicating whether the resolution failed.
        /// </summary>
        public virtual bool IsError
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the error code associated with the result.
        /// </summary>
        public int ErrorCode
        {
            get;
            protected set;
        }

        /// <summary>
        /// Gets the resolution scope provider that was resolved during the resolution process, or null if none was found.
        /// </summary>
        public IScopeProvider ScopeProvider
        {
            get;
            protected set;
        }
    }
}