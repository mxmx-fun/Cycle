using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    static GameController instance;
    public static GameController Instance => instance;

    public GameObject spikePrefab;
    public GameObject LaserCannonPrefab;
    public GameObject groundSpikePrefab;

    public GameObject supplyPrefab;

    public GameObject phantomPrefab;

    public GameObject tipPrefab;
    public Transform tipRoot;

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
        tipPrefab = Resources.Load<GameObject>("Prefab/UI/TipEntity");
        phantomPrefab = Resources.Load<GameObject>("Prefab/Role/RolePhantom");
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

    public void Apply_Easy_Single()
    {
        int num;
        num = Random.Range(1, 5);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Easy);
        }
    }

    public void Apply_Normal_Group()
    {
        int num;
        num = Random.Range(3, 5);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Normal);
        }

        num = Random.Range(1, 3);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Normal);
        }
    }


    public void Apply_Hard_Group()
    {
        int num;
        num = Random.Range(5, 8);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Normal);
        }

        num = Random.Range(1, 2);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Hard);
        }

        num = Random.Range(1, 2);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Hard);
        }

        num = Random.Range(1, 3);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnGround(CycleState.Hard);
        }
    }

    public void Apply_Hell()
    {
        int num;
        num = Random.Range(3, 5);
        for (int i = 0; i < num; i++)
        {
            barrierFactory.SpawnSpike(CycleState.Hell);
        }

        num = Random.Range(1, 3);
        for (int i = 0; i < 3; i++)
        {
            barrierFactory.SpawnLaserCannon(CycleState.Hell);
        }

        for (int i = 0; i < 1; i++)
        {
            barrierFactory.SpawnGround(CycleState.Hell);
        }
    }

    public void CreateTip(string text)
    {
        GameObject tip = Instantiate(tipPrefab, tipRoot);
        tip.GetComponent<TipEntity>().Ctor(text);
    }

    public void CreatePhantom(Vector3 originPos, Vector3 targetPos)
    {
        List<phantomEntity> phantoms = new List<phantomEntity>();
        var head = Instantiate(phantomPrefab);
        head.transform.position = originPos;
        phantoms.Add(head.GetComponent<phantomEntity>());
        var offset = targetPos - originPos;
        var offsetNorm = offset / 5;
        phantomEntity pt = null;
        for (int i = 0; i < 5; i++)
        {
            var phantom = Instantiate(phantomPrefab);
            phantom.transform.position = originPos + offsetNorm * (i + 1);
            if (pt != null)
            {
                var phantomCom = phantom.GetComponent<phantomEntity>();
                phantomCom.parent = pt;
                phantoms.Add(phantomCom);
                pt = phantomCom;
            }
            else
            {
                var phantomCom = phantom.GetComponent<phantomEntity>();
                phantomCom.parent = head.GetComponent<phantomEntity>();
                phantoms.Add(phantomCom);
                pt = phantomCom;
            }
        }
        foreach (var phantom in phantoms)
        {
            phantom.Ctor(0.1f);
        }
    }
}
