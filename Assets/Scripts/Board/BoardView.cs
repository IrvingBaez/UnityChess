using UnityEngine;

public class BoardView : MonoBehaviour
{
    public event System.Action PieceClicked;
    public event System.Action DrawingBoard;

    private Board board;
    private bool flipped;

    [Header("Reference")]
    public ChessPieceView pieceView;

    [Header("White Sprites")]
    public Sprite whiteKingSprite;
    public Sprite whiteQueenSprite;
    public Sprite whiteRookSprite;
    public Sprite whiteBishopSprite;
    public Sprite whiteKnightSprite;
    public Sprite whitePawnSprite;
    
    [Header("Black Sprites")]
    public Sprite blackKingSprite;
    public Sprite blackQueenSprite;
    public Sprite blackRookSprite;
    public Sprite blackBishopSprite;
    public Sprite blackKnightSprite;
    public Sprite blackPawnSprite;

    [HideInInspector] public ChessPiece clicked;

    public void SetBoard(Board board)
    {
        this.board = board;
        board.OnBoardChanged += drawBoard;
        drawBoard();
    }

    private void drawBoard()
    {
        DrawingBoard?.Invoke();
        foreach (ChessPiece piece in board.GetWhitePieces())
        {
            ChessPieceView view = Instantiate(pieceView);
            view.transform.position = SolveWorldPosition(piece.GetPosition());

            switch (piece.GetSymbol())
            {
                case ChessPiece.Symbol.K:
                    view.Initialize(whiteKingSprite, piece, this);
                    break;
                case ChessPiece.Symbol.Q:
                    view.Initialize(whiteQueenSprite, piece, this);
                    break;
                case ChessPiece.Symbol.R:
                    view.Initialize(whiteRookSprite, piece, this);
                    break;
                case ChessPiece.Symbol.B:
                    view.Initialize(whiteBishopSprite, piece, this);
                    break;
                case ChessPiece.Symbol.N:
                    view.Initialize(whiteKnightSprite, piece, this);
                    break;
                case ChessPiece.Symbol.P:
                    view.Initialize(whitePawnSprite, piece, this);
                    break;
            }
        }

        foreach (ChessPiece piece in board.GetBlackPieces())
        {
            ChessPieceView view = Instantiate(pieceView);
            view.transform.position = SolveWorldPosition(piece.GetPosition());

            switch (piece.GetSymbol())
            {
                case ChessPiece.Symbol.K:
                    view.Initialize(blackKingSprite, piece, this);
                    break;
                case ChessPiece.Symbol.Q:
                    view.Initialize(blackQueenSprite, piece, this);
                    break;
                case ChessPiece.Symbol.R:
                    view.Initialize(blackRookSprite, piece, this);
                    break;
                case ChessPiece.Symbol.B:
                    view.Initialize(blackBishopSprite, piece, this);
                    break;
                case ChessPiece.Symbol.N:
                    view.Initialize(blackKnightSprite, piece, this);
                    break;
                case ChessPiece.Symbol.P:
                    view.Initialize(blackPawnSprite, piece, this);
                    break;
            }
        }
    }

    public Vector3 SolveWorldPosition(Position boardPosition)
    {
        Vector3 deface = transform.parent.position;
        Vector3 size = this.GetComponent<Renderer>().bounds.size;

        float tileXSize = size.x / 8;
        float tileYSize = size.x / 8;

        Vector3 origin = deface;
        origin += (3.5f * tileXSize * Vector3.left);
        origin += (3.5f * tileYSize * Vector3.down);

        Vector3 position = origin;
        position += (boardPosition.row - 1) * tileXSize * Vector3.up;
        position += ((int)boardPosition.col) * tileYSize * Vector3.right;

        if (flipped)
        {
            position = 2 * deface - position;
        }

        return position + Vector3.back;
    }

    public void NotifyClick(ChessPiece piece)
    {
        clicked = piece;
        PieceClicked?.Invoke();
    }

    public void flip()
    {
        this.flipped = !flipped;
    }

    public Board getBoard()
    {
        return board;
    }
}
