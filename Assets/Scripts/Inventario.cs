using System.Collections.Generic;
using UnityEngine;

public class Inventario : MonoBehaviour
{
    public int numSlots = 21;

    [System.Serializable]
    public class Slot
    {
        public TipoColetavel tipo;
        public int quantidade;
        public int maxPermitido;

        public Slot()
        {
            tipo = TipoColetavel.NONE;
            quantidade = 0;
            maxPermitido = 10;
        }

        public bool PodeAdicionar()
        {
            return quantidade < maxPermitido;
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

    public void AdicionarItem(TipoColetavel tipo, int quantidade)
    {
        foreach (Slot slot in slots)
        {
            if (slot.tipo == tipo && slot.PodeAdicionar())
            {
                int quantidadeAdicionada = Mathf.Min(quantidade, slot.maxPermitido - slot.quantidade);
                slot.quantidade += quantidadeAdicionada;
                quantidade -= quantidadeAdicionada;

                if (quantidade <= 0)
                    return;
            }
        }

        foreach (Slot slot in slots)
        {
            if (slot.tipo == TipoColetavel.NONE)
            {
                int quantidadeAdicionada = Mathf.Min(quantidade, slot.maxPermitido);
                slot.tipo = tipo;
                slot.quantidade = quantidadeAdicionada;
                quantidade -= quantidadeAdicionada;

                if (quantidade <= 0)
                    return;
            }
        }
    }
}