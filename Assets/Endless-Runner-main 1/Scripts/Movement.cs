using UnityEngine;

public class Movement : MonoBehaviour
{
    public bool emDialogo = false;
    [Header("Configurações de Pista")]
    public float laneDistance = 4f; // Reduzido de 6 para 4 para combinar com a largura visual da sua rua!
    public float laneSpeed = 15f;   
    private int desiredLane = 1;    

    [Header("Configurações de Velocidade")]
    public float currentSpeed = 17f; 
    public float maxSpeed = 35f;
    public float speedIncrease = 0.05f;

    [Header("Física e Pulo")]
    public float jumpForce = 8f;     
    public float gravity = -25f;     
    public Vector3 velocity;
    private bool isGrounded;

    private CharacterController controller;
    private Animator anim;

    // 🔥 VARIÁVEL DA VITÓRIA: Guarda a posição X real onde o jogo começou!
    private float centroOriginalDaRua;

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); 

        // Salva exatamente o X em que a Aya foi colocada no editor (ex: 101.4)
        centroOriginalDaRua = transform.position.x;
    }

    private void Update()
    {
        if (emDialogo)
    {
        // Se você usa Character Controller ou Rigidbody, pode zerar a velocidade aqui
        // ex: velocity.y = 0;
        return; 
    }
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -1f; 
        }

        if (currentSpeed < maxSpeed)
        {
            currentSpeed += speedIncrease * Time.deltaTime;
        }

        // ENTRADAS DE COMANDO (LATERAIS)
        if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || SwipeControls.Instance.SwipeRight)
        {
            if (desiredLane < 2) desiredLane++;
        }

        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || SwipeControls.Instance.SwipeLeft)
        {
            if (desiredLane > 0) desiredLane--;
        }

        // PULO (Apenas se estiver no chão)
        if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || SwipeControls.Instance.SwipeUp) && isGrounded)
        {
            velocity.y = jumpForce;
            if (anim != null) anim.SetTrigger("Jump"); 
        }

        // DESLIZAR / CORTE DE PULO
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeControls.Instance.SwipeDown)
        {
            if (anim != null) anim.SetTrigger("Crouch"); 

            if (!isGrounded)
            {
                velocity.y = -jumpForce; 
            }
        }

       // GRAVIDADE ORIGINAL (Volta a usar Time.deltaTime puro)
        velocity.y += gravity * Time.deltaTime;

        // CÁLCULO FINAL DE MOVIMENTO
        Vector3 targetPosition = transform.position;

        if (desiredLane == 0) targetPosition.x = centroOriginalDaRua - laneDistance;
        else if (desiredLane == 1) targetPosition.x = centroOriginalDaRua;
        else if (desiredLane == 2) targetPosition.x = centroOriginalDaRua + laneDistance;

        // Volta a usar Time.deltaTime para a velocidade normalizar!
        Vector3 moveVector = Vector3.forward * currentSpeed * Time.deltaTime;
        Vector3 screenPos = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);
        moveVector.x = screenPos.x - transform.position.x;
        moveVector.y = velocity.y * Time.deltaTime;

        // Move o personagem
        controller.Move(moveVector);

        // 🔥 REDE DE PROTEÇÃO CONTRA O LIMBO (Coloque logo após o controller.Move)
        // Se por qualquer bug de colisão ou dano a Aya cair abaixo do nível da rua (Y = -2)
        if (transform.position.y < -2f)
        {
            // 1. Teletransporta ela de volta para a altura certa do asfalto (Y = 0 ou 0.5f)
            Vector3 posicaoCorrigida = transform.position;
            posicaoCorrigida.y = 0.5f; 
            transform.position = posicaoCorrigida;

            // 2. Zera a velocidade de queda para ela não continuar acumulando força para baixo
            velocity.y = 0f;

            Debug.LogWarning("Aya caiu do mundo! Rede de proteção ativada para salvá-la.");
        }
        }
        }