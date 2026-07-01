using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventario_UI : MonoBehaviour
{
    public GameObject panelInventario;
    public Jogador jogador;
    public List<Slot_UI> slots = new List<Slot_UI>();

    [Header("Fundo da area de equipamentos")]
    [SerializeField] private bool criarFundoEquipamentos = true;
    [SerializeField] private Color corFundoEquipamentos = new Color(0.62f, 0.45f, 0.25f, 1f);
    [SerializeField] private Vector2 tamanhoFundoEquipamentos = new Vector2(1000f, 477f);
    [SerializeField] private Vector2 posicaoFundoEquipamentos = new Vector2(-415f, 0f);

    [Header("Correcao de brechas do frame")]
    [SerializeField] private bool cobrirBrechasDoFrame = true;
    [SerializeField] private Color corFundoSolidoFrame = new Color(0.62f, 0.45f, 0.25f, 1f);
    [SerializeField] private Vector2 margemFundoSolidoFrame = new Vector2(8f, 8f);

    private bool inventarioAberto;
    private const string NomeFundoSolidoFrame = "InventoryFrame_SolidBackground";
    private const string NomeFundoEquipamentos = "EquipmentPanel_Background";

    private void Awake()
    {
        if (jogador == null)
        {
            jogador = FindObjectOfType<Jogador>();
        }

        CriarFundoSolidoFrame();
        CriarFundoEquipamentos();
        ConfigurarSlots();
        SetInventarioAberto(false);
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
        SetInventarioAberto(!inventarioAberto);

        if (inventarioAberto)
        {
            Refresh();
        }
    }

    private void SetInventarioAberto(bool aberto)
    {
        inventarioAberto = aberto;

        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(aberto);
        }
    }

    private void ConfigurarSlots()
    {
        Slot_UI[] slotsEncontrados = GetComponentsInChildren<Slot_UI>(true);

        if (slotsEncontrados.Length > 0)
        {
            slots = new List<Slot_UI>(slotsEncontrados);
        }

        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                slots[i].Configure(this, i);
            }
        }
    }

    private void Refresh()
    {
        if (jogador == null || jogador.inventario == null)
        {
            return;
        }

        int count = Mathf.Min(slots.Count, jogador.inventario.slots.Count);

        for (int i = 0; i < count; i++)
        {
            if (slots[i] == null)
            {
                continue;
            }

            if (jogador.inventario.slots[i].itemName != "")
            {
                slots[i].SetItem(jogador.inventario.slots[i]);
            }
            else
            {
                slots[i].SetEmpty();
            }
        }

        for (int i = count; i < slots.Count; i++)
        {
            if (slots[i] != null)
            {
                slots[i].SetEmpty();
            }
        }
    }

    public void Remove(int slotID)
    {
        if (jogador == null || jogador.inventario == null)
        {
            return;
        }

        jogador.inventario.Remove(slotID);
        Refresh();

        if (jogador.toolbar != null)
        {
            jogador.toolbar.Refresh();
        }
    }

    private void CriarFundoEquipamentos()
    {
        if (!criarFundoEquipamentos || panelInventario == null)
        {
            return;
        }

        Transform existente = transform.Find(NomeFundoEquipamentos);
        GameObject fundo = existente != null
            ? existente.gameObject
            : new GameObject(NomeFundoEquipamentos, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

        fundo.transform.SetParent(transform, false);
        fundo.transform.SetSiblingIndex(0);

        Image image = fundo.GetComponent<Image>();
        if (image == null)
        {
            image = fundo.AddComponent<Image>();
        }

        image.color = corFundoEquipamentos;
        image.raycastTarget = false;

        RectTransform rect = fundo.GetComponent<RectTransform>();
        if (rect == null)
        {
            return;
        }

        rect.anchorMin = new Vector2(0.5f, 0.5f);
        rect.anchorMax = new Vector2(0.5f, 0.5f);
        rect.pivot = new Vector2(0.5f, 0.5f);
        rect.anchoredPosition = posicaoFundoEquipamentos;
        rect.sizeDelta = tamanhoFundoEquipamentos;
    }

    private void CriarFundoSolidoFrame()
    {
        if (!cobrirBrechasDoFrame || panelInventario == null)
        {
            return;
        }

        RectTransform frameRect = panelInventario.GetComponent<RectTransform>();
        if (frameRect == null)
        {
            return;
        }

        Transform existente = transform.Find(NomeFundoSolidoFrame);
        GameObject fundo = existente != null
            ? existente.gameObject
            : new GameObject(NomeFundoSolidoFrame, typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));

        fundo.transform.SetParent(transform, false);
        fundo.transform.SetSiblingIndex(0);

        Image image = fundo.GetComponent<Image>();
        if (image == null)
        {
            image = fundo.AddComponent<Image>();
        }

        image.color = corFundoSolidoFrame;
        image.raycastTarget = false;

        RectTransform rect = fundo.GetComponent<RectTransform>();
        if (rect == null)
        {
            return;
        }

        rect.anchorMin = frameRect.anchorMin;
        rect.anchorMax = frameRect.anchorMax;
        rect.pivot = frameRect.pivot;
        rect.anchoredPosition = frameRect.anchoredPosition;
        rect.sizeDelta = frameRect.sizeDelta + margemFundoSolidoFrame;
    }
}
