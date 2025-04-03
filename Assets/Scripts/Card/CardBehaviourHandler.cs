using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class CardBehaviourHandler : MonoBehaviour
{
    private Dictionary<string, CardBehaviour> cardBehaviourByName = new();

    private void Awake()
    {
        var cardTypes = GetTypesWith<CardAttribute>();
        foreach (var type in cardTypes)
        {
            var attribute = type.GetCustomAttribute<CardAttribute>();
            if (attribute != null)
            {
                var instance = Activator.CreateInstance(type) as CardBehaviour;
                cardBehaviourByName[attribute.cardName] = instance;
            }
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
}
