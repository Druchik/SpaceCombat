using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Fire : MonoBehaviour {

    //[SerializeField] private GameObject enemyLaserPrefab;
    public Transform enemyLaser;
    private GameController controller;

    public Transform shotSpawn1;
    public Transform shotSpawn2;
    public Transform shotSpawn3;
    public Transform shotSpawn4;
    public float fireRate;
    public float delay;
    public float lifeTime;
    public AudioClip enemyShot;


    void Start()
    {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
        InvokeRepeating("Fire", delay, fireRate);
    }

    void Fire()
    {
        if (controller.CheckAlive())
        {
            Instantiate(enemyLaser, shotSpawn1.position, this.transform.rotation);
            Instantiate(enemyLaser, shotSpawn2.position, this.transform.rotation);
            Instantiate(enemyLaser, shotSpawn3.position, this.transform.rotation);
            Instantiate(enemyLaser, shotSpawn4.position, this.transform.rotation);

            // Воспроизвести звук выстрела лазером
            GetComponent<AudioSource>().PlayOneShot(enemyShot);

            //Destroy(enemyLaser, lifeTime);
        }

    }
}
