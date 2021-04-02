using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BoardView : MonoBehaviour
{
    public event System.Action PieceClicked;
    public event System.Action DrawingBoard;

    private Board board;
    private bool flipped;

    [Header("Reference")]
    public PieceView pieceView;
    public ColorTile colorTile;

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

    [HideInInspector] public Board.Position clicked;

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public void DrawBoard()
    {
        DrawingBoard?.Invoke();

        for (int row = 0; row < 8; row++)
        {
           for(int col = 0; col < 8; col++)
            {
                char? piece = board.GetOnPosition(Board.Position.Create(col, row));
                if (piece == null)
                    continue;

                PieceView view = Instantiate(pieceView);
                view.transform.position = SolveWorldPosition(col, row);
                switch (piece)
                {
                    case 'K':
                        view.Initialize(whiteKingSprite, col, row, this);
                        break;
                    case 'Q':
                        view.Initialize(whiteQueenSprite, col, row, this);
                        break;
                    case 'R':
                        view.Initialize(whiteRookSprite, col, row, this);
                        break;
                    case 'B':
                        view.Initialize(whiteBishopSprite, col, row, this);
                        break;
                    case 'N':
                        view.Initialize(whiteKnightSprite, col, row, this);
                        break;
                    case 'P':
                        view.Initialize(whitePawnSprite, col, row, this);
                        break;
                    case 'k':
                        view.Initialize(blackKingSprite, col, row, this);
                        break;
                    case 'q':
                        view.Initialize(blackQueenSprite, col, row, this);
                        break;
                    case 'r':
                        view.Initialize(blackRookSprite, col, row, this);
                        break;
                    case 'b':
                        view.Initialize(blackBishopSprite, col, row, this);
                        break;
                    case 'n':
                        view.Initialize(blackKnightSprite, col, row, this);
                        break;
                    case 'p':
                        view.Initialize(blackPawnSprite, col, row, this);
                        break;
                }
            }
        }
    }

    public void ShowTile(Board.Position position, Color color)
    {
        ColorTile newTile = Instantiate(colorTile);
        newTile.Initialize(position, this, color);
    }

    public void ShowTiles(List<Board.Position> positions, Color color)
    {
        foreach(Board.Position position in positions)
        {
            ShowTile(position, color);
        }
    }

    public void ShowTiles(List<Board.Move> moves, Color color)
    {
        foreach (Board.Move move in moves)
        {
            ShowTile(move.destiny, color);
        }
    }

    public Vector3 SolveWorldPosition(Board.Position position)
    {
        return SolveWorldPosition(position.col, position.row);
    }

    public Vector3 SolveWorldPosition(int col, int row)
    {
        Vector3 deface = transform.parent.position;
        Vector3 size = this.GetComponent<Renderer>().bounds.size;

        float tileXSize = size.x / 8;
        float tileYSize = size.x / 8;

        Vector3 origin = deface;
        origin += (3.5f * tileXSize * Vector3.left);
        origin += (3.5f * tileYSize * Vector3.down);

        Vector3 position = origin;
        position += (7-row) * tileXSize * Vector3.up;
        position += (col) * tileYSize * Vector3.right;

        if (flipped)
        {
            position = 2 * deface - position;
        }

        return position + Vector3.back;
    }

    public void NotifyClick(int col, int row)
    {
        clicked = Board.Position.Create(col, row);
        PieceClicked?.Invoke();

        ShowTiles(board.BlackSight, new Color(1, 0, 0, 0.5f));
        //ShowTiles(board.whiteSight, new Color(1, 0, 1, 0.5f));
        ShowTile(clicked, new Color(1, 1, 0, 0.5f));
        ShowTiles(board.LegalMoves(clicked), new Color(0, 1, 0, 0.5f));
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
