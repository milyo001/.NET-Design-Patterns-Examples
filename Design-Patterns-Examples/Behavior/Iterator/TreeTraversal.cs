using static System.Console;

namespace DotNetDesignPatternDemos.Structural.Iterator.TreeTraversal
{
    // Used for C++ variant
    public class Node<T>
    {
        public T Value;
        public Node<T> Left, Right;
        public Node<T> Parent;

        public Node(T value)
        {
            this.Value = value;
        }

        public Node(T value, Node<T> left, Node<T> right)
        {
            this.Value = value;
            this.Left = left;
            this.Right = right;
            
            left.Parent = right.Parent = this;
        }
    }

    // Iterator class (C++ style)
    public class InOrderIterator<T>
    {
        public Node<T> Current { get; set; }
        private readonly Node<T> root;
        private bool yieldedStart;

        public InOrderIterator(Node<T> root)
        {
            this.root = Current = root;
            while (Current.Left != null)
                Current = Current.Left;
            
            //      1 <- root
            //     / \
            //    2   3
            //    ^ Current
        }


        // Reset the iterator to the starting position
        public void Reset()
        {
            Current = root;
            yieldedStart = true;
        }

        // Critical method for iterator, if succeded return true
        public bool MoveNext()
        {
            if (!yieldedStart)
            {
                yieldedStart = true;
                return true;
            }

            if (Current.Right != null)
            {
                Current = Current.Right;
                while (Current.Left != null)
                    Current = Current.Left;
                return true;
            }
            else
            {
                var p = Current.Parent;
                while (p != null && Current == p.Right)
                {
                    Current = p;
                    p = p.Parent;
                }
                Current = p;
                return Current != null;
            }
        }
    }

    // C# classic iterator
    public class BinaryTree<T>
    {
        private Node<T> root;

        public BinaryTree(Node<T> root)
        {
            this.root = root;
        }

        public InOrderIterator<T> GetEnumerator()
        {
            return new InOrderIterator<T>(root);
        }

        public IEnumerable<Node<T>> NaturalInOrder
        {
            get
            {
                IEnumerable<Node<T>> TraverseInOrder(Node<T> current)
                {
                    if (current.Left != null)
                    {
                        foreach (var left in TraverseInOrder(current.Left))
                            yield return left;
                    }
                    yield return current;
                    if (current.Right != null)
                    {
                        foreach (var right in TraverseInOrder(current.Right))
                            yield return right;
                    }
                }
                foreach (var node in TraverseInOrder(root))
                    yield return node;
            }
        }
    }

    public class Demo
    {
        public static void Main()
        {
            //   1
            //  / \
            // 2   3

            // In-order:  213
            // Preorder:  123
            // Postorder: 231

            // So the root has value one and has two children with values 2 and 3
            var root = new Node<int>(1,
              new Node<int>(2), new Node<int>(3));

            // C++ style
            var it = new InOrderIterator<int>(root);

            while (it.MoveNext())
            {
                Write(it.Current.Value);
                Write(',');
            }
            WriteLine();

            // C# style
            var tree = new BinaryTree<int>(root);

            WriteLine(string.Join(",", tree.NaturalInOrder.Select(x => x.Value)));

            // Duck typing! tree is BinaryTree, which is not IEnumerable, check GetEnumerator method
            foreach (var node in tree)
                WriteLine(node.Value);
        }
    }
}