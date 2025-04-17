using UnityEngine;

public class BombBehaviour : ItemBehaviour
{
    [SerializeField] ItemAnimation anim;

    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        //TODO: Add Animation after
        Destroy(this.gameObject);
        if (data.FruitCount < amount) return; 
        data.FruitCount -= amount;
    }
}
