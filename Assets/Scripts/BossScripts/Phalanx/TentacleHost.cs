using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleHost : MonoBehaviour
{
    [SerializeField]
    private GameObject TentacleToSpawn;
    [SerializeField]
    private List<GameObject> TentaclesAreadySpawned = new List<GameObject>();
    [SerializeField]
    public int MaxTetacleLength;

    Tentacle TentacleScript;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnTentacle());
    }

  IEnumerator SpawnTentacle()
    {
        while (TentaclesAreadySpawned.Count < MaxTetacleLength + 1)
        {
            yield return new WaitForSeconds(0.7f);
                GameObject SpawnedTentacle = Instantiate(TentacleToSpawn, transform.position, Quaternion.identity);
                TentaclesAreadySpawned.Add(SpawnedTentacle);
                    TentacleScript = SpawnedTentacle.GetComponent<Tentacle>();
                    TentacleScript.SetTarget(TentaclesAreadySpawned[TentaclesAreadySpawned.Count - 2]);
                    TentacleScript.SetID(TentaclesAreadySpawned.Count - 1,gameObject);
        }
    }
}
