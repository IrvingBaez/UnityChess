using UnityEngine;
using System.Collections;

public class CheckTile : MonoBehaviour
{
    public void Initialize(Position position, BoardView board)
    {
        gameObject.transform.position = board.SolveWorldPosition(position);
        gameObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }
}
