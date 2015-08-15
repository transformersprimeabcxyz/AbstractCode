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
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Types;
using AbstractCode.Ast.VisualBasic.Members;

namespace AbstractCode.Ast.VisualBasic
{
    public class VisualBasicLanguage : SourceLanguage
    {
        private static readonly Dictionary<UnaryOperator, string> PrefixUnaryOperatorMapping = new Dictionary
            <UnaryOperator, string>
        {
            [UnaryOperator.AddressOf] = "AddressOf",
            [UnaryOperator.Await] = "Await",
            [UnaryOperator.Not] = "Not",
            [UnaryOperator.BitwiseNot] = "Not",
            //[UnaryOperator.Dereference] = "*",
            [UnaryOperator.Negative] = "-",
            //[UnaryOperator.PreDecrement] = "--",
            //[UnaryOperator.PreIncrement] = "++",
        };

        public static string OperatorToString(UnaryOperator @operator)
        {
            string operatorString;
            if (!PrefixUnaryOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the Visual Basic language.");
            return operatorString;
        }

        public static UnaryOperator UnaryOperatorFromString(string operatorString, bool isPrefix = true)
        {
            foreach (var pair in PrefixUnaryOperatorMapping)
            {
                if (string.Equals(pair.Value, operatorString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid Visual Basic unary operator.");
        }

        private static readonly Dictionary<BinaryOperator, string> BinaryOperatorMapping = new Dictionary
            <BinaryOperator, string>
        {
            [BinaryOperator.Add] = "+",
            [BinaryOperator.Subtract] = "-",
            [BinaryOperator.Multiply] = "*",
            [BinaryOperator.Divide] = "/",
            [BinaryOperator.IntegerDivide] = "\\",
            [BinaryOperator.Modulus] = "Mod",
            [BinaryOperator.Equals] = "=",
            [BinaryOperator.NotEquals] = "<>",
            [BinaryOperator.GreaterThan] = ">",
            [BinaryOperator.GreaterThanOrEquals] = ">=",
            [BinaryOperator.LessThan] = "<",
            [BinaryOperator.LessThanOrEquals] = "<=",
            [BinaryOperator.LogicalAnd] = "AndAlso",
            [BinaryOperator.LogicalOr] = "OrElse",
            //[BinaryOperator.NullCoalescing] = "??", // TODO: If(a, b)
            [BinaryOperator.BitwiseAnd] = "And",
            [BinaryOperator.BitwiseOr] = "Or",
            [BinaryOperator.ShiftLeft] = "<<",
            [BinaryOperator.ShiftRight] = ">>",
            [BinaryOperator.Concat] = "&",
            [BinaryOperator.RaisePower] = "^",
        };

        public static string OperatorToString(BinaryOperator @operator)
        {
            string operatorString;
            if (!BinaryOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the Visual Basic language.");
            return operatorString;
        }

        public static BinaryOperator BinaryOperatorFromString(string operatorString)
        {
            foreach (var pair in BinaryOperatorMapping)
            {
                if (string.Equals(pair.Value, operatorString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid Visual Basic binary operator.");
        }

        private static readonly Dictionary<AssignmentOperator, string> AssignmentOperatorMapping = new Dictionary
            <AssignmentOperator, string>
        {
            [AssignmentOperator.Assign] = "=",
            [AssignmentOperator.Add] = "+=",
            [AssignmentOperator.Subtract] = "-=",
            [AssignmentOperator.Multiply] = "*=",
            [AssignmentOperator.Divide] = "/=",
            [AssignmentOperator.IntegerDivide] = "\\=",
            [AssignmentOperator.ShiftLeft] = "<<=",
            [AssignmentOperator.ShiftRight] = ">>=",
            [AssignmentOperator.Concat] = "&=",
            [AssignmentOperator.RaisePower] = "^",
        };

        public static string OperatorToString(AssignmentOperator @operator)
        {
            string operatorString;
            if (!AssignmentOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the Visual Basic language.");
            return operatorString;
        }

        public static AssignmentOperator AssignmentOperatorFromString(string operatorString)
        {
            foreach (var pair in AssignmentOperatorMapping)
            {
                if (string.Equals(pair.Value, operatorString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid Visual Basic assignment operator.");
        }

        private static readonly Dictionary<MemberAccessor, string> AccessorMapping = new Dictionary
            <MemberAccessor, string>
        {
            [MemberAccessor.Normal] = ".",
            [MemberAccessor.Static] = ".",
            //[MemberAccessor.Pointer] = "->",
        };

        public static string OperatorToString(MemberAccessor accessor)
        {
            string operatorString;
            if (!AccessorMapping.TryGetValue(accessor, out operatorString))
                throw new ArgumentException("Accessor does not exist in the Visual Basic language.");
            return operatorString;
        }

        public static MemberAccessor AccessorFromString(string accessorString)
        {
            foreach (var pair in AccessorMapping)
            {
                if (string.Equals(pair.Value, accessorString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid Visual Basic accessor operator.");
        }

        private static readonly Dictionary<PrimitiveType, string> PrimitiveTypeMapping = new Dictionary
            <PrimitiveType, string>
        {
            [PrimitiveType.Void] = "Void",
            [PrimitiveType.Object] = "Object",
            [PrimitiveType.Boolean] = "Boolean",
            [PrimitiveType.Byte] = "Byte",
            [PrimitiveType.UInt16] = "UShort",
            [PrimitiveType.UInt32] = "UInteger",
            [PrimitiveType.UInt64] = "ULong",
            [PrimitiveType.SByte] = "SByte",
            [PrimitiveType.Int16] = "Short",
            [PrimitiveType.Int32] = "Integer",
            [PrimitiveType.Int64] = "Long",
            [PrimitiveType.Single] = "Single",
            [PrimitiveType.Double] = "Double",
            [PrimitiveType.Decimal] = "Decimal",
            [PrimitiveType.Char] = "Char",
            [PrimitiveType.String] = "String",
        };

        public static string PrimitiveTypeToString(PrimitiveType primitiveType)
        {
            string identifier;
            if (!PrimitiveTypeMapping.TryGetValue(primitiveType, out identifier))
                throw new ArgumentException("Primitive type does not exist in the Visual Basic language.");
            return identifier;
        }

        public static PrimitiveType PrimitiveTypeFromString(string identifier)
        {
            foreach (var pair in PrimitiveTypeMapping)
            {
                if (string.Equals(pair.Value, identifier, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Identifier is not recognized as a valid Visual Basic primitive type.");
        }

        private static readonly Dictionary<Modifier, string> ModifierMapping = new Dictionary<Modifier, string>
        {
            [Modifier.Private] = "Private",
            [Modifier.Internal] = "Friend",
            [Modifier.Protected] = "Protected",
            [Modifier.Public] = "Public",
            [Modifier.Static] = "Shared",
            [Modifier.Sealed] = "NotInheritable",
            [Modifier.Virtual] = "Overridable",
            [Modifier.Abstract] = "MustInherit", // TODO: MustOverride
            [Modifier.Override] = "Overrides",
            [Modifier.Shadow] = "Shadow",
            [Modifier.Partial] = "Partial",
            [Modifier.Const] = "Const",
            [Modifier.ReadOnly] = "ReadOnly",
            [Modifier.Async] = "Async",
            [(Modifier)VisualBasicModifier.Iterator] = "Iterator",
            [(Modifier)VisualBasicModifier.WithEvents] = "WithEvents",
            [(Modifier)VisualBasicModifier.WriteOnly] = "WriteOnly",
        };

        public static Modifier ModifierFromString(string modifierString)
        {
            foreach (var pair in ModifierMapping)
            {
                if (string.Equals(pair.Value, modifierString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Modifier is not recognized as a valid Visual Basic modifier.");
        }

        public static string ModifierToString(Modifier modifier)
        {
            string modifierString;
            if (!ModifierMapping.TryGetValue(modifier, out modifierString))
                throw new ArgumentException("Modifier does not exist in the Visual Basic language.");
            return modifierString;
        }

        private static readonly Dictionary<TypeVariant, string> TypeVariantMapping = new Dictionary<TypeVariant, string>
        {
            [TypeVariant.Class] = "Class",
            [TypeVariant.ValueType] = "Structure",
            [TypeVariant.Enum] = "Enum",
            [TypeVariant.Interface] = "Interface",
        };

        public static TypeVariant TypeVariantFromString(string typeVariantString)
        {
            foreach (var pair in TypeVariantMapping)
            {
                if (string.Equals(pair.Value, typeVariantString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException("Type variant is not recognized as a valid Visual Basic type variant.");
        }

        public static string TypeVariantToString(TypeVariant typeVariant)
        {
            string modifierString;
            if (!TypeVariantMapping.TryGetValue(typeVariant, out modifierString))
                throw new ArgumentException("Type variant does not exist in the Visual Basic language.");
            return modifierString;
        }

        private static readonly Dictionary<LinqOrderingDirection, string> LinqDirectionMapping = new Dictionary
            <LinqOrderingDirection, string>
        {
            [LinqOrderingDirection.Ascending] = "Ascending",
            [LinqOrderingDirection.Descending] = "Descending",
        };

        public static LinqOrderingDirection OrderningDirectionFromString(string operatorString)
        {
            foreach (var pair in LinqDirectionMapping)
            {
                if (string.Equals(pair.Value, operatorString, StringComparison.OrdinalIgnoreCase))
                    return pair.Key;
            }
            throw new ArgumentException(
                "Operator is not recognized as a valid Visual Basic LINQ ordering direction operator.");
        }

        public static string OperatorToString(LinqOrderingDirection @operator)
        {
            string operatorString;
            if (!LinqDirectionMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the Visual Basic language.");
            return operatorString;
        }

        public static VisualBasicLanguage Instance
        {
            get;
            private set;
        }

        static VisualBasicLanguage()
        {
            Instance = new VisualBasicLanguage();
        }

        private readonly LanguageData _data;
        private readonly VisualBasicStringFormatter _stringFormatter;
        private readonly VisualBasicNumberFormatter _numberFormatter;

        private VisualBasicLanguage()
        {
            _data = LanguageData.FromXml(Properties.Resources.VisualBasic);
            _stringFormatter = new VisualBasicStringFormatter();
            _numberFormatter = new VisualBasicNumberFormatter();
        }

        public override string Name
        {
            get { return "Visual Basic"; }
        }

        public override string[] Keywords
        {
            get { return _data.Keywords; }
        }

        public override string[] Modifiers
        {
            get { return _data.Modifiers; }
        }

        public override string[] MemberIdentifiers
        {
            get { return _data.MemberIdentifiers; }
        }

        public override bool IsCaseSensitive
        {
            get { return false; }
        }

        public override StringFormatter StringFormatter
        {
            get { return _stringFormatter; }
        }

        public override NumberFormatter NumberFormatter
        {
            get { return _numberFormatter; }
        }

        public override CompilationUnit Parse(IDocument document)
        {
            // TODO: create a VB.NET grammar and lexer.
            throw new NotImplementedException();
        }

        public override void UpdateSyntaxTree(CompilationUnit compilationUnit, IDocument input, TextRange range)
        {
            throw new NotImplementedException();
        }

        public override IAstVisitor CreateWriter(IOutputFormatter formatter)
        {
            return new VisualBasicAstWriter(formatter, new VisualBasicAstWriterParameters()
            {
                BraceStyle = BraceStyle.NextLine
            });
        }
    }
}