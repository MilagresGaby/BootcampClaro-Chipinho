using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("UI - Textos e Painéis")]
    public TextMeshProUGUI textoPontos;
    public TextMeshProUGUI textoCasasConectadas;
    public Slider barraEnergiaUI; 

    [Header("Mecânicas de Jogo")]
    public float pontosAtuais = 0f;
    public int casasConectadas = 0;
    public int metaDeCasas = 60;
    
    public float energiaAtual = 0f;
    public float energiaMaxima = 100f;

    private Transform playerTransform;
    private float pontosPorDistanciaacumulados = 0f;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Start()
    {
        Movement player = FindAnyObjectByType<Movement>();
        if (player != null) playerTransform = player.transform;

        AtualizarInterface();
    }

    private void Update()
{
    if (playerTransform != null && playerTransform.GetComponent<Movement>().enabled)
    {
        pontosPorDistanciaacumulados += playerTransform.GetComponent<Movement>().currentSpeed * Time.deltaTime * 0.5f;
        
        // ADICIONE ESSA LINHA ABAIXO: Perde 2f de energia por segundo correndo
        AdicionarEnergia(-2f * Time.deltaTime); 

        AtualizarInterface();
    }
}

    public void AdicionarEnergia(float quantidade)
    {
        // CORRIGIDO: Alterado de 'quantity' para 'quantidade'
        energiaAtual = Mathf.Clamp(energiaAtual + quantidade, 0f, energiaMaxima);
        if (barraEnergiaUI != null) barraEnergiaUI.value = energiaAtual / energiaMaxima;
    }

    public bool TentarConectarCasa()
    {
        if (energiaAtual >= 20f)
        {
            energiaAtual -= 20f; 
            casasConectadas++;
            pontosAtuais += 100f; 

            if (barraEnergiaUI != null) barraEnergiaUI.value = energiaAtual / energiaMaxima;
            AtualizarInterface();
            return true; 
        }
        return false; 
    }

    public void AdicionarModulo()
    {
        pontosAtuais += 50f;
        AtualizarInterface();
    }

    private int AtuaisPontosTotais()
    {
        return Mathf.RoundToInt(pontosAtuais + pontosPorDistanciaacumulados);
    }

    // CORRIGIDO: Nome alterado para bater exatamente com as chamadas do código
    private void AtualizarInterface()
    {
        if (textoPontos != null) 
            textoPontos.text = AtuaisPontosTotais().ToString("N0").Replace(",", "."); 

        if (textoCasasConectadas != null) 
            textoCasasConectadas.text = casasConectadas + " / " + metaDeCasas;
    }
}