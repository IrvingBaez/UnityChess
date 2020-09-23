public class HumanPlayer : ChessPlayer
{
    public event System.Action pieceSelected;

    public MoveTile tile;
    private ChessPiece selected;

    public override void Move()
    {
        game.boardView.PieceClicked += PieceClicked;        
    }

    private void PieceClicked()
    {
        ChessPiece clicked = game.boardView.clicked;

        if (clicked.GetColor() != this.color)
        {
            return;
        }

        this.selected = clicked;
        pieceSelected?.Invoke();

        foreach(Move move in this.selected.GetMoves())
        {
            MoveTile newTile = Instantiate(tile);
            newTile.Initialize(this, move);
        }
        this.selected = null;
    }

    public void SetMove(Move move)
    {
        game.boardView.PieceClicked -= PieceClicked;
        game.Move(move);
    }
}
