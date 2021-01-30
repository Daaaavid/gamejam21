using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

public class TwoDimensionalMovement : MonoBehaviour {
    float normalzPosition;
    private Vector2 movement;
    public Vector2 walkingPosition;
    public Vector2 targetPosition;
    private Rigidbody rb;
    public float speed = 0.5f;
    public bool interacted = false;
    [SerializeField]private int state = 0; //0 = normal, 1 interacting, 2 = immovable (in converstation)

    public InteractionBus MovementBus;
    
    // Start is called before the first frame update
    void Start() {
        normalzPosition = transform.position.z;
        rb = GetComponent<Rigidbody>();
        
        MovementBus.OnChange.AddListener(obj => GoToObject(obj.transform.position));
    }

    // Update is called once per frame
    void Update() {
        if (state == 0) {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                //walk right
                Debug.Log("D Pressed");
                movement = (transform.right * speed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                //walk left
                Debug.Log("A Pressed");
                movement = (-transform.right * speed);
            }
            if (Input.GetKey(KeyCode.P)) {
                GoToObject(Vector3.zero);
            }

            if (Input.GetKey(KeyCode.T)) {
                GetComponent<ThoughtBubble>().NewThoughtBubble(GetComponent<ThoughtBubble>()._dialogue, 0);
            }

        } else if (state == 1) {
            if (interacted) { // go back to walking x
                if (transform.position.z <= walkingPosition.y + 0.1f && transform.position.z >= walkingPosition.y - 0.1f) {
                    ReturnToNormalZ();
                    Debug.Log(4);
                } else {
                    Debug.Log(3);
                    movement = new Vector2(Mathf.Clamp(walkingPosition.x - transform.position.x, -1, 1), Mathf.Clamp(walkingPosition.y - transform.position.z, -1, 1)) * speed;
                }
            } else { // go to object
                if (transform.position.x <= walkingPosition.x + 1 && transform.position.x >= walkingPosition.x - 1) {
                    Debug.Log(2);
                    //if(isNearEnough) do: meuk
                    movement = new Vector2(Mathf.Clamp(targetPosition.x - transform.position.x, -1, 1), Mathf.Clamp(targetPosition.y - transform.position.z, -1, 1)) * speed;
                } else {
                    Debug.Log(1);
                    movement = new Vector2(Mathf.Clamp(walkingPosition.x - transform.position.x, -1, 1), Mathf.Clamp(walkingPosition.y - transform.position.z, -1, 1)) * speed;
                }
            }
        }

    }

    public bool GoToObject(Vector3 target) {
       return GoToObject(new Vector2(target.x, target.z));
    }

    public bool GoToObject(Vector2 target) {
        Debug.Log("Going to object");
        if(state == 0) {
            state = 1;
            targetPosition = target;
            walkingPosition = new Vector2(target.x, normalzPosition);
            return true;
        } else {
            return false;
        }
    }

    void ReturnToNormalZ() {
        transform.position = new Vector3(transform.position.x, transform.position.y, normalzPosition);
        interacted = false;
        state = 0;
    }

    private void FixedUpdate()
    {
        if(state == 0) {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, 0);
            movement.x *= 0.9f;
        } else if (state == 1){
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.y);
            movement.x *= 0.9f;
            movement.y *= 0.9f;
        } else {
            rb.velocity = Vector3.zero;
        }
    }
}