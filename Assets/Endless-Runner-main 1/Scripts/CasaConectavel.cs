using UnityEngine;
using System.Collections; // <-- ADICIONE ESTA LINHA AQUI NO TOPO!


    // O resto do seu código continua exatamente igual...
public class CasaConectavel : MonoBehaviour
{
    private bool jaConectou = false;
    private MeshRenderer casaRenderer;

    private void Start()
    {
        casaRenderer = GetComponent<MeshRenderer>();
    }

// Substitua ou atualize esta função no seu CasaConectavel.cs
public void ConectarPelaLinha()
{
    if (!jaConectou)
    {
        if (GameManager.Instance != null && GameManager.Instance.TentarConectarCasa())
        {
            jaConectou = true;
            Debug.Log("Casa conectada com sucesso!");

            if (casaRenderer != null)
            {
                casaRenderer.material.color = Color.green;
            }

            // ATIVA A ANIMAÇÃO DE PULSO VISUAL
            StartCoroutine(AnimarPulsoCasa());

          // Procure essa parte dentro do seu CasaConectavel.cs e mude para isto:
EfeitoRaioConexao raioEfeito = FindAnyObjectByType<EfeitoRaioConexao>();
if (raioEfeito != null)
{
    // Acha o objeto do jogador na cena para pegar a posição real dele
    GameObject jogador = GameObject.FindGameObjectWithTag("Player");
    if (jogador != null)
    {
        // Dispara o raio saindo do corpo do Player até a casa!
        raioEfeito.DispararRaio(jogador.transform.position, transform.position);
    }
}
    }
    }
}

private IEnumerator AnimarPulsoCasa()
{
    Vector3 escalaOriginal = transform.localScale;
    // Define o tamanho máximo do "pulo" visual da casa (1.3x maior)
    Vector3 escalaMaxima = escalaOriginal * 1.3f; 

    float tempo = 0f;
    float duracao = 0.15f; // Rápido para dar sensação de impacto

    // Fase 1: Crescer
    while (tempo < duracao)
    {
        tempo += Time.deltaTime;
        transform.localScale = Vector3.Lerp(escalaOriginal, escalaMaxima, tempo / duracao);
        yield return null;
    }

    tempo = 0f;
    // Fase 2: Voltar ao tamanho normal
    while (tempo < duracao)
    {
        tempo += Time.deltaTime;
        transform.localScale = Vector3.Lerp(escalaMaxima, escalaOriginal, tempo / duracao);
        yield return null;
    }

    transform.localScale = escalaOriginal; // Garante o tamanho exato no fim
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
