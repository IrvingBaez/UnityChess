using UnityEngine;
using System.Diagnostics;
using System.Collections.Generic;
using System.Collections;

public class DynamicMinimaxPlayer : ChessPlayer
{
    private float evaluation;
    private int phase;
    private LinkedList<Node<Board.Move, Board>> frontier;

    [SerializeField] private int limit;
    [SerializeField] private Strategy strategy;

    private int explored;
    private Tree<Board.Move, Board> tree;

    private Stopwatch findChildrenWatch = new Stopwatch();
    private Stopwatch constructTreeWatch = new Stopwatch();

    public override void Move()
    {
        tree = new Tree<Board.Move, Board>(null, board);
        tree.GetRoot().SetValue(strategy.Evaluate(tree.GetRoot().GetContent()));

        frontier = new LinkedList<Node<Board.Move, Board>>();
        frontier.AddLast(tree.GetRoot());
        explored = 0;

        constructTreeWatch.Start();
        
        ExploreFrontier();
        game.Move(tree.GetRoot().GetBestChild().GetIdentifier());

        constructTreeWatch.Stop();

        long constructing = constructTreeWatch.ElapsedMilliseconds;
        long finding = findChildrenWatch.ElapsedMilliseconds;

        print($"{constructing} ms constructing.");
        print($"{finding} ms ({100 * finding / constructing}%) finding children.");
        print($"{explored} boards explored, in {tree.MaxDepth()} turns in the future.");

        constructTreeWatch.Reset();
        findChildrenWatch.Reset();
    }

    private void ExploreFrontier()
    {
        while(tree.GetRoot().GetBestChild() == null || (frontier.Count > 0 && explored < limit))
        {
            explored++;

            Node<Board.Move, Board> exploring = frontier.First.Value;
            frontier.RemoveFirst();

            exploring.SetValue(strategy.Evaluate(exploring.GetContent()));
            UpdateTreeValues(exploring);
            if (IsGoalState(exploring))
            {
                return;
            }
            if (ShouldExpand(exploring))
            {
                EnqueueChildren(exploring);
            }

            print(exploring);
        }
    }

    private void UpdateTreeValues(Node<Board.Move, Board> node)
    {
        Node<Board.Move, Board> parent = node.GetParent();
        if(parent == null) { return; }

        bool even = parent.getDepth() % 2 == 0;
        Node<Board.Move, Board> best = parent.GetBestChild();

        if (color == 1)
        {
            if (even)
            {
                if(best == null || best.GetValue() < node.GetValue())
                {
                    parent.SetBestChild(node);
                    UpdateTreeValues(parent);
                }
            }
            else
            {
                if (best == null || best.GetValue() > node.GetValue())
                {
                    parent.SetBestChild(node);
                    UpdateTreeValues(parent);
                }
            }
        }
        else
        {
            if (even)
            {
                if (best == null || best.GetValue() > node.GetValue())
                {
                    parent.SetBestChild(node);
                    UpdateTreeValues(parent);
                }
            }
            else
            {
                if (best == null || best.GetValue() < node.GetValue())
                {
                    parent.SetBestChild(node);
                    UpdateTreeValues(parent);
                }
            }
        }
    }

    private bool IsGoalState(Node<Board.Move, Board> node)
    {
        return (board.Turn == 1 && float.IsPositiveInfinity(node.GetValue()))
            || (board.Turn == -1 && float.IsNegativeInfinity(node.GetValue()));
    }

    private bool ShouldExpand(Node<Board.Move, Board> node)
    {
        if(color == 1)
        {
            return frontier.Count == 0 || node.GetValue() <= tree.GetRoot().GetValue();
        }

        return frontier.Count == 0 || node.GetValue() >= tree.GetRoot().GetValue();
    }

    private void EnqueueChildren(Node<Board.Move, Board> node)
    {
        findChildrenWatch.Start();

        List<Board.Position> pieces = node.GetContent().Turn == 1 ? node.GetContent().WhitePieces : node.GetContent().BlackPieces;
        
        foreach (Board.Position piece in pieces)
        {
            foreach (Board.Move move in node.GetContent().LegalMoves(piece))
            {
                Node<Board.Move, Board> child = new Node<Board.Move, Board>(move, node.GetContent().CopyAndMove(move));
                node.AddChild(child);

                if(color == 1)
                {
                    if (node.GetValue() < tree.GetRoot().GetValue())
                    {
                        frontier.AddFirst(child);
                    }
                    else
                    {
                        frontier.AddLast(child);
                    }
                }
                else
                {
                    if (node.GetValue() > tree.GetRoot().GetValue())
                    {
                        frontier.AddFirst(child);
                    }
                    else
                    {
                        frontier.AddLast(child);
                    }
                }
            }
        }
        findChildrenWatch.Stop();
    }

    public float GetEvaluation()
    {
        return evaluation;
    }
}
