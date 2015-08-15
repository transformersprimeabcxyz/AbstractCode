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
using System.Collections.ObjectModel;
using System.Linq;

namespace AbstractCode.Ast.Parser
{
    public class GrammarData
    {
        private readonly HashSet<GrammarElement> _allElements;
        private int _lr0ItemCounter;

        public GrammarData(Grammar grammar)
        {
            if (grammar == null)
                throw new ArgumentNullException(nameof(grammar));
            Grammar = grammar;

            AugmentedRoots =  new List<GrammarDefinition>();
            _allElements = new HashSet<GrammarElement>();
            GrammarReductionsMapping = new Dictionary<GrammarDefinition, List<GrammarReduction>>();
            AllReductions = new List<GrammarReduction>();

            foreach (var rootDefinition in grammar.RootDefinitions)
            {
                var augmentedRoot = new GrammarDefinition(rootDefinition.Name + "'", rootDefinition + Grammar.Eof);
                if (rootDefinition == grammar.DefaultRoot)
                    DefaultAugmentedRoot = augmentedRoot;
                AugmentedRoots.Add(augmentedRoot);
                AddReductions(augmentedRoot);
            }
            
            if(DefaultAugmentedRoot==null)
                throw new InvalidOperationException("Default root of grammar is not present in the root definitions collection.");

            AllElements = new ReadOnlyCollection<GrammarElement>(_allElements.ToArray());
        }

        public Grammar Grammar
        {
            get;
        }

        public GrammarDefinition DefaultAugmentedRoot
        {
            get;
        }

        public IList<GrammarDefinition> AugmentedRoots
        {
            get;
        }
        
        public IDictionary<GrammarDefinition, List<GrammarReduction>> GrammarReductionsMapping
        {
            get;
        }

        public IList<GrammarReduction> AllReductions
        {
            get;
        }

        public IList<GrammarElement> AllElements
        {
            get;
        }

        protected void AddReductions(GrammarDefinition root)
        {
            // Avoid infinite recursion.
            if (GrammarReductionsMapping.ContainsKey(root))
                return;
            _allElements.Add(root);

            var reductions = GrammarReductionsMapping[root] = new List<GrammarReduction>();

            // For each sequence in the grammar switch, create a new reduction with 
            // its own LR(0) kernels.
            foreach (var sequence in root.Rule.Switch)
            {
                var reduction = new GrammarReduction(root, sequence);
                reductions.Add(reduction);
                AllReductions.Add(reduction);

                for (int index = 0; index < sequence.Count; index++)
                {
                    var element = sequence[index];

                    reduction.Lr0Items.Add(CreateLr0Item(reduction, index));

                    // The sequence can hold grammar definitions as well.
                    // Therefore we make a recursive loop to handle those.
                    var definition = element as GrammarDefinition;
                    if (definition != null)
                        AddReductions(definition);
                    else
                        _allElements.Add(element);
                }

                reduction.Lr0Items.Add(CreateLr0Item(reduction, sequence.Count));
            }
        }

        protected Lr0Item CreateLr0Item(GrammarReduction reduction, int index)
        {
            return new Lr0Item(_lr0ItemCounter++, reduction, index);
        }
    }
}