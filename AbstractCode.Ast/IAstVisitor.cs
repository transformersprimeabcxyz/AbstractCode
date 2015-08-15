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

using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Statements;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast
{
    public interface IAstVisitor
    {
        void VisitCompilationUnit(CompilationUnit compilationUnit);
        void VisitArrayInitializer(ArrayInitializer initializer);
        void VisitAssignmentExpression(AssignmentExpression expression);
        void VisitBaseReferenceExpression(BaseReferenceExpression expression);
        void VisitBinaryOperatorExpression(BinaryOperatorExpression expression);
        void VisitConditionalExpression(ConditionalExpression expression);
        void VisitCreateArrayExpresion(CreateArrayExpression expression);
        void VisitCreateObjectExpression(CreateObjectExpression expression);
        void VisitDirectionExpression(DirectionExpression expression);
        void VisitExplicitCastExpression(ExplicitCastExpression expression);
        void VisitGetTypeExpression(GetTypeExpression expression);
        void VisitIdentifierExpression(IdentifierExpression expression);
        void VisitIndexerExpression(IndexerExpression expression);
        void VisitInvocationExpression(InvocationExpression expression);
        void VisitLinqExpression(LinqExpression expression);
        void VisitLinqFromClause(LinqFromClause clause);
        void VisitLinqLetClause(LinqLetClause clause);
        void VisitLinqGroupByClause(LinqGroupByClause clause);
        void VisitLinqOrderByClause(LinqOrderByClause clause);
        void VisitLinqOrdering(LinqOrdering ordering);
        void VisitLinqWhereClause(LinqWhereClause clause);
        void VisitLinqSelectClause(LinqSelectClause clause);
        void VisitMemberReferenceExpression(MemberReferenceExpression expression);
        void VisitParenthesizedExpression(ParenthesizedExpression expression);
        void VisitPrimitiveExpression(PrimitiveExpression expression);
        void VisitSafeCastExpression(SafeCastExpression expression);
        void VisitThisReferenceExpression(ThisReferenceExpression expression);
        void VisitTypeCheckExpression(TypeCheckExpression expression);
        void VisitUnaryOperatorExpression(UnaryOperatorExpression expression);

        void VisitAccessorDeclaration(AccessorDeclaration declaration);
        void VisitConstructorDeclaration(ConstructorDeclaration declaration);
        void VisitCustomAttribute(CustomAttribute attribute);
        void VisitCustomAttributeSection(CustomAttributeSection section);
        void VisitDelegateDeclaration(DelegateDeclaration declaration);
        void VisitEnumMemberDeclaration(EnumMemberDeclaration declaration);
        void VisitEventDeclaration(EventDeclaration declaration);
        void VisitFieldDeclaration(FieldDeclaration declaration);
        void VisitIdentifier(Identifier identifier);
        void VisitMethodDeclaration(MethodDeclaration declaration);
        void VisitModifierElement(ModifierElement modifier);
        void VisitNamespaceDeclaration(NamespaceDeclaration declaration);
        void VisitParameterDeclaration(ParameterDeclaration declaration);
        void VisitPropertyDeclaration(PropertyDeclaration declaration);
        void VisitTypeDeclaration(TypeDeclaration declaration);
        void VisitTypeParameterDeclaration(TypeParameterDeclaration declaration);
        void VisitUsingAliasDirective(UsingAliasDirective directive);
        void VisitUsingNamespaceDirective(UsingNamespaceDirective namespaceDirective);
        void VisitVariableDeclarator(VariableDeclarator declarator);

        void VisitAddRemoveHandlerStatement(AddRemoveHandlerStatement statement);
        void VisitBlockStatement(BlockStatement statement);
        void VisitCatchClause(CatchClause catchClause);
        void VisitEmptyStatement(EmptyStatement statement);
        void VisitExpressionStatement(ExpressionStatement statement);
        void VisitDoLoopStatement(DoLoopStatement statement);
        void VisitGotoStatement(GotoStatement statement);
        void VisitIfElseStatement(IfElseStatement statement);
        void VisitLabelStatement(LabelStatement statement);
        void VisitLockStatement(LockStatement statement);
        void VisitReturnStatement(ReturnStatement statement);
        void VisitSwitchCaseLabel(SwitchCaseLabel switchCaseLabel);
        void VisitSwitchSection(SwitchSection switchSection);
        void VisitSwitchStatement(SwitchStatement statement);
        void VisitThrowStatement(ThrowStatement statement);
        void VisitTryCatchStatement(TryCatchStatement statement);
        void VisitUsingStatement(UsingStatement statement);
        void VisitVariableDeclarationStatement(VariableDeclarationStatement statement);
        void VisitWhileLoopStatement(WhileLoopStatement statement);
        void VisitYieldStatement(YieldStatement statement);

        void VisitArrayTypeReference(ArrayTypeReference typeReference);
        void VisitArrayTypeRankSpecifier(ArrayTypeRankSpecifier specifier);
        void VisitMemberTypeReference(MemberTypeReference typeReference);
        void VisitPointerTypeReference(PointerTypeReference typeReference);
        void VisitSimpleTypeReference(SimpleTypeReference typeReference);
        void VisitPrimitiveTypeReference(PrimitiveTypeReference typeReference);
        void VisitAstToken(AstToken token);

        void VisitComment(Comment comment);
    }

    public interface IAstVisitor<out TResult>
    {
        TResult VisitCompilationUnit(CompilationUnit compilationUnit);
        TResult VisitArrayInitializer(ArrayInitializer initializer);
        TResult VisitAssignmentExpression(AssignmentExpression expression);
        TResult VisitBaseReferenceExpression(BaseReferenceExpression expression);
        TResult VisitBinaryOperatorExpression(BinaryOperatorExpression expression);
        TResult VisitConditionalExpression(ConditionalExpression expression);
        TResult VisitCreateArrayExpresion(CreateArrayExpression expression);
        TResult VisitCreateObjectExpression(CreateObjectExpression expression);
        TResult VisitDirectionExpression(DirectionExpression expression);
        TResult VisitExplicitCastExpression(ExplicitCastExpression expression);
        TResult VisitGetTypeExpression(GetTypeExpression expression);
        TResult VisitIdentifierExpression(IdentifierExpression expression);
        TResult VisitIndexerExpression(IndexerExpression expression);
        TResult VisitInvocationExpression(InvocationExpression expression);
        TResult VisitLinqExpression(LinqExpression expression);
        TResult VisitLinqFromClause(LinqFromClause clause);
        TResult VisitLinqLetClause(LinqLetClause clause);
        TResult VisitLinqGroupByClause(LinqGroupByClause clause);
        TResult VisitLinqOrderByClause(LinqOrderByClause clause);
        TResult VisitLinqOrdering(LinqOrdering ordering);
        TResult VisitLinqWhereClause(LinqWhereClause clause);
        TResult VisitLinqSelectClause(LinqSelectClause clause);
        TResult VisitMemberReferenceExpression(MemberReferenceExpression expression);
        TResult VisitParenthesizedExpression(ParenthesizedExpression expression);
        TResult VisitPrimitiveExpression(PrimitiveExpression expression);
        TResult VisitSafeCastExpression(SafeCastExpression expression);
        TResult VisitThisReferenceExpression(ThisReferenceExpression expression);
        TResult VisitTypeCheckExpression(TypeCheckExpression expression);
        TResult VisitUnaryOperatorExpression(UnaryOperatorExpression expression);

        TResult VisitAccessorDeclaration(AccessorDeclaration declaration);
        TResult VisitConstructorDeclaration(ConstructorDeclaration declaration);
        TResult VisitCustomAttribute(CustomAttribute attribute);
        TResult VisitCustomAttributeSection(CustomAttributeSection section);
        TResult VisitDelegateDeclaration(DelegateDeclaration declaration);
        TResult VisitEnumMemberDeclaration(EnumMemberDeclaration declaration);
        TResult VisitEventDeclaration(EventDeclaration declaration);
        TResult VisitFieldDeclaration(FieldDeclaration declaration);
        TResult VisitIdentifier(Identifier identifier);
        TResult VisitMethodDeclaration(MethodDeclaration declaration);
        TResult VisitModifierElement(ModifierElement modifier);
        TResult VisitNamespaceDeclaration(NamespaceDeclaration declaration);
        TResult VisitParameterDeclaration(ParameterDeclaration declaration);
        TResult VisitPropertyDeclaration(PropertyDeclaration declaration);
        TResult VisitTypeDeclaration(TypeDeclaration declaration);
        TResult VisitTypeParameterDeclaration(TypeParameterDeclaration declaration);
        TResult VisitUsingAliasDirective(UsingAliasDirective directive);
        TResult VisitUsingNamespaceDirective(UsingNamespaceDirective namespaceDirective);
        TResult VisitVariableDeclarator(VariableDeclarator declarator);

        TResult VisitAddRemoveHandlerStatement(AddRemoveHandlerStatement statement);
        TResult VisitBlockStatement(BlockStatement statement);
        TResult VisitCatchClause(CatchClause catchClause);
        TResult VisitEmptyStatement(EmptyStatement statement);
        TResult VisitExpressionStatement(ExpressionStatement statement);
        TResult VisitDoWhileLoopStatement(DoLoopStatement statement);
        TResult VisitGotoStatement(GotoStatement statement);
        TResult VisitIfElseStatement(IfElseStatement statement);
        TResult VisitLabelStatement(LabelStatement statement);
        TResult VisitLockStatement(LockStatement statement);
        TResult VisitReturnStatement(ReturnStatement statement);
        TResult VisitSwitchCaseLabel(SwitchCaseLabel switchCaseLabel);
        TResult VisitSwitchSection(SwitchSection switchSection);
        TResult VisitSwitchStatement(SwitchStatement statement);
        TResult VisitThrowStatement(ThrowStatement statement);
        TResult VisitTryCatchStatement(TryCatchStatement statement);
        TResult VisitUsingStatement(UsingStatement statement);
        TResult VisitVariableDeclarationStatement(VariableDeclarationStatement statement);
        TResult VisitWhileLoopStatement(WhileLoopStatement statement);
        TResult VisitYieldStatement(YieldStatement statement);

        TResult VisitArrayTypeReference(ArrayTypeReference typeReference);
        TResult VisitArrayTypeRankSpecifier(ArrayTypeRankSpecifier specifier);
        TResult VisitMemberTypeReference(MemberTypeReference typeReference);
        TResult VisitPointerTypeReference(PointerTypeReference typeReference);
        TResult VisitSimpleTypeReference(SimpleTypeReference typeReference);
        TResult VisitPrimitiveTypeReference(PrimitiveTypeReference typeReference);
        TResult VisitAstToken(AstToken token);

        TResult VisitComment(Comment comment);
    }

    public interface IAstVisitor<in TData, out TResult>
    {
        TResult VisitCompilationUnit(CompilationUnit compilationUnit, TData data);
        TResult VisitArrayInitializer(ArrayInitializer initializer, TData data);
        TResult VisitAssignmentExpression(AssignmentExpression expression, TData data);
        TResult VisitBaseReferenceExpression(BaseReferenceExpression expression, TData data);
        TResult VisitBinaryOperatorExpression(BinaryOperatorExpression expression, TData data);
        TResult VisitConditionalExpression(ConditionalExpression expression, TData data);
        TResult VisitCreateArrayExpresion(CreateArrayExpression expression, TData data);
        TResult VisitCreateObjectExpression(CreateObjectExpression expression, TData data);
        TResult VisitDirectionExpression(DirectionExpression expression, TData data);
        TResult VisitExplicitCastExpression(ExplicitCastExpression expression, TData data);
        TResult VisitGetTypeExpression(GetTypeExpression expression, TData data);
        TResult VisitIdentifierExpression(IdentifierExpression expression, TData data);
        TResult VisitIndexerExpression(IndexerExpression expression, TData data);
        TResult VisitInvocationExpression(InvocationExpression expression, TData data);
        TResult VisitLinqExpression(LinqExpression expression, TData data);
        TResult VisitLinqFromClause(LinqFromClause clause, TData data);
        TResult VisitLinqLetClause(LinqLetClause clause, TData data);
        TResult VisitLinqGroupByClause(LinqGroupByClause clause, TData data);
        TResult VisitLinqOrderByClause(LinqOrderByClause clause, TData data);
        TResult VisitLinqOrdering(LinqOrdering ordering, TData data);
        TResult VisitLinqWhereClause(LinqWhereClause clause, TData data);
        TResult VisitLinqSelectClause(LinqSelectClause clause, TData data);
        TResult VisitMemberReferenceExpression(MemberReferenceExpression expression, TData data);
        TResult VisitParenthesizedExpression(ParenthesizedExpression expression, TData data);
        TResult VisitPrimitiveExpression(PrimitiveExpression expression, TData data);
        TResult VisitSafeCastExpression(SafeCastExpression expression, TData data);
        TResult VisitThisReferenceExpression(ThisReferenceExpression expression, TData data);
        TResult VisitTypeCheckExpression(TypeCheckExpression expression, TData data);
        TResult VisitUnaryOperatorExpression(UnaryOperatorExpression expression, TData data);

        TResult VisitAccessorDeclaration(AccessorDeclaration declaration, TData data);
        TResult VisitConstructorDeclaration(ConstructorDeclaration declaration, TData data);
        TResult VisitCustomAttribute(CustomAttribute attribute, TData data);
        TResult VisitCustomAttributeSection(CustomAttributeSection section, TData data);
        TResult VisitDelegateDeclaration(DelegateDeclaration declaration, TData data);
        TResult VisitEnumMemberDeclaration(EnumMemberDeclaration declaration, TData data);
        TResult VisitEventDeclaration(EventDeclaration declaration, TData data);
        TResult VisitFieldDeclaration(FieldDeclaration declaration, TData data);
        TResult VisitIdentifier(Identifier identifier, TData data);
        TResult VisitMethodDeclaration(MethodDeclaration declaration, TData data);
        TResult VisitModifierElement(ModifierElement modifier, TData data);
        TResult VisitNamespaceDeclaration(NamespaceDeclaration declaration, TData data);
        TResult VisitParameterDeclaration(ParameterDeclaration declaration, TData data);
        TResult VisitPropertyDeclaration(PropertyDeclaration declaration, TData data);
        TResult VisitTypeDeclaration(TypeDeclaration declaration, TData data);
        TResult VisitTypeParameterDeclaration(TypeParameterDeclaration declaration, TData data);
        TResult VisitUsingAliasDirective(UsingAliasDirective directive, TData data);
        TResult VisitUsingNamespaceDirective(UsingNamespaceDirective namespaceDirective, TData data);
        TResult VisitVariableDeclarator(VariableDeclarator declarator, TData data);

        TResult VisitAddRemoveHandlerStatement(AddRemoveHandlerStatement statement, TData data);
        TResult VisitBlockStatement(BlockStatement statement, TData data);
        TResult VisitCatchClause(CatchClause catchClause, TData data);
        TResult VisitEmptyStatement(EmptyStatement statement, TData data);
        TResult VisitExpressionStatement(ExpressionStatement statement, TData data);
        TResult VisitDoWhileLoopStatement(DoLoopStatement statement, TData data);
        TResult VisitGotoStatement(GotoStatement statement, TData data);
        TResult VisitIfElseStatement(IfElseStatement statement, TData data);
        TResult VisitLabelStatement(LabelStatement statement, TData data);
        TResult VisitLockStatement(LockStatement statement, TData data);
        TResult VisitReturnStatement(ReturnStatement statement, TData data);
        TResult VisitSwitchCaseLabel(SwitchCaseLabel switchCaseLabel, TData data);
        TResult VisitSwitchSection(SwitchSection switchSection, TData data);
        TResult VisitSwitchStatement(SwitchStatement statement, TData data);
        TResult VisitThrowStatement(ThrowStatement statement, TData data);
        TResult VisitTryCatchStatement(TryCatchStatement statement, TData data);
        TResult VisitUsingStatement(UsingStatement statement, TData data);
        TResult VisitVariableDeclarationStatement(VariableDeclarationStatement statement, TData data);
        TResult VisitWhileLoopStatement(WhileLoopStatement statement, TData data);
        TResult VisitYieldStatement(YieldStatement statement, TData data);

        TResult VisitArrayTypeReference(ArrayTypeReference typeReference, TData data);
        TResult VisitArrayTypeRankSpecifier(ArrayTypeRankSpecifier specifier, TData data);
        TResult VisitMemberTypeReference(MemberTypeReference typeReference, TData data);
        TResult VisitPointerTypeReference(PointerTypeReference typeReference, TData data);
        TResult VisitSimpleTypeReference(SimpleTypeReference typeReference, TData data);
        TResult VisitPrimitiveTypeReference(PrimitiveTypeReference typeReference, TData data);
        TResult VisitAstToken(AstToken token, TData data);

        TResult VisitComment(Comment comment, TData data);

    }
}
