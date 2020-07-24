using UnityEngine;
using System.Collections;
using System.Transactions;
using System.Collections.Generic;

public class Pawn : ChessPiece
{
    public override void Initialize(ChessPiece.Position position, ChessPiece.Color color)
    {
        this.Initialize(position, ChessPiece.Symbol.P, 1, color);
    }

    public override void SetSight()
    {
        this.sight = new List<Position>();
        Position testing;
        
        switch (this.color)
        {
            case Color.WHITE:
                testing = controller.IndexToPosition((int)position.col - 1, position.row + 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }

                testing = controller.IndexToPosition((int)position.col + 1, position.row + 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }
                break;
            case Color.BLACK:
                testing = controller.IndexToPosition((int)position.col - 1, position.row - 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }

                testing = controller.IndexToPosition((int)position.col + 1, position.row - 1);
                if (testing != null)
                {
                    this.sight.Add(testing);
                }
                break;
        }
    }

    public override void SetMoves()
    {
        SetSight();
        this.moves = new List<Position>();
        Position testing;
        bool freeAhead = false;

        switch (this.color)
        {
            case Color.WHITE:
                testing = controller.IndexToPosition((int)position.col, position.row + 1);
                if (testing != null && controller.GetOnPosition(testing) == null)
                {
                    this.moves.Add(testing);
                    freeAhead = true;
                }

                testing = controller.IndexToPosition((int)position.col, position.row + 2);
                if (testing != null && position.row == 2 && freeAhead && controller.GetOnPosition(testing) == null)
                {
                    this.moves.Add(testing);
                }
                break;
            case Color.BLACK:
                testing = controller.IndexToPosition((int)position.col, position.row - 1);
                if (testing != null && controller.GetOnPosition(testing) == null)
                {
                    this.moves.Add(testing);
                    freeAhead = true;
                }
                
                testing = controller.IndexToPosition((int)position.col, position.row - 2);
                if (testing != null && position.row == 7 && freeAhead && controller.GetOnPosition(testing) == null)
                {
                    this.moves.Add(testing);
                }
                break;
        }
        foreach (ChessPiece.Position pos in sight)
        {
            ChessPiece capture = controller.GetOnPosition(pos);
            if (capture != null && capture.GetColor() != this.color)
            {
                this.moves.Add(pos);
            }
        }

        //Filtrar movimientos inválidos.
    }
}
