using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour {
    private Vector3 normalPosition;
    [Tooltip("express as local position")]
    [SerializeField] private Vector3 openPosition;

    private void Start() {
        normalPosition = transform.localPosition;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.A)) {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openPosition));
        }
        if (Input.GetKey(KeyCode.C)) {
            StopAllCoroutines();
            MoveDoor(normalPosition);
        }
    }

    public void Open() {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(openPosition));
    }

    IEnumerator MoveDoor(Vector3 newPosition) {
        while (Vector3.Distance(transform.localPosition,newPosition) > 0.1f) {
            transform.localPosition = Vector3.Lerp(transform.position, newPosition, Time.deltaTime);
            yield return null;
        }
    }
}
