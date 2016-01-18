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
using System.Linq;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicAstWriter : IVisualBasicAstVisitor
    {
        private static readonly VisualBasicLanguage _language = VisualBasicLanguage.Instance;

        public VisualBasicAstWriter(IOutputFormatter formatter, VisualBasicAstWriterParameters parameters)
        {
            Formatter = formatter;
            Parameters = parameters;
        }

        public IOutputFormatter Formatter
        {
            get;
        }

        protected VisualBasicAstWriterParameters Parameters
        {
            get;
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

        //private void WriteNodes(IEnumerable<AstNode> nodes, bool newline = false)
        //{
        //    foreach (var node in nodes)
        //    {
        //        node.AcceptVisitor(this);

        //        if (newline)
        //            Formatter.WriteLine();
        //    }
        //}

        //private void WriteCommaSeparatedNodes(IEnumerable<AstNode> nodes, bool newline = false)
        //{
        //    WriteStringSeparatedNodes(nodes, () =>
        //    {
        //        Formatter.WriteToken(",");
        //        if (newline)
        //            Formatter.WriteLine();
        //        else
        //            Formatter.WriteSpace();
        //    });
        //}

        //private void WriteSpaceSeparatedNodes(IEnumerable<AstNode> nodes)
        //{
        //    WriteStringSeparatedNodes(nodes, Formatter.WriteSpace);
        //}

        //private void WriteStringSeparatedNodes(IEnumerable<AstNode> nodes, Action writeSeparator)
        //{
        //    bool addSeparator = false;
        //    foreach (var node in nodes)
        //    {
        //        if (addSeparator)
        //            writeSeparator();

        //        node.AcceptVisitor(this);
        //        addSeparator = true;
        //    }
        //}

        private void WriteEndBlockKeywords(string keyword)
        {
            Formatter.WriteKeyword("End");
            Formatter.WriteSpace();
            Formatter.WriteKeyword(keyword);
        }

        private void WriteAsKeyword()
        {
            Formatter.WriteSpace();
            Formatter.WriteKeyword("As");
            Formatter.WriteSpace();
        }

        private string DetermineMethodKeyword(TypeReference returnType)
        {
            var primitive = returnType as PrimitiveTypeReference;
            if (primitive == null || primitive.PrimitiveType == PrimitiveType.Void)
                return "Sub";
            return "Function";
        }

        private void WriteTypeParametersOrArguments(IEnumerable<AstNode> elements)
        {
            var elementsArray = elements.ToArray();
            if (elementsArray.Length > 0)
            {
                Formatter.WriteToken("(");
                Formatter.WriteKeyword("Of");
                Formatter.WriteSpace();
                WriteCommaSeparatedNodes(elementsArray);
                Formatter.WriteToken(")");
            }
        }

        public void VisitEndScopeBlockClause(Statements.EndScopeBlockClause endScopeBlockClause)
        {
            //
        }

        public void VisitEndStatement(Statements.EndStatement endStatement)
        {
            Formatter.StartNode(endStatement);

            Formatter.WriteKeyword("End");

            Formatter.EndNode();
        }

        public void VisitContinueStatement(Statements.ContinueStatement continueStatement)
        {
            Formatter.StartNode(continueStatement);

            Formatter.WriteKeyword("Continue");
            Formatter.WriteSpace();
            Formatter.WriteKeyword(Statements.ContinueStatement.VariantToString(continueStatement.Variant));

            Formatter.EndNode();
        }

        public void VisitExitStatement(Statements.ExitStatement exitStatement)
        {
            Formatter.StartNode(exitStatement);

            Formatter.WriteKeyword("Exit");
            Formatter.WriteSpace();
            Formatter.WriteKeyword(Statements.ExitStatement.VariantToString(exitStatement.Variant));

            Formatter.EndNode();
        }

        public void VisitHandlesClause(Members.HandlesClause handlesClause)
        {
            Formatter.StartNode(handlesClause);

            Formatter.WriteKeyword("Handles");
            Formatter.WriteSpace();
            handlesClause.EventExpression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitImplementsClause(Members.ImplementsClause implementsClause)
        {
            Formatter.StartNode(implementsClause);

            Formatter.WriteKeyword("Implements");
            Formatter.WriteSpace();
            implementsClause.MemberExpression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitCompilationUnit(CompilationUnit compilationUnit)
        {
            Formatter.StartNode(compilationUnit);

            WriteNodes(compilationUnit.Children, true);

            Formatter.EndNode();
        }

        public void VisitArrayInitializer(ArrayInitializer initializer)
        {
            Formatter.StartNode(initializer);

            Formatter.OpenBrace(Parameters.BraceStyle);
            WriteCommaSeparatedNodes(initializer.Elements, true);
            Formatter.CloseBrace(Parameters.BraceStyle);

            Formatter.EndNode();
        }

        public void VisitAssignmentExpression(AssignmentExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken(VisualBasicLanguage.OperatorToString(expression.Operator));
            Formatter.WriteSpace();
            expression.Value.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitBaseReferenceExpression(BaseReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("MyBase");
            Formatter.EndNode();
        }

        public void VisitBinaryOperatorExpression(BinaryOperatorExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Left.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteToken(VisualBasicLanguage.OperatorToString(expression.Operator));
            Formatter.WriteSpace();
            expression.Right.AcceptVisitor(this);
            Formatter.EndNode();
        }

        public void VisitConditionalExpression(ConditionalExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("If");
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(new AstNode[]
            {
                expression.Condition,
                expression.TrueExpression,
                expression.FalseExpression,
            });
            Formatter.WriteToken(")");

            Formatter.EndNode();
        }

        public void VisitCreateArrayExpresion(CreateArrayExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("New");
            Formatter.WriteSpace();
            expression.Type.AcceptVisitor(this);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(expression.Arguments);
            Formatter.WriteToken(")");

            if (expression.Initializer != null)
            {
                expression.Initializer.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitCreateObjectExpression(CreateObjectExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("New");
            Formatter.WriteSpace();
            expression.Type.AcceptVisitor(this);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(expression.Arguments);
            Formatter.WriteToken(")");
            if (expression.Initializer != null)
            {
                expression.Initializer.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitDirectionExpression(DirectionExpression expression)
        {
            Formatter.StartNode(expression);

            expression.Expression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitExplicitCastExpression(ExplicitCastExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("DirectCast");
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(new AstNode[]
            {
                expression.TargetExpression,
                expression.TargetType
            });
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitGetTypeExpression(GetTypeExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("GetType");
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
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(expression.Indices);
            Formatter.WriteToken(")");
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

            Formatter.WriteKeyword("From");
            Formatter.WriteSpace();
            clause.VariableName.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("In");
            Formatter.WriteSpace();
            clause.DataSource.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqLetClause(LinqLetClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("Let");
            Formatter.WriteSpace();
            clause.Variable.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqGroupByClause(LinqGroupByClause clause)
        {
            throw new NotImplementedException();
        }

        public void VisitLinqOrderByClause(LinqOrderByClause clause)
        {
            throw new NotImplementedException();
        }

        public void VisitLinqOrdering(LinqOrdering ordering)
        {
            throw new NotImplementedException();
        }

        public void VisitLinqWhereClause(LinqWhereClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("Where");
            Formatter.WriteSpace();
            clause.Condition.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitLinqSelectClause(LinqSelectClause clause)
        {
            Formatter.StartNode(clause);

            Formatter.WriteKeyword("Select");
            Formatter.WriteSpace();
            clause.Target.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitMemberReferenceExpression(MemberReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            expression.Target.AcceptVisitor(this);
            Formatter.WriteToken(".");
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
                Formatter.WriteKeyword("Nothing");
            else if (expression.Value is string)
                Formatter.WriteToken(_language.StringFormatter.FormatString((string)expression.Value));
            else if (expression.Value is char)
                Formatter.WriteToken(_language.StringFormatter.FormatChar((char)expression.Value));
            else if (expression.Value is bool)
                Formatter.WriteKeyword((bool)expression.Value ? "True" : "False");
            else
                Formatter.WriteToken(_language.NumberFormatter.FormatNumber(expression.Value));

            Formatter.EndNode();
        }

        public void VisitSafeCastExpression(SafeCastExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("TryCast");
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(new AstNode[]
            {
                expression.TargetExpression,
                expression.TargetType
            });
            Formatter.WriteToken(")");
            Formatter.EndNode();
        }

        public void VisitThisReferenceExpression(ThisReferenceExpression expression)
        {
            Formatter.StartNode(expression);
            Formatter.WriteKeyword("Me");
            Formatter.EndNode();
        }

        public void VisitTypeCheckExpression(TypeCheckExpression expression)
        {
            Formatter.StartNode(expression);

            Formatter.WriteKeyword("TypeOf");
            expression.TargetExpression.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("Is");
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
                    Formatter.WriteToken(VisualBasicLanguage.OperatorToString(expression.Operator));
                    break;
                case UnaryOperator.Await:
                    Formatter.WriteKeyword("Await");
                    Formatter.WriteSpace();
                    expression.Expression.AcceptVisitor(this);
                    break;
                default:
                    Formatter.WriteToken(VisualBasicLanguage.OperatorToString(expression.Operator));
                    Formatter.WriteSpace();
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

            string keyword = string.Empty;
            if (declaration.Title == PropertyDeclaration.GetterTitle)
                keyword = "Get";
            else if (declaration.Title == PropertyDeclaration.SetterTitle)
                keyword = "Set";
            else if (declaration.Title == EventDeclaration.AddAccessorTitle)
                keyword = "Add";
            else if (declaration.Title == EventDeclaration.RemoveAccessorTitle)
                keyword = "Remove";

            Formatter.WriteKeyword(keyword);

            // TODO: add parameters

            if (declaration.HasBody)
            {
                declaration.Body.AcceptVisitor(this);
                WriteEndBlockKeywords(keyword);
            }


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

            Formatter.WriteKeyword("Sub");
            Formatter.WriteSpace();
            Formatter.WriteKeyword("New");

            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");

            if (declaration.HasBody)
            {
                declaration.Body.AcceptVisitor(this);
                WriteEndBlockKeywords("Sub");
            }

            Formatter.EndNode();
        }

        public void VisitCustomAttribute(CustomAttribute attribute)
        {
            Formatter.StartNode(attribute);

            switch (attribute.Variant)
            {
                case CustomAttributeVariant.Assembly:
                    Formatter.WriteKeyword("Assembly");
                    break;
                case CustomAttributeVariant.Module:
                    Formatter.WriteKeyword("Module");
                    break;
            }

            if (attribute.Variant != CustomAttributeVariant.Normal)
            {
                Formatter.WriteToken(":");
                Formatter.WriteSpace();
            }

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

            Formatter.WriteToken("<");
            WriteCommaSeparatedNodes(section.Attributes);
            Formatter.WriteToken(">");

            Formatter.EndNode();
        }

        public void VisitDelegateDeclaration(DelegateDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            Formatter.WriteKeyword("Delegate");
            Formatter.WriteSpace();
            var keyword = DetermineMethodKeyword(declaration.DelegateType);
            Formatter.WriteKeyword(keyword);
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");

            if (keyword == "Function")
            {
                WriteAsKeyword();
                declaration.DelegateType.AcceptVisitor(this);
            }

            Formatter.EndNode();
        }

        public void VisitEnumMemberDeclaration(EnumMemberDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            declaration.Declarator.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitEventDeclaration(EventDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.ModifierElements.Count > 0)
            {
                WriteSpaceSeparatedNodes(declaration.ModifierElements);
                Formatter.WriteSpace();
            }

            Formatter.WriteKeyword("Event");
            Formatter.WriteSpace();

            WriteCommaSeparatedNodes(declaration.Declarators);

            if (declaration.EventType != null)
            {
                WriteAsKeyword();
                declaration.EventType.AcceptVisitor(this);
            }

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

            WriteCommaSeparatedNodes(declaration.Declarators);

            WriteAsKeyword();

            declaration.FieldType.AcceptVisitor(this);
            Formatter.WriteSpace();

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

            var keyword = DetermineMethodKeyword(declaration.ReturnType);
            Formatter.WriteKeyword(keyword);
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);
            Formatter.WriteToken("(");
            WriteCommaSeparatedNodes(declaration.Parameters);
            Formatter.WriteToken(")");

            if (keyword == "Function")
            {
                WriteAsKeyword();
                declaration.ReturnType.AcceptVisitor(this);
            }

            var vbMethod = declaration as Members.MethodDeclaration;
            if (vbMethod != null)
            {
                if (vbMethod.ImplementsClause != null)
                    vbMethod.ImplementsClause.AcceptVisitor(this);

                if (vbMethod.HandlesClause != null)
                    vbMethod.HandlesClause.AcceptVisitor(this);
            }

            if (declaration.HasBody)
            {
                declaration.Body.AcceptVisitor(this);
                WriteEndBlockKeywords(keyword);
            }

            Formatter.EndNode();
        }

        public void VisitModifierElement(ModifierElement modifier)
        {
            Formatter.StartNode(modifier);
            Formatter.WriteKeyword(Members.ModifierElement.ModifierToString(modifier.Modifier));
            Formatter.EndNode();
        }

        public void VisitNamespaceDeclaration(NamespaceDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            Formatter.WriteKeyword("Namespace");
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);

            Formatter.Indent();
            Formatter.WriteLine();

            WriteNodes(declaration.UsingDirectives, true);
            WriteNodes(declaration.Types, true);

            Formatter.WriteLine();
            Formatter.Unindent();
            WriteEndBlockKeywords("Namespace");

            Formatter.EndNode();
        }

        public void VisitParameterDeclaration(ParameterDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            Formatter.WriteKeyword(Members.ParameterDeclaration.ModifierToString(declaration.ParameterModifier));
            Formatter.WriteSpace();
            declaration.Declarator.AcceptVisitor(this);
            WriteAsKeyword();
            declaration.ParameterType.AcceptVisitor(this);

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

            Formatter.WriteKeyword("Property");
            Formatter.WriteSpace();
            declaration.Identifier.AcceptVisitor(this);
            WriteAsKeyword();
            declaration.PropertyType.AcceptVisitor(this);

            Formatter.Indent();
            Formatter.WriteLine();

            if (declaration.Getter != null)
                declaration.Getter.AcceptVisitor(this);
            if (declaration.Setter != null)
                declaration.Setter.AcceptVisitor(this);

            Formatter.WriteLine();
            Formatter.Unindent();
            WriteEndBlockKeywords("Property");

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

            var keyword = Members.TypeDeclaration.TypeVariantToString(declaration.TypeVariant);
            Formatter.WriteKeyword(keyword);
            Formatter.WriteSpace();

            declaration.Identifier.AcceptVisitor(this);
            WriteTypeParametersOrArguments(declaration.TypeParameters);

            Formatter.Indent();
            Formatter.WriteLine();

            if (declaration.BaseTypes.Count > 0)
            {
                Formatter.WriteKeyword("Inherits");
                Formatter.WriteSpace();

                var baseTypes = declaration.BaseTypes.ToList();
                baseTypes[0].AcceptVisitor(this);
                baseTypes.RemoveAt(0);

                if (baseTypes.Count > 0)
                {
                    Formatter.WriteLine();
                    Formatter.WriteKeyword("Implements");
                    Formatter.WriteSpace();
                    WriteCommaSeparatedNodes(baseTypes);
                }
                Formatter.WriteLine();
            }

            WriteNodes(declaration.Members, true);

            Formatter.WriteLine();
            Formatter.Unindent();
            WriteEndBlockKeywords(keyword);

            Formatter.EndNode();
        }

        public void VisitTypeParameterDeclaration(TypeParameterDeclaration declaration)
        {
            Formatter.StartNode(declaration);

            if (declaration.Variance != TypeParameterVariance.Invariant)
            {
                Formatter.WriteKeyword(Members.TypeParameterDeclaration.OperatorToString(declaration.Variance));
                Formatter.WriteSpace();
            }

            declaration.Identifier.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitUsingAliasDirective(UsingAliasDirective directive)
        {
            throw new NotImplementedException();
        }

        public void VisitUsingNamespaceDirective(UsingNamespaceDirective namespaceDirective)
        {
            Formatter.StartNode(namespaceDirective);

            Formatter.WriteKeyword("Imports");
            Formatter.WriteSpace();
            namespaceDirective.NamespaceIdentifier.AcceptVisitor(this);

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

            Formatter.WriteKeyword(Statements.AddRemoveHandlerStatement.VariantToString(statement.Variant));
            Formatter.WriteSpace();
            statement.EventExpression.AcceptVisitor(this);
            Formatter.WriteToken(",");
            Formatter.WriteSpace();
            statement.DelegateExpression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitBlockStatement(BlockStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.Indent();
            Formatter.WriteLine();

            WriteNodes(statement.Statements, true);

            Formatter.WriteLine();
            Formatter.Unindent();
            Formatter.EndNode();
        }

        public void VisitCatchClause(CatchClause catchClause)
        {
            Formatter.StartNode(catchClause);

            Formatter.WriteKeyword("Catch");
            if (catchClause.ExceptionType != null)
            {
                Formatter.WriteSpace();
                catchClause.ExceptionIdentifier.AcceptVisitor(this);
                WriteAsKeyword();
                catchClause.ExceptionType.AcceptVisitor(this);
            }

            catchClause.Body.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitDoLoopStatement(DoLoopStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Do");
            statement.Body.AcceptVisitor(this);

            Formatter.WriteKeyword("While");
            Formatter.WriteSpace();
            statement.Condition.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitEmptyStatement(EmptyStatement statement)
        {
            Formatter.StartNode(statement);
            Formatter.WriteLine();
            Formatter.EndNode();
        }

        public void VisitExpressionStatement(ExpressionStatement statement)
        {
            Formatter.StartNode(statement);

            statement.Expression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitGotoStatement(GotoStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Goto");
            Formatter.WriteSpace();
            statement.LabelIdentifier.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitIfElseStatement(IfElseStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("If");
            Formatter.WriteSpace();
            statement.Condition.AcceptVisitor(this);
            Formatter.WriteSpace();
            Formatter.WriteKeyword("Then");

            statement.TrueBlock.AcceptVisitor(this);

            if (statement.FalseBlock != null)
            {
                Formatter.WriteKeyword("Else");
                statement.FalseBlock.AcceptVisitor(this);
            }

            WriteEndBlockKeywords("If");

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

            Formatter.WriteKeyword("SyncLock");
            Formatter.WriteSpace();
            statement.LockObject.AcceptVisitor(this);

            statement.Body.AcceptVisitor(this);
            Formatter.Unindent();
            WriteEndBlockKeywords("SyncLock");


            Formatter.EndNode();
        }

        public void VisitReturnStatement(ReturnStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Return");
            Formatter.WriteSpace();

            if (statement.Value != null)
                statement.Value.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitSwitchCaseLabel(SwitchCaseLabel switchCaseLabel)
        {
            Formatter.StartNode(switchCaseLabel);

            Formatter.WriteKeyword("Case");
            Formatter.WriteSpace();

            if (switchCaseLabel.IsDefaultCase)
                Formatter.WriteKeyword("Else");
            else
                switchCaseLabel.Condition.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitSwitchSection(SwitchSection switchSection)
        {
            Formatter.StartNode(switchSection);

            WriteNodes(switchSection.Labels, true);
            Formatter.Indent();
            WriteNodes(switchSection.Statements);
            Formatter.Unindent();

            Formatter.EndNode();
        }

        public void VisitSwitchStatement(SwitchStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Select Case");
            Formatter.WriteSpace();
            statement.Condition.AcceptVisitor(this);

            Formatter.Indent();
            Formatter.WriteLine();

            WriteNodes(statement.Sections);

            Formatter.Unindent();
            WriteEndBlockKeywords("Select");

            Formatter.EndNode();
        }

        public void VisitThrowStatement(ThrowStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Throw");
            Formatter.WriteSpace();
            statement.Expression.AcceptVisitor(this);

            Formatter.EndNode();
        }

        public void VisitTryCatchStatement(TryCatchStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Try");

            statement.TryBlock.AcceptVisitor(this);
            WriteNodes(statement.CatchClauses);
            if (statement.FinallyBlock != null)
            {
                Formatter.WriteKeyword("Finally");
                statement.FinallyBlock.AcceptVisitor(this);
            }

            WriteEndBlockKeywords("Try");

            Formatter.EndNode();
        }

        public void VisitUsingStatement(UsingStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Using");
            Formatter.WriteSpace();
            statement.DisposableObject.AcceptVisitor(this);
            statement.Body.AcceptVisitor(this);
            WriteEndBlockKeywords("Using");

            Formatter.EndNode();
        }

        public void VisitVariableDeclarationStatement(VariableDeclarationStatement statement)
        {
            Formatter.StartNode(statement);

            if (statement.Parent is BlockStatement)
            {
                Formatter.WriteKeyword("Dim");
                Formatter.WriteSpace();
            }

            WriteNodes(statement.Declarators);
            WriteAsKeyword();
            statement.VariableType.AcceptVisitor(this);

            //if (statement.RequiresEndStatement)
            //    Formatter.WriteLine();

            Formatter.EndNode();
        }

        public void VisitWhileLoopStatement(WhileLoopStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("While");
            Formatter.WriteSpace();
            statement.Condition.AcceptVisitor(this);

            statement.Body.AcceptVisitor(this);

            WriteEndBlockKeywords("While");

            Formatter.EndNode();
        }

        public void VisitYieldStatement(YieldStatement statement)
        {
            Formatter.StartNode(statement);

            Formatter.WriteKeyword("Yield");
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

            Formatter.WriteToken("(");

            for (int i = 0; i < specifier.Dimensions - 1; i++)
                Formatter.WriteToken(",");

            Formatter.WriteToken(")");

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
            // not really supported but to prevent errors we still accept the node.

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

            Formatter.WriteToken(token.Value);

            Formatter.EndNode();
        }

        public void VisitPrimitiveAstToken(AstToken element)
        {
            Formatter.StartNode(element);
            Formatter.WriteToken(element.Value);
            Formatter.EndNode();
        }

        public void VisitComment(Comment comment)
        {
            //
        }

        public void VisitDefault(object item)
        {
        }
    }
}