using System.Collections;
using TMPro;
using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] private ChessPlayer white;
    [SerializeField] private ChessPlayer black;
    public BoardView boardView;

    [Header("UI")]
    [SerializeField] private TMP_Text textState;
    [SerializeField] private TMP_Text textTurn;

    [Header("References")]
    [SerializeField] private CheckTile checkTileRef;
    [SerializeField] private Board boardRef;
    
    private Board board;
    private ChessPlayer currentPlayer;
    private CheckTile checkTile;

    void Start()
    {
        white.SetColor(ChessPiece.Color.WHITE);
        black.SetColor(ChessPiece.Color.BLACK);

        currentPlayer = white;
        NewGame();
    }

    private void NewGame()
    {
        board = new Board();

        white.game = this;
        black.game = this;
        white.SetBoard(board);
        black.SetBoard(board);
        boardView.SetBoard(board);

        boardView.DrawBoard();
        StartCoroutine(Play());
    }

    private IEnumerator Play()
    {
        print(board.Fen());
        switch (board.GetGameState())
        {
            case Board.GameState.ALIVE:
            case Board.GameState.BLACK_CHECK:
            case Board.GameState.WHITE_CHECK:
                yield return null;
                currentPlayer.Move();
                break;
            default:
                yield return null;
                break;
        }
    }

    public void Move(Move move)
    {
        board.PerformMove(move);
        boardView.DrawBoard();

        if (currentPlayer == white)
        {
            currentPlayer = black;
        }
        else
        {
            currentPlayer = white;
        }

        if(checkTile != null)
            Destroy(checkTile.gameObject);

        textState.text = board.GetGameState().ToString();
        textTurn.text = currentPlayer.GetColor().ToString();

        StartCoroutine(Play());
    }
}
