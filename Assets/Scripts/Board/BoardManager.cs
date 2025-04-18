using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    GameManager gameManager;

    [SerializeField] ItemData[] fruitsData;
    [SerializeField] ItemData potData;
    [SerializeField] ItemData bombData;
    [SerializeField] internal List<Transform> tiles;
    internal List<TileBehaviour> tileBehaviours = new();
    internal float tileSpacing = 1.1f;

    readonly Vector3 fruitPosOffset = new(0f, 0.7f, 0f);
    readonly Vector3 potPosOffset = new(0f, 0.4f, 0f);
    readonly Vector3 trapPosOffset = new(0f, 0.8f, 0f);
    readonly Dictionary<Vector3, Transform> tileMapByPosition = new();

    [SerializeField] List<ItemInstance> items = new();

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

    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    private GameObject SpawnFruitAtTile(int tileIndex)
    {
        var fruitNum = UnityEngine.Random.Range(0, 2);
        var fruitData = fruitNum == 0 ? fruitsData[0] : fruitsData[1];
        var fruit = Instantiate(fruitData.itemPrefab, tiles[tileIndex].position + fruitPosOffset, Quaternion.identity);

        var itemBehaviour = fruit.GetComponent<ItemBehaviour>();
        itemBehaviour.RegesterItem(fruitData.value);

        var anim = fruit.GetComponent<ItemAnimation>();
        if (anim != null)
        {
            anim.GetCollectEffect(fruitData.collectEffect);
        }

        tileBehaviours[tileIndex].PlacedFruit();
        tileBehaviours[tileIndex].SetCurrentBehaviour(itemBehaviour);
        return fruit;
    }

    internal void InitFruits(int fruitCount)
    {
        var availableTiles = GetAvailiableTiles();
        int count = Mathf.Min(fruitCount, availableTiles.Count);

        for (int i = 0; i < count; i++)
        {
            SpawnFruitAtTile(availableTiles[i]);
        }
    }

    internal GameObject InitPlacedFruit()
    {
        var availableTiles = GetAvailiableTiles();
        InitPlacedFruitVFX(availableTiles[0]);
        return SpawnFruitAtTile(availableTiles[0]);
    }

    internal void InitPot()
    {
        var availableTiles = GetAvailiableTiles();

        var tileIndex = availableTiles[0];
        var pot = Instantiate(potData.itemPrefab, tiles[tileIndex].position + potPosOffset, Quaternion.identity);
        var itemBehavour = pot.GetComponent<ItemBehaviour>();
        tileBehaviours[tileIndex].PlacedPot();
        tileBehaviours[tileIndex].SetCurrentBehaviour(itemBehavour);
        itemBehavour.RegesterItem(potData.value);
    }

    internal void InitBomb(CharacterData data)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].ownerIndex == data.index)
            {
                tileBehaviours[items[i].tileIndex].ResetTilePlacedTrap();
                Destroy(items[i].itemBehaviour.gameObject);
                items.Remove(items[i]);
            }
        }
        var tileIndex = gameManager.GetCurrentCharacterTileIndex();
        var bomb = Instantiate(bombData.itemPrefab, tiles[tileIndex].position + trapPosOffset, Quaternion.identity);
        var itemBehavour = bomb.GetComponent<ItemBehaviour>();
        var anim = bomb.GetComponent<ItemAnimation>();
        if (anim != null)
        {
            anim.GetCollectEffect(bombData.collectEffect);
        }
        tileBehaviours[tileIndex].PlacedTrap();
        tileBehaviours[tileIndex].SetCurrentBehaviour(itemBehavour);
        itemBehavour.RegesterItem(bombData.value);
        InitBombTrapVFX(tileIndex);
        items.Add(new ItemInstance(data.index, tileIndex, itemBehavour));
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
            if (!tileBehaviours[i].isPlacedFruit &&
                !tileBehaviours[i].isPlacedPot &&
                !tileBehaviours[i].isPlacedCharacter &&
                !tileBehaviours[i].isPlacedTrap)
            {
                availableTiles.Add(i);
            }
        }

        for (int i = 0; i < availableTiles.Count; i++)
        {
            int randomIndex = UnityEngine.Random.Range(i, availableTiles.Count);
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

    internal TileBehaviour GetCurrentTile(int index) => tileBehaviours[index];

    internal void ResetPlacedCharacter(int index)
    {
        tileBehaviours[index].ResetTilePlacedCharacter();
    }

    internal void RegesterPlacedCharacter(int index)
    {
        tileBehaviours[index].PlacedCharacter();
    }

    internal void RegesterCurrentCharacterOnTile(int tileIndex, CharacterBehaviour character)
    {
        tileBehaviours[tileIndex].RegisterCharacter(character);
    }

    internal void UnregesterCurrentChracterOntile(int tileIndex, CharacterBehaviour chracter)
    {
        tileBehaviours[tileIndex].UnregisterCharacter(chracter);
    }

    private void InitPlacedFruitVFX(int tileIndex)
    {
        StartCoroutine(InitFruitCoroutine(tileIndex));
        IEnumerator InitFruitCoroutine(int tileIndex)
        {
            var fruitPlacedVFX = fruitsData[0].initEffect;
            var vfx = Instantiate(fruitPlacedVFX, tiles[tileIndex].position + fruitPosOffset, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(vfx);
        }
    }

    private void InitBombTrapVFX(int tileIndex)
    {
        StartCoroutine(InitBombCoroutine(tileIndex));
        IEnumerator InitBombCoroutine(int tileIndex)
        {
            var bombPlacedVFX = bombData.initEffect;
            var vfx = Instantiate(bombPlacedVFX, tiles[tileIndex].position + fruitPosOffset, Quaternion.identity);
            yield return new WaitForSeconds(1f);
            Destroy(vfx);
        }
    }

    internal int GetPotIndex()
    {
        for (int i = 0; i < tileBehaviours.Count; i++)
        {
            if (tileBehaviours[i].isPlacedPot)
            {
                return i;
            }
        }
        Debug.LogWarning("Can not fount pot index!");
        return -1;
    }

    internal int GetTargetStep()
    {
        int currentCharacterIndex = gameManager.GetCurrentCharacterTileIndex();
        int diceNumber = gameManager.GetDiceNumber();
        int potIndex = GetPotIndex();
        int tileCount = tileBehaviours.Count;

        if (IsOverPot(currentCharacterIndex, diceNumber, potIndex, tileCount))
        {
            if (potIndex >= currentCharacterIndex)
            {
                return potIndex - currentCharacterIndex;
            }
            else
            {
                return (tileCount - currentCharacterIndex) + potIndex;
            }
        }

        return diceNumber;
    }

    private bool IsOverPot(int currentIndex, int diceNumber, int potIndex, int tileCount)
    {
        for (int i = 1; i <= diceNumber; i++)
        {
            int stepIndex = (currentIndex + i) % tileCount;
            if (stepIndex == potIndex)
                return true;
        }
        return false;
    }

    internal bool IsTrapInteract(int characterIndex)
    {
        if (items.Count == 0) return false;

        foreach (var item in items)
        {
            return item.ownerIndex == characterIndex;
        }
        Debug.LogWarning("Not find Character index!");
        return false;
    }

    internal bool CanPlaceTrap()
    {
        var characterIndex = gameManager.GetCurrentCharacterTileIndex();
        if (tileBehaviours[characterIndex].isPlacedTrap)
        {
            return false;
        }
        return true;
    }

    internal void RemoveItem(ItemBehaviour currentItem)
    {
        for(int i = 0; i < items.Count; i++)
        {
            if (items[i].itemBehaviour == currentItem)
            {
                items.Remove(items[i]);
            }
        }
    }
}