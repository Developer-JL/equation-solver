using System;
namespace Equ
{

    /*
     * This class represents a binary expression tree.
     */
    public class BETree
    {

        private TreeNode root;
        private int treeSize;

        public BETree(TreeNode treeNode)
        {

            root = treeNode;
            treeSize = CalTreeSize(treeNode);

        }

        public TreeNode Root
        {

            get
            {

                return root;
            }
            set
            {

                root = value;
            }
        }

        public int TreeSize
        {

            get
            {

                return treeSize;
            }
            set
            {

                treeSize = value;
            }
        }

        /*
         * Helper function that return the size of a tree.
         */
        private int CalTreeSize(TreeNode tree)
        {

            if (tree == null)
                return 0;

            return (CalTreeSize(tree.LeftChild) + CalTreeSize(tree.RightChild) + 1);
        }

    }
}
