using System;
using System.Collections.Generic;
using System.Text;

namespace mankala_tests
{
    public class MinMax
    {
        public MinMax(IHeuristic heuristic)
        {
            h = heuristic;
        }
        IHeuristic h = new PointDiff();
        int player;

        public void Test()
        {
            Board b = new Board();
            Node root = new Node(b, 0, -1);
            root.MakeChildrenForThisNodeOnly();
            root.Shout();
        }

        private class Node
        {
            public Board board;
            public int playersTurn;
            public int move;
            public LinkedList<Node> children = new LinkedList<Node>();
            public Node(Board b, int turn, int move)
            {
                this.board = b;
                this.playersTurn = turn;
                this.move = move;
            }
            public int MakeChildrenForThisNodeOnly(bool real = true)
            {
                if (board.End) return 0;
                else
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (board.stones[i, playersTurn] == 0) continue;
                        Board copy = new Board(board);
                        var finished = copy.MakeMove(i, playersTurn);
                        if (finished)
                        {
                            Node child = new Node(copy, (playersTurn + 1) % 2, i);
                            children.AddLast(child);
                        }
                        else
                        {
                            if (real)
                            {
                                Node child = new Node(copy, playersTurn, i);
                                child.MakeChildrenForThisNodeOnly(false);
                                LinkedList<Node> flat = new LinkedList<Node>();
                                child.Flatten(flat);
                                foreach(var f in flat)
                                {
                                    f.move = i;
                                    children.AddLast(f);
                                }
                            }
                            else
                            {
                                Node child = new Node(copy, playersTurn, i);
                                children.AddLast(child);
                                child.MakeChildrenForThisNodeOnly(false);
                                LinkedList<Node> flat = new LinkedList<Node>();
                            }
                            
                        }

                    }
                    return children.Count;
                }
            }
            public void Flatten(LinkedList<Node> offspring)
            {
                if (children.Count == 0) offspring.AddLast(this);
                else
                {
                    foreach(var ch in children)
                    {
                        ch.Flatten(offspring);
                    }
                }
            }
            public override string ToString()
            {
                return board.ToString();
            }
            public void Shout()
            {
                string temp = ToString() + (children.Count == 0 ? " Nie mam dzieci" : "Moje dzieci to:");
                Console.WriteLine(temp);
                foreach (var ch in children)
                {
                    ch.Shout();
                }
                if (children.Count != 0)
                    Console.WriteLine($" mam {children.Count} dzieci");
            }
        }

        private int MinValue(Node n, int depth)
        {
            if (depth <= 0 || n.MakeChildrenForThisNodeOnly() == 0) 
            {
                return h.Value(n.board, player);
            }
            int value = int.MaxValue;
            foreach (var ch in n.children)
            {
                value = Math.Min(value, MaxValue(ch, depth - 1));
            }
            return value;
        }

        private int MaxValue(Node n, int depth)
        {
            if (depth <= 0 || n.MakeChildrenForThisNodeOnly() == 0)
            {
                return h.Value(n.board, player);
            }
            int value = int.MinValue;
            foreach (var ch in n.children)
            {
                value = Math.Max(value, MinValue(ch, depth -1));
            }
            return value;
        }

        private int MinMaxFirstMove(Node n, int depth)
        {
            int move = -1;
            int max = int.MinValue;
            n.MakeChildrenForThisNodeOnly();
            foreach (var ch in n.children)
            {
                int childValue = MinValue(ch, depth - 1);
                if (childValue > max)
                {
                    max = childValue;
                    move = ch.move;
                }
            }
            return move;
        }

        public int MinMaxSolve(Board b, int player, int depth = 5)
        {
            this.player = player;
            Node root = new Node(b, player, -1);
            int answer = MinMaxFirstMove(root, depth);
            return answer;
        }
        private int MaxValueAB(Node n, int alfa, int beta, int depth)
        {
            if (depth <= 0 || n.MakeChildrenForThisNodeOnly() == 0)
            {
                return h.Value(n.board, player);
            }
            int value = int.MinValue;
            foreach (var ch in n.children)
            {
                value = Math.Max(value, MinValueAB(ch, alfa, beta, depth - 1));
                if (value >= beta) return value;
                alfa = Math.Max(alfa, value);
            }
            return value;
        }
        private int MinValueAB(Node n, int alfa, int beta, int depth)
        {
            if (depth <= 0 || n.MakeChildrenForThisNodeOnly() == 0)
            {
                return h.Value(n.board, player);
            }
            int value = int.MaxValue;
            foreach (var ch in n.children)
            {
                value = Math.Min(value, MaxValueAB(ch, alfa, beta, depth - 1));
                if (value <= alfa) return value;
                beta = Math.Min(beta, value);
            }
            return value;
        }
        private int AlfaBetaFirstMove(Node n, int depth)
        {
            int move = -1;
            int max = int.MinValue;
            n.MakeChildrenForThisNodeOnly();
            foreach (var ch in n.children)
            {
                int childValue = MinValueAB(ch, int.MinValue, int.MaxValue, depth - 1);
                if (childValue > max)
                {
                    max = childValue;
                    move = ch.move;
                }
            }
            return move;
        }
        public int AlfaBetaSolve(Board b, int player, int depth = 5)
        {
            this.player = player;
            Node root = new Node(b, player, -1);
            int answer = AlfaBetaFirstMove(root, depth);
            return answer;
        }
    }
}

