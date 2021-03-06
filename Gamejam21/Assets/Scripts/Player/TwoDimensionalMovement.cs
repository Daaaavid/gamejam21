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
    private float targetProximityTreshold;

    private Rigidbody rb;
    public float speed = 0.5f;
    [FormerlySerializedAs("interacted")] public bool interactionComplete = false;
    [SerializeField] public Transform playermoddel;
    [SerializeField]private int state = 0; //0 = normal, 1 interacting, 2 = first person (normal), 3 first person (in converstation)
    public int State { get { return state; } }
    [SerializeField]private Animator myAnimator;
    [SerializeField] private ThoughtBubble myThoughtBubble;
    [SerializeField] private Camera myCamera;

    public InteractionBus MovementBus;
    public TransformBus MovementBus2;
    
    // Start is called before the first frame update
    void Start() {
        if (walkingPosition != Vector2.zero)
            normalzPosition = walkingPosition.y;
        else
            normalzPosition = transform.position.z;
        rb = GetComponentInChildren<Rigidbody>();
        
        MovementBus.OnChange.AddListener(obj => GoToObject(obj.transform.position, obj.ProximityTreshold));
        MovementBus2.OnChange.AddListener(obj => GoToObject(obj.position, 1f, true));
        MovementBus.OnInvokeReturnPlayer.AddListener(() => interactionComplete = true);
        myThoughtBubble = GetComponent<ThoughtBubble>();
    }

    // Update is called once per frame
    void Update() {
        myAnimator.SetFloat("speed", Vector3.Distance(rb.velocity,Vector3.zero)*0.1f);
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
            //    GoToObject(Vector3.zero);
            }

            if (Input.GetKey(KeyCode.T)) {
            //    GetComponent<ThoughtBubble>().NewThoughtBubble(GetComponent<ThoughtBubble>()._dialogue, 0);
            }

        } else if (state == 1) {
            if (interactionComplete) { // go back to walking x 4
                if (transform.position.z <= walkingPosition.y + 0.1f && transform.position.z >= walkingPosition.y - 0.1f) {
                    ReturnToNormalZ();
                } else {//3
                    movement = new Vector2(Mathf.Clamp(walkingPosition.x - transform.position.x, -1, 1), Mathf.Clamp(walkingPosition.y - transform.position.z, -1, 1)) * speed;
                }
            } else { // go to object
                if (transform.position.x <= walkingPosition.x + 2.5f && transform.position.x >= walkingPosition.x - 2.5f) {
                    if (Distance() < targetProximityTreshold) {
                        MovementBus.OnProximity.Invoke();
                    } else {
                        movement = new Vector2(Mathf.Clamp(targetPosition.x - transform.position.x, -1, 1), Mathf.Clamp(targetPosition.y - transform.position.z, -1, 1)) * speed;
                    } //2
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

    public bool GoToObject(Vector3 target, float proximityTreshold, bool pushThis) {
        return GoToObject(new Vector2(target.x, target.z), proximityTreshold, pushThis);
    }

    public bool GoToObject(Vector3 target, float proximityTreshold) {
       return GoToObject(new Vector2(target.x, target.z), proximityTreshold, false);
    }

    public bool GoToObject(Vector2 target, float proximityTreshold, bool pushThis) {
        Debug.Log("Going to object");
        if(state == 0 || pushThis) {
            if(state == 0)
            state = 1;
            targetPosition = target;
            targetProximityTreshold = proximityTreshold;
            walkingPosition = new Vector2(target.x, normalzPosition);
            myThoughtBubble.ActiveListener(false);
            return true;
        } else {
            return false;
        }
    }
    void ReturnToNormalZ() {
        transform.position = new Vector3(transform.position.x, transform.position.y, normalzPosition);
        interactionComplete = false;
        state = 0;
        myThoughtBubble.ActiveListener(true);
    }

    public void SetFirstPersonMode(bool value) {
        Debug.Log("setting first person mode: " + value);
        if (value)
            state = 2;
        else
            state = 1;
        myCamera.gameObject.SetActive(!value);
        myAnimator.gameObject.SetActive(!value);
    }

    public void GoToDialogueState() {
        if (state == 2)
            state = 3;
        else
            Debug.LogError("state needs to be 2 before going to dialogue state");
    }

    private void FixedUpdate()
    {
        if (state == 0) {
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(movement.x, rb.velocity.y, 0), Time.deltaTime * 10);
            movement.x *= 0.9f;
            movement.y = 0;
        } else if (state == 1){
            rb.velocity = Vector3.Lerp(rb.velocity, new Vector3(movement.x, rb.velocity.y, movement.y), Time.deltaTime * 10);
            movement.x *= 0.9f;
            movement.y *= 0.9f;
        } else {
            rb.velocity = Vector3.zero;
        }
        if(Vector3.Distance(rb.velocity,Vector3.zero) > 5)
        playermoddel.rotation = Quaternion.RotateTowards(playermoddel.rotation, Quaternion.LookRotation(new Vector3(movement.x, 0, movement.y)), Time.deltaTime *400);
        //playermoddel.LookAt(transform.position + new Vector3(movement.x, 0, movement.y));
    }
}
