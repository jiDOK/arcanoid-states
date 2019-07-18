using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [System.Serializable]
    private class Settings
    {
        public int StartNumShips = 3;
        public Vector3 ShipStartPos;
        public Ball BallPrefab;
        public float BallYOffset = 0.338f;
        public int NumMultiballs = 3;
    }
    [SerializeField] private Settings settings;

    private Ship ship;
    private Drain drain;
    private int score;
    private int numShipsLeft;
    private Transform activeBalls;
    public Stack<Ball> BallPool { get; set; } = new Stack<Ball>(4);

    void Awake()
    {
        Init();
    }

    private void Init()
    {
        activeBalls = GameObject.Find("Active Balls").transform;
        drain = FindObjectOfType<Drain>();
        drain.BallInDrain += OnBallInDrain;
        ship = FindObjectOfType<Ship>();
        ShipStateMultiball.MultiballTriggered += OnMultiballTriggered;
        ship.Init(settings.ShipStartPos);
        numShipsLeft = settings.StartNumShips;
        for (int i = 0; i < settings.NumMultiballs + 1; i++)
        {
            Ball ball = Instantiate<Ball>(settings.BallPrefab);
            ball.Ship = ship;
            ball.transform.parent = ship.transform;
            BallPool.Push(ball);
        }
        SpawnOnShip(BallPool.Pop());
    }

    private void OnDisable()
    {
        drain.BallInDrain -= OnBallInDrain;
        ShipStateMultiball.MultiballTriggered -= OnMultiballTriggered;
    }

    public void OnBallInDrain(Ball b)
    {
        if (BallPool.Count < settings.NumMultiballs)
        {
            // not last ball - put back in pool
            b.Init();
            b.gameObject.SetActive(false);
            BallPool.Push(b);
            b.transform.parent = ship.transform;
            if(BallPool.Count == settings.NumMultiballs)
            {
                //ship.Ball = ship.GetComponentInChildren<Ball>();
                ship.Ball = activeBalls.GetComponentInChildren<Ball>();
            }
        }
        else if (numShipsLeft > 0)
        {
            numShipsLeft--;
            ship.Init(settings.ShipStartPos);
            b.Init();
            SpawnOnShip(b);
        }
        else
        {
            // GameOver
        }
    }

    public void SpawnOnShip(Ball ball)
    {
        ship.Ball = ball;
        ball.gameObject.SetActive(true);
        ball.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + settings.BallYOffset, ship.transform.position.z);
        ball.transform.parent = ship.transform;
    }

    public void OnMultiballTriggered()
    {
        for (int i = 0; i < settings.NumMultiballs; i++)
        {
            Ball mb = BallPool.Pop();
            mb.gameObject.SetActive(true);
            mb.transform.position = ship.Ball.transform.position;
            mb.transform.parent = activeBalls;
            mb.Shoot(0f, 360f);
        }
    }
}

