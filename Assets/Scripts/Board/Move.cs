public class Move
{
    public ChessPiece piece;
    public ChessPiece promoteTo;
    public Position destiny;
    public bool isCastling;
    public string notation;

    public Move(ChessPiece piece, Position destiny, ChessPiece promoteTo = null, bool isCastling = false)
    {
        this.piece = piece;
        this.destiny = destiny;
        this.promoteTo = promoteTo;
        this.isCastling = isCastling;
        notation = $"{piece.GetSymbol().ToString().ToLower()}{destiny}";
    }

    public override string ToString()
    {
        return $"{notation}: Move {piece} to {destiny}, promoting to {promoteTo}, is castling? {isCastling}";
    }
}
