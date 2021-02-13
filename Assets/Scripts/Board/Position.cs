using System;

public class Position
{
    public readonly Board.Column col;
    public readonly int row;

    public Position(Board.Column col, int row)
    {
        this.col = col;
        this.row = row;
    }

    public static Position StringToPosition(string position)
    {
        if(position.Length > 2)
        {
            return null;
        }
        
        int row;
        if(int.TryParse(position[1].ToString(), out row))
        {
            switch (position[0])
            {
                case 'a':
                    return new Position(Board.Column.A, row);
                case 'b':
                    return new Position(Board.Column.B, row);
                case 'c':
                    return new Position(Board.Column.C, row);
                case 'd':
                    return new Position(Board.Column.D, row);
                case 'e':
                    return new Position(Board.Column.E, row);
                case 'f':
                    return new Position(Board.Column.F, row);
                case 'g':
                    return new Position(Board.Column.G, row);
                case 'h':
                    return new Position(Board.Column.H, row);
            }
        }

        return null;
    }

    public static Position IndexToPosition(int col, int row)
    {
        if (row < 1 || row > 8 || col < 0 || col > 7)
        {
            return null;
        }

        try
        {
            return new Position((Board.Column)col, row);
        }
        catch (Exception)
        {
            return null;
        }
    }

    public static float SquareDistance(Position pos1, Position pos2)
    {
        float cols = (int)pos1.col - (int)pos2.col;
        float rows = pos1.row - pos2.row;

        return cols * cols + rows * rows;
    }

    public override string ToString()
    {
        return col + row.ToString();
    }

    public override bool Equals(object obj)
    {
        Position other = obj as Position;

        return other.col == this.col && other.row == this.row;
    }
}