using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public GameObject spikePrefab;
    public GameObject LaserCannonPrefab;
    public GameObject groundSpikePrefab;

    public int EvadeCount;

    public void Awake() {
        EvadeCount = 0;
    }
}
