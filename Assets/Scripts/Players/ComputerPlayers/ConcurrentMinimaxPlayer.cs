using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcurrentMinimaxPlayer : ChessPlayer
{
    private Tree<Move, Board> tree;
    private Move bestMove;
    private Strategy strategy;

    void Start()
    {
        tree = new Tree<Move, Board>(null, board);
    }

    void Update()
    {
        StartCoroutine("Process");
    }
    
    private IEnumerable Process()
    {
        return null;
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
