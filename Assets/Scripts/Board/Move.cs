using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    public Position origin;
    public Position destiny;
    public char? promotion;

    internal Move(Position origin, Position destiny, char? promotion = null)
    {
        this.origin = origin;
        this.destiny = destiny;
        this.promotion = promotion;
    }

    public override string ToString()
    {
        return $"From {origin} to {destiny}";
    }

    public static Move sampleMove = new Move(Position.Create(0, 6), Position.Create(0, 5));
}