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

    public void Awake() {
        instance = this;
    }

    public void Start() {
        spikePrefab = Resources.Load<GameObject>("Prefab/Barrier/Spike");
        LaserCannonPrefab = Resources.Load<GameObject>("Prefab/Barrier/LaserCannon");
        groundSpikePrefab = Resources.Load<GameObject>("Prefab/Barrier/GroundSpike");
        supplyPrefab = Resources.Load<GameObject>("Prefab/Supply/RandomItem");
        barrierFactory = new BarrierFactory(spikePrefab, LaserCannonPrefab, groundSpikePrefab);
        supplyFactory = new SupplyFactory(supplyPrefab);
    }

    public void AddEvadeCount() {
        evade.evadeComboCount ++;
        evade.evadeCount ++;
        evade.UpdateTxt();
    }

    

    
}
