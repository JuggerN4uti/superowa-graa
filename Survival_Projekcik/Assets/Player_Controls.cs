using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Player_Controls : MonoBehaviour
{
    public Rigidbody2D body;
    public Camera main_camera;
    public Transform Gun_barrel;
    public GameObject Bullet_prefab;
    public TMPro.TextMeshProUGUI ammo_details;

    Vector2 movement;
    Vector2 mouse_position;

    public float movement_speed;
    public float fire_rate = 10f;
    public int mag_size = 7;
    public int loaded_ammo;

    public float time_between_shots = 0;
    public float reload_time = 0;
    public float bullet_force = 1f;

    void Start()
    {
        loaded_ammo = mag_size;
        Display_Ammo();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        mouse_position = main_camera.ScreenToWorldPoint(Input.mousePosition);
        if (time_between_shots > 0)
        {
            time_between_shots -= fire_rate;
        }

        if ((Input.GetKeyDown(KeyCode.R)) && (mag_size != loaded_ammo))
        {
            loaded_ammo = mag_size;
            Display_Ammo();
        }

        if (Input.GetKey(KeyCode.Mouse0)) 
        {
            if ((time_between_shots <= 0) && (loaded_ammo > 0))
                Fire();
        }
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.Mouse0))
            body.MovePosition(body.position + movement * 0.4f * movement_speed * Time.deltaTime);
        else body.MovePosition(body.position + movement * movement_speed * Time.deltaTime);

        Vector2 lookDir = mouse_position - body.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
        body.rotation = angle;
    }

    void Fire()
    {
        GameObject bullet = Instantiate(Bullet_prefab, Gun_barrel.position, Gun_barrel.rotation);
        Rigidbody2D bullet_body = bullet.GetComponent<Rigidbody2D>();
        bullet_body.AddForce(Gun_barrel.up * bullet_force, ForceMode2D.Impulse);

        loaded_ammo--;
        Display_Ammo();
        time_between_shots = 4000;
    }

    void Display_Ammo()
    {
        ammo_details.text = loaded_ammo.ToString("0") + "/" + mag_size.ToString("0");
    }
}
