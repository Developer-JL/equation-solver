using System;
namespace Equ
{

    /*
	 * This class represents the treenode in a binary expression tree.
	 */
    public class TreeNode
    {

        public enum Operator { Value, Plus, Minus, Times, Divide, Mod, Square, NoOp };

        private int numberValue;
        private Operator op;
        private TreeNode parent;
        private TreeNode leftChild;
        private TreeNode rightChild;

        /*
         * Operator treenode constructor.
         */
        public TreeNode(Operator op)
        {
            this.op = op;
        }

        /*
         * Value treenode constructor.
         */
        public TreeNode(int numberValue)
        {

            this.numberValue = numberValue;
            op = Operator.Value;

        }

        public int NumberValue
        {

            get
            {
                return numberValue;
            }
        }

        public Operator Op
        {

            get
            {
                return op;
            }
        }

        public TreeNode Parent
        {

            get
            {

                return parent;
            }
            set
            {

                parent = value;
            }

        }

        public TreeNode LeftChild
        {

            get
            {

                return leftChild;
            }
            set
            {

                if (op != Operator.Value)
                {

                    leftChild = value;
                }
            }
        }

        public TreeNode RightChild
        {

            get
            {

                return rightChild;
            }
            set
            {

                if (op != Operator.Value)
                {

                    rightChild = value;
                }
            }
        }

        /*
		 * Tests if a treenode is a value treenode.
		 */
        public Boolean IsValue()
        {

            return op == Operator.Value;
        }

        /*
         * Tests if a treenode is a operator treenode.
         */
        public Boolean IsOperator()
        {

            return op != Operator.Value && op != Operator.NoOp;
        }

        public override String ToString()
        {

            if (IsValue())
            {

                return "" + numberValue;
            }

            switch (op)
            {

                case Operator.Value: return "value";
                case Operator.Plus: return "+";
                case Operator.Minus: return "-";
                case Operator.Times: return "*";
                case Operator.Divide: return "/";
                case Operator.Mod: return "%";
                case Operator.Square: return "^2";
                case Operator.NoOp: return "";
            }

            return "";
        }
    }
}
