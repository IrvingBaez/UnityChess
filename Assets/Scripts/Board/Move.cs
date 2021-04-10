using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public readonly Position origin;
    public readonly Position destiny;
    public readonly char? capture;
    public readonly char? promotion;
    public bool[] castling;
    public Position enPassant;
    public int halfturn;

    internal Move(Position origin, Position destiny, char? capture, char? promotion = null)
    {
        this.origin = origin;
        this.destiny = destiny;
        this.promotion = promotion;
        this.capture = capture;
    }

    public override string ToString()
    {
        return $"From {origin} to {destiny}";
    }

    public static Move sampleMove = new Move(Position.Create(0, 6), Position.Create(0, 5), null);
}