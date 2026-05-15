using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public float moveSpeed = 1f;
    public float rotationSpeed = 10f;
    private PlayerController controller;
    private Vector2 movement;

    public static PlayerMovement instance;
    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        rb=GetComponent<Rigidbody2D>();
        movement = new Vector2();
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        
        movement.Normalize();
        if (movement != Vector2.zero)
        {
            float targetAngle = Mathf.Atan2(movement.y, movement.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, targetAngle - 90)); 

            // Rotar hacia objetivo
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }
}
