using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Slot_UI : MonoBehaviour
{
    [SerializeField] private Image slotBackground;
    public Image itemIcon;
    public TextMeshProUGUI quantidadeText;

    [SerializeField] private GameObject Highlight;
    [SerializeField] private Button removeButton;
    [SerializeField] private Color inventorySlotColor = new Color(0.62f, 0.45f, 0.25f, 1f);
    [SerializeField] private Color inventorySlotBorderColor = new Color(0.27f, 0.18f, 0.10f, 1f);

    private Inventario_UI inventarioUI;
    private int slotIndex = -1;
    private bool isInventorySlot;

    private void Awake()
    {
        if (slotBackground == null)
        {
            slotBackground = GetComponent<Image>();
        }

        if (removeButton == null)
        {
            removeButton = GetComponentInChildren<Button>(true);
        }

        isInventorySlot = removeButton != null;
        MostrarFundoDoSlot();
    }

    public void Configure(Inventario_UI ui, int index)
    {
        inventarioUI = ui;
        slotIndex = index;

        if (removeButton != null)
        {
            removeButton.onClick.RemoveListener(RemoveCurrentSlot);
            removeButton.onClick.AddListener(RemoveCurrentSlot);
        }

        isInventorySlot = removeButton != null;
        MostrarFundoDoSlot();
    }

    public void SetItem(Inventario.Slot slot)
    {
        MostrarFundoDoSlot();

        if (slot != null)
        {
            if (itemIcon != null)
            {
                itemIcon.sprite = slot.icon;
                itemIcon.color = new Color(1, 1, 1, 1);
            }

            if (quantidadeText != null)
            {
                quantidadeText.text = slot.quantidade.ToString();
                quantidadeText.gameObject.SetActive(true);
            }

            if (removeButton != null)
            {
                removeButton.gameObject.SetActive(true);
            }
        }
    }

    public void SetEmpty()
    {
        MostrarFundoDoSlot();

        if (itemIcon != null)
        {
            itemIcon.sprite = null;
            itemIcon.color = new Color(1, 1, 1, 0);
        }

        if (quantidadeText != null)
        {
            quantidadeText.text = "";
            quantidadeText.gameObject.SetActive(false);
        }

        if (removeButton != null)
        {
            removeButton.gameObject.SetActive(false);
        }
    }

    public void setHighlight(bool isOn)
    {
        if (Highlight != null)
        {
            Highlight.SetActive(isOn);
        }
    }

    private void RemoveCurrentSlot()
    {
        if (inventarioUI != null && slotIndex >= 0)
        {
            inventarioUI.Remove(slotIndex);
        }
    }

    private void MostrarFundoDoSlot()
    {
        if (slotBackground == null)
        {
            return;
        }

        Color color = slotBackground.color;

        if (isInventorySlot)
        {
            slotBackground.sprite = null;
            slotBackground.color = inventorySlotColor;

            Outline outline = slotBackground.GetComponent<Outline>();
            if (outline == null)
            {
                outline = slotBackground.gameObject.AddComponent<Outline>();
            }

            outline.effectColor = inventorySlotBorderColor;
            outline.effectDistance = new Vector2(4f, -4f);
        }
        else if (color.a <= 0f)
        {
            color.a = 1f;
            slotBackground.color = color;
        }

        slotBackground.enabled = true;
    }
}
