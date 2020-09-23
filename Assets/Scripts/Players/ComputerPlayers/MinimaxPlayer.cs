using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimaxPlayer : ChessPlayer
{
    private Tree<Move, Board> Tree;
    private int depth;
    private Strategy strategy;

    public void Start()
    {
        Tree<string, int> tree = new Tree<string, int>("1", 0);
        tree.GetRoot().AddChild("1-1", 0);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-1", 0, 1);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-2", 0, 2);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-3", 0, 3);
        tree.GetRoot().AddChild("1-2", 0);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-1", 0, 2);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-2", 0, 3);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-3", 0, 5);

        print(tree);

        MinimaxProcessor<string, int> processor =
            new MinimaxProcessor<string, int>(MinimaxProcessor<string, int>.Mode.MIN, tree);
        processor.Process();

        print(tree);
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
