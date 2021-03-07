using Unity.Jobs;
using UnityEngine;

public abstract class ChessPlayer : MonoBehaviour
{
    public Game game;
    protected Board board;
    protected int color;

    public abstract void Move();

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public void SetColor(int color)
    {
        this.color = color;
    }

    public int GetColor()
    {
        return color;
    }

    public float Evaluation()
    {
        if(this is MinimaxPlayer computer)
        {
            return computer.GetEvaluation();
        }

        return 0;
    }
}
