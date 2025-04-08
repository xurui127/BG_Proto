using UnityEngine;

public class ItemInstance : MonoBehaviour
{
    ItemData itemData;
    
    public ItemInstance(ItemData itemData)
    {
        this.itemData = itemData;
    }
}
