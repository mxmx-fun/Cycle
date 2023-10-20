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

    public BarrierFactory barrierFactory;
    public SupplyFactory supplyFactory;

    public void Awake() {
        instance = this;
    }

    public void Start() {
        barrierFactory = new BarrierFactory(spikePrefab, LaserCannonPrefab, groundSpikePrefab);
        supplyFactory = new SupplyFactory(supplyPrefab);
    }

    public void AddEvadeCount() {
        evade.evadeComboCount ++;
        evade.evadeCount ++;
        evade.UpdateTxt();
    }

    

    
}
