using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryScript : MonoBehaviour
{
    [SerializeField]
    private AudioClip ExplosionAudio;
    // Start is called before the first frame update
    void Start()
    {
        AudioSource.PlayClipAtPoint(ExplosionAudio, transform.position);
        Destroy(gameObject, 2);
    }
}
