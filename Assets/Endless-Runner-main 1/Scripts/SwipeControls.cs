using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    public static SwipeControls Instance;

    private bool tap, swipeLeft, swipeRight, swipeUp, swipeDown;
    private bool isDraging = false;
    private Vector2 startTouch, swipeDelta;

    public bool Tap { get { return tap; } }
    public Vector2 SwipeDelta { get { return swipeDelta; } }
    public bool SwipeLeft { get { return swipeLeft; } }
    public bool SwipeRight { get { return swipeRight; } }
    public bool SwipeUp { get { return swipeUp; } }
    public bool SwipeDown { get { return swipeDown; } }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    private void Update()
    {
        // Reseta todos os comandos no início de cada frame
        tap = swipeLeft = swipeRight = swipeUp = swipeDown = false;

        // 📱 SUPORTE PARA COMPUTADOR (MOUSEDOWN / MOUSEUP)
        if (Input.GetMouseButtonDown(0))
        {
            tap = true;
            isDraging = true;
            startTouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDraging = false;
            ResetSwipe();
        }

        // 📱 SUPORTE PARA CELULAR (TOUCH)
        if (Input.touches.Length > 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                tap = true;
                isDraging = true;
                startTouch = Input.touches[0].position;
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                isDraging = false;
                ResetSwipe();
            }
        }

        // CALCULAR A DISTÂNCIA DO ARRASTO CORRETAMENTE
        swipeDelta = Vector2.zero;
        if (isDraging)
        {
            if (Input.touches.Length > 0)
                swipeDelta = Input.touches[0].position - startTouch;
            else if (Input.GetMouseButton(0))
                swipeDelta = (Vector2)Input.mousePosition - startTouch;
        }

        // CRUZOU A ZONA MORTA? (Verifica se o arrasto foi longo o suficiente)
        if (swipeDelta.magnitude > 80) // Reduzido de 100 para 80 para ficar mais responsivo!
        {
            float x = swipeDelta.x;
            float y = swipeDelta.y;
            
            // Verifica se o movimento foi mais Horizontal do que Vertical
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0) swipeLeft = true;
                else swipeRight = true;
            }
            else // Se foi mais Vertical do que Horizontal
            {
                if (y < 0) swipeDown = true;
                else swipeUp = true;
            }

            ResetSwipe();
        }
    }

    private void ResetSwipe()
    {
        startTouch = swipeDelta = Vector2.zero;
        isDraging = false;
    }
}