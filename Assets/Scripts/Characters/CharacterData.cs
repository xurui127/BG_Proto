using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] public List<Card_SO> currentCards = new();
    public int gold = 0;

    public void AddGold(int amount)
    {
        gold += amount; 
    }
}
