using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fx : MonoBehaviour
{

    [SerializeField]
    private ParticleSystem[] ParticleFXs;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(TransitionFX());
        }
    }
    public void StartTransitionFX()
    {
        StartCoroutine(TransitionFX());
    }
     IEnumerator TransitionFX()
    {
       ParticleSystem SpawnedPartcle = Instantiate(ParticleFXs[0], new Vector3(0, 10, 0), Quaternion.Euler(new Vector3(90,0,0)));
        ParticleSystem SpawnedPartcle2 = Instantiate(ParticleFXs[1], new Vector3(0, 10, 0), Quaternion.Euler(new Vector3(90, 0, 0)));
        yield return new WaitForSeconds(3);
        SpawnedPartcle.Stop();
        SpawnedPartcle2.Stop();
        //start another couroutine to destroy

    }
}
