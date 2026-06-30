using UnityEngine;

public class CameraDinamica : MonoBehaviour
{
    [SerializeField] private Transform alvo;
    [SerializeField] private float pixelsPorUnidade = 16f;
    [SerializeField] private float resolucaoVerticalReferencia = 1080f;
    [SerializeField] private int escalaPixelPerfect = 4;
    [SerializeField] private bool ajustarZoomNoPlay = true;

    private Vector3 compensacaoCamera;
    private Camera cameraComponente;

    private void Start()
    {
        cameraComponente = GetComponent<Camera>();
        if (cameraComponente != null)
        {
            cameraComponente.allowHDR = false;
            cameraComponente.allowMSAA = false;

            if (ajustarZoomNoPlay)
            {
                cameraComponente.orthographicSize = resolucaoVerticalReferencia / (pixelsPorUnidade * escalaPixelPerfect * 2f);
            }
        }

        compensacaoCamera = transform.position - alvo.position;
    }

    private void LateUpdate()
    {
        Vector3 targetPosition = alvo.position + compensacaoCamera;
        targetPosition.x = Mathf.Round(targetPosition.x * pixelsPorUnidade) / pixelsPorUnidade;
        targetPosition.y = Mathf.Round(targetPosition.y * pixelsPorUnidade) / pixelsPorUnidade;
        transform.position = targetPosition;
    }
}
