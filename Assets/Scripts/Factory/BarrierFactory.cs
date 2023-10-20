using UnityEngine;

public class BarrierFactory
{
    GameObject spikePrefab;
    GameObject laserCannonPrefab;
    GameObject groundSpikePrefab;
    public BarrierFactory(GameObject spikePrefab, GameObject laserCannonPrefab, GameObject groundPrefab)
    {
        this.spikePrefab = spikePrefab;
        this.laserCannonPrefab = laserCannonPrefab;
        this.groundSpikePrefab = groundPrefab;
    }

    public void SpawnSpike(CycleState state)
    {
        GameObject supply = GameObject.Instantiate(spikePrefab);
        supply.GetComponent<SpikeEntity>().Ctor(state);
    }

    public void SpawnLaserCannon(CycleState state)
    {
        GameObject supply = GameObject.Instantiate(laserCannonPrefab);
        supply.GetComponent<LaserCannonEntity>().Ctor(state);
    }

    public void SpawnGround(CycleState state)
    {
        GameObject supply = GameObject.Instantiate(groundSpikePrefab);
        supply.GetComponent<GroundSpikeEntity>().Ctor(state);
    }
}