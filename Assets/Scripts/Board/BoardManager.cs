using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private GameObject carrotPrefab;
    [SerializeField] private GameObject tomatoPrefab;
    [SerializeField] internal List<Transform> tiles;
    internal List<TileBehaviour> tileBehaviours = new();
    internal float tileSpacing = 1.1f;

    readonly Vector3 fruitOffset = new(0f, 0.7f, 0f);
    readonly Dictionary<Vector3, Transform> tileMapByPosition = new();

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
    private void Awake()
    {
        RegisterTiles();
        RegesterTileBehaviours();
    }

    internal void InitFruits(int fruitCount)
    {
        var availableTiles = GetAvailiableTiles();

        int count = Mathf.Min(fruitCount, availableTiles.Count);

        for (int i = 0; i < count; i++)
        {
            var tileIndex = availableTiles[i];

            int fruitNum = Random.Range(0, 2);

            var fruit = fruitNum == 0 ?
                      Instantiate(carrotPrefab, tiles[tileIndex].position + fruitOffset, Quaternion.identity) :
                      Instantiate(tomatoPrefab, tiles[tileIndex].position + fruitOffset, Quaternion.identity);
            tileBehaviours[tileIndex].isPlaced = true;
        }
    }


    private void RegisterTiles()
    {
        tileMapByPosition.Clear();
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                Vector3 pos = tile.position;
                tileMapByPosition[pos] = tile;
            }
        }
    }

    private void RegesterTileBehaviours()
    {
        foreach (var tile in tiles)
        {
            if (tile != null)
            {
                tileBehaviours.Add(tile.GetComponent<TileBehaviour>());
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
            if (tileMapByPosition.ContainsKey(neighborPos))
            {
                neighbors.Add(tileMapByPosition[neighborPos]);
            }
        }
        return neighbors;
    }

    private Vector3 RoundPosition(Vector3 pos) =>
        new(Mathf.Round(pos.x * 100f) / 100f,
            Mathf.Round(pos.y * 100f) / 100f,
            Mathf.Round(pos.z * 100f) / 100f
        );

    public List<int> GetAvailiableTiles()
    {
        List<int> availableTiles = new();
        for (int i = 0; i < tileBehaviours.Count; i++)
        {
            if (!tileBehaviours[i].isPlaced)
            {
                availableTiles.Add(i);
            }
        }

        for (int i = 0; i < availableTiles.Count; i++)
        {
            int randomIndex = Random.Range(i, availableTiles.Count);
            (availableTiles[i], availableTiles[randomIndex]) = (availableTiles[randomIndex], availableTiles[i]);
        }
        return availableTiles;
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