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
using System.Threading.Tasks;
using AbstractCode.Ast.CSharp;
using AbstractCode.Ast.Parser;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AbstractCode.Tests.Ast.Parser
{
    [TestClass]
    public class ParserAutomatonSerializerTests
    {
        [TestMethod]
        public void TestGrammar1()
        {
            TestSerializer(new TestGrammar1());
        }

        [TestMethod]
        public void TestGrammar2()
        {
            TestSerializer(new TestGrammar2());
        }

        [TestMethod]
        public void TestGrammar3()
        {
            TestSerializer(new TestGrammar3());
        }

        [TestMethod]
        public void TestGrammar4()
        {
            TestSerializer(new TestGrammar4());
        }

        [TestMethod]
        public void CSharpGrammar()
        {
            TestSerializer(CSharpLanguage.Instance.Grammar);
        }

        private static void TestSerializer(Grammar grammar)
        {
            var data = new GrammarData(grammar);
            var serializer = new ParserAutomatonSerializer(data);
            var result = GrammarCompiler.Compile(data);
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, result.Automaton);
                stream.Position = 0;
                var newAutomaton = serializer.Deserialize(stream);

                Assert.AreEqual(result.Automaton.DefaultInitialState.Id, newAutomaton.DefaultInitialState.Id);
                Assert.AreEqual(result.Automaton.States.Count, newAutomaton.States.Count);

                for (int index = 0; index < result.Automaton.States.Count; index++)
                {
                    var expectedState = result.Automaton.States[index];
                    var actualState = newAutomaton.States[index];

                    ValidateState(expectedState, actualState);
                }
            }
        }

        private static void ValidateState(ParserState expectedState, ParserState actualState)
        {
            Assert.AreEqual(expectedState.Id, actualState.Id);
            ValidateAction(expectedState.DefaultAction, actualState.DefaultAction);
            Assert.AreEqual(expectedState.Actions.Count, actualState.Actions.Count);

            var expectedActions = expectedState.Actions.ToArray();
            var actualActions = actualState.Actions.ToArray();

            for (int index = 0; index < expectedActions.Length; index++)
            {
                var expectedPair = expectedActions[index];
                var actualPair = actualActions[index];
                Assert.AreEqual(expectedPair.Key,  actualPair.Key);
                ValidateAction(expectedPair.Value, actualPair.Value);
            }
        }

        private static void ValidateAction(ParserAction expectedAction, ParserAction actualAction)
        {
            if (expectedAction == null)
            {
                Assert.IsNull(actualAction);
                return;
            }

            Assert.IsInstanceOfType(actualAction, expectedAction.GetType());

            var shiftAction = expectedAction as ShiftParserAction;
            if (shiftAction != null)
            {
                Assert.AreEqual(shiftAction.NextState.Id, ((ShiftParserAction)actualAction).NextState.Id);
                return;
            }

            var reduceAction = expectedAction as ReduceParserAction;
            if (reduceAction != null)
            {
                Assert.AreEqual(reduceAction.Reduction, ((ReduceParserAction)actualAction).Reduction);
                return;
            }
        }
    }
}
