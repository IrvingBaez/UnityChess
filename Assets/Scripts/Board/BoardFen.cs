using System.Linq;

public partial class Board
{
    public Board(string fen)
    {
        int index = 0;

        //board
        while (fen[0] != ' ')
        {
            int leap;

            if (int.TryParse(fen[0].ToString(), out leap))
            {
                index += leap;
            }
            else
            {
                if (fen[0] != '/')
                {
                    SetOnPosition(index, fen[0]);
                    index++;
                }
            }

            fen = fen.Substring(1);
        }

        //turn
        fen = fen.Substring(1);
        Turn = fen[0] == 'w' ? 1 : -1;

        fen = fen.Substring(1);

        //castling rights
        fen = fen.Substring(1);
        while (fen[0] != ' ')
        {
            switch (fen[0])
            {
                case 'K':
                    whiteCastling[1] = true;
                    break;
                case 'Q':
                    whiteCastling[0] = true;
                    break;
                case 'k':
                    blackCastling[1] = true;
                    break;
                case 'q':
                    blackCastling[0] = true;
                    break;
            }
            fen = fen.Substring(1);
        }

        //En passant
        fen = fen.Substring(1);
        if (fen[0] != '-')
        {
            int.TryParse(fen[1].ToString(), out int col);
            enPassant = (long) 1 << (int)fen[0] - 97;
            fen = fen.Substring(1);
        }
        fen = fen.Substring(2);

        //Halfmove count
        if (fen[1] == ' ')
        {
            int.TryParse(fen[0].ToString(), out halfTurn);
            fen = fen.Substring(2);
        }
        else
        {
            int.TryParse(fen.Substring(1, 2), out halfTurn);
            fen = fen.Substring(3);
        }

        //Fullmove count
        int.TryParse(fen, out fullTurn);

        FindSight();
        FindLegalMoves();
        FindGameState();
    }

    public string Fen()
    {
        string fen = "";
        int freeSpaces = 0;

        for (int row = 0; row < 8; row++)
        {
            for(int col = 0; col < 8; col++)
            {
                if(GetOnPosition(col + row * 8) == null)
                {
                    freeSpaces++;
                }
                else
                {
                    if (freeSpaces > 0)
                    {
                        fen += freeSpaces;
                        freeSpaces = 0;
                    }
                    fen += GetOnPosition(col + row * 8);
                }
            }

            if (freeSpaces > 0)
            {
                fen += freeSpaces;
                freeSpaces = 0;
            }

            if(row < 7)
            {
                fen += "/";
            }
        }

        switch (Turn)
        {
            case 1:
                fen += " w ";
                break;
            case -1:
                fen += " b ";
                break;
        }

        bool castling = false;

        if (whiteCastling[1])
        {
            fen += "K";
            castling = true;
        }

        if (whiteCastling[0])
        {
            fen += "Q";
            castling = true;
        }

        if (blackCastling[1])
        {
            fen += "k";
            castling = true;
        }

        if (blackCastling[0])
        {
            fen += "q";
            castling = true;
        }

        if (!castling)
        {
            fen += "-";
        }

        if (enPassant != 0)
        {
            fen += $" {(char)(enPassant + 97)}{enPassant + 1} ";
        }
        else
        {
            fen += " - ";
        }

        fen += $"{halfTurn} {fullTurn}";

        return fen;
    }
}
