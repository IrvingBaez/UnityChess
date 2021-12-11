public class HumanPlayer : ChessPlayer
{
    public event System.Action pieceSelected;

    public MoveTile tile;
    private int? selected;

    public override void Move()
    {
        game.BoardView.PieceClicked += PieceClicked;        
    }

    private void PieceClicked()
    {
        int clicked = game.BoardView.clicked;

        if (board.GetPieceColor(clicked) != color)
        {
            return;
        }

        this.selected = clicked;
        pieceSelected?.Invoke();

        if(selected != null){
            foreach(Move move in board.legalMoves[selected.Value])
            {
                MoveTile newTile = Instantiate(tile);
                newTile.Initialize(this, move);
            }
        }
        
        this.selected = null;
    }

    public void SetMove(Move move)
    {
        game.BoardView.PieceClicked -= PieceClicked;
        game.Move(move);
    }
}
