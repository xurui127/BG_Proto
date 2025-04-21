using UnityEngine;

public class FruitBehaviour : ItemBehaviour
{
    [SerializeField] ItemAnimation anim;
    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        anim.TransiteToCollectState();
        data.FruitCount += amount;
    }
}
