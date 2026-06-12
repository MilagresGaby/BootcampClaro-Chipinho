using UnityEngine;

public class Movement : MonoBehaviour
{
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
    private Vector3 velocity;
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

        // GRAVIDADE
        velocity.y += gravity * Time.deltaTime;

        // CÁLCULO FINAL DE MOVIMENTO (CORRIGIDO!)
        Vector3 targetPosition = transform.position;

        // Agora a matemática usa o ponto onde a pista realmente foi criada no mapa!
        if (desiredLane == 0) targetPosition.x = centroOriginalDaRua - laneDistance;
        else if (desiredLane == 1) targetPosition.x = centroOriginalDaRua;
        else if (desiredLane == 2) targetPosition.x = centroOriginalDaRua + laneDistance;

        Vector3 moveVector = Vector3.forward * currentSpeed * Time.deltaTime;
        Vector3 screenPos = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);
        moveVector.x = screenPos.x - transform.position.x;
        moveVector.y = velocity.y * Time.deltaTime;

        controller.Move(moveVector);
    }
}