using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hybrid : Strategy
{
    private float material = 1;
    private float space = 0.1f;
    private float distanceToKing = 0;

    private void Start()
    {
        this.name = "Hybrid";
    }

    public override float Evaluate(Board board)
    {
        this.board = board;

        float result = 0;
        result += material * Material();
        result += space * Space();
        result -= distanceToKing * DistanceToKing();

        return FilterCheckMate(result);
    }
}
