using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] public Dictionary<string, Card_SO> currentCards = new();
    public int gold = 0;

    public int AddGold(int amount)
    {
        gold += amount;
        return gold;
    }
}
