using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    private static event System.Action TileClicked;
    
    private Game controller;
    private HumanPlayer player;
    private Position position;
    private Move move;

    public void Initialize(HumanPlayer player, Move move)
    {
        TileClicked += DestroyTiles;
        player.pieceSelected += DestroyTiles;

        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        this.player = player;
        this.move = move;
        position = move.destiny;

        transform.position = controller.boardView.SolveWorldPosition(position);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void OnMouseDown()
    {
        this.player.SetMove(move);
        TileClicked?.Invoke();
    }

    private void DestroyTiles()
    {
        TileClicked -= DestroyTiles;
        player.pieceSelected -= DestroyTiles;
        Destroy(gameObject);
    }
}
