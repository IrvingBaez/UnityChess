using UnityEngine;
using System.Linq;
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

    [HideInInspector] public int clicked;

    public void SetBoard(Board board)
    {
        this.board = board;
    }

    public void DrawBoard()
    {
        DrawingBoard?.Invoke();

        for(int position = 63; position >= 0; position--){
            char? piece = board.GetOnPosition(position);
            if (piece == null)
                continue;

            PieceView view = Instantiate(pieceView);
            view.transform.position = SolveWorldPosition(position);
            switch (piece)
            {
                case 'K':
                    view.Initialize(whiteKingSprite, position, this);
                    break;
                case 'Q':
                    view.Initialize(whiteQueenSprite, position, this);
                    break;
                case 'R':
                    view.Initialize(whiteRookSprite, position, this);
                    break;
                case 'B':
                    view.Initialize(whiteBishopSprite, position, this);
                    break;
                case 'N':
                    view.Initialize(whiteKnightSprite, position, this);
                    break;
                case 'P':
                    view.Initialize(whitePawnSprite, position, this);
                    break;
                case 'k':
                    view.Initialize(blackKingSprite, position, this);
                    break;
                case 'q':
                    view.Initialize(blackQueenSprite, position, this);
                    break;
                case 'r':
                    view.Initialize(blackRookSprite, position, this);
                    break;
                case 'b':
                    view.Initialize(blackBishopSprite, position, this);
                    break;
                case 'n':
                    view.Initialize(blackKnightSprite, position, this);
                    break;
                case 'p':
                    view.Initialize(blackPawnSprite, position, this);
                    break;
            }
        }
    }

    public void ShowTile(int position, Color color)
    {
        ColorTile newTile = Instantiate(colorTile);
        newTile.Initialize(position, this, color);
    }

    public void ShowTiles(List<int> positions, Color color)
    {
        foreach(int position in positions)
        {
            ShowTile(position, color);
        }
    }

    public void ShowTiles(List<Move> moves, Color color)
    {
        foreach (Move move in moves)
        {
            ShowTile(move.destiny, color);
        }
    }

    public Vector3 SolveWorldPosition(int position)
    {
        return SolveWorldPosition(7 - (position % 8), (int)position / 8);
    }

    private Vector3 SolveWorldPosition(int col, int row)
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

    public void NotifyClick(int position)
    {
        clicked = position;
        PieceClicked?.Invoke();

        if(board.legalMoves.TryGetValue(clicked, out List<Move> moves)){
            ShowTiles(moves, new Color(0, 1, 0, 0.5f));
        }
        
        //ShowTiles(board.enemySight.Values.SelectMany(x => x).ToList(), new Color(1, 0, 0, 0.5f));
        //ShowTiles(board.whiteSight, new Color(1, 0, 1, 0.5f));
        ShowTile(clicked, new Color(1, 1, 0, 0.5f));
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
