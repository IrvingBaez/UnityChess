using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public readonly int origin;
    public readonly int destiny;
    public readonly char? promotion;
    public char? capture;
    public bool[] castling;
    public long enPassant;
    public int halfturn;

    internal Move(int origin, int destiny, char? promotion = null)
    {
        this.origin = origin;
        this.destiny = destiny;
        this.promotion = promotion;
    }

    public override string ToString()
    {
        return $"From {origin} to {destiny}";
    }

    public static Move sampleMove = new Move(55, 39, null);
}