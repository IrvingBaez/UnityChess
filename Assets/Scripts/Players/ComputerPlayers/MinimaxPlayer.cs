using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxPlayer : ChessPlayer
{
    private Tree<Move, Board> tree;
    [SerializeField] private int depth = 2;
    [SerializeField] private Strategy strategy;
    
    private Stopwatch findChildrenWatch = new Stopwatch();
    private Stopwatch evaluationWatch = new Stopwatch();
    private Stopwatch constructTreeWatch = new Stopwatch();

    public override void Move()
    {
        tree = new Tree<Move, Board>(null, board);

        constructTreeWatch.Start();
        strategy.checkmateWatch.Reset();

        ConstructTree(tree.GetRoot());
        constructTreeWatch.Stop();

        MinimaxProcessor<Move, Board>.Mode mode = (MinimaxProcessor<Move, Board>.Mode)(int)color;
        MinimaxProcessor<Move, Board> processor = new MinimaxProcessor<Move, Board>(mode, tree);
        
        processor.Process();
        game.Move(processor.GetSequence()[0]);

        long constructing = constructTreeWatch.ElapsedMilliseconds;
        long finding = findChildrenWatch.ElapsedMilliseconds;
        long evaluating = evaluationWatch.ElapsedMilliseconds;
        long evaluatingCheck = strategy.checkmateWatch.ElapsedMilliseconds;

        print($"{constructing} ms constructing.");
        print($"{finding} ms ({100 * finding / constructing}%) finding children.");
        print($"{evaluating} ms ({100 * evaluating / constructing}%) evaluating.");
        print($"{evaluatingCheck} ms ({100 * evaluatingCheck / constructing}%) evaluating check.");

        constructTreeWatch.Reset();
        findChildrenWatch.Reset();
    }

    private void ConstructTree(Node<Move, Board> node)
    {
        findChildrenWatch.Start();
        FindChildren(node);
        findChildrenWatch.Stop();

        if (node.IsLeaf())
        {
            evaluationWatch.Start();
            node.SetValue(strategy.Evaluate(node.GetContent()));
            evaluationWatch.Stop();
        }

        foreach (Node<Move, Board> child in node.GetChildren())
        {
            if(child.getDepth() < depth)
            {
                ConstructTree(child);
            }
            else
            {
                evaluationWatch.Start();
                child.SetValue(strategy.Evaluate(child.GetContent()));
                evaluationWatch.Stop();
            }
        }
    }

    private void FindChildren(Node<Move, Board> node)
    {
        List<ChessPiece> pieces;

        if(node.GetContent().GetTurn() == ChessPiece.Color.WHITE)
        {
            pieces = node.GetContent().GetWhitePieces();
        }
        else
        {
            pieces = node.GetContent().GetBlackPieces();
        }

        foreach(ChessPiece piece in pieces)
        {
            foreach(Move move in piece.GetMoves())
            {
                node.AddChild(move, node.GetContent().CopyAndMove(move));
            }
        }
    }
}
