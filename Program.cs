using System;
using System.Collections.Generic;

namespace Equ
{

    /**
	 * This is a C# program that will act as a simple equation solver.
	 */
    class MainClass
    {

        public static void Main(string[] args)
        {

            SolveEqu(args);

        }

        /*
         * Helper function that creates a TreeNode with the appropriate operator
         * when given a string that's "+", "-", "*", "/" or "%". If the string is wrong
         * it gives a NoOp value.
         */
        private static TreeNode CreateOpNode(String op)
        {

            switch (op)
            {

                case "+": return new TreeNode(TreeNode.Operator.Plus);
                case "-": return new TreeNode(TreeNode.Operator.Minus);
                case "*": return new TreeNode(TreeNode.Operator.Times);
                case "/": return new TreeNode(TreeNode.Operator.Divide);
                case "%": return new TreeNode(TreeNode.Operator.Mod);

            }

            return new TreeNode(TreeNode.Operator.NoOp);
        }

        /*
         * Helper function that get coefficient of X or X^2.
         */
        private static string GetCoefficient(string operand)
        {

            int xPosition = 0;

            for (int i = 0; i < operand.Length; i++)
            {

                if (operand[i] == 'X')
                {

                    xPosition = i;

                }

            }

            if (xPosition == 0)
            {

                return "1";

            }
            else
            {

                return operand.Substring(0, xPosition);

            }

        }

        /*
         * Solve the whole equation.
         */
        private static void SolveEqu(string[] firstEquation)
        {

            string input;
            List<string> equation = new List<string>();

            try
            {

                if (firstEquation.Length != 0)
                {

                    string arguments = "";

                    foreach (string element in firstEquation)
                    {

                        arguments += element;
                    }

                    if (arguments.Contains("calc"))
                    {

                        equation = Tokenise(arguments);

                        if (IsValid(equation))
                        {

                            SolveEquation(equation);

                        }

                    }
                    else
                    {

                        Console.WriteLine("Invalid input, missing key word 'calc'!");

                    }

                }

                Console.WriteLine("Please enter your equation:");

                while ((input = Console.ReadLine()) != "end")
                {

                    if (input.Contains("calc"))
                    {
                        equation = Tokenise(input);

                        if (IsValid(equation))
                        {

                            SolveEquation(equation);

                        }
                    }
                    else
                    {

                        Console.WriteLine("Invalid input, missing key word 'calc'!");

                    }

                    Console.WriteLine("Please enter your equation:");

                }

            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message);

            }

        }

        /*
         * Helper function that does input validation. 
         */
        private static bool IsValid(List<string> input)
        {

            string first = input[0];
            string last = input[input.Count - 1];

            if ((first != "=") && (last != "=") && (HasX(input) || HasXSquare(input)) && HasEqualSign(input))
            {

                return true;
            }
            else if ((!HasX(input)) && (!HasXSquare(input)))
            {

                Console.WriteLine("Invalid input, no X variable in equation!");
                return false;

            }
            else
            {

                Console.WriteLine("Invalid input, not a valid equation!");
                return false;
            }

        }

        /*
         * Helper function that checks if there is X in input. 
         */
        private static bool HasX(List<string> input)
        {

            foreach (string element in input)
            {

                if (HasX(element))
                    return true;

            }

            return false;

        }

        /*
         * Helper function that checks if there is X^2 in input. 
         */
        private static bool HasXSquare(List<string> input)
        {

            foreach (string element in input)
            {

                if (HasXSquare(element))
                    return true;

            }

            return false;

        }

        /*
         * Helper function that checks if there is a "=" in input. 
         */
        private static bool HasEqualSign(List<string> input)
        {

            foreach (string element in input)
            {

                if (element == "=")
                    return true;

            }

            Console.WriteLine("Invalid input, no = sign in equation!");
            return false;

        }

        /*
         * Helper function that solve the equation. 
         */

        private static void SolveEquation(List<string> equation)
        {

            List<string> leftEquation = new List<string>();
            List<string> rightEquation = new List<string>();
            bool atLeftSide = true;

            for (int i = 0; i < equation.Count; i++)
            {

                if (equation[i] == "=")
                {

                    atLeftSide = false;

                }

                if (atLeftSide)
                {

                    leftEquation.Add(equation[i]);

                }

                if ((!atLeftSide) && (equation[i] != "="))
                {

                    rightEquation.Add(equation[i]);

                }

            }

            leftEquation = FixLastElement(leftEquation);
            rightEquation = FixLastElement(rightEquation);

            List<int> left = Calculate(leftEquation);
            List<int> right = Calculate(rightEquation);

            if (HasXSquare(equation))
            {

                int xSquCoe = left[2] - right[2];
                int xCoe = left[0] - right[0];
                int con = left[1] - right[1];
                SolveQuadEqu(xSquCoe, xCoe, con);

            }
            else
            {
                int xCoefficient = left[0] - right[0];
                int constant = left[1] - right[1];
                int result = -1 * constant / xCoefficient;

                Console.WriteLine("X = " + result);
            }

        }

        /*
         * Helper function that solve the Quadratic Equation, like aX^2 + bX + c = 0.
         */
        private static void SolveQuadEqu(int a, int b, int c)
        {

            double d = b * b - 4 * a * c;

            if (d > 0)
            {

                double result1 = (-b + Math.Sqrt(d)) / (2 * a);
                double result2 = (-b - Math.Sqrt(d)) / (2 * a);
                Console.WriteLine("X = " + result1 + ", " + result2);

            }
            if (d == 0)
            {

                double result = (-b + Math.Sqrt(d)) / (2 * a);
                Console.WriteLine("X = " + result);

            }
            if (d < 0)
            {

                Console.WriteLine("X is imaginary");

            }

        }

        /*
         * Helper function that fixs the last element is a operator.
         */
        private static List<string> FixLastElement(List<string> equation)
        {

            string lastElement = equation[equation.Count - 1];

            if (IsAOperator(lastElement))
            {

                switch (lastElement)
                {

                    case "+": equation.Add("0"); return equation;
                    case "-": equation.Add("0"); return equation;
                    case "*": equation.Add("1"); return equation;
                    case "/": equation.Add("1"); return equation;
                    case "%": equation.Add("1"); return equation;

                }

            }

            equation = HandleMinus(equation);

            return equation;

        }

        /*
         * Helper function that test if a string contains a number.
         */
        private static bool HasNumber(string testString)
        {

            foreach (char element in testString)
            {

                if (Isdigit(element))
                {

                    return true;

                }

            }

            return false;

        }

        /*
         * Helper function that calculate the coefficients and constants.
         */

        private static List<int> Calculate(List<string> equation)
        {

            List<string> xCoefficients = new List<string>();
            List<string> constants = new List<string>();
            List<string> xSquareCoefficients = new List<string>();
            Queue<string> equa = new Queue<string>();
            int ignoreMe = 0;
            int xCoef = 0;
            int cons = 0;
            int xSquCoef = 0;

            for (int i = 0; i < equation.Count; i++)
            {

                equa.Enqueue(equation[i]);

            }

            while (equa.Count != 0)
            {

                string op = equa.Dequeue();

                if (HasX(op))
                {

                    string coefficient = GetCoefficient(op);
                    xCoefficients.Add(coefficient);

                }
                else if (HasXSquare(op))
                {

                    string xSquareCoeff = GetCoefficient(op);
                    xSquareCoefficients.Add(xSquareCoeff);

                }
                else if (Int32.TryParse(op, out ignoreMe))
                {

                    constants.Add(op);

                }
                else if (IsAOperator(op))
                {

                    if (equa.Peek() == "(")
                    {

                        if (constants.Count == 0)
                        {

                            constants.Add("0");
                            constants.Add(op);

                        }
                        else
                        {

                            constants.Add(op);

                        }

                    }

                    if (HasX(equa.Peek()))
                    {

                        if (xCoefficients.Count == 0)
                        {

                            xCoefficients.Add("0");
                            xCoefficients.Add(op);

                        }
                        else
                        {

                            xCoefficients.Add(op);

                        }

                    }

                    if (HasXSquare(equa.Peek()))
                    {

                        if (xSquareCoefficients.Count == 0)
                        {

                            xSquareCoefficients.Add("0");
                            xSquareCoefficients.Add(op);

                        }
                        else
                        {

                            xSquareCoefficients.Add(op);

                        }

                    }

                    if (Int32.TryParse(equa.Peek(), out ignoreMe))
                    {

                        if (constants.Count == 0)
                        {

                            constants.Add("0");
                            constants.Add(op);

                        }
                        else
                        {

                            constants.Add(op);

                        }

                    }

                }
                else if (op == "(" || op == ")")
                {

                    constants.Add(op);

                }
                else if (!HasX(op) && !HasXSquare(op) && HasNumber(op) && !Int32.TryParse(op, out ignoreMe))
                {

                    throw new ArgumentOutOfRangeException();
                }

            }

            if (xCoefficients.Count != 0)
            {

                xCoef = Evaluate(BuildTree(xCoefficients).Root);

            }

            if (constants.Count != 0)
            {

                cons = Evaluate(BuildTree(constants).Root);

            }

            if (xSquareCoefficients.Count != 0)
            {

                xSquCoef = Evaluate(BuildTree(xSquareCoefficients).Root);

            }

            List<int> result = new List<int>();
            result.Add(xCoef);
            result.Add(cons);
            result.Add(xSquCoef);
            return result;


        }

        /*
         * Helper function that sets and returns weight of a operator.
         */
        private static int GetWeight(String op)
        {

            int weight = -1;

            switch (op)
            {

                case "+": weight = 1; break;
                case "-": weight = 1; break;
                case "*": weight = 2; break;
                case "/": weight = 2; break;
                case "%": weight = 2; break;
                case "#": weight = 4; break;
            }

            return weight;
        }


        /*
         * Helper function that converts a Infix vector of string to to a Postfix vector of string.
         */
        private static List<string> InfixToPostfix(List<string> tokens)
        {

            Stack<string> temp = new Stack<string>();
            List<string> postfix = new List<string>();
            int ignoreMe = 0;

            for (int i = 0; i < tokens.Count; i++)
            {

                if (IsAOperator(tokens[i]))
                {

                    while (temp.Count != 0 && temp.Peek() != "(" && HasHigherPrecedence(temp.Peek(), tokens[i]))
                    {

                        postfix.Add(temp.Peek());   // If operator in satck has higher precedence, add string
                        temp.Pop();                      // in stack to result until reach a "(".
                    }

                    temp.Push(tokens[i]);

                }
                else if (Int32.TryParse(tokens[i], out ignoreMe))
                {

                    postfix.Add(tokens[i]);

                }
                else if (tokens[i] == "(")
                {

                    temp.Push(tokens[i]);

                }
                else if (tokens[i] == ")")
                {

                    while (temp.Count != 0 && temp.Peek() != "(")
                    {

                        postfix.Add(temp.Peek());  // If it's a ), add strng in stack to result until reach a (.
                        temp.Pop();
                    }

                    temp.Pop();
                }
            }

            while (temp.Count != 0)
            {

                postfix.Add(temp.Peek());  // Add anyting left in temp to result.
                temp.Pop();
            }

            return postfix;
        }

        /*
         * Helper function that tests whether a string is a opreator.
         */
        private static bool IsAOperator(string s)
        {

            if (s == "+" || s == "-" || s == "*" || s == "/" || s == "%" || s == "#")
            {
                return true;
            }

            return false;
        }

        /*
         * Helper function that check if the first operator has higher precedence than the second one.
         */
        private static bool HasHigherPrecedence(string op1, string op2)
        {

            int op1Weight = GetWeight(op1);
            int op2Weight = GetWeight(op2);

            if (op1Weight == op2Weight)
            {

                if (IsRightAssociative(op1))
                    return false;                // When op1 and op2 have the same weight, op1 has higer precedence.
                return true;
            }

            return op1Weight > op2Weight;

        }

        /*
         * Helper function that check wether a operator is right associative or not. 
         */
        private static bool IsRightAssociative(string op)
        {

            if (op == "#")
                return true;   // Since there is no # in our input string, this will always return false.
            return false;
        }

        /*
         * This function takes a vector of strings representing an expression (as produced
         * by tokenise(string), and builds an ExprTree representing the same expression.
         */
        private static BETree BuildTree(List<string> tokens)
        {

            List<string> postfix = InfixToPostfix(tokens);   // Changes the vector of strings from infix to postfix.
            Stack<TreeNode> nodeStack = new Stack<TreeNode>();
            TreeNode node;
            int numberValue;


            for (int i = 0; i < postfix.Count; i++)
            {

                if (IsAOperator(postfix[i]))
                {

                    node = CreateOpNode(postfix[i]);    // If it's a operator, create a TreeNode with it.

                    node.RightChild = nodeStack.Peek();
                    nodeStack.Pop();                         // Set top node of stack as its right child, pop the top node.
                    node.LeftChild = nodeStack.Peek();      //  Set top node of stack as its left child, pop the top node.
                    nodeStack.Pop();
                    nodeStack.Push(node);                    //  Push the newly create node to the stack.
                }
                else if (Int32.TryParse(postfix[i], out numberValue))
                {

                    node = new TreeNode(numberValue);             // If it's a number, create a treeNode with it then push it to stack.
                    nodeStack.Push(node);
                }
            }

            node = nodeStack.Peek();
            return new BETree(node);       // Use top node on stack as a root node to create a new tree.
        }

        /*
         * This function takes a TreeNode and does the maths to calculate
         * the value of the expression it represents.
         */
        private static int Evaluate(TreeNode treeNode)
        {

            // if it is a leaf node, return its value.
            if (treeNode.IsValue())
            {

                return treeNode.NumberValue;

            }
            else if (treeNode.IsOperator())
            {


                // If it is a operator, recursively evaluate its children.
                if (treeNode.ToString() == "+")
                {

                    return Evaluate(treeNode.LeftChild) + Evaluate(treeNode.RightChild);

                }
                else if (treeNode.ToString() == "-")
                {

                    return Evaluate(treeNode.LeftChild) - Evaluate(treeNode.RightChild);

                }
                else if (treeNode.ToString() == "*")
                {

                    return Evaluate(treeNode.LeftChild) * Evaluate(treeNode.RightChild);

                }
                else if (treeNode.ToString() == "/")
                {

                    return Evaluate(treeNode.LeftChild) / Evaluate(treeNode.RightChild);

                }
                else if (treeNode.ToString() == "%")
                {

                    return Evaluate(treeNode.LeftChild) % Evaluate(treeNode.RightChild);

                }


            }

            return -1;
        }

        /*
         * Helper function that tests whether a string contains X.
         */
        private static bool HasX(string operand)
        {

            if (!HasXSquare(operand) && operand.Contains("X"))
            {

                return true;
            }
            return false;
        }

        /*
         * Helper function that tests whether a string contains X^2.
         */
        private static bool HasXSquare(string operand)
        {

            if (operand.Contains("X^2"))
            {

                return true;
            }
            return false;
        }

        /*
         * This function takes a string representing an equation and breaks
         * it up into components (number, operators, parentheses).
         * It returns the broken up equation as a List of strings.
         */
        private static List<string> Tokenise(string equation)
        {

            List<string> tokens = new List<string>();  // Stores the result.
            Queue<char> buffer = new Queue<char>();    // Temporaryly stores the char read in.
            string element = "";       // Stores string going to add to the result.


            for (int i = 0; i < equation.Length; i++)
            {

                if (equation[i] == ' ' || equation[i] == 'c' || equation[i] == 'a' || equation[i] == 'l')
                {

                    continue;          // If the char is a space or the key word, go to the next char.

                }
                else if (IsOperator(equation[i]) && IsOperator(buffer.ToArray()[buffer.ToArray().Length - 1]) && IsSameOperator(buffer.ToArray()[buffer.ToArray().Length - 1], equation[i]))
                {

                    continue;

                }
                else if (buffer.Count == 0 && (Isdigit(equation[i]) || IsOperator(equation[i])))
                {

                    buffer.Enqueue(equation[i]);

                }
                else if (Isdigit(buffer.ToArray()[buffer.ToArray().Length - 1]) && Isdigit(equation[i]))
                {

                    buffer.Enqueue(equation[i]);   // If the buffer stores a number and the char read in is a number, add the char to buffer.

                }
                else if ((Isdigit(buffer.ToArray()[buffer.ToArray().Length - 1]) && IsOperator(equation[i])) || (IsOperator(buffer.ToArray()[buffer.ToArray().Length - 1]) && Isdigit(equation[i])))
                {

                    while (buffer.Count != 0)
                    {

                        element += buffer.Peek();     // If the type in buffer is different from the char read in, take all the things from buffer,
                        buffer.Dequeue();                 //  and make a string with them, then add the string to the result. Add the current char to buffer.
                    }

                    tokens.Add(element);
                    element = "";
                    buffer.Enqueue(equation[i]);

                }
                else if (IsOperator(buffer.ToArray()[buffer.ToArray().Length - 1]) && IsOperator(equation[i]))
                {

                    string s = "";
                    s += buffer.Peek();
                    tokens.Add(s);
                    buffer.Dequeue();                        // If the one in buffer and current char are both operators, take out and add the one in buffer to result.
                    buffer.Enqueue(equation[i]);         //  And add the current one to the buffer.
                }
            }

            while (buffer.Count != 0)
            {

                element += buffer.Peek();        // Add anything left in buffer to a string then add it to result.
                buffer.Dequeue();
            }

            tokens.Add(element);
            return tokens;

        }

        /*
         * Helper function that deals with Minus operator in front of a number.
         */
        private static List<string> HandleMinus(List<string> equation)
        {


            Queue<string> temp = new Queue<string>();

            foreach (string element in equation)
            {
                temp.Enqueue(element);
            }

            equation.Clear();

            while (temp.Count != 0)
            {
                string op = temp.Dequeue();

                if (IsAOperator(op) && IsAOperator(temp.Peek()))
                {

                    equation.Add(op);
                    string negative = temp.Dequeue() + temp.Dequeue();
                    equation.Add(negative);

                }
                else
                {
                    equation.Add(op);
                }

            }

            return equation;

        }

        /*
         * Helper function that tests whether a char is a number.
         */
        private static bool Isdigit(char c)
        {

            switch (c)
            {
                case '0':
                case '1':
                case '2':
                case '3':
                case '4':
                case '5':
                case '6':
                case '7':
                case '8':
                case 'X':
                case '^':
                case '9': return true;
            }

            return false;

        }

        /*
         * Helper function that tests whether a char is a operator or "(" ,")".
         */
        private static bool IsOperator(char c)
        {

            switch (c)
            {
                case '+':
                case '-':
                case '*':
                case '/':
                case '%':
                case '=':
                case '(':
                case ')': return true;
            }

            return false;

        }

        /*
         * Helper function that tests whether two operator are the same.
         */
        private static bool IsSameOperator(char op1, char op2)
        {

            if ((op1 == '+' && op2 == '+') || (op1 == '-' && op2 == '-') || (op1 == '*' && op2 == '*') || (op1 == '/' && op2 == '/') || (op1 == '%' && op2 == '%'))
            {

                return true;

            }

            return false;

        }

    }
}
