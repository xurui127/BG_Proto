using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] public List<Transform> tiles;
    [SerializeField] public float tileSpacing = 1.1f;
    private Dictionary<Vector3, Transform> tileMap = new Dictionary<Vector3, Transform>();

    private void Awake()
    {
        RegisterTiles();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var tile in tiles)
        {
            List<Transform> neighbors = GetNeighbors(tile);
            foreach (var neighbor in neighbors)
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
        List<Transform> neighbors = new List<Transform>();
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

    private Vector3 RoundPosition(Vector3 pos)
    {
        return new Vector3(
            Mathf.Round(pos.x * 100f) / 100f,
            Mathf.Round(pos.y * 100f) / 100f,
            Mathf.Round(pos.z * 100f) / 100f
        );
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