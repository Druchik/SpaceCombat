using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour {

    [SerializeField] private GameObject enemyLaserPrefab;
    public GameObject _enemyShot;
    private GameController controller;

    public Transform shotSpawn;
    public float fireRate;
    public float delay;
    public float speed;
    public float lifeTime;
    public AudioClip enemyShot;


    void Start ()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
        InvokeRepeating("Fire", delay, fireRate);
    }
	
	void Fire () {
        if (controller.CheckAlive())
        {
            _enemyShot = Instantiate(enemyLaserPrefab, shotSpawn.position, shotSpawn.rotation) as GameObject;

            // Воспроизвести звук выстрела лазером
            GetComponent<AudioSource>().PlayOneShot(enemyShot);

            Destroy(_enemyShot, lifeTime);
        }
            
    }

    void FixedUpdate()
    {
        if (controller.CheckAlive())
            if (_enemyShot != null)
            _enemyShot.transform.Translate(Vector3.up * Time.deltaTime * speed);
    }

}
