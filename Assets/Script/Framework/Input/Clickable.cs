using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Clickable
{
    public bool Down(Vector3 pos)
    {
        return false;
    }

    public void Hold(Vector3 pos)
    {
    }

    public void Up(Vector3 pos)
    {
    }

    public int GetLayer()
    {
        return 0;
    }
}
