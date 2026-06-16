using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform alvo; // Arraste seu Player para cá no Inspector
    public Vector3 offset = new Vector3(0f, 13.6f, -7.4f); // Altura e distância da câmera
    public float velocidadeProcura = 10f;

    void LateUpdate()
    {
        if (alvo != null)
        {
            // Calcula a posição que a câmera deveria estar
            Vector3 posicaoDesejada = alvo.position + offset;
            // Suaviza o movimento para não dar trancos
            transform.position = Vector3.Lerp(transform.position, posicaoDesejada, velocidadeProcura * Time.deltaTime);
            // Faz a câmera sempre olhar para o jogador
            Vector3 pontoDeOlhar = alvo.position + new Vector3(0f, 1f, 0f);
            transform.LookAt(pontoDeOlhar);
        }
    }
}