using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GerenciadorPontuacao : MonoBehaviour
{
    [SerializeField] private int pontos;
    [SerializeField] private int metaDePontos = 1000;
    [SerializeField] private int intervaloMensagemImpacto = 100;
    [SerializeField] private int familiasAjudadasPorMarco = 10;
    [SerializeField] private GameObject painelPontuacao;
    [SerializeField] private TextMeshProUGUI pontuacaoText;
    [SerializeField] private TextMeshProUGUI impactoText;
    [SerializeField] private GameObject painelImpacto;
    [SerializeField] private GameObject painelFimDeJogo;
    [SerializeField] private TextMeshProUGUI fimDeJogoText;

    private int proximoMarcoImpacto;
    private bool jogoFinalizado;
    private Coroutine rotinaImpacto;

    public int Pontos
    {
        get { return pontos; }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += AoCarregarCena;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= AoCarregarCena;
    }

    private void Awake()
    {
        Time.timeScale = 1f;
        intervaloMensagemImpacto = Mathf.Max(1, intervaloMensagemImpacto);
        proximoMarcoImpacto = intervaloMensagemImpacto;
        CriarHUDSeNecessario();
        CriarPainelImpactoSeNecessario();
        CriarPainelFimDeJogoSeNecessario();
        AtualizarHUD();
    }

    private void AoCarregarCena(Scene scene, LoadSceneMode mode)
    {
        pontuacaoText = null;
        painelPontuacao = null;
        impactoText = null;
        fimDeJogoText = null;
        painelImpacto = null;
        painelFimDeJogo = null;

        CriarHUDSeNecessario();
        CriarPainelImpactoSeNecessario();
        CriarPainelFimDeJogoSeNecessario();
        AtualizarHUD();
    }

    public void AdicionarPontos(int quantidade)
    {
        if (jogoFinalizado || quantidade <= 0)
        {
            return;
        }

        pontos += quantidade;
        AtualizarHUD();
        VerificarMensagemImpacto();
        VerificarFimDeJogo();
    }

    private void AtualizarHUD()
    {
        if (pontuacaoText != null)
        {
            pontuacaoText.text = "Meta: " + pontos + " / " + metaDePontos;
        }
    }

    private void VerificarMensagemImpacto()
    {
        while (pontos >= proximoMarcoImpacto && proximoMarcoImpacto < metaDePontos)
        {
            int familiasAjudadas = (proximoMarcoImpacto / intervaloMensagemImpacto) * familiasAjudadasPorMarco;
            MostrarMensagemImpacto(familiasAjudadas + " familias ajudadas!");
            proximoMarcoImpacto += intervaloMensagemImpacto;
        }
    }

    private void VerificarFimDeJogo()
    {
        if (pontos < metaDePontos || jogoFinalizado)
        {
            return;
        }

        jogoFinalizado = true;
        pontos = metaDePontos;
        AtualizarHUD();
        MostrarFimDeJogo();
    }

    private void MostrarMensagemImpacto(string mensagem)
    {
        if (painelImpacto == null || impactoText == null)
        {
            return;
        }

        painelImpacto.SetActive(true);
        impactoText.text = mensagem;

        if (rotinaImpacto != null)
        {
            StopCoroutine(rotinaImpacto);
        }

        rotinaImpacto = StartCoroutine(EsconderImpactoDepoisDeTempo());
    }

    private IEnumerator EsconderImpactoDepoisDeTempo()
    {
        yield return new WaitForSeconds(3f);

        if (painelImpacto != null)
        {
            painelImpacto.SetActive(false);
        }
    }

    private void MostrarFimDeJogo()
    {
        if (fimDeJogoText != null)
        {
            int familiasAjudadas = (metaDePontos / intervaloMensagemImpacto) * familiasAjudadasPorMarco;
            fimDeJogoText.text = "Meta alcancada!\nVoce doou alimentos suficientes para ajudar a comunidade.\n" +
                familiasAjudadas + " familias foram ajudadas.\nPontuacao final: " + pontos;
        }

        if (painelFimDeJogo != null)
        {
            painelFimDeJogo.SetActive(true);
        }

        Time.timeScale = 0f;
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

        RemoverObjetosDuplicados("PainelPontuacao");

        painelPontuacao = new GameObject("PainelPontuacao", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        painelPontuacao.transform.SetParent(canvas.transform, false);
        painelPontuacao.transform.SetAsLastSibling();

        RectTransform panelRect = painelPontuacao.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(1f, 1f);
        panelRect.anchorMax = new Vector2(1f, 1f);
        panelRect.pivot = new Vector2(1f, 1f);
        panelRect.anchoredPosition = new Vector2(-14f, -14f);
        panelRect.sizeDelta = new Vector2(220f, 42f);

        Image background = painelPontuacao.GetComponent<Image>();
        background.color = new Color(0f, 0f, 0f, 0.82f);
        background.raycastTarget = false;

        GameObject textObject = new GameObject("TextoPontuacao", typeof(RectTransform));
        textObject.transform.SetParent(painelPontuacao.transform, false);

        pontuacaoText = textObject.AddComponent<TextMeshProUGUI>();
        pontuacaoText.fontSize = 22;
        pontuacaoText.color = Color.white;
        pontuacaoText.alignment = TextAlignmentOptions.Center;
        pontuacaoText.enableAutoSizing = true;
        pontuacaoText.fontSizeMin = 14;
        pontuacaoText.fontSizeMax = 22;
        pontuacaoText.raycastTarget = false;

        RectTransform textRect = pontuacaoText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(12f, 0f);
        textRect.offsetMax = new Vector2(-12f, 0f);
    }

    private void CriarPainelImpactoSeNecessario()
    {
        if (painelImpacto != null && impactoText != null)
        {
            painelImpacto.SetActive(false);
            return;
        }

        Canvas canvas = ObterOuCriarCanvas();
        RemoverObjetosDuplicados("PainelImpactoDoacao");

        painelImpacto = new GameObject("PainelImpactoDoacao", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        painelImpacto.transform.SetParent(canvas.transform, false);
        painelImpacto.transform.SetAsLastSibling();

        RectTransform panelRect = painelImpacto.GetComponent<RectTransform>();
        panelRect.anchorMin = new Vector2(0.5f, 1f);
        panelRect.anchorMax = new Vector2(0.5f, 1f);
        panelRect.pivot = new Vector2(0.5f, 1f);
        panelRect.anchoredPosition = new Vector2(0f, -80f);
        panelRect.sizeDelta = new Vector2(440f, 54f);

        Image background = painelImpacto.GetComponent<Image>();
        background.color = new Color(0.09f, 0.35f, 0.16f, 0.88f);
        background.raycastTarget = false;

        GameObject textObject = new GameObject("TextoImpactoDoacao", typeof(RectTransform));
        textObject.transform.SetParent(painelImpacto.transform, false);

        impactoText = textObject.AddComponent<TextMeshProUGUI>();
        impactoText.fontSize = 26;
        impactoText.color = Color.white;
        impactoText.alignment = TextAlignmentOptions.Center;
        impactoText.raycastTarget = false;

        RectTransform textRect = impactoText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = new Vector2(12f, 0f);
        textRect.offsetMax = new Vector2(-12f, 0f);

        painelImpacto.SetActive(false);
    }

    private void CriarPainelFimDeJogoSeNecessario()
    {
        if (painelFimDeJogo != null && fimDeJogoText != null)
        {
            painelFimDeJogo.SetActive(false);
            return;
        }

        Canvas canvas = ObterOuCriarCanvas();
        CriarEventSystemSeNecessario();
        RemoverObjetosDuplicados("PainelFimDeJogo");

        painelFimDeJogo = new GameObject("PainelFimDeJogo", typeof(RectTransform), typeof(CanvasRenderer), typeof(Image));
        painelFimDeJogo.transform.SetParent(canvas.transform, false);
        painelFimDeJogo.transform.SetAsLastSibling();

        RectTransform panelRect = painelFimDeJogo.GetComponent<RectTransform>();
        panelRect.anchorMin = Vector2.zero;
        panelRect.anchorMax = Vector2.one;
        panelRect.offsetMin = Vector2.zero;
        panelRect.offsetMax = Vector2.zero;

        Image overlay = painelFimDeJogo.GetComponent<Image>();
        overlay.color = new Color(0f, 0f, 0f, 0.72f);
        overlay.raycastTarget = true;

        GameObject boxObject = new GameObject("CaixaFimDeJogo", typeof(RectTransform));
        boxObject.transform.SetParent(painelFimDeJogo.transform, false);

        RectTransform boxRect = boxObject.GetComponent<RectTransform>();
        boxRect.anchorMin = new Vector2(0.5f, 0.5f);
        boxRect.anchorMax = new Vector2(0.5f, 0.5f);
        boxRect.pivot = new Vector2(0.5f, 0.5f);
        boxRect.anchoredPosition = Vector2.zero;
        boxRect.sizeDelta = new Vector2(620f, 320f);

        Image boxBackground = boxObject.AddComponent<Image>();
        boxBackground.color = new Color(0.62f, 0.45f, 0.25f, 1f);
        boxBackground.raycastTarget = true;

        Outline outline = boxObject.AddComponent<Outline>();
        outline.effectColor = new Color(0.27f, 0.18f, 0.10f, 1f);
        outline.effectDistance = new Vector2(6f, -6f);

        GameObject textObject = new GameObject("TextoFimDeJogo", typeof(RectTransform));
        textObject.transform.SetParent(boxObject.transform, false);

        fimDeJogoText = textObject.AddComponent<TextMeshProUGUI>();
        fimDeJogoText.fontSize = 30;
        fimDeJogoText.color = Color.white;
        fimDeJogoText.alignment = TextAlignmentOptions.Center;
        fimDeJogoText.raycastTarget = false;

        RectTransform textRect = fimDeJogoText.rectTransform;
        textRect.anchorMin = new Vector2(0f, 0.32f);
        textRect.anchorMax = new Vector2(1f, 1f);
        textRect.offsetMin = new Vector2(34f, 0f);
        textRect.offsetMax = new Vector2(-34f, -28f);

        CriarBotaoFimDeJogo(boxObject.transform, "Jogar novamente", new Vector2(-135f, -106f), ReiniciarJogo);
        CriarBotaoFimDeJogo(boxObject.transform, "Sair", new Vector2(135f, -106f), SairDoJogo);

        painelFimDeJogo.SetActive(false);
    }

    private void CriarBotaoFimDeJogo(Transform parent, string texto, Vector2 posicao, UnityEngine.Events.UnityAction acao)
    {
        GameObject buttonObject = new GameObject(texto, typeof(RectTransform));
        buttonObject.transform.SetParent(parent, false);

        RectTransform buttonRect = buttonObject.GetComponent<RectTransform>();
        buttonRect.anchorMin = new Vector2(0.5f, 0.5f);
        buttonRect.anchorMax = new Vector2(0.5f, 0.5f);
        buttonRect.pivot = new Vector2(0.5f, 0.5f);
        buttonRect.anchoredPosition = posicao;
        buttonRect.sizeDelta = new Vector2(220f, 54f);

        Image image = buttonObject.AddComponent<Image>();
        image.color = new Color(0.27f, 0.18f, 0.10f, 1f);

        Button button = buttonObject.AddComponent<Button>();
        button.onClick.AddListener(acao);

        GameObject textObject = new GameObject("Texto", typeof(RectTransform));
        textObject.transform.SetParent(buttonObject.transform, false);

        TextMeshProUGUI buttonText = textObject.AddComponent<TextMeshProUGUI>();
        buttonText.text = texto;
        buttonText.fontSize = 24;
        buttonText.color = Color.white;
        buttonText.alignment = TextAlignmentOptions.Center;
        buttonText.raycastTarget = false;

        RectTransform textRect = buttonText.rectTransform;
        textRect.anchorMin = Vector2.zero;
        textRect.anchorMax = Vector2.one;
        textRect.offsetMin = Vector2.zero;
        textRect.offsetMax = Vector2.zero;
    }

    private Canvas ObterOuCriarCanvas()
    {
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            return canvas;
        }

        GameObject canvasObject = new GameObject("Canvas");
        canvas = canvasObject.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvasObject.AddComponent<CanvasScaler>();
        canvasObject.AddComponent<GraphicRaycaster>();
        return canvas;
    }

    private void RemoverObjetosDuplicados(string nome)
    {
        GameObject[] objetos = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject objeto in objetos)
        {
            if (objeto.name == nome)
            {
                Destroy(objeto);
            }
        }
    }

    private void CriarEventSystemSeNecessario()
    {
        if (EventSystem.current != null)
        {
            return;
        }

        GameObject eventSystemObject = new GameObject("EventSystem");
        eventSystemObject.AddComponent<EventSystem>();
        eventSystemObject.AddComponent<StandaloneInputModule>();
    }

    private void ReiniciarJogo()
    {
        Time.timeScale = 1f;
        pontos = 0;
        jogoFinalizado = false;
        proximoMarcoImpacto = intervaloMensagemImpacto;

        if (GerenciadorJogo.instance != null)
        {
            GerenciadorJogo.instance = null;
            Destroy(gameObject);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void SairDoJogo()
    {
        Time.timeScale = 1f;
        Application.Quit();
    }
}
