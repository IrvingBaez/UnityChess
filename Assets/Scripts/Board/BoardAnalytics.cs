using System.Diagnostics;

public partial class Board
{
    private Stopwatch sightWatch = new Stopwatch();
    private Stopwatch moveWatch = new Stopwatch();
    private Stopwatch kingMoveWatch = new Stopwatch();
    private Stopwatch pieceMoveWatch = new Stopwatch();
    private Stopwatch pawnMoveWatch = new Stopwatch();
    private Stopwatch performeMoveWatch = new Stopwatch();
    private Stopwatch undoMoveWatch = new Stopwatch();
    private Stopwatch totalWatch = new Stopwatch();
    public void StartTimers(){
        sightWatch.Reset();
        moveWatch.Reset();
        kingMoveWatch.Reset();
        pieceMoveWatch.Reset();
        pawnMoveWatch.Reset();
        performeMoveWatch.Reset();
        undoMoveWatch.Reset();
        totalWatch.Start();
    }

    public void CheckTimers(){
        sightWatch.Stop();
        moveWatch.Stop();
        performeMoveWatch.Stop();
        undoMoveWatch.Stop();
        totalWatch.Stop();

        UnityEngine.Debug.Log($"{totalWatch.ElapsedMilliseconds} ms total");
        UnityEngine.Debug.Log($"{sightWatch.ElapsedMilliseconds} ms sighting");
        UnityEngine.Debug.Log($"{moveWatch.ElapsedMilliseconds} ms checking moves");
        UnityEngine.Debug.Log($"{kingMoveWatch.ElapsedMilliseconds} ms checking king moves");
        UnityEngine.Debug.Log($"{pieceMoveWatch.ElapsedMilliseconds} ms checking piece moves");
        UnityEngine.Debug.Log($"{pawnMoveWatch.ElapsedMilliseconds} ms checking pawn moves");
        UnityEngine.Debug.Log($"{performeMoveWatch.ElapsedMilliseconds} ms moving");
        UnityEngine.Debug.Log($"{undoMoveWatch.ElapsedMilliseconds} ms unmoving");
    }
}
