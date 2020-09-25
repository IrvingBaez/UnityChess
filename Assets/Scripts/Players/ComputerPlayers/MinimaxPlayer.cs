using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxPlayer : ChessPlayer
{
    private Tree<Move, Board> tree;
    [SerializeField] private int depth = 2;
    [SerializeField] private Strategy strategy;

    public override void Move()
    {
        tree = new Tree<Move, Board>(null, board);
        ConstructTree(tree.GetRoot());

        MinimaxProcessor<Move, Board>.Mode mode = (MinimaxProcessor<Move, Board>.Mode)(int)color;
        MinimaxProcessor<Move, Board> processor = new MinimaxProcessor<Move, Board>(mode, tree);

        processor.Process();

        game.Move(processor.GetSequence()[0]);
    }

    private void ConstructTree(Node<Move, Board> node)
    {
        FindChildren(node);

        if (node.IsLeaf())
        {
            node.SetValue(strategy.Evaluate(node.GetContent()));
        }

        foreach (Node<Move, Board> child in node.GetChildren())
        {
            if(child.getDepth() < depth)
            {
                ConstructTree(child);
            }
            else
            {
                child.SetValue(strategy.Evaluate(child.GetContent()));
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
