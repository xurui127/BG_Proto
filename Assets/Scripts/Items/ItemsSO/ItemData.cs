using UnityEngine;

public enum ItemType
{
    Fruit,
    Trap,
    Default
}

[CreateAssetMenu(fileName = "New Item", menuName = "Item Data/Item")]
public class ItemData : ScriptableObject
{
    public string id;
    public string itemName;
    public ItemType type;
    public GameObject itemPrefab;
    public GameObject initEffect;
    public GameObject collectEffect;
    public int value;
}
