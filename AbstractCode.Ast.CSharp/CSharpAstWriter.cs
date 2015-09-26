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
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractCode.Ast;
using AbstractCode.Ast.CSharp.Expressions;
using AbstractCode.Ast.CSharp.Members;
using AbstractCode.Ast.CSharp.Statements;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;
using Microsoft.SqlServer.Server;
using AssignmentExpression = AbstractCode.Ast.Expressions.AssignmentExpression;
using BinaryOperatorExpression = AbstractCode.Ast.Expressions.BinaryOperatorExpression;
using CatchClause = AbstractCode.Ast.Statements.CatchClause;
using ConditionalExpression = AbstractCode.Ast.Expressions.ConditionalExpression;
using ConstructorDeclaration = AbstractCode.Ast.Members.ConstructorDeclaration;
using DirectionExpression = AbstractCode.Ast.Expressions.DirectionExpression;
using DoLoopStatement = AbstractCode.Ast.Statements.DoLoopStatement;
using ExplicitCastExpression = AbstractCode.Ast.Expressions.ExplicitCastExpression;
using IfElseStatement = AbstractCode.Ast.Statements.IfElseStatement;
using LockStatement = AbstractCode.Ast.Statements.LockStatement;
using MemberReferenceExpression = AbstractCode.Ast.Expressions.MemberReferenceExpression;
using ModifierElement = AbstractCode.Ast.Members.ModifierElement;
using ParameterDeclaration = AbstractCode.Ast.Members.ParameterDeclaration;
using SwitchCaseLabel = AbstractCode.Ast.Statements.SwitchCaseLabel;
using SwitchStatement = AbstractCode.Ast.Statements.SwitchStatement;
using TypeCheckExpression = AbstractCode.Ast.Expressions.TypeCheckExpression;
using TypeDeclaration = AbstractCode.Ast.Members.TypeDeclaration;
using TypeParameterDeclaration = AbstractCode.Ast.Members.TypeParameterDeclaration;
using UnaryOperatorExpression = AbstractCode.Ast.Expressions.UnaryOperatorExpression;
using UsingStatement = AbstractCode.Ast.Statements.UsingStatement;
using WhileLoopStatement = AbstractCode.Ast.Statements.WhileLoopStatement;
using YieldStatement = AbstractCode.Ast.Statements.YieldStatement;

namespace AbstractCode.Ast.CSharp
{
    public class CSharpAstWriter : ICSharpAstVisitor
    {
        private static readonly CSharpLanguage _language = CSharpLanguage.Instance;
        private bool _inForOrFixedStatementHeader;

        public CSharpAstWriter(IOutputFormatter formatter, CSharpAstWriterParameters parameters)
        {
            Formatter = formatter;
            Parameters = parameters;
        }

        public IOutputFormatter Formatter
        {
            get;
            private set;
        }

        protected CSharpAstWriterParameters Parameters
        {
            get;
            private set;
        }

        private void WriteNodes(IEnumerable<AstNode> nodes, bool elementsOnNewLine = false)
        {
            WriteSeparatedNodes(nodes, elementsOnNewLine ? (Action)Formatter.WriteLine : () => { });
        }

        private void WriteCommaSeparatedNodes(IEnumerable<AstNode> nodes, bool elementsOnNewLine = false)
        {
            WriteSeparatedNodes(nodes, () =>
            {
                Formatter.WriteToken(",");
                if (elementsOnNewLine)
                    Formatter.WriteLine();
                else 
                    Formatter.WriteSpace();
            });
        }

        private void WriteSpaceSeparatedNodes(IEnumerable<AstNode> nodes)
        {
            WriteSeparatedNodes(nodes, Formatter.WriteSpace);
        }

        private void WriteSeparatedNodes(IEnumerable<AstNode> nodes, Action writeSeparator)
        {
            var nodesArray = nodes.ToArray();
            for (int index = 0; index < nodesArray.Length; index++)
            {
                var node = nodesArray[index];

                node.AcceptVisitor(this);
                if (index < nodesArray.Length - 1)
                    writeSeparator();
            }
        }

        private void WriteSemicolon()
        {
            if (!_inForOrFixedStatementHeader)
                Formatter.WriteToken(";");
        }
        
        private void WriteTypeParametersOrArguments(IEnumerable<AstNode> elements)
        {
            var elementsArray = elements.ToArray();
            if (elementsArray.Length > 0)
            {
                Formatter.WriteToken("<");
                WriteCommaSeparatedNodes(elementsArray);
                Formatter.WriteToken(">");
            }
        }

        private void WriteEmbeddedStatement(Statement statement)
        {
            var blockStatement = statement as BlockStatement;
            if (blockStatement != null)
            {
                VisitBlockStatement(blockStatement);
            }
            else
            {
                Formatter.WriteLine();
                Formatter.Indent();
                statement.AcceptVisitor(this);
                Formatter.Unindent();
            }
        }

        public void VisitUndocumentedCSharpKeywordExpression(UndocumentedCSharpKeywordExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword(UndocumentedCSharpKeywordExpression.KeywordToString(expression.Keyword));

            if (expression.Arguments.Count > 0)
            {
                Formatter.WriteToken("(");
                WriteCommaSeparatedNodes(expression.Arguments);
                Formatter.WriteToken(")");
            }

            Formatter.EndNode();
        }

        public void VisitCheckedExpression(Expressions.CheckedExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("checked");
            Formatter.WriteToken("(");
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }
        
        public void VisitDefaultExpression(Expressions.DefaultExpression expression)
        {
            Formatter.StartNode(expression);
        }

        public void VisitForLoopStatement(Statements.ForLoopStatement statement)
        {
            Formatter.StartNode(statement);

            _inForOrFixedStatementHeader = true;
            Formatter.WriteKeyword("for");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");

            WriteCommaSeparatedNodes(statement.Initializers);
            Formatter.WriteToken(";");
            Formatter.WriteSpace();
            statement.Condition?.AcceptVisitor(this);
            Formatter.WriteToken(";");
            Formatter.WriteSpace();
            WriteCommaSeparatedNodes(statement.Iterators);

            Formatter.WriteToken(")");
            _inForOrFixedStatementHeader = false;

            WriteEmbeddedStatement(statement.Body);

            Formatter.EndNode();
        }

        public void VisitFixedStatement(FixedStatement statement)
        {
            Formatter.StartNode(statement);

            _inForOrFixedStatementHeader = true;
            Formatter.WriteKeyword("fixed");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.VariableDeclaration.AcceptVisitor(this);
            Formatter.WriteToken(")");
            _inForOrFixedStatementHeader = false;

            WriteEmbeddedStatement(statement.Body);

            Formatter.EndNode();
        }

        public void VisitForeachLoopStatement(Statements.ForeachLoopStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("foreach");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");

            statement.Type.AcceptVisitor(this);
            Formatter.WriteSpace();
            statement.Identifier.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("in");
            Formatter.WriteSpace();
            statement.Target.AcceptVisitor(this);

            Formatter.WriteToken(")");

            WriteEmbeddedStatement(statement.Body);

            Formatter.EndNode();
        }

        public void VisitSizeOfExpression(Expressions.SizeOfExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("sizeof");
            Formatter.WriteToken("(");
            expression.TargetType.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitStackAllocExpression(Expressions.StackAllocExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("stackalloc");
            Formatter.WriteSpace();
            expression.Type.AcceptVisitor(this);
            Formatter.WriteToken("[");
            expression.Counter.AcceptVisitor(this);
            Formatter.WriteToken("]");
            Formatter.EndNode();
        }

        public void VisitUncheckedExpression(Expressions.UncheckedExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("unchecked");
            Formatter.WriteToken("(");
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitBreakStatement(Statements.BreakStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("break");
            WriteSemicolon();
            Formatter.EndNode();
        }

        public void VisitCheckedStatement(Statements.CheckedStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("checked");
            statement.Body.AcceptVisitor(this);
            Formatter.EndNode();
        }
        
        public void VisitContinueStatement(Statements.ContinueStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("continue");
            WriteSemicolon();
            Formatter.EndNode();
        }

        public void VisitUncheckedStatement(Statements.UncheckedStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("unchecked");
            statement.Body.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisistUnsafeStatement(Statements.UnsafeStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("unsafe");
            statement.Body.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitYieldBreakStatement(Statements.YieldBreakStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteKeyword("yield");
            Formatter.WriteSpace();
            Formatter.WriteKeyword("break");
            WriteSemicolon();
            Formatter.EndNode();
        }

        public void VisitConstructorInitializer(Members.ConstructorInitializer initializer)
        {
            Formatter.StartNode(initializer);

            Formatter.WriteKeyword(Members.ConstructorInitializer.VariantToString(initializer.Variant));
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(initializer.Arguments);
            Formatter.WriteToken(")");

            Formatter.EndNode();
        }

        public void VisitCompilationUnit(CompilationUnit compilationUnit)
        {
            WriteNodes(compilationUnit.Children, true);
        }

        public void VisitArrayInitializer(ArrayInitializer initializer)
        {
            Formatter.StartNode(initializer);

            Formatter.OpenBrace(Parameters.ArrayInitializerBraceStyle);
            WriteCommaSeparatedNodes(initializer.Elements, true);
            Formatter.CloseBrace(Parameters.ArrayInitializerBraceStyle);

            Formatter.EndNode();
        }

        public void VisitAssignmentExpression(AssignmentExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken(CSharpLanguage.OperatorToString(expression.Operator));
            Formatter.WriteSpace();
            expression.Value.AcceptVisitor(this);
            Formatter.EndNode();
        }
        
        public void VisitBaseReferenceExpression(BaseReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("base");
            Formatter.EndNode();
        }

        public void VisitBinaryOperatorExpression(BinaryOperatorExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Left.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken(CSharpLanguage.OperatorToString(expression.Operator));
            Formatter.WriteSpace();
            expression.Right.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitConditionalExpression(ConditionalExpression expression)
        {
            Formatter.StartNode(expression);

            expression.Condition.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken("?");
            Formatter.WriteSpace();
            expression.TrueExpression.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken(":");
            Formatter.WriteSpace();
            expression.FalseExpression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitCreateArrayExpresion(CreateArrayExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("new");
            Formatter.WriteSpace();
            expression.Type.AcceptVisitor(this);
            Formatter.WriteToken("[");
            WriteCommaSeparatedNodes(expression.Arguments);
            Formatter.WriteToken("]");

            expression.Initializer?.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitCreateObjectExpression(CreateObjectExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("new");
            Formatter.WriteSpace();
            expression.Type.AcceptVisitor(this);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(expression.Arguments);
            Formatter.WriteToken(")");

            expression.Initializer?.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitDirectionExpression(DirectionExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword(CSharpLanguage.OperatorToString(expression.Direction));
            Formatter.WriteSpace();
            expression.Expression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitExplicitCastExpression(ExplicitCastExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteToken("(");
            expression.TargetType.AcceptVisitor(this);
            Formatter.WriteToken(")");
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitGetTypeExpression(GetTypeExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("typeof");
            Formatter.WriteToken("(");
            expression.TargetType.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitIdentifierExpression(IdentifierExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Identifier.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitIndexerExpression(IndexerExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteToken("[");
            WriteCommaSeparatedNodes(expression.Indices);
            Formatter.WriteToken("]");
            Formatter.EndNode();
        }

        public void VisitInvocationExpression(InvocationExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(expression.Arguments);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitLinqExpression(LinqExpression expression)
        {
            Formatter.StartNode(expression);

            WriteNodes(expression.Clauses, true);

            Formatter.EndNode();
        }

        public void VisitLinqFromClause(LinqFromClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("from");
            Formatter.WriteSpace();
            clause.VariableName.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("in");
            Formatter.WriteSpace();
            clause.DataSource.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqLetClause(LinqLetClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("let");
            Formatter.WriteSpace();
            clause.Variable.AcceptVisitor(this);
            
            Formatter.EndNode();
        }

        public void VisitLinqGroupByClause(LinqGroupByClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("group");
            Formatter.WriteSpace();
            clause.Expression.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("by");
            Formatter.WriteSpace();
            clause.KeyExpression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqOrderByClause(LinqOrderByClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("orderby");
            Formatter.WriteSpace();
            WriteCommaSeparatedNodes(clause.Ordernings);

            Formatter.EndNode();
        }

        public void VisitLinqOrdering(LinqOrdering ordering)
        {
            Formatter.StartNode(ordering);

            ordering.Expression.AcceptVisitor(this);
            if (ordering.Direction != LinqOrderingDirection.None)
            {
                Formatter.WriteSpace();
                Formatter.WriteKeyword(CSharpLanguage.OperatorToString(ordering.Direction));
            }

            Formatter.EndNode();
        }

        public void VisitLinqWhereClause(LinqWhereClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("where");
            Formatter.WriteSpace();
            clause.Condition.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqSelectClause(LinqSelectClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("select");
            Formatter.WriteSpace();
            clause.Target.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitMemberReferenceExpression(MemberReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteToken(CSharpLanguage.OperatorToString(expression.Accessor));
            expression.Identifier.AcceptVisitor(this);
            Formatter.EndNode();
        }
        
        public void VisitParenthesizedExpression(ParenthesizedExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteToken("(");
            expression.Expression.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitPrimitiveExpression(PrimitiveExpression expression)
        {
            Formatter.StartNode(expression);

            if (expression.Value == null)
                Formatter.WriteKeyword("null");
            else if (expression.Value is string)
                Formatter.WriteToken(_language.StringFormatter.FormatString((string)expression.Value));
            else if (expression.Value is char)
                Formatter.WriteToken(_language.StringFormatter.FormatChar((char)expression.Value));
            else if (expression.Value is bool)
                Formatter.WriteKeyword((bool)expression.Value ? "true" : "false");
            else
                Formatter.WriteToken(_language.NumberFormatter.FormatNumber(expression.Value));

            Formatter.EndNode();
        }

        public void VisitSafeCastExpression(SafeCastExpression expression)
        {
            Formatter.StartNode(expression);
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("as");
            Formatter.WriteSpace();
            expression.TargetType.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitThisReferenceExpression(ThisReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("this");
            Formatter.EndNode();
        }

        public void VisitTypeCheckExpression(TypeCheckExpression expression)
        {
            Formatter.StartNode(expression);
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("is");
            Formatter.WriteSpace();
            expression.TargetType.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitUnaryOperatorExpression(UnaryOperatorExpression expression)
        {
            Formatter.StartNode(expression);

            switch (expression.Operator)
            {
                case UnaryOperator.PostDecrement:
                case UnaryOperator.PostIncrement:
                    expression.Expression.AcceptVisitor(this);
                    Formatter.WriteToken(CSharpLanguage.OperatorToString(expression.Operator));
                    break;
                case UnaryOperator.Await:
                    Formatter.WriteKeyword("await");
                    Formatter.WriteSpace();
                    expression.Expression.AcceptVisitor(this);
                    break;
                default:
                    Formatter.WriteToken(CSharpLanguage.OperatorToString(expression.Operator));
                    expression.Expression.AcceptVisitor(this);
                    break;
            }

            Formatter.EndNode();
        }

        public void VisitAccessorDeclaration(AccessorDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            if (declaration.Title == PropertyDeclaration.GetterTitle)
                Formatter.WriteKeyword("get");
            else if (declaration.Title == PropertyDeclaration.SetterTitle)
                Formatter.WriteKeyword("set");
            else if (declaration.Title == EventDeclaration.AddAccessorTitle)
                Formatter.WriteKeyword("add");
            else if (declaration.Title == EventDeclaration.RemoveAccessorTitle)
                Formatter.WriteKeyword("remove");

            if (declaration.HasBody)
                declaration.Body.AcceptVisitor(this);
            else
                WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitConstructorDeclaration(ConstructorDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            var typeParent = declaration.Parent as TypeDeclaration;
            if (typeParent == null)
                declaration.Identifier.AcceptVisitor(this);
            else
                typeParent.Identifier.AcceptVisitor(this);

            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");

            if (declaration.HasBody)
                declaration.Body.AcceptVisitor(this);
            else
                WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitCustomAttribute(CustomAttribute attribute)
        {
            Formatter.StartNode(attribute);

            attribute.Type.AcceptVisitor(this);

            if (attribute.Arguments.Count > 0)
            {
                Formatter.WriteToken("(");
                WriteCommaSeparatedNodes(attribute.Arguments);
                Formatter.WriteToken(")");
            }

            Formatter.EndNode();
        }

        public void VisitCustomAttributeSection(CustomAttributeSection section)
        {
            Formatter.StartNode(section);

            Formatter.WriteToken("[");
            WriteCommaSeparatedNodes(section.Attributes);
            Formatter.WriteToken("]");

            Formatter.EndNode();
        }

        public void VisitDelegateDeclaration(DelegateDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            Formatter.WriteKeyword("delegate");
            Formatter.WriteSpace();
            declaration.DelegateType.AcceptVisitor(this);
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitEnumMemberDeclaration(EnumMemberDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            declaration.Declarator.AcceptVisitor(this);
            Formatter.WriteToken(",");

            Formatter.EndNode();
        }

        public void VisitEventDeclaration(EventDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            Formatter.WriteKeyword("event");
            Formatter.WriteSpace();
            declaration.EventType.AcceptVisitor(this);
            Formatter.WriteSpace();
            WriteCommaSeparatedNodes(declaration.Declarators);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitFieldDeclaration(FieldDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            declaration.FieldType.AcceptVisitor(this);
            Formatter.WriteSpace();

            WriteCommaSeparatedNodes(declaration.Declarators);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitIdentifier(Identifier identifier)
        {
            Formatter.StartNode(identifier);

            Formatter.WriteIdentifier(identifier.Name);

            Formatter.EndNode();
        }

        public void VisitMethodDeclaration(MethodDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            declaration.ReturnType.AcceptVisitor(this);
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");

            if (declaration.HasBody)
                declaration.Body.AcceptVisitor(this);
            else
                WriteSemicolon();

            Formatter.WriteLine();

            Formatter.EndNode();
        }

        public void VisitModifierElement(ModifierElement modifier)
        {
            Formatter.StartNode(modifier);
            Formatter.WriteKeyword(CSharpLanguage.ModifierToString(modifier.Modifier));
            Formatter.EndNode();
        }

        public void VisitNamespaceDeclaration(NamespaceDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            Formatter.WriteKeyword("namespace");
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            Formatter.OpenBrace(Parameters.NamespaceBraceStyle);

            WriteNodes(declaration.UsingDirectives);
            WriteNodes(declaration.Types);

            Formatter.CloseBrace(Parameters.NamespaceBraceStyle);
            Formatter.WriteLine();


            Formatter.EndNode();
        }

        public void VisitParameterDeclaration(ParameterDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteSpace();
            }

            // TODO: param modifier.

            declaration.ParameterType.AcceptVisitor(this);

            if (declaration.Declarator != null)
            {
                Formatter.WriteSpace();
                declaration.Declarator.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitPropertyDeclaration(PropertyDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            declaration.PropertyType.AcceptVisitor(this);
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);

            if (declaration.Parameters.Count > 0)
            {
                Formatter.WriteToken("[");
                WriteCommaSeparatedNodes(declaration.Parameters);
                Formatter.WriteToken("]");
            }

            Formatter.OpenBrace(Parameters.PropertyBraceStyle);

            if (declaration.Getter != null)
            {
                declaration.Getter.AcceptVisitor(this);
                if (declaration.Setter != null)
                    Formatter.WriteLine();
            }

            declaration.Setter?.AcceptVisitor(this);

            Formatter.CloseBrace(Parameters.PropertyBraceStyle);

            Formatter.EndNode();
        }

        public void VisitTypeDeclaration(TypeDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.CustomAttributeSections.Count > 0)
            {
                WriteNodes(declaration.CustomAttributeSections, true);
                Formatter.WriteLine();
            }

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }
            
            Formatter.WriteKeyword(CSharpLanguage.TypeVariantToString(declaration.TypeVariant));
            Formatter.WriteSpace();

            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);

            if (declaration.BaseTypes.Count > 0)
            {
                Formatter.WriteSpace();
                Formatter.WriteToken(":");
                Formatter.WriteSpace();

                WriteCommaSeparatedNodes(declaration.BaseTypes);
            }

            Formatter.OpenBrace(Parameters.TypeBraceStyle);

            WriteSeparatedNodes(declaration.Members, () =>
            {
                Formatter.WriteLine();
                // Formatter.WriteLine();
            });

            Formatter.CloseBrace(Parameters.TypeBraceStyle);
            Formatter.WriteLine();

            Formatter.EndNode();
        }

        public void VisitTypeParameterDeclaration(TypeParameterDeclaration declaration)
        {
            Formatter.StartNode(declaration);
            
            // TODO: type param variance
            declaration.Identifier.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitUsingAliasDirective(UsingAliasDirective directive)
        {
            Formatter.StartNode(directive);

            directive.AliasIdentifier.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken("=");
            Formatter.WriteSpace();
            directive.TypeImport.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitUsingNamespaceDirective(UsingNamespaceDirective namespaceDirective)
        {
            Formatter.StartNode(namespaceDirective);

            Formatter.WriteKeyword("using");
            Formatter.WriteSpace();
            namespaceDirective.NamespaceIdentifier.AcceptVisitor(this);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitVariableDeclarator(VariableDeclarator declarator)
        {
            Formatter.StartNode(declarator);

            declarator.Identifier.AcceptVisitor(this);

            if (declarator.Value != null)
            {
                Formatter.WriteSpace();
                Formatter.WriteToken("=");
                Formatter.WriteSpace();
                declarator.Value.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitAddRemoveHandlerStatement(AddRemoveHandlerStatement statement)
        {
            Formatter.StartNode(statement);

            statement.EventExpression.AcceptVisitor(this);
            Formatter.WriteSpace();
            switch (statement.Variant)
            {
                case AddRemoveHandlerVariant.Add:
                    Formatter.WriteToken("+=");
                    break;
                case AddRemoveHandlerVariant.Remove:
                    Formatter.WriteToken("-=");
                    break;
            }
            Formatter.WriteSpace();
            statement.DelegateExpression.AcceptVisitor(this);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitBlockStatement(BlockStatement statement)
        {
            Formatter.StartNode(statement);

            BraceStyle braceStyle;
            if (statement.Parent is MethodDeclaration)
                braceStyle = Parameters.MethodBraceStyle;
            else if (statement.Parent is ConstructorDeclaration)
                braceStyle = Parameters.ConstructorBraceStyle;
            else if (statement.Parent is PropertyDeclaration)
                braceStyle = Parameters.PropertyBraceStyle;
            else if (statement.Parent is EventDeclaration)
                braceStyle = Parameters.EventBraceStyle;
            else if (statement.Parent is AccessorDeclaration)
                braceStyle = Parameters.AccessorBraceStyle;
            else
                braceStyle = Parameters.StatementBraceStyle;

            Formatter.OpenBrace(braceStyle);
            WriteNodes(statement.Statements, true);
            Formatter.WriteLine();
            Formatter.CloseBrace(braceStyle);
            
            Formatter.EndNode();
        }

        public void VisitCatchClause(CatchClause catchClause)
        {
            Formatter.StartNode(catchClause);

            Formatter.WriteKeyword("catch");
            if (catchClause.ExceptionType != null)
            {
                Formatter.WriteSpace();
                Formatter.WriteToken("(");
                catchClause.ExceptionType.AcceptVisitor(this);
                if (catchClause.ExceptionIdentifier != null)
                {
                    Formatter.WriteSpace();
                    catchClause.ExceptionIdentifier.AcceptVisitor(this);
                }
                Formatter.WriteToken(")");
            }

            catchClause.Body.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitDoLoopStatement(DoLoopStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("do");

            WriteEmbeddedStatement(statement.Body);

            Formatter.WriteSpace();
            Formatter.WriteKeyword("while");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.Condition.AcceptVisitor(this);
            Formatter.WriteToken(")");
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitEmptyStatement(EmptyStatement statement)
        {
            Formatter.StartNode(statement);
            WriteSemicolon();
            Formatter.EndNode();
        }

        public void VisitExpressionStatement(ExpressionStatement statement)
        {
            Formatter.StartNode(statement);

            statement.Expression.AcceptVisitor(this);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitGotoStatement(GotoStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("goto");
            Formatter.WriteSpace();
            statement.LabelIdentifier.AcceptVisitor(this);
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitIfElseStatement(IfElseStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("if");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.Condition.AcceptVisitor(this);
            Formatter.WriteToken(")");

            WriteEmbeddedStatement(statement.TrueBlock);

            if (statement.FalseBlock != null)
            {
                // TODO: make configurable
                //Formatter.WriteLine();
                Formatter.WriteKeyword("else");
                WriteEmbeddedStatement(statement.FalseBlock);
            }

            Formatter.EndNode();
        }

        public void VisitLabelStatement(LabelStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteIdentifier(statement.Label);
            Formatter.WriteToken(":");

            Formatter.EndNode();
        }

        public void VisitLockStatement(LockStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("lock");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.LockObject.AcceptVisitor(this);
            Formatter.WriteToken(")");
            WriteEmbeddedStatement(statement.Body);

            Formatter.EndNode();
        }

        public void VisitReturnStatement(ReturnStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("return");

            if (statement.Value != null)
            {
                Formatter.WriteSpace();
                statement.Value.AcceptVisitor(this);
            }

            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitSwitchCaseLabel(SwitchCaseLabel switchCaseLabel)
        {
            Formatter.StartNode(switchCaseLabel);

            if (switchCaseLabel.IsDefaultCase)
            {
                Formatter.WriteKeyword("default");
            }
            else
            {
                Formatter.WriteKeyword("case");
                Formatter.WriteSpace();
                switchCaseLabel.Condition.AcceptVisitor(this);
            }

            Formatter.WriteToken(":");

            Formatter.EndNode();
        }

        public void VisitSwitchSection(SwitchSection switchSection)
        {
            Formatter.StartNode(switchSection);

            WriteNodes(switchSection.Labels, true);
            Formatter.WriteLine();
            Formatter.Indent();
            WriteNodes(switchSection.Statements, true);
            Formatter.Unindent();

            Formatter.EndNode();
        }

        public void VisitSwitchStatement(SwitchStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("switch");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.Condition.AcceptVisitor(this);
            Formatter.WriteToken(")");
            Formatter.OpenBrace(Parameters.StatementBraceStyle);

            WriteNodes(statement.Sections, true);

            Formatter.CloseBrace(Parameters.StatementBraceStyle);

            Formatter.EndNode();
        }

        public void VisitThrowStatement(ThrowStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("throw");

            if (statement.Expression != null)
            {
                Formatter.WriteSpace();
                statement.Expression.AcceptVisitor(this);
            }

            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitTryCatchStatement(TryCatchStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("try");
            statement.TryBlock.AcceptVisitor(this);
            
            if (statement.CatchClauses.Count > 0)
            {
                Formatter.WriteLine();
                WriteNodes(statement.CatchClauses, true);
            }
            
            if (statement.FinallyBlock != null)
            {
                Formatter.WriteLine();
                Formatter.WriteKeyword("finally");
                statement.FinallyBlock.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitUsingStatement(UsingStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("using");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.DisposableObject.AcceptVisitor(this);
            Formatter.WriteToken(")");
            statement.Body.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitVariableDeclarationStatement(VariableDeclarationStatement statement)
        {
            Formatter.StartNode(statement);

            statement.VariableType.AcceptVisitor(this);
            Formatter.WriteSpace();
            WriteNodes(statement.Declarators);
            
            WriteSemicolon();

            Formatter.EndNode();
        }

        public void VisitWhileLoopStatement(WhileLoopStatement statement)
        {
            Formatter.StartNode(statement);
            
            Formatter.WriteKeyword("while");
            Formatter.WriteSpace();
            Formatter.WriteToken("(");
            statement.Condition.AcceptVisitor(this);
            Formatter.WriteToken(")");

            WriteEmbeddedStatement(statement.Body);

            Formatter.EndNode();
        }

        public void VisitYieldStatement(YieldStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("yield");
            Formatter.WriteSpace();
            Formatter.WriteKeyword("return");
            Formatter.WriteSpace();
            statement.Value.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitArrayTypeReference(ArrayTypeReference typeReference)
        {
            Formatter.StartNode(typeReference);

            typeReference.BaseType.AcceptVisitor(this);
            typeReference.RankSpecifier.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitArrayTypeRankSpecifier(ArrayTypeRankSpecifier specifier)
        {
            Formatter.StartNode(specifier);

            Formatter.WriteToken("[");

            for (int i = 0; i < specifier.Dimensions - 1; i++)
                Formatter.WriteToken(",");

            Formatter.WriteToken("]");

            Formatter.EndNode();
        }

        public void VisitMemberTypeReference(MemberTypeReference typeReference)
        {
            Formatter.StartNode(typeReference);

            typeReference.Target.AcceptVisitor(this);
            Formatter.WriteToken(".");
            typeReference.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(typeReference.TypeArguments);

            Formatter.EndNode();
        }

        public void VisitPointerTypeReference(PointerTypeReference typeReference)
        {
            Formatter.StartNode(typeReference);

            typeReference.BaseType.AcceptVisitor(this);
            Formatter.WriteToken("*");

            Formatter.EndNode();
        }

        public void VisitSimpleTypeReference(SimpleTypeReference typeReference)
        {
            Formatter.StartNode(typeReference);

            typeReference.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(typeReference.TypeArguments);

            Formatter.EndNode();
        }

        public void VisitPrimitiveTypeReference(PrimitiveTypeReference typeReference)
        {
            Formatter.StartNode(typeReference);

            Formatter.WriteKeyword(Types.PrimitiveTypeReference.TypeToString(typeReference.PrimitiveType));

            Formatter.EndNode();
        }

        public void VisitAstToken(AstToken token)
        {
            Formatter.StartNode(token);
            //Formatter.WriteToken(token.Value);
            Formatter.EndNode();
        }

        public void VisitComment(Comment comment)
        {
            Formatter.StartNode(comment);

            if (comment.CommentType == CommentType.Block)
                Formatter.WriteTokenLine("/*");

            var lines = comment.Contents.Split('\n');
            foreach (var line in lines)
            {
                switch (comment.CommentType)
                {
                    case CommentType.Block: Formatter.WriteToken(" * "); break;
                    case CommentType.SingleLine: Formatter.WriteToken("// "); break;
                    case CommentType.Documentation: Formatter.WriteToken("/// "); break;
                }

                Formatter.WriteTokenLine(line);
            }

            Formatter.WriteTokenLine("*/");

            Formatter.EndNode();
        }

        public void VisitDefault(object item)
        {
            //
        }
    }
}
