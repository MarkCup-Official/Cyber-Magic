using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Multiple touches handler, support both touch input and mouse click.
/// </summary>
public class TouchHandler : MonoBehaviour
{
    /// <summary>
    /// Pointer delegate.
    /// </summary>
    /// <param name="id">Mouse ID, for multiple touches</param>
    /// <param name="pos">Touch position on screen</param>
    public delegate void Pointer(int id,Vector2 pos);


    /// <summary>
    /// Touch down event. Will be invoke only when touch start.
    /// </summary>
    public event Pointer PointerDown;

    /// <summary>
    /// Touch down event. Will be invoke only when touch release.
    /// </summary>
    public event Pointer PointerUp;

    /// <summary>
    /// Touch down event. Will be invoke in every frame when touch hold.
    /// </summary>
    public event Pointer PointerHold;


    //private

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                Touch touch = Input.GetTouch(i);
                Vector2 pos = touch.position;
                int id=touch.fingerId;

                switch (touch.phase)
                {
                    case TouchPhase.Began:
                        PointerDown?.Invoke(id, pos);
                        break;
                    case TouchPhase.Moved:
                        PointerHold?.Invoke(id, pos);
                        break;
                    case TouchPhase.Stationary:
                        PointerHold?.Invoke(id, pos);
                        break;
                    case TouchPhase.Ended:
                        PointerUp?.Invoke(id, pos);
                        break;
                    case TouchPhase.Canceled:
                        PointerUp?.Invoke(id, pos);
                        break;
                }
            }
        }
        else if (Input.GetMouseButtonDown(0))
        {
            Vector2 pos = Input.mousePosition;
            PointerDown?.Invoke(-1, pos);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            Vector2 pos = Input.mousePosition;
            PointerUp?.Invoke(-1, pos);
        }
        else if (Input.GetMouseButton(0))
        {
            Vector2 pos = Input.mousePosition;
            PointerHold?.Invoke(-1, pos);
        }
        else if (Input.GetMouseButtonDown(1))
        {
            Vector2 pos = Input.mousePosition;
            PointerDown?.Invoke(-1, pos);
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Vector2 pos = Input.mousePosition;
            PointerUp?.Invoke(-1, pos);
        }
        else if (Input.GetMouseButton(1))
        {
            Vector2 pos = Input.mousePosition;
            PointerHold?.Invoke(-1, pos);
        }
    }
}
