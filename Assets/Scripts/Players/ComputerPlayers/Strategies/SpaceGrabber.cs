public class SpaceGrabber : Strategy
{
    private void Start()
    {
        this.name = "SpaceGrabber";
    }

    public override float Evaluate(Board board)
    {
        this.board = board;

        return FilterCheckMate(Space());
    }
}
