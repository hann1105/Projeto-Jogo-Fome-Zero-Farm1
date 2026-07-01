using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public int numSlots = 24;

    [System.Serializable]
    public class Slot
    {
        public string itemName;
        public int quantidade;
        public int maxPermitido;
        public Sprite icon;
        public ItemData itemData;

        public Slot()
        {
            itemName = "";
            quantidade = 0;
            maxPermitido = 9;
            icon = null;
            itemData = null;
        }

        public bool PodeAdicionar()
        {
            return quantidade < maxPermitido;
        }

        public void AdicionarItem(Item item)
        {
            AdicionarItem(item.Data);
        }

        public void AdicionarItem(ItemData data)
        {
            itemData = data;
            itemName = data.itemName;
            icon = data.icon;
            quantidade++;
        }

        public void RemoverItem()
        {
            if (quantidade <= 0)
            {
                return;
            }

            quantidade--;

            if (quantidade == 0)
            {
                icon = null;
                itemName = "";
                itemData = null;
            }
        }
    }

    public List<Slot> slots = new List<Slot>();

    private void Awake()
    {
        slots.Clear();

        for (int i = 0; i < numSlots; i++)
        {
            slots.Add(new Slot());
        }
    }

    public void Add(Item item)
    {
        Add(item.Data);
    }

    public void Add(ItemData itemData)
    {
        if (itemData == null)
        {
            return;
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == itemData.itemName && slot.PodeAdicionar())
            {
                slot.AdicionarItem(itemData);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                slot.AdicionarItem(itemData);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        Slot slot = GetSlot(index);
        if (slot != null)
        {
            slot.RemoverItem();
        }
    }

    public int RemovePorNome(string itemName, int quantidade)
    {
        if (string.IsNullOrEmpty(itemName) || quantidade <= 0)
        {
            return 0;
        }

        int removidos = 0;

        foreach (Slot slot in slots)
        {
            while (slot.itemName == itemName && slot.quantidade > 0 && removidos < quantidade)
            {
                slot.RemoverItem();
                removidos++;
            }

            if (removidos >= quantidade)
            {
                break;
            }
        }

        return removidos;
    }

    public int RemoveTodosPorNome(string itemName)
    {
        if (string.IsNullOrEmpty(itemName))
        {
            return 0;
        }

        int removidos = 0;

        foreach (Slot slot in slots)
        {
            while (slot.itemName == itemName && slot.quantidade > 0)
            {
                slot.RemoverItem();
                removidos++;
            }
        }

        return removidos;
    }

    public Slot GetSlot(int index)
    {
        if (index < 0 || index >= slots.Count)
        {
            return null;
        }

        return slots[index];
    }
}
