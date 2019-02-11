using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    // Переменная для звука во время попадания лазера
    public AudioClip hitSound;

    public Transform LaserSpawn;
    public Transform LaserSpawnWithAmmo1;
    public Transform LaserSpawnWithAmmo2;

    // Анимация при уничтожении объекта
    public Transform explosion;

    // Переменная для звука выстрела лазером
    public AudioClip shootSound;

    public int health = 5;
    //private bool alive;

    //Скорость корабля
    public float playerSpeed = 2.0f;

    //Текущая скорость
    public float currentSpeed = 0.0f;

    //Создание переменных для перемещения
    public List<KeyCode> upButton;
    public List<KeyCode> downButton;
    public List<KeyCode> leftButton;
    public List<KeyCode> rightButton;

    //Сохранение последнего перемещения
    private Vector3 lastMovement = new Vector3();

    //Переменная для лазера
    public Transform laser;

    //Задержка между выстрелами (кулдаун)
    public float timeBetweenFires = 0.3f;

    //Счетчик задержки между выстрелами
    public float timeTilNextFire = 0.0f;

    //Кнопка, используемая для выстрела
    public List<KeyCode> shootButton;

    private GameController controller;

    private bool pickUpAmmo;
    private float timePickUpAmmo;

    // Use this for initialization
    void Start ()
    {
        pickUpAmmo = false;
        controller = GameObject.FindGameObjectWithTag("GameController").GetComponent("GameController") as GameController;
    }
	
	// Update is called once per frame
	void Update () {

        // Поворот героя к мышке
        Rotation();
        
        // Перемещение героя
        Movement();

        foreach (KeyCode element in shootButton)
        {
            if(Input.GetKey(element) && timeTilNextFire < 0)
            {
                timeTilNextFire = timeBetweenFires;
                ShootLaser();
                break;
            }
        }
        timeTilNextFire -= Time.deltaTime; 
    }

    void Rotation()
    {
        //Показываем игроку где мышка
        Vector3 worldPos = Input.mousePosition;
        worldPos = Camera.main.ScreenToWorldPoint(worldPos);

        //Сохраняем координаты указателя мыши
        float dx = this.transform.position.x - worldPos.x;
        float dy = this.transform.position.y - worldPos.y;

        //Вычисляем угол между объектами "Корабль" и "Указатель"
        float angle = Mathf.Atan2(dy, dx) * Mathf.Rad2Deg;

        //Трансформируем угол в вектор
        Quaternion rot = Quaternion.Euler(new Vector3(0, 0, angle + 90));

        //Изменяем поворот героя
        this.transform.rotation = rot;
    }

    void Movement()
    {
        //Необходимое движение
        Vector3 movement = new Vector3();

        //Проверка нажатых клавиш
        movement += MoveIfPressed(upButton, Vector3.up);
        movement += MoveIfPressed(downButton, Vector3.down);
        movement += MoveIfPressed(leftButton, Vector3.left);
        movement += MoveIfPressed(rightButton, Vector3.right);

        //Если нажато несколько кнопок обрабатываем это
        movement.Normalize();

        //Проверка нажатия кнопки

        if(movement.magnitude > 0)
        {
            //После нажатия двигаемся в этом направлении
            currentSpeed = playerSpeed;
            this.transform.Translate(movement * Time.deltaTime * playerSpeed, Space.World);
            lastMovement = movement;
        }
        else
        {
            //Если ничего не нажато
            this.transform.Translate(lastMovement * Time.deltaTime * currentSpeed, Space.World);

            //Замедление со временем
            currentSpeed *= 0.9f;
        }
    }

    //Возвращает движение если нажата кнопка
    Vector3 MoveIfPressed(List<KeyCode> keyList, Vector3 Movement)
    {
        //Проверяем кнопки из списка
        foreach (KeyCode element in keyList)
        {
            if (Input.GetKey(element))
                //Если нажато покидаем функцию
                return Movement;
        }
        //Если кнопки не нажаты, то не двигаемся
        return Vector3.zero;
    }
    // Создание лазера
    void ShootLaser()
    {
        if(pickUpAmmo && Time.realtimeSinceStartup - timePickUpAmmo <= 15)
        {   
            // Создаём лазер на этой позиции
            Instantiate(laser, LaserSpawnWithAmmo1.position, this.transform.rotation);
            Instantiate(laser, LaserSpawnWithAmmo2.position, this.transform.rotation);

            // Воспроизвести звук выстрела лазером
            GetComponent<AudioSource>().PlayOneShot(shootSound);
        }
        else
        {
            // Создаём лазер на этой позиции
            Instantiate(laser, LaserSpawn.position, this.transform.rotation);

            // Воспроизвести звук выстрела лазером
            GetComponent<AudioSource>().PlayOneShot(shootSound);
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Contains("enemyLaser"))
        {
            health--;
            controller.UpdateHealth(health);
            Destroy(collision.gameObject);
            // Воспроизвести звук попадания выстрела
            GetComponent<AudioSource>().PlayOneShot(hitSound);
        }
        if (health <= 0)
        {
            // Срабатывает при уничтожении объекта
            if (explosion)
            {
                GameObject exploder = ((Transform)Instantiate(explosion, this.transform.position, this.transform.rotation)).gameObject;
                Destroy(exploder, 2.0f);
            }
            Destroy(this.gameObject);
            controller.SetAlive(false);
            controller.GameOver();
        }

        if (collision.gameObject.name.Contains("Health"))
        {
            Destroy(collision.gameObject);
            if (health != 5)
            {
                health++;
                controller.UpdateHealth(health);
            }
        }

        if (collision.gameObject.name.Contains("Ammo"))
        {
            Destroy(collision.gameObject);
            pickUpAmmo = true;
            timePickUpAmmo = Time.time;
            //Debug.Log(Time.time);
        }
    }
}
