using UnityEngine;

public class Movement : MonoBehaviour
{
    [Header("Configurações de Pista")]
    public float laneDistance = 6f; 
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

    private void Start()
    {
        controller = GetComponent<CharacterController>();
        anim = GetComponent<Animator>(); 
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

        // DESLIZAR / CORTE DE PULO (CORRIGIDO: REMOVIDO O ISGROUNDED!)
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S) || SwipeControls.Instance.SwipeDown)
        {
            if (anim != null) anim.SetTrigger("Crouch"); 

            // Se o jogador estiver no meio de um pulo, joga ele direto para o chão rápido (Dinamismo!)
            if (!isGrounded)
            {
                velocity.y = -jumpForce; 
            }
        }

        // GRAVIDADE
        velocity.y += gravity * Time.deltaTime;

        // CÁLCULO FINAL DE MOVIMENTO
        Vector3 targetPosition = transform.position;

        if (desiredLane == 0) targetPosition.x = -laneDistance;
        else if (desiredLane == 1) targetPosition.x = 0;
        else if (desiredLane == 2) targetPosition.x = laneDistance;

        Vector3 moveVector = Vector3.forward * currentSpeed * Time.deltaTime;
        Vector3 screenPos = Vector3.Lerp(transform.position, targetPosition, laneSpeed * Time.deltaTime);
        moveVector.x = screenPos.x - transform.position.x;
        moveVector.y = velocity.y * Time.deltaTime;

        controller.Move(moveVector);
    }
}