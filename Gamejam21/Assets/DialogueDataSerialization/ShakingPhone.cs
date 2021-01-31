using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakingPhone : MonoBehaviour
{
    [SerializeField] private Vector3 rotation1;
    [SerializeField] private Vector3 rotation2;
    [SerializeField] private Transform horn;
    [SerializeField] private Transform wantedPosition;
    [SerializeField] private Transform dialogueBox;
	
	public AudioSource phoneloopSource;
	public AudioSource phonehornSource;
	//public AudioSource bossvoSource;
	//public AudioClip pickupClip;
	//public AudioClip endClip;


    public void Start() {
        StartCoroutine(WaitTillRing());
    }

    public void Update() {
        if (Input.GetKey(KeyCode.P)) {
            PickUpPhone();
        }
    }

    IEnumerator WaitTillRing() {
        yield return new WaitForSeconds(5);
        GetComponent<AudioSource>().Play();
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
		if(phoneloopSource != null){
			phoneloopSource.Play();
		}
		if(phonehornSource != null){
			phonehornSource.Play();
		}
    }

    public void PickUpPhone() {
        StopAllCoroutines();
        GetComponent<AudioSource>().Stop();
        StartCoroutine(AwnserPhone());
    }
}
