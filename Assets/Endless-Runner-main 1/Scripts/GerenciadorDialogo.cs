using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement; 

[System.Serializable]
public class LinhaDialogo
{
    public string nome;       // Quem fala (Aya ou Chipinho)
    public Sprite avatar;     // A foto do personagem que vai aparecer
    [TextArea(3, 5)]
    public string texto;      // A frase que vai ser dita
}

public class GerenciadorDialogo : MonoBehaviour
{
    [Header("Componentes do Canvas")]
    public GameObject painelDialogo;
    public TextMeshProUGUI campoNome;
    public TextMeshProUGUI campoTexto;
    public Image campoAvatar;

    [Header("Lista de Diálogos da Fase")]
    public List<LinhaDialogo> dialogosDaFase;
    private int indiceAtual = 0;

    // A VARIÁVEL INTERRUPTOR: Falsa por padrão, vira verdadeira apenas na vitória!
    private bool ehODialogoFinal = false; 

    [Header("Referências do Jogador")]
    private Movement playerMovement; 
    private Animator playerAnim;     

    private void Start()
{
    // 🔥 FALTAVA ESSA LINHA DAQUI PARA ENCONTRAR O PLAYER NA CENA!
    playerMovement = FindAnyObjectByType<Movement>();

    if (playerMovement != null)
    {
        // Encontra o componente Animator na Aya
        playerAnim = playerMovement.GetComponent<Animator>();
        if (playerAnim == null)
        {
            playerAnim = playerMovement.GetComponentInChildren<Animator>();
        }

        // Agora sim configuramos o estado inicial do diálogo com segurança
        playerMovement.enabled = false; 
        
        if (playerAnim != null)
        {
            playerAnim.speed = 1f;
        }
    }

    // Inicializa o Canvas se houver texto
    if (dialogosDaFase.Count > 0)
    {
        painelDialogo.SetActive(true);
        ExibirProximaFala();
    }
    else
    {
        FinalizarDialogo();
    }
}

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (painelDialogo.activeSelf)
            {
                ExibirProximaFala();
            }
        }
    }

    public void ExibirProximaFala()
    {
        if (indiceAtual >= dialogosDaFase.Count)
        {
            FinalizarDialogo();
            return;
        }

        LinhaDialogo falaAtual = dialogosDaFase[indiceAtual];
        campoNome.text = falaAtual.nome;
        campoAvatar.sprite = falaAtual.avatar;
        campoTexto.text = falaAtual.texto;

        indiceAtual++;
    }

    private void FinalizarDialogo()
    {
        painelDialogo.SetActive(false); //

        // =======================================================
        // 🏆 SE FOR O DIÁLOGO DA VITÓRIA, CARREGA A TELA DE VITÓRIA!
        // =======================================================
        if (ehODialogoFinal)
        {
            SceneManager.LoadScene("TelaVitoria");
            return; 
        }

        if (playerMovement != null) //
        {
            playerMovement.velocity = Vector3.zero; //
            playerMovement.enabled = true; //
            
            if (playerAnim != null) //
            {
                playerAnim.SetTrigger("StartRun"); //
            }
        }
    }
    
    public void IniciarDialogoFinal(List<LinhaDialogo> falasFinais)
    {
        indiceAtual = 0;
        dialogosDaFase = falasFinais;
        ehODialogoFinal = true; // INTERRUPTOR LIGADO!

        Movement player = FindAnyObjectByType<Movement>();
        if (player != null) 
        {
            player.enabled = false; // Para o avanço da corrida
            
            playerAnim = player.GetComponent<Animator>();
            if (playerAnim == null) playerAnim = player.GetComponentInChildren<Animator>();

            if (playerAnim != null)
            {
                playerAnim.speed = 1f;
                playerAnim.SetTrigger("Win");
            }
        }

        painelDialogo.SetActive(true);
        ExibirProximaFala();
    }
}