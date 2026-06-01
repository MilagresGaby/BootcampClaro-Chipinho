using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeControls : MonoBehaviour
{
    #region Instance
    private static SwipeControls instance;
    public static SwipeControls Instance
    {
        get
        {
            if (instance == null)
            {
                // CORRIGIDO: Usa o método moderno recomendado pela Unity
                instance = FindAnyObjectByType<SwipeControls>();
                if (instance == null)
                {
                    instance = new GameObject("Spawned SwipeControls", typeof(SwipeControls)).GetComponent<SwipeControls>();
                }
            }
            return instance;
        }
        set
        {
            instance = value;
        }
    }
    #endregion

    private float deadzone = 5f;
    public bool swipeup; 
    public bool swipeleft, swiperight;
    private Vector2 swipedelta, starttouch;
    private float sqrdeadzone;

    private void Start()
    {
        sqrdeadzone = deadzone * deadzone;
    }

    public void LateUpdate()
    {
        swipeleft = swiperight = swipeup = false;
        UpdateMobile();
    }

    public void UpdateMobile()
    {
        // --- SUPORTE PARA MOUSE ---
        if (Input.GetMouseButtonDown(0))
        {
            starttouch = Input.mousePosition;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            starttouch = swipedelta = Vector2.zero;
        }

        // --- SUPORTE PARA TOQUE ---
        if (Input.touches.Length != 0)
        {
            if (Input.touches[0].phase == TouchPhase.Began)
            {
                starttouch = Input.touches[0].position;               
            }
            else if (Input.touches[0].phase == TouchPhase.Ended || Input.touches[0].phase == TouchPhase.Canceled)
            {
                starttouch = swipedelta = Vector2.zero;
            }
        }

        // --- CÁLCULO DO MOVIMENTO ---
        swipedelta = Vector2.zero;
        if (starttouch != Vector2.zero)
        {
            if (Input.touches.Length != 0)
                swipedelta = Input.touches[0].position - starttouch;
            else if (Input.GetMouseButton(0))
                swipedelta = (Vector2)Input.mousePosition - starttouch;
        }

        if (swipedelta.sqrMagnitude > sqrdeadzone)
        {
            float x = swipedelta.x;
            float y = swipedelta.y;
            
            if (Mathf.Abs(x) > Mathf.Abs(y))
            {
                if (x < 0) swipeleft = true;
                else swiperight = true;
            }           
            else
            {
                if (y > 0) swipeup = true;
            }
            starttouch = swipedelta = Vector2.zero;
        }      
    }
}