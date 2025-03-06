using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    public List<Transform> tiles;
    public float tileSpacing = 1.1f;

    readonly Dictionary<Vector3, Transform> tileMap = new();

    private void Awake()
    {
        RegisterTiles();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var tile in tiles)
        {
            foreach (var neighbor in GetNeighbors(tile))
            {
                Gizmos.DrawLine(tile.position, neighbor.position);
            }
        }
    }

    private void RegisterTiles()
    {
        tileMap.Clear();
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                Vector3 pos = tile.position;
                tileMap[pos] = tile;
            }
        }
    }

    private List<Transform> GetNeighbors(Transform tile)
    {
        List<Transform> neighbors = new();
        Vector3 pos = tile.position;

        Vector3[] directions =
        {
            Vector3.forward,
            Vector3.back,
            Vector3.left,
            Vector3.right,
        };

        foreach (var dir in directions)
        {
            Vector3 neighborPos = RoundPosition(pos + dir * tileSpacing);
            if (tileMap.ContainsKey(neighborPos))
            {
                neighbors.Add(tileMap[neighborPos]);
            }
        }
        return neighbors;
    }

    private Vector3 RoundPosition(Vector3 pos) =>
        new (Mathf.Round(pos.x * 100f) / 100f,
            Mathf.Round(pos.y * 100f) / 100f,
            Mathf.Round(pos.z * 100f) / 100f
        );

    public int GetRandomTileIndex() 
    {
        return Random.Range(1, tiles.Count - 1); 
    }

    public Quaternion GetDirction(int index)
    {
        var currentTile = tiles[index];
        Transform nextTile;
        if (index + 1 < tiles.Count - 1)
        {
            nextTile = tiles[index + 1];
        }
        else
        {
            nextTile = tiles[0];
        }
        Vector3 dirction = nextTile.position - currentTile.position;

        return Quaternion.LookRotation(dirction);
    }
}
//if (tiles.Count < 2) return;

//Gizmos.color = Color.red;

//for (int i = 0; i < tiles.Count - 1; i++)
//{
//    Gizmos.DrawLine(tiles[i].position, tiles[i + 1].position);
//}
//if (tiles.Count > 2)
//{
//    Gizmos.DrawLine(tiles[tiles.Count - 1].position, tiles[0].position);
//}