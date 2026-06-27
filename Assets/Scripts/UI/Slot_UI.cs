using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot_UI : MonoBehaviour
{
    public Image itemIcon;
    public TextMeshProUGUI quantidadeText;

    [SerializeField] private GameObject Highlight;

    public void SetItem(Inventario.Slot slot)
    {
        if (slot != null)
        {
            itemIcon.sprite = slot.icon;
            itemIcon.color = new Color(1, 1, 1, 1);

            quantidadeText.text = slot.quantidade.ToString();
        }
    }

    public void SetEmpty()
    {
        itemIcon.sprite = null;
        itemIcon.color = new Color(1, 1, 1, 0);

        quantidadeText.text = "";
    }

    public void setHighlight(bool isOn)
    {
        Highlight.SetActive(isOn);
    }
}