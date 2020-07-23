using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTile : MonoBehaviour
{
    private static event System.Action TileClicked;
    
    private Game controller;
    private HumanPlayer player;
    private ChessPiece.Position position;

    public void Initialize(HumanPlayer player, ChessPiece.Position position)
    {
        TileClicked += DestroyTiles;
        player.pieceSelected += DestroyTiles;

        this.controller = GameObject.FindGameObjectWithTag("GameController").GetComponent<Game>();
        this.player = player;
        this.position = position;

        transform.position = controller.board.SolveWorldPosition(position);
        transform.position = new Vector3(transform.position.x, transform.position.y, -1);
    }

    public void OnMouseDown()
    {
        this.player.SetDestiny(position);
        TileClicked?.Invoke();
    }

    private void DestroyTiles()
    {
        TileClicked -= DestroyTiles;
        player.pieceSelected -= DestroyTiles;
        Destroy(gameObject);
    }
}
