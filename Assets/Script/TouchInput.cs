using UnityEngine;
using UnityEngine.InputSystem;

public class TouchInput : MonoBehaviour
{
    public static Vector2 SwipeDirection { get; private set; }
    private Vector2 startTouchPos;
    private Vector2 endTouchPos;
    private bool swipeDetected;

    private void Update()
    {
        SwipeDirection = Vector2.zero;

        if (Touchscreen.current == null) return;

        if (Touchscreen.current.primaryTouch.press.isPressed)
        {
            Vector2 currentPos = Touchscreen.current.primaryTouch.position.ReadValue();

            if (!swipeDetected)
            {
                startTouchPos = currentPos;
                swipeDetected = true;
            }
        }
        else if (swipeDetected)
        {
            endTouchPos = Touchscreen.current.primaryTouch.position.ReadValue();
            Vector2 swipe = endTouchPos - startTouchPos;

            if (swipe.magnitude > 50f) // soglia per evitare tocchi accidentali
            {
                if (Mathf.Abs(swipe.x) > Mathf.Abs(swipe.y))
                {
                    SwipeDirection = swipe.x > 0 ? Vector2.right : Vector2.left;
                }
                else
                {
                    SwipeDirection = swipe.y > 0 ? Vector2.up : Vector2.down;
                }
            }

            swipeDetected = false;
        }
    }
}
