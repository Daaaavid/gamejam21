using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Interaction;

public class ShakingPhone : MonoBehaviour {
    [SerializeField] private Vector3 rotation1;
    [SerializeField] private Vector3 rotation2;
    [SerializeField] private Transform horn;
    [SerializeField] private Transform wantedPosition;
    [SerializeField] private Transform dialogueBox;
    [SerializeField] private AudioSource audiosource1;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioClip noise;
    [SerializeField] private ThoughtBubble FirstPersonThoughtBubble;

    public void Start() {
        audiosource1 = GetComponent<AudioSource>();
        StartCoroutine(WaitTillRing());
    }

    public void Update() {
        if (Input.GetKey(KeyCode.P)) {
            PickUpPhone();
        }
    }

    IEnumerator WaitTillRing() {
        yield return new WaitForSeconds(8);
        GetComponent<AudioSource>().Play();
        this.GetComponent<InteractableObject>().SplitValue = 1;
        StartCoroutine(ShakePhone(rotation1, rotation2));
    }

    IEnumerator ShakePhone(Vector3 position1, Vector3 position2) {
        float time = 0;
        while (time < 1) {
            transform.rotation = Quaternion.Euler(Vector3.Lerp(position1, position2, time));
            time += Time.deltaTime*5;
            yield return null;
        }
        StartCoroutine(ShakePhone(position2, position1));
    }

    IEnumerator AwnserPhone() {
        FirstPersonThoughtBubble.TurnOff();
        Vector3 startRotation = transform.rotation.eulerAngles;
        Vector3 startPosition = transform.position;
        Vector3 endRotation = wantedPosition.rotation.eulerAngles;
        float time = 0;
        while (time < 1) {
            transform.rotation = Quaternion.Lerp(Quaternion.Euler(startRotation),wantedPosition.rotation, time);
            transform.position = Vector3.Lerp(startPosition, wantedPosition.position, time);
            time += Time.deltaTime*2;
            yield return null;
        }
        dialogueBox.gameObject.SetActive(true);
    }

    public void PickUpPhone() {
        if (this.GetComponent<InteractableObject>().SplitValue == 0 || dialogueBox.gameObject.activeSelf)
            return;

        StopAllCoroutines();
        audiosource1.Stop();
        audiosource1.clip = noise;
        audiosource1.Play();
        audioSource2.Play();
        StartCoroutine(AwnserPhone());
    }
}
