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
    private GameState gameState = GameState.ALIVE;
    private CheckTile checkTile;

    public enum GameState { ALIVE, WHITE_CHECK, WHITE_MATE, BLACK_CHECK, BLACK_MATE, STALE_MATE, INVALID };

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

        Play();
    }

    private void Play()
    {
        print(board.Fen());
        if (gameState == GameState.ALIVE || gameState == GameState.BLACK_CHECK || gameState == GameState.WHITE_CHECK)
        {
            currentPlayer.Move();
        }
    }

    public void Move(Move move)
    {
        board.PerformMove(move);

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
        
        SetGameState();

        textState.text = gameState.ToString();
        textTurn.text = currentPlayer.GetColor().ToString();

        Play();
    }

    private void SetGameState()
    {
        King king = null;

        switch (currentPlayer.GetColor())
        {
            case ChessPiece.Color.WHITE:
                king = board.GetWhiteKing();
                break;
            case ChessPiece.Color.BLACK:
                king = board.GetBlackKing();
                break;
        }
        
        gameState = king.GetGameState();
        
        if(gameState == GameState.WHITE_CHECK || gameState == GameState.BLACK_CHECK)
        {
            OnCheck(king);
        }
    }

    private void OnCheck(King victim)
    {
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
}
