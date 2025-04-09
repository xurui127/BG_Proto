using UnityEngine;

public class FruitBehaviour : ItemBehaviour
{
    [SerializeField] FruitAnimation anim;
    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        anim.SetPlayGetFruitAnimation();
        data.FruitCount += amount;
    }
}
