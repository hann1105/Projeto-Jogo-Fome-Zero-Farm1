using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coletavel : MonoBehaviour
{
    public TipoColetavel tipo;
    public Sprite icon;
    public Rigidbody2D rb2d;
    private void Awake()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D colisao)
    {
        Jogador jogador = colisao.GetComponent<Jogador>();

        if (jogador)
        {
            jogador.inventario.Add(this);
            Destroy(this.gameObject);
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