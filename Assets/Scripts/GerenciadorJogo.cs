using UnityEngine;

public class GerenciadorJogo : MonoBehaviour
{
    public static GerenciadorJogo instance;
    
    public GerenciadorItem gerenciadorItem;
    public GerenciadorDeBlocos gerenciadorDeBlocos;

    public void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        gerenciadorItem = GetComponent<GerenciadorItem>();
        gerenciadorDeBlocos = GetComponent<GerenciadorDeBlocos>();
    }
}
