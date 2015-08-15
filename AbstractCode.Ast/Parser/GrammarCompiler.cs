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
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AbstractCode.Ast.Parser
{
    public static class GrammarCompiler
    {
        public static GrammarCompilationResult Compile(Grammar grammar)
        {
            return Compile(new GrammarData(grammar));
        }

        public static GrammarCompilationResult Compile(GrammarData grammarData)
        {
            if (grammarData == null)
                throw new ArgumentNullException(nameof(grammarData));

            var context = new GrammarCompilationContext(grammarData);
            CreateLr0States(context);
            ConstructAugmentedGrammar(context);
            ComputeLookaheads(context);
            PlaceReduceActions(context);

            var automaton = CreateAutomaton(context);
            var result = new GrammarCompilationResult(context, automaton);
            return result;
        }

        private static void CreateLr0States(GrammarCompilationContext context)
        {
            foreach (var augmentedRoot in context.GrammarData.AugmentedRoots)
            {
                var initialState = CreateInitialState(context, augmentedRoot);
                context.InitialStates.Add(augmentedRoot, initialState);

                // Don't replace this with a foreach loop. States are being added on the fly.
                for (int i = 0; i < context.States.Count; i++)
                    ExpandState(context, context.States[i]);

                ReplaceRootReductionWithAcceptance(augmentedRoot, initialState);
            }
        }

        private static IntermediateParserState CreateInitialState(GrammarCompilationContext context, GrammarDefinition augmentedRoot)
        {
            // The initial state contains the very first LR(0) item, which is the augmented root '.Root'.
            var initialItem = context.GrammarData.GrammarReductionsMapping[augmentedRoot][0].Lr0Items[0];
            var initialState = context.GetOrCreateState(new [] { initialItem });
            return initialState;
        }

        private static void ReplaceRootReductionWithAcceptance(GrammarDefinition augmentedRoot, IntermediateParserState initialState)
        {
            // The augmented root only contains one sequence.
            var augmentedRootSequence = augmentedRoot.Rule.Switch[0];
            var rootDefinition = (GrammarDefinition)augmentedRootSequence[0];

            // Replace the existing reduce action with the accept action.
            var shiftAction = (ShiftParserAction)initialState.State.Actions[rootDefinition];
            var shiftState = shiftAction.NextState;
            shiftState.Actions[Grammar.Eof] = new AcceptParserAction();
            shiftState.DefaultAction = null;
        }

        private static void ExpandState(GrammarCompilationContext context, IntermediateParserState state)
        {
            // Check for possible shifts.
            foreach (var shifter in state.GetShifters())
            {
                var currentElement = shifter.Kernel.Element;

                // Get all LR(1) items sharing the same grammar element, get or 
                // create the state that corresponds to the shifted items, and create 
                // a transition to that state.
                var shiftedKernels = from item in state.Items
                    let kernel = item.Kernel
                    where kernel.Element == currentElement
                    select kernel.NextItem;
                var kernelsArray = shiftedKernels.ToArray();

                var nextState = context.GetOrCreateState(kernelsArray);
                shifter.NextItem = nextState.Items.First(x => x.Kernel == shifter.Kernel.NextItem);
                context.GetOrCreateTransition(state, currentElement, nextState);
                if (!state.State.Actions.ContainsKey(currentElement))
                    state.State.Actions[currentElement] = new ShiftParserAction(nextState.State);
            }
        }

        private static void ConstructAugmentedGrammar(GrammarCompilationContext context)
        {
            // Augmented grammar is constructed as described in page 197 of
            // http://web.stanford.edu/class/archive/cs/cs143/cs143.1128/lectures/06/Slides06.pdf

            foreach (var state in context.States)
            {
                foreach (var item in state.Items)
                {
                    if (!item.Kernel.IsInitializer)
                        continue;

                    var reduction = new AugmentedGrammarReduction()
                    {
                        Product = context.GetOrCreateAnnotatedElement(item.Kernel.Reduction.Product,
                            state.GetOutgoingTransition(item.Kernel.Reduction.Product)),
                        Reduction = item.Kernel.Reduction
                    };

                    for (var element = item; !element.Kernel.IsFinalizer; element = element.NextItem)
                    {
                        ParserTransition transition = null;

                        var definition = element.Kernel.Element as GrammarDefinition;
                        if (definition != null)
                            transition = element.State.GetOutgoingTransition(definition);
                        
                        var annotatedElement = context.GetOrCreateAnnotatedElement(element.Kernel.Element, transition);
                        annotatedElement.Parents.Add(reduction);
                        reduction.Sequence.Add(annotatedElement);
                    }
                }
            }
        }

        private static void ComputeLookaheads(GrammarCompilationContext context)
        {
            foreach (var state in context.States)
            {
                foreach (var item in state.Items)
                {
                    if (!item.Kernel.IsInitializer)
                        continue;

                    var finalizer = item.GetFinalizer();
                    if (finalizer.State.Items.Count == 1)
                        continue;

                    var annotatedElement = context.AllAnnotatedElements.FirstOrDefault(x => x.Element == item.Kernel.Reduction.Product && x.Transition?.Source == state);
                    if (annotatedElement!= null)    
                        finalizer.Lookahead.UnionWith(annotatedElement.GetFollowSet());
                }
            }
        }

        private static void PlaceReduceActions(GrammarCompilationContext context)
        {
            foreach (var state in context.States)
            {
                var finalizers = state.GetFinalizers().ToArray();
                if (state.Items.Count == finalizers.Length)
                {
                    // state only has finalizers reducing to the same product.
                    state.State.DefaultAction = new ReduceParserAction(finalizers[0].Kernel.Reduction);
                }
                else
                {
                    // 
                    foreach (var item in finalizers)
                    {
                        foreach (var lookahead in item.Lookahead)
                        {
                            var reduceAction = new ReduceParserAction(item.Kernel.Reduction);
                            ParserAction action;
                            if (state.State.Actions.TryGetValue(lookahead, out action))
                                context.Conflicts.Add(new GrammarReductionConflict(state.State, lookahead, action,
                                    reduceAction));
                            else
                                state.State.Actions[lookahead] = reduceAction;
                        }
                    }
                }
            }
        }

        private static ParserAutomaton CreateAutomaton(GrammarCompilationContext context)
        {
            var automaton = new ParserAutomaton(context.GrammarData.Grammar);
            foreach (var initialState in context.InitialStates)
                automaton.InitialStates.Add((GrammarDefinition)initialState.Key.Rule.Switch[0][0], initialState.Value.State);
            automaton.DefaultInitialState = context.InitialStates[context.GrammarData.DefaultAugmentedRoot].State;
            automaton.States.AddRange(context.States.Select(x => x.State));
            return automaton;
        }
    }

    public class AnnotatedGrammarElement
    {
        private HashSet<TokenGrammarElement> _firstSet;
        private HashSet<TokenGrammarElement> _followSet;

        public AnnotatedGrammarElement(GrammarElement element, ParserTransition transition)
        {
            Element = element;
            Transition = transition;
            Parents = new HashSet<AugmentedGrammarReduction>();
        }

        public HashSet<AugmentedGrammarReduction> Parents
        {
            get;
        }
        
        public GrammarElement Element
        {
            get;
        }

        public ParserTransition Transition
        {
            get;
        }

        public HashSet<TokenGrammarElement> GetFirstSet()
        {
            return _firstSet ?? (_firstSet = new HashSet<TokenGrammarElement>(Element.GetFirstItems()));
        }

        public HashSet<TokenGrammarElement> GetFollowSet()
        {
            if (_followSet != null)
                return _followSet;

            _followSet = new HashSet<TokenGrammarElement>();
            var token = Element as TokenGrammarElement;
            if (token != null)
            {
                _followSet.Add(token);
                return _followSet;
            }
            
            foreach (var reduction in Parents)
            {
                var index = reduction.Sequence.IndexOf(this);
                if (index != -1)
                {
                    bool containsEpsilon = false;
                    if (index < reduction.Sequence.Count - 1)
                    {
                        foreach (var item in reduction.Sequence[index + 1].GetFirstSet())
                        {
                            if (item == Grammar.Epsilon)
                                containsEpsilon = true;
                            else
                                _followSet.Add(item);
                        }
                    }

                    if (containsEpsilon || index == reduction.Sequence.Count - 1)
                        _followSet.UnionWith(reduction.Product.GetFollowSet());
                }
            }
            
            return _followSet;
        }

        public override string ToString()
        {
            if (Transition == null)
                return Element.ToString();
            return string.Format("{0}({1}\u2192{2})",
                Element,
                Transition.Source.State.Id,
                Transition.Destination?.State.Id);
        }
    }

    public class AugmentedGrammarReduction 
    {
        public AugmentedGrammarReduction()
        {
            Sequence = new List<AnnotatedGrammarElement>();
        }

        public GrammarReduction Reduction
        {
            get;
            set;
        }

        public AnnotatedGrammarElement Product
        {
            get;
            set;
        }

        public IList<AnnotatedGrammarElement> Sequence
        {
            get;
        }
        
        public override string ToString()
        {
            return string.Format("{0} \u2192 {1}", Product, string.Join(" ", Sequence.Select(x => x.ToString())));
        }
    }
}