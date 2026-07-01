using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;

public class GerenciadorDeBlocos : MonoBehaviour
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;

    private readonly HashSet<Vector3Int> interactablePositions = new HashSet<Vector3Int>();

    private void Awake()
    {
        if (interactableMap == null)
        {
            return;
        }

        TilemapRenderer tilemapRenderer = interactableMap.GetComponent<TilemapRenderer>();
        if (tilemapRenderer != null)
        {
            tilemapRenderer.enabled = false;
        }

        foreach (var pos in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(pos);

            if (tile != null && (hiddenInteractableTile == null || tile == hiddenInteractableTile))
            {
                interactablePositions.Add(pos);
                interactableMap.SetTile(pos, null);
            }
        }
    }

    public bool isInteractable(Vector3Int pos)
    {
        return interactablePositions.Contains(pos);
    }

    public Vector3 GetCellOriginWorld(Vector3Int pos)
    {
        if (interactableMap == null)
        {
            return pos;
        }

        return interactableMap.CellToWorld(pos);
    }

    public Vector3 GetCellCenterWorld(Vector3Int pos)
    {
        if (interactableMap == null)
        {
            return pos + new Vector3(0.5f, 0.5f, 0f);
        }

        return interactableMap.GetCellCenterWorld(pos);
    }

    public Vector3Int WorldToCell(Vector3 worldPosition)
    {
        if (interactableMap == null)
        {
            return Vector3Int.FloorToInt(worldPosition);
        }

        return interactableMap.WorldToCell(worldPosition);
    }
}
