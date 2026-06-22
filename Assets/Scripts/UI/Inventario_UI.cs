using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventario_UI : MonoBehaviour
{
    public GameObject panelInventario;
    public Jogador jogador;
    public List<Slot_UI> slots = new List<Slot_UI>();

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventario();
        }
    }

    public void ToggleInventario()
    {
        
        if(!panelInventario.activeSelf)
        {
            panelInventario.SetActive(true);
            Refresh();
        }
        else
        {
            panelInventario.SetActive(false);
        }


    }

    private void Awake()
{
    panelInventario.SetActive(false); // garante que começa fechado
}

   void Refresh()
{
    // ADICIONA ESSE LOG para debugar
    if (slots.Count != jogador.inventario.slots.Count)
    {
        Debug.LogError($"Slots UI: {slots.Count} | Slots inventário: {jogador.inventario.slots.Count} — números diferentes!");
        return;
    }

    for (int i = 0; i < slots.Count; i++)
    {
        if (jogador.inventario.slots[i].tipo != TipoColetavel.NONE)
            slots[i].SetItem(jogador.inventario.slots[i]);
        else
            slots[i].SetEmpty();
    }
}

public void Remove(int slotID)
    {
        // Buscando o item no nosso Gerenciador usando as variáveis em português
        Coletavel itemParaDropar = GerenciadorJogo.instance.gerenciadorItem.ObterItemPorTipo(
            jogador.inventario.slots[slotID].tipo);
        
        if(itemParaDropar != null)
        {
            jogador.DroparItem(itemParaDropar);
            jogador.inventario.Remove(slotID);
            Refresh(); 
        }
    }

}