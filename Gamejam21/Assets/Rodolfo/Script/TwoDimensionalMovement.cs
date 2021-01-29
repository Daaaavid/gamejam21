using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwoDimensionalMovement : MonoBehaviour
{
    private Vector2 movement;
    private Rigidbody rb;
    public float speed = 0.5f;
    // Start is called before the first frame update
    void Start()
    {
       rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {       
        if(Input.GetKey(KeyCode.D))
        {
            //walk right
            Debug.Log("D Pressed");            
            movement = (transform.right * speed);
        }
        if (Input.GetKey(KeyCode.A))
        {
            //walk left
            Debug.Log("A Pressed");
            movement = (-transform.right * speed);            
        }
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector3(movement.x, 0, 0);
        movement.x *= 0.9f;
    }
}
