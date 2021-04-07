public class HumanPlayer : ChessPlayer
{
    public event System.Action pieceSelected;

    public MoveTile tile;
    private Position selected;

    public override void Move()
    {
        game.BoardView.PieceClicked += PieceClicked;        
    }

    private void PieceClicked()
    {
        Position clicked = game.BoardView.clicked;

        if (board.GetPieceColor(clicked) != color)
        {
            return;
        }

        this.selected = clicked;
        pieceSelected?.Invoke();

        foreach(Move move in board.LegalMoves[selected])
        {
            MoveTile newTile = Instantiate(tile);
            newTile.Initialize(this, move);
        }
        this.selected = null;
    }

    public void SetMove(Move move)
    {
        game.BoardView.PieceClicked -= PieceClicked;
        game.Move(move);
    }
}
