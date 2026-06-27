using System.Collections.Generic;
using UnityEngine;

public class Inventario_UI : MonoBehaviour
{
    public GameObject panelInventario;
    public Jogador jogador;
    public List<Slot_UI> slots = new List<Slot_UI>();

    private void Awake()
    {
        panelInventario.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventario();
        }
    }

    public void ToggleInventario()
    {
        if (!panelInventario.activeSelf)
        {
            panelInventario.SetActive(true);
            Refresh();
        }
        else
        {
            panelInventario.SetActive(false);
        }
    }

    private void Refresh()
    {
        if (slots.Count != jogador.inventario.slots.Count)
        {
            Debug.LogError($"Slots UI: {slots.Count} | Slots inventario: {jogador.inventario.slots.Count} - numeros diferentes!");
            return;
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (jogador.inventario.slots[i].itemName != "")
            {
                slots[i].SetItem(jogador.inventario.slots[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove(int slotID)
    {
        Item itemParaDropar = GerenciadorJogo.instance.gerenciadorItem.ObterItemPorNome(
            jogador.inventario.slots[slotID].itemName);

        if (itemParaDropar != null)
        {
            jogador.DroparItem(itemParaDropar);
            jogador.inventario.Remove(slotID);
            Refresh();
        }
    }
}
