using UnityEngine;

public class FruitBehaviour : ItemBehaviour
{
    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        Destroy(gameObject);

        data.FruitCount += amount;
    }

}
