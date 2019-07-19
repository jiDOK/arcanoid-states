using System;
using UnityEngine;

public class Block : MonoBehaviour
{
    public static Action<Block> BlockHit;

    [SerializeField] private int points = 50;
    public int Points { get => points; }

    private void OnCollisionEnter(Collision collision)
    {
        BlockHit?.Invoke(this);
        gameObject.SetActive(false);
    }
}
