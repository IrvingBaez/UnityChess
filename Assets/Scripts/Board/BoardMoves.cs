using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
public partial class Board
{
    //List<Position> enemyPieces;
    //Dictionary<Position, List<Position>> enemyPins;
    //Dictionary<Position, List<Position>> enemyChecks;
    //List<Position> playerPieces;

    long playerSight;
    long playerPieces;
    long enemyPieces;

    public Dictionary<int, List<Move>> legalMoves;

    public void FindLegalMoves(){
        moveWatch.Start();
        legalMoves = new Dictionary<int, List<Move>>();

        long pawns      = Turn == 1 ? whitePawns    : blackPawns;
        long knights    = Turn == 1 ? whiteKnights  : blackKnights;
        long bishops    = Turn == 1 ? whiteBishops  : blackBishops;
        long rooks      = Turn == 1 ? whiteRooks    : blackRooks;
        long queens      = Turn == 1 ? whiteQueens   : blackQueens;
        long king       = Turn == 1 ? whiteKing     : blackKing;

        for(int position = 0; position < 64; position ++){
            if(IsBitSet(pawns, position)){
                legalMoves.Add(position, LongToMoveList(position, PawnMoves(position)));
            }

            if(IsBitSet(king, position)){
                legalMoves.Add(position, LongToMoveList(position, KingMoves(position)));
            }

            if(IsBitSet(bishops, position)){
                legalMoves.Add(position, LongToMoveList(position, PieceMoves(position, BishopSight(position))));
            }

            if(IsBitSet(rooks, position)){
                legalMoves.Add(position, LongToMoveList(position, PieceMoves(position, RookSight(position))));
            }

            if(IsBitSet(queens, position)){
                legalMoves.Add(position, LongToMoveList(position, PieceMoves(position, QueenSight(position))));
            }

            if(IsBitSet(knights, position)){
                legalMoves.Add(position, LongToMoveList(position, PieceMoves(position, KnightSight(position))));
            }
        }

        moveWatch.Stop();
    }

    private List<Move> LongToMoveList(int origin, long destinies){
        List<Move> moves = new List<Move>();

        int color = GetPieceColor(origin);
        bool isPromotion = IsBitSet((Turn == 1 ? whitePawns : blackPawns), origin) && origin / 8 == 3.5 + 3.5 * color;
        char[] promotions = color == 1 ? new char[]{'Q', 'R', 'B', 'N'} : new char[]{'q', 'r', 'b', 'n'};


        for(int destiny = 0; destiny < 64; destiny++){
            if(IsBitSet(destinies, destiny)){
                if(isPromotion){
                    foreach(char promotion in promotions){
                        moves.Add(new Move(origin, destiny, promotion));
                    }
                }else{
                    moves.Add(new Move(origin, destiny));
                }
            }
        }

        return moves;
    }

    // TODO: add castling.
    private long KingMoves(long king)
    {
        kingMoveWatch.Start();
        long moveMask = KingSight(king);
        moveMask ^= moveMask & enemySight;

        kingMoveWatch.Stop();
        return moveMask;
    }

    private long PieceMoves(int position, long sight){
        pieceMoveWatch.Start();

        if(enemyChecks.Count > 1)
            return 0;
        
        long destinies = sight;
        if(enemyPins.TryGetValue(position, out long pin)){
            destinies &= pin;
        }

        if(enemyChecks.Count == 1){
            destinies &= enemyChecks.FirstOrDefault().Value;
        }

        pieceMoveWatch.Stop();
        return destinies;
    }

    private long PawnMoves(int position)
    {
        pawnMoveWatch.Start();

        if(enemyChecks.Count > 1)
            return 0;

        int color = GetPieceColor(position);

        // Captures.
        // TODO: enpassant.
        long destinies = PawnSight(position);
        if(GetOnPosition(position + 9 * color) == null)
            destinies ^= (long) 1 << position + 9 * color;

        if(GetOnPosition(position + 7 * color) == null)
            destinies ^= (long) 1 << position + 7 * color;

        //Steps and double steps
        int forward = position + 8 * color;
        if(GetOnPosition(forward) == null){
            destinies |= (long) 1 << forward;

            if (position / 8 == 3.5 - 2.5 * color){
                forward += 8 * color;
                if (GetOnPosition(forward) == null){
                    destinies |= (long) 1 << forward;
                }
            }
        }

        if(enemyPins.TryGetValue(position, out long pin)){
            destinies &= pin;
        }

        if(enemyChecks.Count == 1){
            destinies &= enemyChecks.FirstOrDefault().Value;
        }

        pawnMoveWatch.Stop();
        return destinies;
    }
}
