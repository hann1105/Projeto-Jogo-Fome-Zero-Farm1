using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GerenciadorDeBlocos : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap; // Referência ao Tilemap

    [SerializeField] private Tile hiddenInteractableTile; // Referência ao Tilemap invisível
    void Start()
    {
        foreach(var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            interactableMap.SetTile(pos, hiddenInteractableTile);

        }
    }
    public bool isInteractable(Vector3Int pos)
    {
        TileBase tile = interactableMap.GetTile(pos);
        if (tile != null && tile.name == "Interactable")
        {
            return true;
        }
        return false;
    }

}
