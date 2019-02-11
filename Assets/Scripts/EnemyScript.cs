using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyScript : MonoBehaviour {

    [SerializeField] GameObject[] enemies;

    // Переменная для звука во время попадания лазера
    public AudioClip hitSound;

    // Анимация при уничтожении объекта
    public Transform explosion;

    public int scoreValue;

    // Сколько раз нужно попасть во врага, чтобы уничтожить его
    public int health;

    //private GameController controller;

    // Use this for initialization
    void Start()
    {
        //controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        //Проверяем коллизию с объектом типа «лазер»
        if (collision.gameObject.name.Contains("laser"))
        {
            LaserScript laser = collision.gameObject.GetComponent("LaserScript") as LaserScript;
            health -= laser.damage;
            Destroy(collision.gameObject);

            // Воспроизвести звук попадания выстрела
            GetComponent<AudioSource>().PlayOneShot(hitSound);

            if(health == 0)
            {
                // Срабатывает при уничтожении объекта
                if (explosion)
                {
                    GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
                    Destroy(exploder, 2.0f);
                }
                Transform transform = this.gameObject.transform;
                Destroy(this.gameObject);

                if(enemies.Length != 0)
                {
                    int id = Random.Range(0, enemies.Length);
                    GameObject icon = enemies[id];

                    Instantiate(icon, transform.position, icon.transform.rotation);
                }

                GameController controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
                controller.AddScore(scoreValue);
                controller.KilledEnemy();
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
