﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialGrabber : Strategy
{
    private void Start()
    {
        this.name = "MaterialGrabber";
    }

    public override float Evaluate(Board board)
    {
        this.board = board;

        return FilterCheckMate(Material());
    }
}
