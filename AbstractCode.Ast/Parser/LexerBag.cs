using System.Collections.Generic;

namespace AbstractCode.Ast.Parser
{
    public class LexerBag
    {
        public LexerBag()
        {
            SpecialNodes = new List<AstNode>();
        }

        public IList<AstNode> SpecialNodes
        {
            get;
        }

        public void InsertNodesIntoAstNode(AstNode target)
        {
            foreach (var specialNode in SpecialNodes)
            {
                var node = target.GetNodeAtLocation(specialNode.Range.Start);
                if (node == null)
                    target.InsertChild(AstNodeTitles.UnprocessedNode, 0, specialNode);
                else
                    node.Parent.InsertChild(AstNodeTitles.UnprocessedNode, node.Parent.Children.IndexOf(node) + 1,
                        specialNode);
            }
        }
    }
}