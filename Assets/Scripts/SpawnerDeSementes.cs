using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerDeSementes : MonoBehaviour
{
    [Header("Sementes")]
    [SerializeField] private string[] nomesDasSementes =
    {
        "Sementes Cenoura",
        "Sementes Batata",
        "Sementes Repolho"
    };

    [Header("Spawn")]
    [SerializeField] private float intervaloEntreSpawns = 20f;
    [SerializeField] private bool spawnarAoIniciar = true;
    [SerializeField] private int maximoDeSementesNoChao = 3;
    [SerializeField] private Vector2 offsetDoSpawn = new Vector2(0f, -0.85f);
    [SerializeField] private float distanciaMinimaDoCentro = 0.45f;
    [SerializeField] private float raioAleatorio = 0.35f;
    [SerializeField] private float impulsoAoSpawnar = 0.25f;

    private readonly List<Item> sementesSpawnadas = new List<Item>();

    private void Start()
    {
        StartCoroutine(RotinaDeSpawn());
    }

    private IEnumerator RotinaDeSpawn()
    {
        if (spawnarAoIniciar)
        {
            SpawnarSementeAleatoria();
        }

        while (true)
        {
            yield return new WaitForSeconds(intervaloEntreSpawns);
            SpawnarSementeAleatoria();
        }
    }

    private void SpawnarSementeAleatoria()
    {
        LimparSementesColetadas();

        if (sementesSpawnadas.Count >= maximoDeSementesNoChao ||
            nomesDasSementes == null ||
            nomesDasSementes.Length == 0 ||
            GerenciadorJogo.instance == null ||
            GerenciadorJogo.instance.gerenciadorItem == null)
        {
            return;
        }

        Item prefab = ObterPrefabAleatorio();
        if (prefab == null)
        {
            return;
        }

        Vector2 deslocamento = GerarDeslocamentoSeguro();
        Vector2 origem = (Vector2)transform.position + offsetDoSpawn;
        Item semente = Instantiate(prefab, origem + deslocamento, Quaternion.identity);

        if (semente.rb2d != null && impulsoAoSpawnar > 0f)
        {
            semente.rb2d.AddForce(deslocamento.normalized * impulsoAoSpawnar, ForceMode2D.Impulse);
        }

        sementesSpawnadas.Add(semente);
    }

    private Vector2 GerarDeslocamentoSeguro()
    {
        Vector2 direcao = Random.insideUnitCircle.normalized;
        if (direcao == Vector2.zero)
        {
            direcao = Vector2.down;
        }

        float distanciaExtra = raioAleatorio > 0f ? Random.Range(0f, raioAleatorio) : 0f;
        return direcao * (distanciaMinimaDoCentro + distanciaExtra);
    }

    private Item ObterPrefabAleatorio()
    {
        int inicio = Random.Range(0, nomesDasSementes.Length);

        for (int i = 0; i < nomesDasSementes.Length; i++)
        {
            string nome = nomesDasSementes[(inicio + i) % nomesDasSementes.Length];
            Item item = GerenciadorJogo.instance.gerenciadorItem.ObterItemPorNome(nome);

            if (item != null)
            {
                return item;
            }
        }

        return null;
    }

    private void LimparSementesColetadas()
    {
        for (int i = sementesSpawnadas.Count - 1; i >= 0; i--)
        {
            if (sementesSpawnadas[i] == null)
            {
                sementesSpawnadas.RemoveAt(i);
            }
        }
    }
}
