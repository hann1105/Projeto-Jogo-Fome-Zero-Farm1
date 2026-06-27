using System.Collections.Generic;
using System.Collections;
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

        public Slot()
        {
            itemName = "";
            quantidade = 0;
            maxPermitido = 9;
           
        }

        public bool PodeAdicionar()
        {
            if (quantidade < maxPermitido)
            {
                return true;
            }
            return false;
        }

        public void AdicionarItem(Item item)
        {
            this.itemName = item.Data.itemName;
            this.icon = item.Data.icon;
            quantidade++;
        }
        
        public void RemoverItem()
        {
            if(quantidade > 0)
            {
                quantidade--;

                if(quantidade == 0)
                {
                    icon = null;
                    itemName = "";
                }
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
        foreach (Slot slot in slots)
        {
            if (slot.itemName == item.Data.itemName && slot.PodeAdicionar())
            {
                slot.AdicionarItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.itemName == "")
            {
                slot.AdicionarItem(item);
                return;
            }
        }
    }

    public void Remove(int index)
    {
        slots[index].RemoverItem();
    }
}
