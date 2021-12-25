using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{

    [SerializeField]
    private float RotSpeed;
    [SerializeField]
    private GameObject Explosion;
    [SerializeField]
    private GameObject FxManagerObject;

    Fx FxManage;

    Collider2D AsteroidCollider;

    SpawnManager _SpawnManager;

    private void Start()
    {
        if(FxManagerObject != null)
        {
            FxManage = FxManagerObject.GetComponent<Fx>();
        }
        AsteroidCollider = GetComponent<Collider2D>();
        _SpawnManager = GameObject.Find("Spawn Manager").GetComponent<SpawnManager>();
    }

    void Update()
    {
        transform.Rotate(0, 0, RotSpeed * Time.deltaTime);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Laser")
        {
            AsteroidCollider.enabled = false;
            _SpawnManager.StartSpawning();
            Instantiate(Explosion, transform.position, Quaternion.identity);
            FxManage.StartTransitionFX();
            Destroy(gameObject);
        }
    }
}
