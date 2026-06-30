using System.Collections.Generic;
using UnityEngine;

public class Toolbar_UI : MonoBehaviour
{
    [SerializeField] private List<Slot_UI> toolbarSlots = new List<Slot_UI>();
    [SerializeField] private Jogador jogador;

    private Slot_UI selectedSlot;

    public int SelectedIndex { get; private set; }

    private void Start()
    {
        if (jogador == null)
        {
            jogador = FindObjectOfType<Jogador>();
        }

        SelectSlot(0);
        Refresh();
    }

    private void Update()
    {
        checkAlphaNumericKeys();
        Refresh();
    }

    public void SelectSlot(int index)
    {
        if (index < 0 || index >= toolbarSlots.Count)
        {
            return;
        }

        if (selectedSlot != null)
        {
            selectedSlot.setHighlight(false);
        }

        selectedSlot = toolbarSlots[index];
        selectedSlot.setHighlight(true);
        SelectedIndex = index;
    }

    public void Refresh()
    {
        if (jogador == null || jogador.inventario == null)
        {
            return;
        }

        int count = Mathf.Min(toolbarSlots.Count, jogador.inventario.slots.Count);
        for (int i = 0; i < count; i++)
        {
            Inventario.Slot slot = jogador.inventario.slots[i];
            if (slot.itemName != "")
            {
                toolbarSlots[i].SetItem(slot);
            }
            else
            {
                toolbarSlots[i].SetEmpty();
            }
        }
    }

    private void checkAlphaNumericKeys()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            SelectSlot(0);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            SelectSlot(1);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            SelectSlot(2);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            SelectSlot(3);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            SelectSlot(4);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            SelectSlot(5);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            SelectSlot(6);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            SelectSlot(7);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            SelectSlot(8);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            SelectSlot(9);
        }
    }
}
