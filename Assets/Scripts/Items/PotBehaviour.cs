using UnityEngine;


public class PotBehaviour : ItemBehaviour
{
    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        if (data.FruitCount >= 10)
        {
            data.CostFruits(data.FruitCount / 10);
            data.UpdateGoal(data.FruitCount / 10);
        }
        if (data.GoalCount >= 3)
        {
            Debug.Log("Finish");
        }
    }
}
