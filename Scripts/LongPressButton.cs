using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class LongPressButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public float longPressDuration = 1.0f;
    private bool isPointerDown = false;
    private bool longPressTriggered = false;
    private float pointerDownTimer = 0f;

    public UnityAction onLongPress;

    private void Update()
    {
        if (isPointerDown)
        {
            pointerDownTimer += Time.deltaTime;

            if (pointerDownTimer >= longPressDuration)
            {
                if (!longPressTriggered)
                {
                    longPressTriggered = true;
                    onLongPress?.Invoke();
                }
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        longPressTriggered = false;
        pointerDownTimer = 0f;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Reset();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Reset();
    }

    private void Reset()
    {
        isPointerDown = false;
        longPressTriggered = false;
        pointerDownTimer = 0f;
    }
}
