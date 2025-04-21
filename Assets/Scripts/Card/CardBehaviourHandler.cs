using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class CardBehaviourHandler : MonoBehaviour
{
    [SerializeField] Card_SO[] cardData;
    Dictionary<string, CardBehaviour> cardBehaviourByName = new();
    Dictionary<string, Card_SO> cardDataByName = new();
    List<CardBehaviour> cardBehaviours = new();


    private void Awake()
    {
        InitCardDataByName();
        var cardTypes = Util.GetTypesWith<CardAttribute>();
        foreach (var type in cardTypes)
        {
            var attribute = type.GetCustomAttribute<CardAttribute>();
            if (attribute != null)
            {
                var instance = Activator.CreateInstance(type) as CardBehaviour;
                cardBehaviourByName[attribute.cardName] = instance;
                cardBehaviours.Add(instance);
            }
        }
    }

    private void Start()
    {
        for (int i = 0; i < cardData.Length; i++)
        {
            if (cardBehaviourByName.TryGetValue(cardData[i].name, out var behaviour))
            {
                var amount = cardDataByName[cardData[i].name].value;
                behaviour.OnGameStart(amount);
            }
            else
            {
                Debug.LogWarning($"No CardBehaviour found for card: {cardData[i].name}");
            }
        }

    }

    internal void OnCardExecute(string cardName)
    {
        if (cardBehaviourByName.TryGetValue(cardName, out var card))
        {
            card.OnExecute();
        }
        else
        {
            Debug.LogWarning($"Card {cardName} not found!");
        }
    }

    private void InitCardDataByName()
    {
        foreach (var data in cardData)
        {
            cardDataByName[data.name] = data;
        }
    }
}
