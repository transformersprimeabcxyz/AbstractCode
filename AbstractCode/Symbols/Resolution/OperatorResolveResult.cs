using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;

namespace AbstractCode.Symbols.Resolution
{
    public class OperatorResolveResult : ResolveResult
    {
        public OperatorResolveResult(OperatorType @operator, TypeDefinition operatorType, params ResolveResult[] operands)
            : base(operatorType)
        {
            Operator = @operator;
            OperatorType = operatorType;
            Operands = new ReadOnlyCollection<ResolveResult>(operands);
        }

        public OperatorResolveResult(OperatorType @operator, TypeDefinition operatorType, IEnumerable<ResolveResult> operands)
            : this(@operator, operatorType, operands.ToArray())
        {
        }

        public OperatorType Operator
        {
            get;
            set;
        }

        public TypeDefinition OperatorType
        {
            get;
            set;
        }

        public IList<ResolveResult> Operands
        {
            get;
            set;
        }
    }

    public enum OperatorType
    {
        // Unary operators
        Positive,
        Negative,
        Not,
        BitwiseNot,
        PostIncrement,
        PostDecrement,
        Dereference,
        AddressOf,
        Await,
        PreDecrement,
        PreIncrement,

        // Binary operators
        Add,
        Subtract,
        Multiply,
        Divide,
        Modulus,

        Equals,
        NotEquals,

        GreaterThan,
        GreaterThanOrEquals,
        LessThan,
        LessThanOrEquals,

        LogicalAnd,
        LogicalOr,
        NullCoalescing,

        BitwiseAnd,
        BitwiseOr,
        BitwiseXor,

        ShiftLeft,
        ShiftRight,
        
        Concat,
        IntegerDivide,
        RaisePower,
        Like,

        // Assignment operators
        Assign,

        AddAssign,
        SubtractAssign,
        MultiplyAssign,
        DivideAssign,
        ModulusAssign,

        BitwiseAndAssign,
        BitwiseOrAssign,
        BitwiseXorAssign,

        ShiftLeftAssign,
        ShiftRightAssign,
        
        ConcatAssign,
        IntegerDivideAssign,
        RaisePowerAssign,
    }
}
