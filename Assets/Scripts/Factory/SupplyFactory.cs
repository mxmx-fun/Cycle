using UnityEngine;

public class SupplyFactory
{
    GameObject supplyPrefab;
    public SupplyFactory( GameObject supplyPrefab) {
        this.supplyPrefab = supplyPrefab;
    }

    public void SpawnSupply(EvadeLevel lv)
    {
        GameObject supply = GameObject.Instantiate(supplyPrefab);
        supply.GetComponent<RandomSupplyEntity>().Ctor(lv);
    }

}