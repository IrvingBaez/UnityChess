using System.Linq;
using System.Collections.Generic;

public partial class Board{
    Dictionary<Position, List<Position>> playerSight;
    Dictionary<Position, List<Position>> playerPins;
    Dictionary<Position, List<Position>> playerChecks;

    private void FindSight(){
        WhiteSight  = new Dictionary<Position, List<Position>>();
        BlackSight  = new Dictionary<Position, List<Position>>();
        WhitePins   = new Dictionary<Position, List<Position>>();
        BlackPins   = new Dictionary<Position, List<Position>>();
        WhiteChecks = new Dictionary<Position, List<Position>>();
        BlackChecks = new Dictionary<Position, List<Position>>();

        playerSight = WhiteSight;
        playerPins = WhitePins;
        playerChecks = WhiteChecks;
        foreach(Position position in WhitePieces){
            FindSight(position);
        }

        playerSight = BlackSight;
        playerPins = BlackPins;
        playerChecks = BlackChecks;
        foreach (Position position in BlackPieces){
            FindSight(position);
        }
    }

    private void FindSight(Position position){
        switch (GetOnPosition(position)){
            case 'K':
            case 'k':
                KingSight(position);
                break;
            case 'Q':
            case 'q':
                QueenSight(position);
                break;
            case 'R':
            case 'r':
                RookSight(position);
                break;
            case 'B':
            case 'b':
                BishopSight(position);
                break;
            case 'N':
            case 'n':
                KnightSight(position);
                break;
            case 'P':
            case 'p':
                PawnSight(position);
                break;
        }
    }

    private void KingSight(Position position){
        List<Position> kingSight = new List<Position>();

        for (int i = -1; i <= 1; i++){
            for (int j = -1; j <= 1; j++){
                if (j == 0 && i == 0)
                    continue;
                if (Position.Create(position.col + i, position.row + j, out Position checking))
                    kingSight.Add(checking);
            }
        }

        playerSight[position] = kingSight;
    }

    private void QueenSight(Position position){
        SlidingSight(position, new List<int[]>{
            new int[]{ 1, 1},
            new int[]{ 1,-1},
            new int[]{-1, 1},
            new int[]{-1,-1},
            new int[]{ 0,-1},
            new int[]{ 0, 1},
            new int[]{-1, 0},
            new int[]{ 1, 0}
        });
    }

    private void RookSight(Position position){
        SlidingSight(position, new List<int[]>{
            new int[]{ 0,-1},
            new int[]{ 0, 1},
            new int[]{-1, 0},
            new int[]{ 1, 0}
        });
    }

    private void BishopSight(Position position){
        SlidingSight(position, new List<int[]>{
            new int[]{ 1, 1},
            new int[]{ 1,-1},
            new int[]{-1, 1},
            new int[]{-1,-1}
        });
    }

    private void SlidingSight(Position position, List<int[]> directions){
        List<Position> sight = new List<Position>();
        
        foreach(int[] direction in directions){
            int step = 1;
            int foundPieces = 0;
            bool kingFound = false;
            Position pinned = null;
            List<Position> treath = new List<Position>{ position };
            
            while(Position.Create(position.col + step * direction[0],
                                  position.row + step * direction[1], 
                                  out Position checking)){
                
                if(foundPieces == 0 || (foundPieces == 1 && kingFound)){
                    sight.Add(checking);
                }

                if(foundPieces == 2 || (foundPieces == 1 && kingFound)){
                    break;
                }

                treath.Add(checking);

                char? foundPiece = GetOnPosition(checking);
                if(foundPiece != null){
                    if(char.IsUpper(foundPiece.Value) == char.IsUpper(GetOnPosition(position).Value)){
                        break;
                    }
                    
                    foundPieces++;

                    if(IsEnemyKing(position, checking)){
                        kingFound = true;
                    }else{
                        pinned = checking;
                    }
                }

                step++;
            }

            if(kingFound){
                if(foundPieces == 1){
                    playerChecks[position] = treath;
                }else{
                    playerPins[pinned] = treath;
                }
            }
        }

        playerSight[position] = sight;
    }

    private void KnightSight(Position position){
        List<Position> knightSight = new List<Position>();
        Position checking;

        int[] skips = new int[] { -2, -1, 1, 2 };
        
        foreach (int i in skips){
            foreach(int j in skips){
                if((i + j) % 2 != 0){
                    if (Position.Create(position.col + i, position.row + j, out checking)){
                        knightSight.Add(checking);
                        
                        if(IsEnemyKing(position, checking)){
                            playerChecks[position] = new List<Position>{ position };
                        }
                    }
                }
            }
        }

        playerSight[position] = knightSight;
    }

    private void PawnSight(Position position){
        List<Position> pawnSight = new List<Position>();
        Position checking;

        int color = -GetPieceColor(position);
        
        if (Position.Create(position.col - 1, position.row + color, out checking)){
            pawnSight.Add(checking);
            
            if(IsEnemyKing(position, checking)){
                playerChecks[position] = new List<Position>{ position };
            }
        }

        if (Position.Create(position.col + 1, position.row + color, out checking)){
            pawnSight.Add(checking);

            if(IsEnemyKing(position, checking)){
                playerChecks[position] = new List<Position>{ position };
            }
        }

        playerSight[position] = pawnSight;
    }

    internal bool IsEnemyKing(Position piece, Position king){
        char? kingChar = GetOnPosition(king);
        if(kingChar == null || char.ToUpper(kingChar.Value) != 'K'){
            return false;
        }

        return char.IsUpper(GetOnPosition(piece).Value) ^ char.IsUpper(kingChar.Value);
    }
}
