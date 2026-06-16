using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic; 
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject painelDerrotaUI;

    [Header("UI - Textos e Painéis")]
    public TextMeshProUGUI textoPontos; //
    public TextMeshProUGUI textoCasasConectadas; //
    public Slider barraEnergiaUI; //

    [Header("Mecânicas de Jogo")]
    public float pontosAtuais = 0f; //
    public int casasConectadas = 0; //
    public int metaDeCasas = 60; //
    
    public float energiaAtual = 0f; //
    public float energiaMaxima = 100f; //

    [Header("Contadores Específicos por Fase")]
    [HideInInspector] public int torresConectadas = 0;
    [HideInInspector] public int casasSolaresConectadas = 0;

    [Header("Diálogo de Vitória (Final da Fase)")]
    public List<LinhaDialogo> dialogoFinalDaFase; // 🔥 O campo vai aparecer aqui no Inspector!
    private bool faseConcluida = false; //

    private Transform playerTransform; //
    private float pontosPorDistanciaacumulados = 0f; //

    private void Awake()
    {
        if (Instance == null) Instance = this; //
        else Destroy(gameObject); //
    }

    private void Start()
    {
        Application.targetFrameRate = 60; //
        Movement player = FindAnyObjectByType<Movement>(); //
        if (player != null) playerTransform = player.transform; //

        AtualizarInterface(); //
    }
    public void DispararDerrota()
{
    // 1. Ativa visualmente o menu de derrota na tela
    if (painelDerrotaUI != null)
    {
        painelDerrotaUI.SetActive(true);
    }

    // 2. Opcional: Pausa o tempo do jogo para os carros e o cenário pararem de mover
    Time.timeScale = 0f; 

    Debug.Log("💀 FIM DE JOGO! O jogador perdeu.");
}

    private void Update()
    {
        if (playerTransform != null && playerTransform.GetComponent<Movement>().enabled) //
        {
            pontosPorDistanciaacumulados += playerTransform.GetComponent<Movement>().currentSpeed * Time.deltaTime * 0.5f; //
            
            // Perde 2f de energia por segundo correndo
            AdicionarEnergia(-2f * Time.deltaTime); //

            AtualizarInterface(); //
        }
    }

    public void AdicionarEnergia(float quantidade)
    {
        energiaAtual = Mathf.Clamp(energiaAtual + quantidade, 0f, energiaMaxima); //
        if (barraEnergiaUI != null) barraEnergiaUI.value = energiaAtual / energiaMaxima; //
    }

    // 🎯 NOVA VERSÃO INTELIGENTE: Recebe o tipo da casa por parâmetro automaticamente!
    public bool TentarConectarCasa(CasaConectavel.TipoConstrucao tipo)
    {
        if (energiaAtual >= 20f) //
        {
            energiaAtual -= 20f; //
            casasConectadas++; //

            // Contabiliza de acordo com o tipo selecionado na Unity
            if (tipo == CasaConectavel.TipoConstrucao.TorreFase1)
            {
                torresConectadas++;
                pontosAtuais += 250f; // Torres dão um bônus maior de pontos!
            }
            else if (tipo == CasaConectavel.TipoConstrucao.CasaSolarFase2)
            {
                casasSolaresConectadas++;
                pontosAtuais += 150f; // Painéis solares dão um pouco mais
            }
            else
            {
                pontosAtuais += 100f; //
            }

            if (barraEnergiaUI != null) barraEnergiaUI.value = energiaAtual / energiaMaxima; //
            AtualizarInterface(); //

            // 🔥 CHECAGEM DA VITÓRIA CRUCIAL
            if (casasConectadas >= metaDeCasas && !faseConcluida) //
            {
                faseConcluida = true; //
                
                GerenciadorDialogo gd = FindAnyObjectByType<GerenciadorDialogo>(); //
                if (gd != null) //
                {
                    gd.IniciarDialogoFinal(dialogoFinalDaFase); //
                }
                else
                {
                    Debug.LogError("Não encontrei o GerenciadorDialogo na cena para rodar as falas finais!"); //
                }
            }

            return true; //
        }
        return false; //
    }

    public void AdicionarModulo()
    {
        pontosAtuais += 50f; //
        AtualizarInterface(); //
    }

    private int AtuaisPontosTotais()
    {
        return Mathf.RoundToInt(pontosAtuais + pontosPorDistanciaacumulados); //
    }

    private void AtualizarInterface()
    {
        if (textoPontos != null) //
            textoPontos.text = AtuaisPontosTotais().ToString("N0").Replace(",", "."); //

        if (textoCasasConectadas != null) //
        {
            // Se o script detectar que você ativou torres na cena, atualiza o texto para "Torres:"
            if (torresConectadas > 0)
            {
                textoCasasConectadas.text = "Torres: " + casasConectadas + " / " + metaDeCasas;
            }
            // Se detectar casas solares, muda o texto automaticamente para "Painéis:"
            else if (casasSolaresConectadas > 0)
            {
                textoCasasConectadas.text = "Painéis: " + casasConectadas + " / " + metaDeCasas;
            }
            // Fallback para o padrão original de casas comuns
            else
            {
                textoCasasConectadas.text = casasConectadas + " / " + metaDeCasas; //
            }
        }
    }
}