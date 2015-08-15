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
    /// Represents a parameter in a method.
    /// </summary>
    public abstract class ParameterDefinition : VariableDefinition
    {
        /// <summary>
        /// Gets the index of the parameter in the method's parameter collection.
        /// </summary>
        public abstract int Index
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the parameter's argument value is passed by value.
        /// </summary>
        public abstract bool IsByVal
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the parameter's argument value is passed by reference.
        /// </summary>
        public abstract bool IsByRef
        {
            get;
        }

        /// <summary>
        /// Gets a value indicating whether the parameter's argument is given by the method itself and is used as a return value.
        /// </summary>
        public abstract bool IsOut
        {
            get;
        }
    }
}