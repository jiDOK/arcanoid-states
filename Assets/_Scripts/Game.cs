using UnityEngine;

public class Game : MonoBehaviour
{
    [SerializeField] Settings settings;
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int startNumShips = 3;

    private Ship ship;
    private Drain drain;
    private int score;
    private int numShipsLeft;

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        ship = FindObjectOfType<Ship>();
        drain = FindObjectOfType<Drain>();
        drain.BallInDrain += OnBallInDrain;
        numShipsLeft = startNumShips;
        ship.Init();
        for (int i = 0; i < ship.NumMultiballs + 1; i++)
        {
            Ball ball = Instantiate<Ball>(ballPrefab);
            ball.Ship = ship;
            ball.transform.parent = ship.transform;
            ship.BallPool.Push(ball);
        }
        ship.SpawnOnShip(ship.BallPool.Pop());
    }

    private void OnDisable()
    {
        drain.BallInDrain -= OnBallInDrain;
    }

    public void OnBallInDrain(Ball b)
    {
        if (ship.BallPool.Count < ship.NumMultiballs)
        {
            // not last ball - put back in pool
            b.Init();
            b.gameObject.SetActive(false);
            ship.BallPool.Push(b);
            b.transform.parent = ship.transform;
        }
        else if (numShipsLeft > 0)
        {
            numShipsLeft--;
            ship.Init();
            b.Init();
            ship.SpawnOnShip(b);
        }
        else
        {
            // GameOver
        }
    }
}

[System.Serializable]
public class Settings
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private int startNumShips = 3;
}
