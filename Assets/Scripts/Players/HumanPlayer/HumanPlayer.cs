public class HumanPlayer : ChessPlayer
{
    public event System.Action pieceSelected;

    public MoveTile tile;
    private Board.Position selected;

    public override void Move()
    {
        game.BoardView.PieceClicked += PieceClicked;        
    }

    private void PieceClicked()
    {
        Board.Position clicked = game.BoardView.clicked;

        if (board.GetPieceColor(clicked) != color)
        {
            return;
        }

        this.selected = clicked;
        pieceSelected?.Invoke();

        foreach(Board.Move move in board.LegalMoves(selected))
        {
            MoveTile newTile = Instantiate(tile);
            newTile.Initialize(this, move);
        }
        this.selected = null;
    }

    public void SetMove(Board.Move move)
    {
        game.BoardView.PieceClicked -= PieceClicked;
        game.Move(move);
    }
}
