using System.Collections.Generic;
using System.Linq;
public partial class Board
{
    List<Position> enemyPieces;
    Dictionary<Position, List<Position>> enemyPins;
    Dictionary<Position, List<Position>> enemyChecks;
    List<Position> playerPieces;

    public void FindLegalMoves(){
        LegalMoves  = new Dictionary<Position, List<Move>>();

        playerSight     = Turn == 1 ? WhiteSight    : BlackSight;
        playerPieces    = Turn == 1 ? WhitePieces   : BlackPieces;
        enemyPieces     = Turn == 1 ? BlackPieces   : WhitePieces;
        enemyPins       = Turn == 1 ? BlackPins     : WhitePins;
        enemyChecks     = Turn == 1 ? BlackChecks   : WhiteChecks;
        
        foreach(Position position in playerPieces){
            FindLegalMoves(position);
        }
    }

    public void FindLegalMoves(Position position)
    {
        if(GetPieceColor(position) != Turn){
            return;
        }

        switch (GetOnPosition(position)){
            case 'K':
            case 'k':
                KingMoves(position);
                break;
            case 'Q':
            case 'q':
            case 'R':
            case 'r':
            case 'B':
            case 'b':
            case 'N':
            case 'n':
                PieceMoves(position);
                break;
            case 'P':
            case 'p':
                PawnMoves(position);
                break;
        }
    }

    private void KingMoves(Position position)
    {
        List<Move> kingMoves = new List<Move>();

        List<Position> opponentSight = FlatDict(Turn == 1 ? BlackSight : WhiteSight);
        bool[] castlig = Turn == 1 ? whiteCastling : blackCastling;

        foreach (Position checking in playerSight[position]){
            if (!opponentSight.Contains(checking) && !playerPieces.Contains(checking)){
                kingMoves.Add(new Move(position, checking));
            }
        }

        if (castlig[0] &&
            !playerPieces.Contains(Position.Create(position.col - 1, position.row)) &&
            !playerPieces.Contains(Position.Create(position.col - 2, position.row)) &&
            !playerPieces.Contains(Position.Create(position.col - 3, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col - 1, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col - 2, position.row))
            ){
            kingMoves.Add(new Move(position, Position.Create(position.col - 2, position.row)));
        }

        if (castlig[1] &&
            !playerPieces.Contains(Position.Create(position.col + 1, position.row)) &&
            !playerPieces.Contains(Position.Create(position.col + 2, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col + 1, position.row)) &&
            !opponentSight.Contains(Position.Create(position.col + 2, position.row))
            ){
            kingMoves.Add(new Move(position, Position.Create(position.col + 2, position.row)));
        }

        LegalMoves[position] = kingMoves;
    }

    private void PieceMoves(Position position){
        if(enemyChecks.Count == 2){
            LegalMoves[position] = new List<Move>();
            return;
        }
        
        List<Position> destinies = playerSight[position].Except(playerPieces).ToList();

        if(enemyPins.TryGetValue(position, out List<Position> pin)){
            destinies = pin.Intersect(destinies).ToList();
        }

        if(enemyChecks.Count == 1){
            destinies = enemyChecks.Values.SelectMany(x => x).ToList().Intersect(destinies).ToList();
        }
        
        List<Move> queenMoves = new List<Move>();
        foreach(Position destiny in destinies){
            queenMoves.Add(new Move(position, destiny));
        }
        LegalMoves[position] = queenMoves;
    }

    private void PawnMoves(Position position)
    {
        if(enemyChecks.Count == 2){
            LegalMoves[position] = new List<Move>();
            return;
        }

        //Captures and en passant
        List<Position> destinies = playerSight[position].Intersect(enemyPieces.Union(new List<Position>{ enPassant })).ToList();

        //Steps and double steps
        int step = -GetPieceColor(position);
        Position forward = Position.Create(position.col, position.row + step);
        
        if(GetOnPosition(forward) == null){
            destinies.Add(forward);
            if (position.row == (1 - step) * 2.5 + 1){
                forward = Position.Create(position.col, position.row + 2 * step);
                if (GetOnPosition(forward) == null){
                    destinies.Add(forward);
                }
            }
        }

        if(enemyPins.TryGetValue(position, out List<Position> pin)){
            destinies = pin.Intersect(destinies).ToList();
        }

        if(enemyChecks.Count == 1){
            destinies = enemyChecks.Values.SelectMany(x => x).ToList().Intersect(destinies).ToList();
        }

        List<Move> pawnMoves = new List<Move>();
        foreach(Position destiny in destinies){
            switch(destiny.row){
                case 0:
                    pawnMoves.Add(new Move(position, destiny, 'Q'));
                    pawnMoves.Add(new Move(position, destiny, 'R'));
                    pawnMoves.Add(new Move(position, destiny, 'B'));
                    pawnMoves.Add(new Move(position, destiny, 'N'));
                    break;
                case 7:
                    pawnMoves.Add(new Move(position, destiny, 'q'));
                    pawnMoves.Add(new Move(position, destiny, 'r'));
                    pawnMoves.Add(new Move(position, destiny, 'b'));
                    pawnMoves.Add(new Move(position, destiny, 'n'));
                    break;
                default:
                    pawnMoves.Add(new Move(position, destiny));
                    break;
            }
        }
        LegalMoves[position] = pawnMoves;
    }
}
