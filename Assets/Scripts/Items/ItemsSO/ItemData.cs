using UnityEngine;

public enum ItemType
{
    Fruit,
    Default
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item Data/Item")]
public class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public ItemType type;
    public GameObject itemPrefab;
    public int value;
}
