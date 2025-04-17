using UnityEngine;

public class BombBehaviour : ItemBehaviour
{
    [SerializeField] ItemAnimation anim;

    int? ownerIndex;

    internal override void RegesterItem(int amount, int? ownerIndex)
    {
        this.amount = amount;
        this.ownerIndex = ownerIndex;
    }
    internal override void OnInteract(CharacterData data)
    {
        if (ownerIndex == data.index) return;

        data.FruitCount -= amount;
    }

}
