using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GerenciadorPontuacao : MonoBehaviour
{
    [SerializeField] private int pontos;
    [SerializeField] private TextMeshProUGUI pontuacaoText;

    public int Pontos
    {
        get { return pontos; }
    }

    private void Awake()
    {
        CriarHUDSeNecessario();
        AtualizarHUD();
    }

    public void AdicionarPontos(int quantidade)
    {
        pontos += quantidade;
        AtualizarHUD();
    }

    private void AtualizarHUD()
    {
        if (pontuacaoText != null)
        {
            pontuacaoText.text = "Pontos: " + pontos;
        }
    }

    private void CriarHUDSeNecessario()
    {
        if (pontuacaoText != null)
        {
            return;
        }

        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas == null)
        {
            GameObject canvasObject = new GameObject("Canvas");
            canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasObject.AddComponent<CanvasScaler>();
            canvasObject.AddComponent<GraphicRaycaster>();
        }

        GameObject panelObject = new GameObject("PainelPontuacao", typeof(RectTransform));
        panelObject.transform.SetParent(canvas.transform, false);
        panelObject.transform.SetAsLastSibling();

        RectTransform panelRect = panelObject.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0f, 1f);
        panelRect.anchorMax = new Vector2(0f, 1f);
        panelRect.pivot = new Vector2(0f, 1f);
        panelRect.anchoredPosition = new Vector2(18f, -18f);
        panelRect.sizeDelta = new Vector2(200f, 48f);

        Image background = panelObject.AddComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.65f);
        background.raycastTarget = false;

        GameObject textObject = new GameObject("TextoPontuacao", typeof(RectTransform));
        textObject.transform.SetParent(panelObject.transform, false);

        pontuacaoText = textObject.AddComponent<TextMeshProUGUI>();
        pontuacaoText.fontSize = 28;
        pontuacaoText.color = Color.white;
        pontuacaoText.alignment = TextAlignmentOptions.MidlineLeft;
        pontuacaoText.raycastTarget = false;

        RectTransform textRect = pontuacaoText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(12f, 0f);
        textRect.offsetMax = new Vector2(-12f, 0f);
    }
}
