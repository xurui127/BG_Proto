using System;
using System.Collections.Generic;
using UnityEngine;

public class CharacterData : MonoBehaviour
{
    [SerializeField] public Dictionary<string, Card_SO> currentCards = new();
    internal CardInstance[] sourceDeck;
    internal List<CardInstance> activeDeck = new();
    internal List<CardInstance> hand = new();
    internal List<CardInstance> graveyard = new();

    public int fruitCount = 0;

    public int AddFruits(int amount)
    {
        fruitCount += amount;
        return fruitCount;
    }

    internal void GenerateDeck(Card_SO[] cards)
    {
        int copyCount = 4;
        sourceDeck = new CardInstance[cards.Length * copyCount];
        int deckIndex = 0;
        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = 0; j < copyCount; j++)
            {
                sourceDeck[deckIndex] = new CardInstance(cards[i], deckIndex);
                deckIndex++;
            }
        }
        activeDeck = new List<CardInstance>(sourceDeck);
    }

    internal void DrawHand()
    {
        // Refill deck with graveyard if not enough cards
        if (activeDeck.Count < 3)
        {
            activeDeck.AddRange(graveyard);
            graveyard.Clear();
        }

        // Randomly pick 3 cards from deck to add to hand
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, activeDeck.Count);
            hand.Add(activeDeck[randomIndex]);
            activeDeck.RemoveAt(randomIndex);
        }
    }

    internal void DiscardHand()
    {
        graveyard.AddRange(hand);
        hand.Clear();
    }
}
