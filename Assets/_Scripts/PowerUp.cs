using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public static Action<PU> PUCollected;
    public static Action PUDestroyed;
    [SerializeField] private PU puType;
    public PU PUType { get => puType; }

    private void OnEnable()
    {
        Game.NewShip += OnNewShip;
    }
    private void OnDisable()
    {
        Game.NewShip -= OnNewShip;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("ship"))
        {
            PUCollected?.Invoke(puType);
        }
        else
        {
            PUDestroyed?.Invoke();
        }
        Destroy(gameObject);
    }

    public void OnNewShip()
    {
        PUDestroyed?.Invoke();
        Destroy(gameObject);
    }
}

public enum PU { None, Expand, Fastship, Multiball }
