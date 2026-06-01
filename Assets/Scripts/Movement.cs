using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    private Transform Player;

    // Faixas fixas para a pista larga de tamanho 18 (X fixo)
    private float[] lanesX = new float[] { -6f, 0f, 6f }; 
    private int currentLane = 1; // Começa no centro (X = 0)

    // VELOCIDADE DINÂMICA
    public float baseSpeed = 4f;       
    public float maxSpeed = 15f;       
    public float speedMultiplier = 0.05f; 
    public float currentSpeed; 

    // Variáveis do Pulo Manual Estável
    public float jumpHeight = 3.5f;   
    private float currentJumpSpeed = 12f;     
    private bool isJumping = false;
    private float currentJumpY = 0f;
    private float targetJumpY = 0f;

    // VARIÁVEIS PARA O PARKOUR (CHÃO DINÂMICO)
    private float currentFloorY = 0f; // Altura atual do chão detectado
    public LayerMask groundLayers;    // Quais camadas contam como chão sólido

    // Sistema de Agachar (Slide) Limpo
    private bool isCrouching = false;
    public float crouchDuration = 0.7f; 
    private float crouchTimer = 0f;

    private void Start()
    {
        Player = GetComponent<Transform>();
        currentSpeed = baseSpeed; 
    }

    private void Update()
    {
        // 1. ACELERAÇÃO DO JOGO
        currentSpeed = Mathf.MoveTowards(currentSpeed, maxSpeed, speedMultiplier * Time.deltaTime);
        float forwardMovement = currentSpeed * Time.deltaTime;

        // 2. DETECÇÃO DE COMANDOS UNIFICADOS
        bool apertouEsquerda = Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeControls.Instance.swipeleft;
        bool apertouDireita = Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeControls.Instance.swiperight;
        bool apertouPulo = Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || SwipeControls.Instance.swipeup;
        bool apertouAgachar = Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow);

        // 3. TROCA DE FAIXAS FIXAS
        if (apertouEsquerda && currentLane > 0) currentLane--; 
        if (apertouDireita && currentLane < 2) currentLane++; 

        float targetX = lanesX[currentLane];
        float newX = Mathf.MoveTowards(Player.position.x, targetX, 25f * Time.deltaTime);

        // 4. LASER INVISÍVEL (RAYCAST) PARA DETECTAR O CHÃO ABAIXO DO PLAYER
        RaycastHit hit;
        Vector3 rayStart = new Vector3(Player.position.x, Player.position.y + 2f, Player.position.z);
        
        if (Physics.Raycast(rayStart, Vector3.down, out hit, 10f, groundLayers))
        {
            currentFloorY = hit.point.y;
        }
        else
        {
            currentFloorY = 0f;
        }

        // 5. LÓGICA DO PULO MANUAL UNIFICADO
        if (apertouPulo && !isJumping && !isCrouching)
        {
            isJumping = true;
            targetJumpY = jumpHeight;
            currentJumpSpeed = 12f; 

            Animator anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.ResetTrigger("Crouch");
                anim.SetTrigger("Jump");
            }
        }

        if (isJumping)
        {
            currentJumpY = Mathf.MoveTowards(currentJumpY, targetJumpY, currentJumpSpeed * Time.deltaTime);
            if (Mathf.Approximately(currentJumpY, jumpHeight) && targetJumpY != 0f) targetJumpY = 0f;
            if (Mathf.Approximately(currentJumpY, 0f) && targetJumpY == 0f) 
            {
                isJumping = false;
                currentJumpSpeed = 12f; 
            }
        }

        // 6. LÓGICA DE AGACHAR (SLIDE)
        if (apertouAgachar && !isCrouching)
        {
            isCrouching = true;
            crouchTimer = crouchDuration;

            if (isJumping)
            {
                targetJumpY = 0f;          
                currentJumpSpeed = 28f;    
            }

            Animator anim = GetComponentInChildren<Animator>();
            if (anim != null)
            {
                anim.ResetTrigger("Jump");
                anim.SetTrigger("Crouch");
            }
        }

        if (isCrouching)
        {
            crouchTimer -= Time.deltaTime;
            if (crouchTimer <= 0)
            {
                isCrouching = false;
            }
        }

        // 7. APLICAÇÃO FINAL DA MOVIMENTAÇÃO DINÂMICA
        float newY = currentJumpY + 1.0f; 
        float newZ = Player.position.z + forwardMovement;

        Player.position = new Vector3(newX, newY, newZ);
    }
}