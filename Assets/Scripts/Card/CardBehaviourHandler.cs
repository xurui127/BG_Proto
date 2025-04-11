using System;
using System.Collections.Generic;
using System.Linq;
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
        var cardTypes = GetTypesWith<CardAttribute>();
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
        for(int i = 0; i < cardBehaviours.Count; i++)
        {
            var amount = cardDataByName[cardData[i].name].value;
            cardBehaviours[i].OnGameStart(amount); 
        }
       
    }
    private IEnumerable<Type> GetTypesWith<T>() where T : Attribute
    {
        return from a in AppDomain.CurrentDomain.GetAssemblies()
               from t in a.GetTypes()
               where t.GetCustomAttributes(typeof(T), true).Length > 0
               select t;
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
