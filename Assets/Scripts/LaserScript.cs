using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserScript : MonoBehaviour {

    // Как долго существует лазер
    public float lifeTime = 2.0f;

    // Как быстро движется лазер
    public float speed = 5.0f;

    // Как много наносит урона лазер при соприкосновении с врагами
    public int damage = 1;

	// Use this for initialization
	void Start () {
        // Уничтожение лазера по окончанию таймера
        Destroy(gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update () {
        transform.Translate(Vector3.up * Time.deltaTime * speed);
	}
}
