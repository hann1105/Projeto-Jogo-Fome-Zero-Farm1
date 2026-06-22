using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jogador : MonoBehaviour
{
    public Inventario inventario;

    private void Awake()
    {
        inventario = GetComponent<Inventario>();
    }

    public void DroparItem(Coletavel item)
    {
        Vector2 localOrigem = transform.position;

        Vector2 deslocamento = Random.insideUnitCircle * 1.25f;

        Coletavel itemDropado = Instantiate(item, localOrigem + deslocamento, Quaternion.identity);

        itemDropado.rb2d.AddForce(deslocamento * 2f, ForceMode2D.Impulse);
    }
}