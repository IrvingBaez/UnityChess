using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Position : IEquatable<Position>
{
    public int col;
    public int row;

    private Position(int col, int row)
    {
        this.col = col;
        this.row = row;
    }

    public Position Clone()
    {
        return new Position(col, row);
    }

    public static bool Create(int col, int row, out Position position)
    {
        if(0 <= col && col <= 7 && 0 <= row && row <= 7)
        {
            position = new Position(col, row);
            return true;
        }

        position = null;
        return false;
    }

    public static Position Create(int col, int row)
    {
        if(0 <= col && col <= 7 && 0 <= row && row <= 7)
        {
            return new Position(col, row);
        }

        return null;
    }

    public override string ToString()
    {
        return $"[{col}, {row}]";
    }

    public bool Equals(Position other)
    {
        return other != null && col == other.col && row == other.row;
    }

    public override bool Equals(object obj) => Equals(obj as Position);
    public override int GetHashCode() => (col, row).GetHashCode();
}