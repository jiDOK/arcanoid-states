using System;
using System.Collections.Generic;
using UnityEngine;

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
        public float MinTimeBetweenPUs = 5f;
        public float MaxTimeBetweenPUs = 15f;
        public PowerUp[] PowerUps;
    }
    [SerializeField] private Settings settings;

    private class ShipStatePool
    {
        public ShipState[] shipStates = new[] {
            new ShipStateExpand(),
            new ShipStateFastball(),
            new ShipStateFastship(),
            new ShipStateInputSwitch(),
            new ShipStateMultiball(),
            new ShipStateDefault()
        };
    }
    private ShipStatePool shipStatePool;

    public static Action<int> ScoreChanged;
    public static Action NewShip;

    public Stack<Ball> BallPool { get; set; } = new Stack<Ball>(4);
    private Ship ship;
    private Drain drain;
    private int score;
    private int numShipsLeft;
    private float curTimeToPU;
    private float puTimer;
    private Transform activeBalls;
    private bool puSpawned;

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        puTimer += Time.deltaTime;
    }

    private void Init()
    {
        shipStatePool = new ShipStatePool();
        activeBalls = GameObject.Find("Active Balls").transform;
        drain = FindObjectOfType<Drain>();
        drain.BallInDrain += OnBallInDrain;
        PowerUp.PUCollected += OnPUCollected;
        PowerUp.PUDestroyed += OnPUDestroyed;
        ShipStateMultiball.MultiballTriggered += OnMultiballTriggered;
        Block.BlockHit += OnBlockHit;
        ship = FindObjectOfType<Ship>();
        ship.Init(settings.ShipStartPos);
        ship.SwitchState(shipStatePool.shipStates[0]);
        numShipsLeft = settings.StartNumShips;
        curTimeToPU = UnityEngine.Random.Range(settings.MinTimeBetweenPUs, settings.MaxTimeBetweenPUs);
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

    public void OnBlockHit(Block block)
    {
        score += block.Points;
        ScoreChanged?.Invoke(score);
        if (puTimer > curTimeToPU && !puSpawned && ship.currentState!= shipStatePool.shipStates[(int)PU.Multiball])
        {
            puTimer = 0f;
            puSpawned = true;
            var randIdx = UnityEngine.Random.Range(0, settings.PowerUps.Length);
            Instantiate<PowerUp>(settings.PowerUps[randIdx], block.transform.position, Quaternion.identity);
        }
    }

    public void OnPUCollected(PU pu)
    {
        puSpawned = false;
        ship.SwitchState(shipStatePool.shipStates[(int)pu]);
        //if (ship.currentState != shipStatePool.shipStates[(int)PU.Multiball])
        //{
        //    ship.SwitchState(shipStatePool.shipStates[(int)pu]);
        //}
    }

    public void OnPUDestroyed()
    {
        puSpawned = false;
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
            if (BallPool.Count == settings.NumMultiballs)
            {
                ship.Ball = activeBalls.GetComponentInChildren<Ball>();
            }
        }
        else if (numShipsLeft > 0)
        {
            // must be last ball
            numShipsLeft--;
            ship.Init(settings.ShipStartPos);
            b.Init();
            SpawnOnShip(b);
            ship.SwitchState(shipStatePool.shipStates[5]);
            NewShip?.Invoke();
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

