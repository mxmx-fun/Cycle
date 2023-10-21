using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    static GameController instance;
    public static GameController Instance => instance;

    public GameObject spikePrefab;
    public GameObject LaserCannonPrefab;
    public GameObject groundSpikePrefab;

    public GameObject supplyPrefab;

    public EvadeCombo evade;
    public Cycle cycle;

    public BarrierFactory barrierFactory;
    public SupplyFactory supplyFactory;

    public void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        spikePrefab = Resources.Load<GameObject>("Prefab/Barrier/Spike");
        LaserCannonPrefab = Resources.Load<GameObject>("Prefab/Barrier/LaserCannon");
        groundSpikePrefab = Resources.Load<GameObject>("Prefab/Barrier/GroundSpike");
        supplyPrefab = Resources.Load<GameObject>("Prefab/Supply/RandomItem");
        barrierFactory = new BarrierFactory(spikePrefab, LaserCannonPrefab, groundSpikePrefab);
        supplyFactory = new SupplyFactory(supplyPrefab);
    }

    public void AddEvadeCount()
    {
        evade.evadeComboCount++;
        evade.evadeCount++;
        evade.UpdateTxt();
    }

    public void GameOver()
    {
        cycle.Apply_State(CycleState.Over);
    }

    public void Apply_Easy()
    {
        for (int i = 0; i < 5; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Easy);
        }
    }

    public void Apply_Normal()
    {
        for (int i = 0; i < 8; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Normal);
        }

        for (int i = 0; i < 2; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Normal);
        }
    }


    public void Apply_Hard()
    {
        for (int i = 0; i < 8; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Normal);
        }

        for (int i = 0; i < 1; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Hard);
        }

        for (int i = 0; i < 2; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Hard);
        }

        for (int i = 0; i < 1; i++)
        {
            barrierFactory.SpawnGround(CycleState.Hard);
        }
    }

        public void Apply_Hell()
    {
        for (int i = 0; i < 2; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Hell);
        }

        for (int i = 0; i < 3; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Hell);
        }

        for (int i = 0; i < 1; i++)
        {
            barrierFactory.SpawnGround(CycleState.Hell);
        }
    }
}
