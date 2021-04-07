using System;
using System.Collections;
using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxPlayer : ChessPlayer
{
    public override void Move()
    {
    }
    /*
    private Tree<Move> tree;
    private float evaluation;
    [SerializeField] private int depth = 2;
    [SerializeField] private Strategy strategy;
    
    private Stopwatch findChildrenWatch = new Stopwatch();
    private Stopwatch evaluationWatch = new Stopwatch();
    private Stopwatch constructTreeWatch = new Stopwatch();

    public override void Move()
    {
        tree = new Tree<Move>(null);

        constructTreeWatch.Start();
        strategy.checkmateWatch.Reset();

        ConstructTree(tree.GetRoot());
        constructTreeWatch.Stop();

        MinimaxProcessor<Move>.Mode mode = (MinimaxProcessor<Move>.Mode)(int)color;
        MinimaxProcessor<Move> processor = new MinimaxProcessor<Move>(mode, tree);
        
        processor.Process();
        evaluation = tree.GetRoot().GetValue();
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
        evaluationWatch.Reset();
        findChildrenWatch.Reset();
    }

    private void ConstructTree(Node<Move> node)
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

        foreach (Node<Move> child in node.GetChildren())
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

    private void FindChildren(Node<Move> node)
    {
        UnityEngine.Debug.Log($"{node} has the children:");
        List<Position> pieces = node.GetContent().Turn == 1 ? node.GetContent().WhitePieces : node.GetContent().BlackPieces;
        
        foreach(Position piece in pieces)
        {
            foreach(Move move in node.GetContent().LegalMoves[piece])
            {
                node.AddChild(move, node.GetContent().CopyAndMove(move));
                UnityEngine.Debug.Log($"\t{node.getChild(move)}");
            }
        }
    }

    public float GetEvaluation()
    {
        return evaluation;
    }
    */
}
