using UnityEngine;

public class CameraDinamica : MonoBehaviour
{
    [SerializeField] private Transform alvo; // Traduzido de 'target' para 'alvo'

    private Vector3 compensacaoCamera; // Traduzido de 'camOffset' para 'compensacaoCamera'

    void Start()
    {
        // Define a distância inicial entre a câmera e o alvo
        compensacaoCamera = transform.position - alvo.position;
    }

    private void FixedUpdate()
    {
        // A câmera mantém sempre a mesma distância do alvo
        transform.position = alvo.position + compensacaoCamera;
    }

}
