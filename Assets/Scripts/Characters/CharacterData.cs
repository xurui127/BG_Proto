using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CharacterData : MonoBehaviour
{
    public Dictionary<string, Card_SO> currentCards = new();
    internal CardInstance[] sourceDeck;
    internal List<CardInstance> activeDeck = new();
    internal List<CardInstance> hand = new();
    internal List<CardInstance> graveyard = new();
    internal int index;

    int _fruitCount = 0;
    int _goalCount = 0;
    const int requiredGoalCount = 10;
    internal int FruitCount
    {
        get => _fruitCount;
        set
        {
            if (_fruitCount != value)
            {
                _fruitCount = value;
                OnFruitCountChangedEvent?.Invoke(_fruitCount);
            }
        }
    }

    internal int GoalCount
    {
        get => _goalCount;
        set
        {
            if (_goalCount != value)
            {
                _goalCount = value;
                OnGoalCountChangedEvent?.Invoke(_goalCount);
            }
        }
    }

    internal UnityAction<int> OnFruitCountChangedEvent;
    internal UnityAction<int> OnGoalCountChangedEvent;

    public int UpdateFruits(int amount)
    {
        FruitCount += amount;
        return FruitCount;
    }

    internal int UpdateGoal(int quotient)
    {
        GoalCount += quotient;
        return GoalCount;
    }

    internal void CostFruits(int quotient)
    {
        FruitCount -= 10 * quotient;
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

    internal bool HasReachedPotRequirement() => _fruitCount >= requiredGoalCount;
}
