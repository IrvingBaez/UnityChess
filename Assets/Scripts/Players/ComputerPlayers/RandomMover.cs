using UnityEngine;

public class RandomMover : ChessPlayer
{
    public void Start()
    {
        Tree<string, int> tree = new Tree<string, int>("1", 0);
        tree.GetRoot().AddChild("1-1", 0);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-1", 0, 1);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-2", 0, 2);
        tree.GetRoot().GetChildren()[0].AddChild("1-1-3", 0, 3);
        tree.GetRoot().AddChild("1-2", 0);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-1", 0, 2);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-2", 0, 3);
        tree.GetRoot().GetChildren()[1].AddChild("1-2-3", 0, 5);

        print(tree);

        MinimaxProcessor<string, int> processor = 
            new MinimaxProcessor<string, int>(MinimaxProcessor<string, int>.Mode.MIN, tree);
        processor.Process();

        print(tree);
    }

    public override void Move()
    {
        ChessPiece piece = null;
        Move move = null;

        switch (this.color)
        {
            case ChessPiece.Color.WHITE:
                do
                {
                    piece = board.GetWhitePieces()[Random.Range(0, board.GetWhitePieces().Count)];
                }
                while (piece == null || piece.GetMoves().Count == 0);

                move = piece.GetMoves()[Random.Range(0, piece.GetMoves().Count)];
                break;
            case ChessPiece.Color.BLACK:
                do
                {
                    piece = board.GetBlackPieces()[Random.Range(0, board.GetBlackPieces().Count)];
                }
                while (piece == null || piece.GetMoves().Count == 0);

                move = piece.GetMoves()[Random.Range(0, piece.GetMoves().Count)];
                break;
        }

        this.game.Move(move);
    }
}
