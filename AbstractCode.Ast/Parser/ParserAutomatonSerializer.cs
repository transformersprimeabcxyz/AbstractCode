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
using System.IO;
using System.Linq;
using System.Text;

namespace AbstractCode.Ast.Parser
{
    public class ParserAutomatonSerializer
    {
        private sealed class SerializerContext
        {
            public readonly ParserAutomaton Automaton;
            public readonly BinaryWriter Writer;

            public SerializerContext(ParserAutomaton automaton, BinaryWriter writer)
            {
                Writer = writer;
                Automaton = automaton;
            }
        }

        private sealed class DeserializerContext
        {
            public readonly ParserAutomaton Automaton;
            public readonly BinaryReader Reader;
            
            public DeserializerContext(Grammar grammar, BinaryReader reader)
            {
                Reader = reader;
                Automaton = new ParserAutomaton(grammar);
            }
        }

        private const byte ShiftParserActionId = 0;
        private const byte ReduceParserActionId = 1;
        private const byte AcceptarserActionId = 2;

        public ParserAutomatonSerializer(GrammarData data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));
            Data = data;
        }

        public GrammarData Data
        {
            get;
        }

        public void Serialize(Stream outputStream, ParserAutomaton automaton)
        {
            var writer = new BinaryWriter(outputStream);
            var context = new SerializerContext(automaton, writer);

            writer.Write((ushort)automaton.States.Count);
            writer.Write((ushort)automaton.DefaultInitialState.Id);
            writer.Write((byte)automaton.InitialStates.Count);

            foreach (var state in context.Automaton.InitialStates)
                WriteInitialStateEntry(writer, state);

            foreach (var state in context.Automaton.States)
                WriteState(context, state);
        }

        public ParserAutomaton Deserialize(Stream inputStream)
        {
            var reader = new BinaryReader(inputStream);
            var context = new DeserializerContext(Data.Grammar, reader);

            int statesCount = reader.ReadUInt16();
            int defaultInitialStateId = reader.ReadUInt16();
            int initialStatesCount = reader.ReadByte();

            var initialStateEntries = new Dictionary<int, int>();
            for (int i = 0; i < initialStatesCount; i++)
                initialStateEntries.Add(reader.ReadUInt16(), reader.ReadUInt16());

            for (int i = 0; i < statesCount; i++)
            {
                context.Automaton.States.Add(new ParserState()
                {
                    Id = i
                });
            }

            context.Automaton.DefaultInitialState = context.Automaton.States[defaultInitialStateId];
            foreach (var entry in initialStateEntries)
                context.Automaton.InitialStates.Add((GrammarDefinition)Data.AllElements[entry.Key],
                    context.Automaton.States[entry.Value]);
            

            for (int i = 0; i < statesCount; i++)
            {
                ReadAndInitializeState(context, i);
            }

            return context.Automaton;

        }

        private void WriteInitialStateEntry(BinaryWriter writer, KeyValuePair<GrammarDefinition, ParserState> state)
        {
            writer.Write((ushort)Data.AllElements.IndexOf(state.Key));
            writer.Write((ushort)state.Value.Id);
        }
        

        private void WriteState(SerializerContext context, ParserState state)
        {
            var writer = context.Writer;
            bool hasDefaultAction = state.DefaultAction != null;
            writer.Write(hasDefaultAction);
            if (hasDefaultAction)
            {
                WriteAction(context, state.DefaultAction);
            }
            else
            {
                writer.Write((ushort)state.Actions.Count);
                foreach (var elementActionPair in state.Actions)
                {
                    writer.Write((short)Data.AllElements.IndexOf(elementActionPair.Key));
                    WriteAction(context, elementActionPair.Value);
                }
            }
        }

        private void ReadAndInitializeState(DeserializerContext context, int id)
        {
            var state = context.Automaton.States[id];
            var reader = context.Reader;
            bool hasDefaultAction = reader.ReadBoolean();
            if (hasDefaultAction)
            {
                state.DefaultAction = ReadAction(context);
            }
            else
            {
                int actionsCount = reader.ReadUInt16();
                for (int i = 0; i < actionsCount; i++)
                {
                    int elementId = reader.ReadInt16();
                    var element = elementId == -1 ? Grammar.Eof : Data.AllElements[elementId];
                    state.Actions.Add(element, ReadAction(context));
                }
            }
        }

        private void WriteAction(SerializerContext context, ParserAction action)
        {
            var shiftAction = action as ShiftParserAction;
            var writer = context.Writer;
            if (shiftAction != null)
            {
                writer.Write(ShiftParserActionId);
                writer.Write((ushort)shiftAction.NextState.Id);
                return;
            }

            var reduceAction = action as ReduceParserAction;
            if (reduceAction != null)
            {
                writer.Write(ReduceParserActionId);
                writer.Write((ushort)Data.AllReductions.IndexOf(reduceAction.Reduction));
                return;
            }

            if (action is AcceptParserAction)
            {
                writer.Write(AcceptarserActionId);
                return;
            }

            throw new NotSupportedException("Action is not supported.");
        }

        private ParserAction ReadAction(DeserializerContext context)
        {
            var reader = context.Reader;
            var actionType = reader.ReadByte();
            switch (actionType)
            {
                case ShiftParserActionId:
                    return new ShiftParserAction(context.Automaton.States[reader.ReadUInt16()]);
                case ReduceParserActionId:
                    return new ReduceParserAction(Data.AllReductions[reader.ReadUInt16()]);
                case AcceptarserActionId:
                    return new AcceptParserAction();
                default:
                    throw new NotSupportedException("Unrecognized or unsupported action type " + actionType);
            }
        }
        
    }
}
