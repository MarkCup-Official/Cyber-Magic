using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dice : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;

    public Sprite[] sprites;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public int id;

    void Update()
    {
        id = Mathf.Clamp(id, 0, 6);
        spriteRenderer.sprite = sprites[id];
    }

    public void Show(int id)
    {
        this.id = id;
    }

    public void RandomDice()
    {
        int r = Random.Range(1, 7);
        while (r == id)
        {
            r = Random.Range(1, 7);
        }
        Show(r);
    }

    public static long sleep = 200;

    public void Tell()
    {
        List<long> li = new();
        for (int i = 0; i < id; i++)
        {
            li.Add(10);
            li.Add(sleep);
        }
        Vibration.VibratePattern(li.ToArray(), -1);
    }
}
