using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ChessPlayer white;
    [SerializeField] private ChessPlayer black;
    public BoardView BoardView;

    [Header("UI")]
    [SerializeField] private TMP_Text textState;
    [SerializeField] private TMP_Text textTurn;
    [SerializeField] private TMP_Text textBlackEval;
    [SerializeField] private TMP_Text textWhiteEval;

    private Board board;
    private ChessPlayer currentPlayer;
    private readonly string initialPosition = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";

    void Start()
    {
        white.SetColor(1);
        black.SetColor(-1);

        currentPlayer = white;
        NewGame();
    }

    private void NewGame()
    {
        white.game = this;
        black.game = this;
        board = new Board(initialPosition);
        
        white.SetBoard(board);
        black.SetBoard(board);

        BoardView.SetBoard(board);
        BoardView.DrawBoard();

        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        print(board.Fen());
        switch (board.State)
        {
            case Board.GameState.ALIVE:
            case Board.GameState.CHECK_TO_BLACK:
            case Board.GameState.CHECK_TO_WHITE:
                yield return null;
                currentPlayer.Move();
                break;
            default:
                yield return null;
                break;
        }
    }

    public void Move(Board.Move move)
    {
        board.PerformMove(move);
        BoardView.DrawBoard();

        if (currentPlayer == white)
        {
            currentPlayer = black;
        }
        else
        {
            currentPlayer = white;
        }

        textState.text = board.State.ToString();
        textTurn.text = currentPlayer.GetColor().ToString();
        textBlackEval.text = black.Evaluation().ToString();
        textWhiteEval.text = white.Evaluation().ToString();

        StartCoroutine(Play());
    }
}
