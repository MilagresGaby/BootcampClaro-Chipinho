using UnityEngine;

public class CasaConectavel : MonoBehaviour
{
    private bool jaConectou = false;
    private MeshRenderer casaRenderer;

    private void Start()
    {
        casaRenderer = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Se for o jogador que passou e a casa ainda não foi conectada
        if (other.CompareTag("Player") && !jaConectou)
        {
            // Pergunta ao GameManager se temos energia para conectar
            if (GameManager.Instance != null && GameManager.Instance.TentarConectarCasa())
            {
                jaConectou = true;
                Debug.Log("Casa conectada com sucesso!");

                // EFEITO VISUAL TEMPORÁRIO: Muda a cor da casa para Verde para indicar sucesso
                if (casaRenderer != null)
                {
                    casaRenderer.material.color = Color.green;
                }

                // Aqui os artistas do seu grupo poderão ativar as animações/partículas de Wi-Fi depois
            }
        }
    }
}