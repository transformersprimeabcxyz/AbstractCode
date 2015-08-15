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
using AbstractCode.Collections.Generic;
using AbstractCode.Symbols;

namespace AbstractCode.Ast
{
    public class SourceAssembly : AssemblyDefinition
    {
        public SourceAssembly(string name)
        {
            Version = new Version(1, 0, 0, 0);
            Name = name;
            CompilationUnits = new EventBasedCollection<CompilationUnit>();

            CompilationUnits.InsertingItem += CompilationUnits_InsertingItem;
            CompilationUnits.InsertedItem += CompilationUnitsOnInsertedItem;
            CompilationUnits.RemovedItem += CompilationUnitsOnRemovedItem;
        }

        public override string Name
        {
            get;
        }

        public override Version Version
        {
            get;
        }

        public override string FilePath
        {
            get { return string.Empty; }
        }

        public EventBasedCollection<CompilationUnit> CompilationUnits
        {
            get;
        }

        private static void CompilationUnits_InsertingItem(object sender, CollectionChangingEventArgs<CompilationUnit> e)
        {
            if (e.TargetObject.Assembly != null)
            {
                e.Cancel = true;
                throw new InvalidOperationException("Syntax tree is already added to another assembly.");
            }
        }

        private static void CompilationUnitsOnRemovedItem(object sender, CollectionChangedEventArgs<CompilationUnit> e)
        {
            e.TargetObject.Assembly = null;
        }

        private void CompilationUnitsOnInsertedItem(object sender, CollectionChangedEventArgs<CompilationUnit> e)
        {
            e.TargetObject.Assembly = this;
        }


        public override IEnumerable<NamespaceDefinition> GetNamespaceDefinitions()
        {
            foreach (var unit in CompilationUnits)
            {
                foreach (var definition in unit.GetNamespaceDefinitions())
                    yield return definition;
            }
        }
    }
}
