using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    private static event System.Action TileClicked;
    
    private HumanPlayer player;
    private Board.Position position;
    private Board.Move move;

    public void Initialize(HumanPlayer player, Board.Move move)
    {
        TileClicked += DestroyTiles;
        player.pieceSelected += DestroyTiles;

        this.player = player;
        this.move = move;
        position = move.destiny;

        transform.position = player.game.BoardView.SolveWorldPosition(position);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void OnMouseDown()
    {
        player.SetMove(move);
        TileClicked?.Invoke();
    }

    private void DestroyTiles()
    {
        TileClicked -= DestroyTiles;
        player.pieceSelected -= DestroyTiles;
        Destroy(gameObject);
    }
}
