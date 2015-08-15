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
namespace AbstractCode.Symbols
{
    /// <summary>
    /// Represents a member in an assembly or type.
    /// </summary>
    public abstract class MemberDefinition : INamedDefinition
    {
        /// <inheritDoc />
        public abstract string Name
        {
            get;
        }

        /// <summary>
        /// Gets the assembly that defines the member.
        /// </summary>
        public virtual AssemblyDefinition Assembly
        {
            get { return DeclaringType?.Assembly; }
        }
        
        /// <summary>
        /// Gets the type that defines the member.
        /// </summary>
        public abstract TypeDefinition DeclaringType
        {
            get;
        }
        
        /// <summary>
        /// Gets the member return type of the member.
        /// </summary>
        public abstract TypeDefinition MemberType
        {
            get;
        }

        /// <summary>
        /// Gets the full name of the member.
        /// </summary>
        public virtual string FullName
        {
            get
            {
                var parentName = DeclaringType?.FullName;
                // string returnTypeName = ReturnType.FullName;
                // return parentName != null ? string.Format("{0} {1}::{2}", returnTypeName, parentName, Name) : string.Format("{0} {1}", returnTypeName, parentName, Name);
                return string.IsNullOrEmpty(parentName) ? Name : string.Format("{0}::{1}", parentName, Name);
            }
        }

        public abstract IScope GetDeclaringScope();

        public override string ToString()
        {
            return FullName;
        }
    }
}