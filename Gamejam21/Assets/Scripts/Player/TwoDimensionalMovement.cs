using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;
using UnityEngine.Serialization;

public class TwoDimensionalMovement : MonoBehaviour {
    float normalzPosition;
    private Vector2 movement;
    public Vector2 walkingPosition;
    public Vector2 targetPosition;

    private Rigidbody rb;
    public float speed = 0.5f;
    [FormerlySerializedAs("interacted")] public bool interactionComplete = false;
    [SerializeField] private Transform playermoddel;
    [SerializeField]private int state = 0; //0 = normal, 1 interacting, 2 = immovable (in converstation)

    public float ProximityTreshold = 0.5f;
    
    public InteractionBus MovementBus;
    public TransformBus MovementBus2;
    
    // Start is called before the first frame update
    void Start() {
        normalzPosition = transform.position.z;
        rb = GetComponentInChildren<Rigidbody>();
        
        MovementBus.OnChange.AddListener(obj => GoToObject(obj.transform.position));
        MovementBus2.OnChange.AddListener(obj => GoToObject(obj.position, true));
        MovementBus.OnInvokeReturnPlayer.AddListener(() => interactionComplete = true);
    }

    // Update is called once per frame
    void Update() {
        if (state == 0) {
            if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
                //walk left
                movement = (-Vector3.right * speed);
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
                //walk right
                movement = (Vector3.right * speed);
            }
            if (Input.GetKey(KeyCode.P)) {
                GoToObject(Vector3.zero);
            }

            if (Input.GetKey(KeyCode.T)) {
                GetComponent<ThoughtBubble>().NewThoughtBubble(GetComponent<ThoughtBubble>()._dialogue, 0);
            }

        } else if (state == 1) {
            if (interactionComplete) { // go back to walking x 4
                if (transform.position.z <= walkingPosition.y + 0.1f && transform.position.z >= walkingPosition.y - 0.1f) {
                    ReturnToNormalZ();
                } else {//3
                    movement = new Vector2(Mathf.Clamp(walkingPosition.x - transform.position.x, -1, 1), Mathf.Clamp(walkingPosition.y - transform.position.z, -1, 1)) * speed;
                }
            } else { // go to object
                if (transform.position.x <= walkingPosition.x + 1 && transform.position.x >= walkingPosition.x - 1) {
                    Debug.Log(Distance());
                    if (Distance() < ProximityTreshold) {
                        MovementBus.OnProximity.Invoke();
                    }//2
                    movement = new Vector2(Mathf.Clamp(targetPosition.x - transform.position.x, -1, 1), Mathf.Clamp(targetPosition.y - transform.position.z, -1, 1)) * speed;
                } else {//1
                    movement = new Vector2(Mathf.Clamp(walkingPosition.x - transform.position.x, -1, 1), Mathf.Clamp(walkingPosition.y - transform.position.z, -1, 1)) * speed;
                }
            }
        }

    }

    private float Distance()
    {
        var a = new Vector2(targetPosition.x, targetPosition.y);
        var position = transform.position;
        var b = new Vector2(position.x, position.z);
        return Vector2.Distance(a, b);
    }

    public bool GoToObject(Vector3 target, bool pushThis) {
        return GoToObject(new Vector2(target.x, target.z), pushThis);
    }

    public bool GoToObject(Vector3 target) {
       return GoToObject(new Vector2(target.x, target.z), false);
    }

    public bool GoToObject(Vector2 target, bool pushThis) {
        Debug.Log("Going to object");
        if(state == 0 || pushThis) {
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
        interactionComplete = false;
        state = 0;
    }

    private void FixedUpdate()
    {
        if(state == 0) {
            rb.velocity = new Vector3(movement.x, rb.velocity.y, 0);
            movement.x *= 0.9f;
            movement.y = 0;
        } else if (state == 1){
            rb.velocity = new Vector3(movement.x, rb.velocity.y, movement.y);
            movement.x *= 0.9f;
            movement.y *= 0.9f;
        } else {
            rb.velocity = Vector3.zero;
        }
        playermoddel.LookAt(transform.position + new Vector3( movement.x, 0, movement.y));
    }
}
