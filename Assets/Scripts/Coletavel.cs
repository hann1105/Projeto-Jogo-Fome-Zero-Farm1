using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public TipoColetavel tipo;
    public int quantidade = 1;

    private void OnTriggerEnter2D(Collider2D colisao)
    {
        Jogador jogador = colisao.GetComponent<Jogador>();

        if (jogador != null)
        {
            jogador.inventario.AdicionarItem(tipo, quantidade);
            Destroy(gameObject);
        }
    }
}

public enum TipoColetavel
{
    NONE,
    MACA,
    SEMENTE_CENOURA,
    SEMENTE_BATATA,
    SEMENTE_MORANGO,
    SEMENTE_TOMATE,
    SEMENTE_REPOLHO


}