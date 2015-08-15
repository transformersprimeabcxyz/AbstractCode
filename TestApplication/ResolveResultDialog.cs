using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using AbstractCode.Symbols;
using AbstractCode.Symbols.Resolution;

namespace AbstractCode.TestApplication
{
    public partial class ResolveResultDialog : Form
    {
        public ResolveResultDialog(ResolveResult result)
        {
            InitializeComponent();
            treeView1.Nodes.Add(CreateNode("Result", result));
        }

        private static TreeNode CreateNode(string name, object value)
        {
            var node = new TreeNode(string.Format("{0}: {1}", name, value ?? "null"));

            if (value is ResolveResult)
            {
                foreach (var property in value.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
                {
                    node.Nodes.Add(CreateNode(property.Name, property.GetValue(value, null)));
                }
            }
            else if (value is IEnumerable<INamedDefinition> || value is IEnumerable<ResolveResult>)
            {
                foreach (var element in (IEnumerable)value)
                {
                    node.Nodes.Add(CreateNode("element", element));
                }
            }

            node.Expand();

            return node;
        }
    }
}