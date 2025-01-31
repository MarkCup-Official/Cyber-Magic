using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DButton : MonoBehaviour,Clickable
{
    public SpriteRenderer sr;
    public Sprite[] sprites;

    public UnityEvent eve;

    

    
    
public bool Down(Vector3 pos)
    {
            sr.sprite = sprites[1];
        return true;
    }

    public void Hold(Vector3 pos)
    {
    }

    public void Up(Vector3 pos)
    {
            sr.sprite = sprites[0];
            if (Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(pos)) != null)
            {
                eve.Invoke();
            }
    }

    public int GetLayer()
    {
        return 0;
    }
}
