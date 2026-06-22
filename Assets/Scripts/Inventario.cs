using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public int numSlots = 24;

    [System.Serializable]
    public class Slot
    {
        public TipoColetavel tipo;
        public int quantidade;
        public int maxPermitido;
        public Sprite icon;

        public Slot()
        {
            tipo = TipoColetavel.NONE;
            quantidade = 0;
            maxPermitido = 10;
            icon = null;
        }

        public bool PodeAdicionar()
        {
            return quantidade < maxPermitido;
        }

        public void AdicionarItem(Coletavel item)
        {
            tipo = item.tipo;
            icon = item.icon;
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
                    tipo = TipoColetavel.NONE;
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

    public void Add(Coletavel item)
    {
        foreach (Slot slot in slots)
        {
            if (slot.tipo == item.tipo && slot.PodeAdicionar())
            {
                slot.AdicionarItem(item);
                return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.tipo == TipoColetavel.NONE)
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