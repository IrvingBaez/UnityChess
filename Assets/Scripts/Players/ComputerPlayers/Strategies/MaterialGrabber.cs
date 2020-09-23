using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGrabber : Strategy
{
    public override float Evaluate(Board board)
    {
        this.board = board;

        return Material();
    }
}
