using System;
using UnityEngine;

public class Game : MonoBehaviour
{
    public event Action BoardChanged;

    public Board boardRef;
    public BoardView boardView;
    [SerializeField] private CheckTile checkTileRef;

    public ChessPlayer white;
    public ChessPlayer black;
    private ChessPlayer currentPlayer;

    private Board board;
    private King whiteKing;
    private King blackKing;
    private GameState gameState = GameState.ALIVE;
    private CheckTile checkTile;

    public enum GameState { ALIVE, WHITE_CHECK, WHITE_MATE, BLACK_CHECK, BLACK_MATE, STALE_MATE, INVALID };

    void Start()
    {
        white.SetColor(ChessPiece.Color.WHITE);
        black.SetColor(ChessPiece.Color.BLACK);

        white.PlayerMoved += OnPlayerMoved;
        black.PlayerMoved += OnPlayerMoved;

        currentPlayer = white;

        NewGame();
        Play();
    }

    private void NewGame()
    {
        this.board = Instantiate(boardRef);
        this.board.Initialize();
        this.whiteKing = board.getWhitePieces()[0] as King;
        this.blackKing = board.getBlackPieces()[0] as King;
        boardView.setBoard(this.board);
    }

    private void Play()
    {
        if(gameState == GameState.ALIVE || gameState == GameState.BLACK_CHECK || gameState == GameState.WHITE_CHECK)
        {
            currentPlayer.Move();
        }
    }

    private void OnPlayerMoved()
    {
        if (currentPlayer == white)
        {
            currentPlayer = black;
        }
        else
        {
            currentPlayer = white;
        }

        SetGameState();
        Play();
    }
    

    public void Move(ChessPiece piece, ChessPiece.Position destiny)
    {
        if(gameState == GameState.BLACK_CHECK || gameState == GameState.WHITE_CHECK)
        {
            Destroy(checkTile.gameObject);
        }

        board.Move(piece, destiny);

        BoardChanged?.Invoke();

        SetGameState();
    }

    private void SetGameState()
    {
        if (whiteKing.IsInCheck())
        {
            OnCheck(whiteKing);
            gameState = GameState.BLACK_CHECK;
            return;
        }
        if (blackKing.IsInCheck())
        {
            OnCheck(blackKing);
            gameState = GameState.WHITE_CHECK;
            return;
        }

        gameState = GameState.ALIVE;
    }

    private void OnCheck(King victim)
    {
        if (checkTile != null)
        {
            Destroy(checkTile.gameObject);
        }

        checkTile = Instantiate(checkTileRef);
        checkTile.Initialize(victim.GetPosition(), boardView);

        switch (victim.GetColor())
        {
            case ChessPiece.Color.WHITE:
                gameState = GameState.BLACK_CHECK;
                break;
            case ChessPiece.Color.BLACK:
                gameState = GameState.WHITE_CHECK;
                break;
        }
    }

    public ChessPiece.Position IndexToPosition(int col, int row)
    {
        if (row < 1 || row > 8 || col < 0 || col > 7)
        {
            return null;
        }

        try
        {
            return new ChessPiece.Position((ChessPiece.Column)col, row);
        }
        catch (System.Exception)
        {
            return null;
        }
    }
}
