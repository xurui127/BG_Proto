using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] public List<Card_SO> currentCards = new();
    int gold = 0;

    public int AddGold(int amount)
    {
        gold += amount; 
        return gold;
    }
}
