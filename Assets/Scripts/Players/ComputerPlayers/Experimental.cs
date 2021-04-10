using UnityEngine;

public class Experimental : ChessPlayer
{
    public readonly float evaluation;
    [SerializeField] private int limit;
    [SerializeField] private Strategy strategy;
    private float baseEvaluation;
    private int positionsAnalized = 0;

    public override void Move()
    {
        Process(board, 0);

        print($"{positionsAnalized} positions analized.");

        game.Move(board.AllLegalMoves()[0]);
    }

    private float Process(Board node, int depth, string separator = ""){
        positionsAnalized++;

        if(depth == limit){
            return strategy.Evaluate(node);
        }

        float eval = board.Turn == 1 ? float.NegativeInfinity : float.PositiveInfinity;
        foreach(Move move in node.AllLegalMoves()){
            //print(separator + move);
            node.PerformMove(move);
            eval = Process(node, depth + 1, separator + "\t");
            node.undoMove(move);
        }
        return eval;
    }
}
