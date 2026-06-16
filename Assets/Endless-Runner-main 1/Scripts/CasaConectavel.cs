using UnityEngine;
using System.Collections; //

public class CasaConectavel : MonoBehaviour
{
    // 🎛️ Cria um menu de opções diretamente no Inspector da Unity
    public enum TipoConstrucao { Normal, TorreFase1, CasaSolarFase2 }

    [Header("Configuração do Tipo")]
    public TipoConstrucao tipoDestaConstrucao = TipoConstrucao.Normal;

    [Header("Cores de Brilho Especiais")]
    public Color corTorre = Color.cyan;       // Azul elétrico para a Torre
    public Color corSolar = Color.yellow;     // Amarelo para a Casa Solar
    public Color corNormal = Color.green;     // Verde padrão original

    private bool jaConectou = false; //
    private MeshRenderer casaRenderer; //

    private void Start()
    {
        casaRenderer = GetComponent<MeshRenderer>(); //
    }

    public void ConectarPelaLinha() //
    {
        if (!jaConectou) //
        {
            if (GameManager.Instance != null && GameManager.Instance.TentarConectarCasa(tipoDestaConstrucao)) //
            {
                jaConectou = true; //
                Debug.Log($"Construção do tipo {tipoDestaConstrucao} conectada com sucesso!");

                // 🎨 SISTEMA UNIFICADO DE BRILHO: Muda a cor baseado na sua escolha do Inspector
                if (casaRenderer != null)
                {
                    switch (tipoDestaConstrucao)
                    {
                        case TipoConstrucao.TorreFase1:
                            casaRenderer.material.color = corTorre;
                            break;
                        case TipoConstrucao.CasaSolarFase2:
                            casaRenderer.material.color = corSolar;
                            break;
                        default:
                            casaRenderer.material.color = corNormal; //
                            break;
                    }
                }

                // ATIVA A ANIMAÇÃO DE PULSO VISUAL
                StartCoroutine(AnimarPulsoCasa()); //

                // Dispara o efeito do raio de conexão
                EfeitoRaioConexao raioEfeito = FindAnyObjectByType<EfeitoRaioConexao>(); //
                if (raioEfeito != null) //
                {
                    GameObject jogador = GameObject.FindGameObjectWithTag("Player"); //
                    if (jogador != null) //
                    {
                        raioEfeito.DispararRaio(jogador.transform.position, transform.position); //
                    }
                }
            }
        }
    }

    private IEnumerator AnimarPulsoCasa() //
    {
        Vector3 escalaOriginal = transform.localScale; //
        Vector3 escalaMaxima = escalaOriginal * 1.3f; //

        float tempo = 0f; //
        float duracao = 0.15f; //

        // Fase 1: Crescer
        while (tempo < duracao) //
        {
            tempo += Time.deltaTime; //
            transform.localScale = Vector3.Lerp(escalaOriginal, escalaMaxima, tempo / duracao); //
            yield return null; //
        }

        tempo = 0f; //
        // Fase 2: Voltar ao tamanho normal
        while (tempo < duracao) //
        {
            tempo += Time.deltaTime; //
            transform.localScale = Vector3.Lerp(escalaMaxima, escalaOriginal, tempo / duracao); //
            yield return null; //
        }

        transform.localScale = escalaOriginal; //
    }

    private void OnTriggerEnter(Collider other) //
    {
        // Mantém a colisão direta do jogador funcionando em harmonia se ele encostar fisicamente
        if (other.CompareTag("Player") && !jaConectou) //
        {
            ConectarPelaLinha(); 
        }
    }
}