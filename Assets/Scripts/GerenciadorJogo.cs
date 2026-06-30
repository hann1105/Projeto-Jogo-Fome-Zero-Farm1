using UnityEngine;

public class GerenciadorJogo : MonoBehaviour
{
    public static GerenciadorJogo instance;

    public GerenciadorItem gerenciadorItem;
    public GerenciadorDeBlocos gerenciadorDeBlocos;
    public GerenciadorDePlantacao gerenciadorDePlantacao;
    public GerenciadorPontuacao gerenciadorPontuacao;

    public void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(gameObject);

        gerenciadorItem = GetComponent<GerenciadorItem>();
        gerenciadorDeBlocos = GetComponent<GerenciadorDeBlocos>();
        gerenciadorDePlantacao = GetComponent<GerenciadorDePlantacao>();
        gerenciadorPontuacao = GetComponent<GerenciadorPontuacao>();

        if (gerenciadorDePlantacao == null)
        {
            gerenciadorDePlantacao = gameObject.AddComponent<GerenciadorDePlantacao>();
        }

        if (gerenciadorPontuacao == null)
        {
            gerenciadorPontuacao = gameObject.AddComponent<GerenciadorPontuacao>();
        }
    }
}
