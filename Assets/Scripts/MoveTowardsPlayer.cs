using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsPlayer : MonoBehaviour {
    // Переменная для координат объекта player
    private Transform player;
    public Rigidbody2D enemy;

    private GameController controller;

    // Скорость движения врага
    public float speed = 1.5f;

    // Use this for initialization
    void Start () {
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
        if (controller.CheckAlive())
            player = GameObject.Find("playerShip").transform;
    }
	
	// Update is called once per frame
	void Update () {
        if (controller.CheckAlive())
        {
            Rotation();
            transform.Translate(0, Time.deltaTime * speed, 0);
        }
    }

    void Rotation()
    {
        if(controller.CheckAlive())
        {
            var turn = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.forward, player.position - transform.position), Time.deltaTime * 5.0f);
            enemy.MoveRotation(turn.eulerAngles.z);
        }
    }
}
