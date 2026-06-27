using UnityEngine;
using System.Collections.Generic;

public class GerenciadorItem : MonoBehaviour
{
    public Item[] items;

    private Dictionary<string, Item> nomePraItemDict = 
        new Dictionary<string, Item>();

    private void Awake()
    {
        foreach(Item item in items)
        {
          AdicionarItem(item);  
        }
    }

    private void AdicionarItem(Item item)
    {
        // Alterado de Constainskey para ContainsKey
        if (!nomePraItemDict.ContainsKey(item.Data.itemName))
        {
            nomePraItemDict.Add(item.Data.itemName, item);
        }
    }

    public Item ObterItemPorNome(string key)
    {
        if (nomePraItemDict.ContainsKey(key))
        {
            return nomePraItemDict[key];
        }
        return null;
    }
}

