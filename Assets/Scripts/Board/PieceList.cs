using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PieceList {
    private int[] pieceIndexes;
    private int pieceCount;

    public int Count { get {return pieceCount;} }

    public PieceList(int size = 16){
        pieceIndexes = new int[size];
        pieceCount = 0;
    }

    public void Add(int position){
        pieceIndexes[pieceCount] = position;
        pieceCount++;
    }

    public void Remove(int position){
        pieceIndexes[position] = pieceIndexes[pieceCount - 1];
        pieceCount--;
    }
}
