using System.Linq;
using System.Collections.Generic;

public partial class Board{
    long friends;
    long foes;
    long king;
    long enemySight;
    Dictionary<int, long> enemyPins;
    Dictionary<int, long> enemyChecks;

    private void FindSight(){
        sightWatch.Start();

        long whitePieces = whiteQueens & whiteBishops & whiteKnights & whiteRooks & whitePawns;
        long blackPieces = blackQueens & blackBishops & blackKnights & blackRooks & blackPawns;

        foes = Turn == 1 ? blackPieces & blackKing : whitePieces & whiteKing;
        friends = Turn == 1 ? whitePieces : blackPieces;
        king = Turn == 1 ? whiteKing : blackKing;
        enemyChecks = new Dictionary<int, long>();

        enemySight = KingSight(Turn == 1 ? blackKing : whiteKing);
        enemySight |= QueenSight(Turn == 1 ? blackQueens : whiteQueens);
        enemySight |= RookSight(Turn == 1 ? blackRooks : whiteRooks);
        enemySight |= BishopSight(Turn == 1 ? blackBishops : whiteBishops);
        enemySight |= KnightSight(Turn == 1 ? blackKnights : whiteKnights);
        enemySight |= PawnSight(Turn == 1 ? blackPawns : whitePawns);

        sightWatch.Stop();
    }

    // TODO: Optimize
    private long KingSight(long king){
        long sightMask = king << 9 | 
        king << 8 |
        king << 7 |
        king << 1 |
        king >> 1 |
        king >> 8 |
        king >> 7 |
        king >> 9;
        
        return sightMask & QueenSight(king);
    }

    private long QueenSight(long queens){
        long sight = 0;

        for(int pos = 0; pos < 64; pos++){
            if(IsBitSet(queens, pos)){
                sight |= SlidingSight(pos, new int[]{0, 1, 2, 3, 4, 5, 6, 7});
            }
        }

        return sight;
    }

    private long RookSight(long rooks){
        long sight = 0;

        for(int pos = 0; pos < 64; pos++){
            if(IsBitSet(rooks, pos)){
                sight |= SlidingSight(pos, new int[]{1, 3, 4, 6});
            }
        }

        return sight;
    }

    private long BishopSight(long bishops){
        long sight = 0;

        for(int pos = 0; pos < 64; pos++){
            if(IsBitSet(bishops, pos)){
                sight |= SlidingSight(pos, new int[]{0, 2, 5, 7});
            }
        }

        return sight;
    }

    private long KnightSight(long knights){
        long sight = 0;

        for(int pos = 0; pos < 64; pos++){
            if(IsBitSet(knights, pos)){
                int[] threatPositions = knightThreats[pos];
                for(int i = 0; i < threatPositions.Length; i++){
                    sight |= (long)1 << threatPositions[i];
                }
            }
        }

        return sight;
    }

    private long PawnSight(long pawns){
        long sight = 0;

        for(int pos = 0; pos < 64; pos++){
            if(IsBitSet(pawns, pos)){
                int[] threatPositions = new int[]{pos - Turn * 9, pos - Turn * 7};
                
                for(int i = 0; i < threatPositions.Length; i++){
                    if((pos / 8) - Turn == threatPositions[i] / 8)
                        sight |= (long)1 << threatPositions[i];
                }
            }
        }

        return sight;
    }

    private long SlidingSight(int position, int[] directionIndexes){
        long obstacles = (friends ^ position) | foes;
        long sight = 0;

        for(int index = 0; index < directionIndexes.Length; index++){
            int direction = directions[directionIndexes[index]];

            long sightLine = 0;
            bool kingFound = false;
            int obstaclesFound = 0;
            int pinnedPosition = 0;

            for(int step = 1; step <= squaresToEdge[directionIndexes[index], position]; step++){
                int checkingPos = position + step * direction;
                long checkingMask = (long)1 << checkingPos;

                if(obstaclesFound > 1)
                    break;
                
                if((sightLine & king) != 0 && !kingFound){
                    kingFound = true;
                    enemyChecks.Add(position, sightLine & ((long) 1 << position));

                    if(pinnedPosition != 0)
                        enemyPins.Add(pinnedPosition, sightLine & ((long) 1 << position));
                }

                if(obstaclesFound == 0)
                    sightLine |= checkingMask;

                if(IsBitSet(obstacles, checkingPos)){
                    obstaclesFound++;
                    
                    if(IsBitSet(foes, checkingPos))
                        pinnedPosition = checkingPos;
                }
            }
            
            sight |= sightLine;
        }

        return sight;
    }

    bool IsBitSet(long bitMask, int pos){
        return (bitMask & (1 << pos)) != 0;
    }
}
