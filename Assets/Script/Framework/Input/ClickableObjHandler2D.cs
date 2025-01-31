using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Clickable Object Handler. For 2D collider
/// </summary>
[RequireComponent(typeof(TouchHandler))]
public class ClickableObjHandler2D : MonoBehaviour
{
    /// <summary>
    /// Target collider's layer mask
    /// </summary>
    public LayerMask mask;

    /// <summary>
    /// Pointer delegate
    /// </summary>
    /// <param name="id">Mouse ID, for multiple touches</param>
    /// <param name="pos">Touch position on screen</param>
    public delegate void Pointer(int id, Vector2 pos);

    /// <summary>
    /// Mouse down event. Event will be invoke when touched position has no clickable object.
    /// </summary>
    public event Pointer ElsePointerDown;

    /// <summary>
    /// Mouse up event. Event will be invoke when touched position has no clickable object
    /// </summary>
    public event Pointer ElsePointerUp;

    /// <summary>
    /// Mouse hold event. Event will be invoke when touched position has no clickable object
    /// </summary>
    public event Pointer ElsePointerHold;


    //private

    private readonly Dictionary<int, List<Clickable>> moveableObjs = new();

    private TouchHandler touchHandler=null;

    private void Awake()
    {
        touchHandler=GetComponent<TouchHandler>();
        touchHandler.PointerDown += Down;
        touchHandler.PointerUp += Up;
        touchHandler.PointerHold += Hold;
    }

    private void Down(int id,Vector2 pos)
    {
        Vector2 point = Camera.main.ScreenToWorldPoint(pos);
        Collider2D[] collider = Physics2D.OverlapPointAll(point, mask);
        bool hasObj = false;
        List<Clickable> clicks = new();
        foreach (Collider2D col in collider)
        {
            Clickable[] cs= col.GetComponents<Clickable>();
            foreach (var c in cs)
            {
                clicks.Add(c);
                hasObj = true;
            }
        }
        if (hasObj)
        {
            if (moveableObjs.ContainsKey(id))
            {
                moveableObjs[id].Clear();
            }
            else
            {
                moveableObjs[id] = new();
            }
            clicks.Sort((a, b) => b.GetLayer().CompareTo(a.GetLayer()));
            for (int i = 0; i < clicks.Count; i++)
            {
                moveableObjs[id].Add (clicks[i]);
                if (clicks[i].Down(pos))
                {
                    break;
                }
            }
        }
        else
        {
            ElsePointerDown?.Invoke(id, pos);
        }
    }

    private void Hold(int id, Vector2 pos)
    {
        if (moveableObjs.ContainsKey(id)&& moveableObjs[id].Count>0)
        {
            foreach (var c in moveableObjs[id])
            {
                c.Hold(pos);
            }
        }
        else
        {
            ElsePointerHold?.Invoke(id, pos);
        }
    }

    private void Up(int id, Vector2 pos)
    {
        if (moveableObjs.ContainsKey(id) && moveableObjs[id].Count > 0)
        {
            foreach (var c in moveableObjs[id])
            {
                c.Up(pos);
            }
            moveableObjs[id].Clear();
        }
        else
        {
            ElsePointerUp?.Invoke(id, pos);
        }
    }

    private void OnDestroy()
    {
        if (touchHandler != null)
        {
            touchHandler.PointerDown -= Down;
            touchHandler.PointerUp -= Up;
            touchHandler.PointerHold -= Hold;
        }
    }
}
