using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorDoor : MonoBehaviour {
    [SerializeField]private Vector3 normalPosition;
    [Tooltip("express as local position")]
    [SerializeField] private Vector3 openPosition;

    private void Start() {
        normalPosition = transform.localPosition;
    }

    private void Update() {
        if (Input.GetKey(KeyCode.O)) {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(openPosition));
        }
        if (Input.GetKey(KeyCode.C)) {
            StopAllCoroutines();
            StartCoroutine(MoveDoor(normalPosition));
        }
    }

    public void Open() {
        StopAllCoroutines();
        StartCoroutine(MoveDoor(openPosition));
    }

    public void Close() {
        StopAllCoroutines();
        StartCoroutine(ShutIt(normalPosition));
    }

    IEnumerator MoveDoor(Vector3 newPosition) {
        while (Vector3.Distance(transform.localPosition,newPosition) > 0.00001f) {
            transform.localPosition = Vector3.Lerp(transform.localPosition, newPosition, Time.deltaTime);
            yield return null;
        }
    }
    IEnumerator ShutIt(Vector3 newPosition) {
        Vector3 startPosition = transform.localPosition;
        float time = 0;
        while (time < 1) {
            transform.localPosition = Vector3.Lerp(startPosition, newPosition, time);
            time += Time.deltaTime;
            yield return null;
        }
    }
    
}
