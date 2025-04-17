
using System;

[Serializable]
public class ItemInstance
{
    public int ownerIndex;
    public int tileIndex;
    public ItemBehaviour itemBehaviour;

    public ItemInstance(int ownerIndex, int tileIndex, ItemBehaviour itemBehaviour)
    {
        this.ownerIndex = ownerIndex;
        this.tileIndex = tileIndex;
        this.itemBehaviour = itemBehaviour;
    }
}
