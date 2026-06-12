using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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

    [Header("Referências do Jogador")]
    private Movement playerMovement; 
    private Animator playerAnim;     

    private void Start()
    {
        playerMovement = FindAnyObjectByType<Movement>();

        if (playerMovement != null)
        {
            playerAnim = playerMovement.GetComponent<Animator>();
            if (playerAnim == null)
            {
                playerAnim = playerMovement.GetComponentInChildren<Animator>();
            }
        }

        if (playerMovement != null)
        {
            playerMovement.enabled = false; // Desativa o script de correr
            
            // 🔥 TRUQUE DO PAUSE: Como não temos Idle, congelamos a animação atual em velocidade 0!
            if (playerAnim != null)
            {
                playerAnim.speed = 0f; 
            }
        }

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
        painelDialogo.SetActive(false); 

        if (playerMovement != null)
        {
            playerMovement.enabled = true; // Reativa a corrida
            
            // 🔥 TRUQUE DO PLAY: O diálogo acabou, despausamos a animação voltando a velocidade para 1!
            if (playerAnim != null)
            {
                playerAnim.speed = 1f; 
            }
        }
    }
    
    public void IniciarDialogoFinal(List<LinhaDialogo> falasFinais)
    {
        indiceAtual = 0;
        dialogosDaFase = falasFinais;

        Movement player = FindAnyObjectByType<Movement>();
        if (player != null) 
        {
            player.enabled = false;
            
            // Pausa a animação na tela de vitória também
            Animator animFinal = player.GetComponentInChildren<Animator>();
            if (animFinal != null) animFinal.speed = 0f;
        }

        painelDialogo.SetActive(true);
        ExibirProximaFala();
    }
}