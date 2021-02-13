using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stalker : Strategy
{
    private void Start()
    {
        this.name = "Stalker";
    }

    public override float Evaluate(Board board)
    {
        this.board = board;

        return FilterCheckMate(-DistanceToKing());
    }
}
