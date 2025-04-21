using UnityEngine.Events;


public class PotBehaviour : ItemBehaviour
{
    internal static UnityAction<bool> OpenEndPanelEvent;
    internal override void RegesterItem(int amount)
    {
        this.amount = amount;
    }

    internal override void OnInteract(CharacterData data)
    {
        if (data.FruitCount >= 10)
        {
            var fruitCount = data.FruitCount;
            var convertCount = fruitCount / 10;
            data.CostFruits(convertCount);
            data.UpdateGoal(convertCount);
        }
        if (data.GoalCount >= 3)
        {
            OpenEndPanelEvent?.Invoke(data.index == 0);
        }
    }
}
