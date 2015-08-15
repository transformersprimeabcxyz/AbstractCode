using System;

namespace AbstractCode.Ast.Types
{
    public class ArrayTypeRankSpecifier : AstNode
    {
        private int _dimensions;

        public ArrayTypeRankSpecifier()
            : this(1)
        {
        }

        public ArrayTypeRankSpecifier(int dimensions)
        {
            Dimensions = dimensions;
        }

        public AstToken LeftBracket
        {
            get { return GetChildByTitle(AstNodeTitles.LeftBracket); }
            set { SetChildByTitle(AstNodeTitles.LeftBracket, value); }
        }

        public int Dimensions
        {
            get { return _dimensions; }
            set
            {
                if (value <= 0)
                    throw new ArgumentOutOfRangeException();
                _dimensions = value;
            }
        }

        public AstToken RightBracket
        {
            get { return GetChildByTitle(AstNodeTitles.RightBracket); }
            set { SetChildByTitle(AstNodeTitles.RightBracket, value); }
        }

        public override string ToString()
        {
            return string.Format("[{0}]", new string(',', Dimensions - 1));
        }

        public override bool Match(AstNode other)
        {
            var specifier = other as ArrayTypeRankSpecifier;
            return specifier != null
                   && Dimensions == specifier.Dimensions;
        }

        public override void AcceptVisitor(IAstVisitor visitor)
        {
            visitor.VisitArrayTypeRankSpecifier(this);
        }

        public override TResult AcceptVisitor<TResult>(IAstVisitor<TResult> visitor)
        {
           return  visitor.VisitArrayTypeRankSpecifier(this);
        }

        public override TResult AcceptVisitor<TData, TResult>(IAstVisitor<TData, TResult> visitor, TData data)
        {
           return  visitor.VisitArrayTypeRankSpecifier(this, data);
        }
    }
}
