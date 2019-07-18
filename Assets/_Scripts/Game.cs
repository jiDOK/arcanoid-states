using UnityEngine;
using System.Collections.Generic;

public class Game : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        public int startNumShips = 3;
        public Vector3 shipStartPos;
        public Ball ballPrefab;
        public float ballYoffset = 0.338f;
        public int numMultiballs = 3;
    }
    [SerializeField] private Settings settings;

    public Stack<Ball> BallPool { get; set; } = new Stack<Ball>(4);
    private Ship ship;
    private Drain drain;
    private int score;
    private int numShipsLeft;
    private Transform activeBalls;

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
        ship.Init(settings.shipStartPos);
        numShipsLeft = settings.startNumShips;
        for (int i = 0; i < settings.numMultiballs + 1; i++)
        {
            Ball ball = Instantiate<Ball>(settings.ballPrefab);
            ball.Ship = ship;
            ball.transform.parent = ship.transform;
            BallPool.Push(ball);
        }
        SpawnOnShip(BallPool.Pop());
    }

    private void OnDisable()
    {
        drain.BallInDrain -= OnBallInDrain;
        ShipStateMultiball.MultiballTriggered += OnMultiballTriggered;
    }

    public void OnBallInDrain(Ball b)
    {
        if (BallPool.Count < settings.numMultiballs)
        {
            // not last ball - put back in pool
            b.Init();
            b.gameObject.SetActive(false);
            BallPool.Push(b);
            b.transform.parent = ship.transform;
            if(BallPool.Count == settings.numMultiballs)
            {
                //ship.Ball = ship.GetComponentInChildren<Ball>();
                ship.Ball = activeBalls.GetComponentInChildren<Ball>();
            }
        }
        else if (numShipsLeft > 0)
        {
            numShipsLeft--;
            ship.Init(settings.shipStartPos);
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
        ball.transform.position = new Vector3(ship.transform.position.x, ship.transform.position.y + settings.ballYoffset, ship.transform.position.z);
        ball.transform.parent = ship.transform;
    }

    public void OnMultiballTriggered()
    {
        for (int i = 0; i < settings.numMultiballs; i++)
        {
            Ball mb = BallPool.Pop();
            mb.gameObject.SetActive(true);
            mb.transform.position = ship.Ball.transform.position;
            mb.transform.parent = activeBalls;
            mb.Shoot(0f, 360f);
        }
    }
}

