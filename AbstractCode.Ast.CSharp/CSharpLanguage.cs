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

// Using the precompiled parser improves performance significantly. Don't use this if you
// intend to debug the construction of the parser automaton.
#define USE_PRECOMPILED_CS_PARSER

using System;
using System.Collections.Generic;
using System.IO;
using AbstractCode.Ast.CSharp.Members;
using AbstractCode.Ast.Expressions;
using AbstractCode.Ast.Members;
using AbstractCode.Ast.Parser;
using AbstractCode.Ast.Types;

namespace AbstractCode.Ast.CSharp
{
    public sealed class CSharpLanguage : SourceLanguage
    {
        private static readonly Dictionary<UnaryOperator, string> PrefixUnaryOperatorMapping = new Dictionary<UnaryOperator, string>
        {
            [UnaryOperator.AddressOf] = "&",
            [UnaryOperator.Await] = "await",
            [UnaryOperator.BitwiseNot] = "~",
            [UnaryOperator.Dereference] = "*",
            [UnaryOperator.Negative] = "-",
            [UnaryOperator.Not] = "!",
            [UnaryOperator.PreDecrement] = "--",
            [UnaryOperator.PreIncrement] = "++",
        };

        private static readonly Dictionary<UnaryOperator, string> PostUnaryOperatorMapping = new Dictionary<UnaryOperator, string>
        {
            [UnaryOperator.PostDecrement] = "--",
            [UnaryOperator.PostIncrement] = "++",
        };

        public static string OperatorToString(UnaryOperator @operator)
        {
            string operatorString;
            if (!PrefixUnaryOperatorMapping.TryGetValue(@operator, out operatorString) &&
                !PostUnaryOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the C# language.");
            return operatorString;
        }

        public static UnaryOperator UnaryOperatorFromString(string operatorString, bool isPrefix = true)
        {
            foreach (var pair in isPrefix ? PrefixUnaryOperatorMapping : PostUnaryOperatorMapping)
            {
                if (pair.Value == operatorString)
                    return pair.Key;
            }
            throw new ArgumentException(string.Format("Operator is not recognized as a valid C# {0} unary operator.",
                isPrefix ? "prefix" : "postfix"));
        }
        
        private static readonly Dictionary<BinaryOperator, string> BinaryOperatorMapping = new Dictionary<BinaryOperator, string>
        {
            [BinaryOperator.Add] = "+",
            [BinaryOperator.Subtract] = "-",
            [BinaryOperator.Multiply] = "*",
            [BinaryOperator.Divide] = "/",
            [BinaryOperator.Modulus] = "%",
            [BinaryOperator.Equals] = "==",
            [BinaryOperator.NotEquals] = "!=",
            [BinaryOperator.GreaterThan] = ">",
            [BinaryOperator.GreaterThanOrEquals] = ">=",
            [BinaryOperator.LessThan] = "<",
            [BinaryOperator.LessThanOrEquals] = "<=",
            [BinaryOperator.LogicalAnd] = "&&",
            [BinaryOperator.LogicalOr] = "||",
            [BinaryOperator.NullCoalescing] = "??",
            [BinaryOperator.BitwiseAnd] = "&",
            [BinaryOperator.BitwiseOr] = "|",
            [BinaryOperator.BitwiseXor] = "^",
            [BinaryOperator.ShiftLeft] = "<<",
            [BinaryOperator.ShiftRight] = ">>",
        };

        public static string OperatorToString(BinaryOperator @operator)
        {
            string operatorString;
            if (!BinaryOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the C# language.");
            return operatorString;
        }

        public static BinaryOperator BinaryOperatorFromString(string operatorString)
        {
            foreach (var pair in BinaryOperatorMapping)
            {
                if (pair.Value == operatorString)
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid C# binary operator.");
        }

        private static readonly Dictionary<AssignmentOperator, string> AssignmentOperatorMapping = new Dictionary<AssignmentOperator, string>
        {
            [AssignmentOperator.Assign] = "=",
            [AssignmentOperator.Add] = "+=",
            [AssignmentOperator.Subtract] = "-=",
            [AssignmentOperator.Multiply] = "*=",
            [AssignmentOperator.Divide] = "/=",
            [AssignmentOperator.Modulus] = "%=",
            [AssignmentOperator.BitwiseAnd] = "&=",
            [AssignmentOperator.BitwiseOr] = "|=",
            [AssignmentOperator.BitwiseXor] = "^=",
            [AssignmentOperator.ShiftLeft] = "<<=",
            [AssignmentOperator.ShiftRight] = ">>=",
        };

        public static string OperatorToString(AssignmentOperator @operator)
        {
            string operatorString;
            if (!AssignmentOperatorMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the C# language.");
            return operatorString;
        }

        public static AssignmentOperator AssignmentOperatorFromString(string operatorString)
        {
            foreach (var pair in AssignmentOperatorMapping)
            {
                if (pair.Value == operatorString)
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid C# assignment operator.");
        }

        private static readonly Dictionary<MemberAccessor, string> AccessorMapping = new Dictionary<MemberAccessor, string>
        {
            [MemberAccessor.Normal] = ".",
            [MemberAccessor.Static] = ".",
            [MemberAccessor.Pointer] = "->",
            [MemberAccessor.NullPropagation] = "?.",
        };

        public static string OperatorToString(MemberAccessor accessor)
        {
            string operatorString;
            if (!AccessorMapping.TryGetValue(accessor, out operatorString))
                throw new ArgumentException("Accessor does not exist in the C# language.");
            return operatorString;
        }

        public static MemberAccessor AccessorFromString(string accessorString)
        {
            foreach (var pair in AccessorMapping)
            {
                if (pair.Value == accessorString)
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid C# accessor operator.");
        }
        
        private static readonly Dictionary<PrimitiveType, string> PrimitiveTypeMapping = new Dictionary<PrimitiveType, string>
        {
            [PrimitiveType.Void] = "void",
            [PrimitiveType.Object] = "object",
            [PrimitiveType.Boolean] = "bool",
            [PrimitiveType.Byte] = "byte",
            [PrimitiveType.UInt16] = "ushort",
            [PrimitiveType.UInt32] = "uint",
            [PrimitiveType.UInt64] = "ulong",
            [PrimitiveType.SByte] = "sbyte",
            [PrimitiveType.Int16] = "short",
            [PrimitiveType.Int32] = "int",
            [PrimitiveType.Int64] = "long",
            [PrimitiveType.Single] = "float",
            [PrimitiveType.Double] = "double",
            [PrimitiveType.Decimal] = "decimal",
            [PrimitiveType.Char] = "char",
            [PrimitiveType.String] = "string",
        };

        public static string PrimitiveTypeToString(PrimitiveType primitiveType)
        {
            string identifier;
            if (!PrimitiveTypeMapping.TryGetValue(primitiveType, out identifier))
                throw new ArgumentException("Primitive type does not exist in the C# language.");
            return identifier;
        }

        public static PrimitiveType PrimitiveTypeFromString(string identifier)
        {
            foreach (var pair in PrimitiveTypeMapping)
            {
                if (pair.Value == identifier)
                    return pair.Key;
            }
            throw new ArgumentException("Identifier is not recognized as a valid C# primitive type.");
        }

        private static readonly Dictionary<Modifier, string> ModifierMapping = new Dictionary<Modifier, string>
        {
            [Modifier.Private] = "private",
            [Modifier.Internal] = "internal",
            [Modifier.Protected] = "protected",
            [Modifier.Public] = "public",
            [Modifier.Static] = "static",
            [Modifier.Sealed] = "sealed",
            [Modifier.Virtual] = "virtual",
            [Modifier.Abstract] = "abstract",
            [Modifier.Override] = "override",
            [Modifier.Shadow] = "new",
            [Modifier.Partial] = "partial",
            [Modifier.Const] = "const",
            [Modifier.ReadOnly] = "readonly",
            [Modifier.Extern] = "extern",
            [Modifier.Async] = "async",
            [(Modifier)CSharpModifier.Unsafe] = "unsafe"
        };

        public static Modifier ModifierFromString(string modifierString)
        {
            foreach (var pair in ModifierMapping)
            {
                if (pair.Value == modifierString)
                    return pair.Key;
            }
            throw new ArgumentException("Modifier is not recognized as a valid C# modifier.");
        }

        public static string ModifierToString(Modifier modifier)
        {
            string modifierString;
            if (!ModifierMapping.TryGetValue(modifier, out modifierString))
                throw new ArgumentException("Modifier does not exist in the C# language.");
            return modifierString;
        }

        private static readonly Dictionary<TypeVariant, string> TypeVariantMapping = new Dictionary<TypeVariant, string>
        {
            [TypeVariant.Class] = "class",
            [TypeVariant.ValueType] = "struct",
            [TypeVariant.Enum] = "enum",
            [TypeVariant.Interface] = "interface",
        };

        public static TypeVariant TypeVariantFromString(string typeVariantString)
        {
            foreach (var pair in TypeVariantMapping)
            {
                if (pair.Value == typeVariantString)
                    return pair.Key;
            }
            throw new ArgumentException("Type variant is not recognized as a valid C# type variant.");
        }

        public static string TypeVariantToString(TypeVariant typeVariant)
        {
            string modifierString;
            if (!TypeVariantMapping.TryGetValue(typeVariant, out modifierString))
                throw new ArgumentException("Type variant does not exist in the C# language.");
            return modifierString;
        }

        private static readonly Dictionary<FieldDirection, string> DirectionMapping = new Dictionary<FieldDirection, string>
        {
            [FieldDirection.Ref] = "ref",
            [FieldDirection.Out] = "out",
        };

        public static FieldDirection DirectionFromString(string operatorString)
        {
            foreach (var pair in DirectionMapping)
            {
                if (pair.Value == operatorString)
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid C# field direction operator.");
        }

        public static string OperatorToString(FieldDirection @operator)
        {
            string operatorString;
            if (!DirectionMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the C# language.");
            return operatorString;
        }

        private static readonly Dictionary<LinqOrderingDirection, string> LinqDirectionMapping = new Dictionary<LinqOrderingDirection, string>
        {
            [LinqOrderingDirection.Ascending] = "ascending",
            [LinqOrderingDirection.Descending] = "descending",
        };

        public static LinqOrderingDirection OrderningDirectionFromString(string operatorString)
        {
            foreach (var pair in LinqDirectionMapping)
            {
                if (pair.Value == operatorString)
                    return pair.Key;
            }
            throw new ArgumentException("Operator is not recognized as a valid C# LINQ ordering direction operator.");
        }

        public static string OperatorToString(LinqOrderingDirection @operator)
        {
            string operatorString;
            if (!LinqDirectionMapping.TryGetValue(@operator, out operatorString))
                throw new ArgumentException("Operator does not exist in the C# language.");
            return operatorString;
        }

        public static CSharpLanguage Instance
        {
            get;
            private set;
        }

        static CSharpLanguage()
        {
            Instance = new CSharpLanguage();
        }

        private readonly LanguageData _data;
        private readonly CSharpStringFormatter _stringFormatter;
        private readonly CSharpNumberFormatter _numberFormatter;
        private AutomatonSourceParser _parser;

        private CSharpLanguage()
        {
            _data = LanguageData.FromXml(Properties.Resources.CSharp);
            _stringFormatter = new CSharpStringFormatter();
            _numberFormatter = new CSharpNumberFormatter();
            Grammar = new CSharpGrammar();
        }

        public AutomatonSourceParser Parser
        {
            get
            {
                if (_parser != null)
                    return _parser;

#if USE_PRECOMPILED_CS_PARSER
                using (var stream = new MemoryStream(Properties.Resources.Automaton))
                {
                    var serializer = new ParserAutomatonSerializer(new GrammarData(Grammar));
                    _parser = new AutomatonSourceParser(serializer.Deserialize(stream));
                }
#else
                _parser = new AutomatonSourceParser(Grammar);
#endif
                return _parser;
            }
        }

        public override string Name
        {
            get { return "C#"; }
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
            get { return true; }
        }

        public override StringFormatter StringFormatter
        {
            get { return _stringFormatter; }
        }

        public override NumberFormatter NumberFormatter
        {
            get { return _numberFormatter; }
        }

        public CSharpGrammar Grammar
        {
            get;
        }

        public override CompilationUnit Parse(IDocument input)
        {
            return Parser.Parse(new CSharpLexer(input.CreateReader())).CreateAstNode<CompilationUnit>();
        }
        
        public override void UpdateSyntaxTree(CompilationUnit compilationUnit, IDocument input, TextRange range)
        {
            throw new NotImplementedException();
        }

        public override IAstVisitor CreateWriter(IOutputFormatter formatter)
        {
            return new CSharpAstWriter(formatter, new CSharpAstWriterParameters()
            {
                NamespaceBraceStyle = BraceStyle.NextLine,
                TypeBraceStyle = BraceStyle.NextLine,
                PropertyBraceStyle = BraceStyle.NextLine,
                EventBraceStyle = BraceStyle.NextLine,
                AccessorBraceStyle = BraceStyle.NextLine,
                ConstructorBraceStyle = BraceStyle.NextLine,
                MethodBraceStyle = BraceStyle.NextLine,
                ArrayInitializerBraceStyle = BraceStyle.NextLine,
                StatementBraceStyle = BraceStyle.NextLine,
            });
        }
    }
}
