using UnityEngine;
using System.Collections.Generic;

// ═══════════════════════════════════════════════════════
// GRADEDEROCA — Grid 5×4 de células de plantio
// Onde colocar: GameObject vazio "GradeDeRoca" na cena
// ═══════════════════════════════════════════════════════
public class GradeDeRoca : MonoBehaviour
{
    public static GradeDeRoca Instance;

    [Header("=== CONFIGURAÇÃO DO GRID ===")]
    public int colunas = 5;
    public int linhas = 4;
    public float espacamento = 1.2f;    // distância entre células
    public GameObject prefabCelula;     // prefab de cada célula de solo

    [Header("=== CULTURAS DISPONÍVEIS ===")]
    // Arraste os ScriptableObjects de Cultura aqui
    public List<Cultura> culturasDisponiveis;

    // Grid interno: matriz de células
    private CelulaRoca[,] grade;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        CriarGrid();
    }

    // ── CRIAR O GRID NO INÍCIO ───────────────────────────
    void CriarGrid()
    {
        grade = new CelulaRoca[colunas, linhas];

        for (int x = 0; x < colunas; x++)
        {
            for (int y = 0; y < linhas; y++)
            {
                // Calcula posição no mundo
                Vector3 posicao = transform.position +
                    new Vector3(x * espacamento, y * espacamento, 0);

                // Instancia o prefab da célula
                GameObject obj = Instantiate(prefabCelula, posicao, Quaternion.identity);
                obj.transform.parent = transform;
                obj.name = $"Celula_{x}_{y}";

                // Guarda referência e inicializa
                CelulaRoca celula = obj.GetComponent<CelulaRoca>();
                celula.Inicializar(x, y);
                grade[x, y] = celula;
            }
        }
    }

    // ── ACESSO AO GRID ───────────────────────────────────
    public CelulaRoca ObterCelula(int x, int y)
    {
        if (x < 0 || x >= colunas || y < 0 || y >= linhas) return null;
        return grade[x, y];
    }

    // ── EVENTOS CLIMÁTICOS ───────────────────────────────
    public void AplicarSecaEmTodas()
    {
        foreach (var celula in grade)
            celula.AplicarSeca();
    }

    public void AplicarPragaEmTodas()
    {
        // Remove uma célula aleatória que tenha planta
        List<CelulaRoca> comPlanta = new List<CelulaRoca>();
        foreach (var celula in grade)
            if (celula.temPlanta) comPlanta.Add(celula);

        if (comPlanta.Count > 0)
        {
            int sorteio = Random.Range(0, comPlanta.Count);
            comPlanta[sorteio].DestruirPlanta();
        }
    }

    public void AplicarChuvaEmTodas()
    {
        foreach (var celula in grade)
            celula.Irrigar(gratis: true);
    }

    // ── AVANÇO DE DIA (chamado pelo GameClock) ───────────
    public void AvancarDia()
    {
        foreach (var celula in grade)
            celula.AvancarDia();
    }
}
