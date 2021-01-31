using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransistionCamera : MonoBehaviour {

    public void MakeTransition(Vector3 target, TwoDimensionalMovement Player, Elevator targetElevator) {
        StopAllCoroutines();
        StartCoroutine(Move(target, Player, targetElevator));
    }

    IEnumerator Move(Vector3 target, TwoDimensionalMovement player, Elevator targetElevator) {
        Vector3 startPosition = transform.position;
        float time = 0;
        while(time < 2) {
            transform.position = Vector3.Lerp(startPosition, target, time/2);
            time += Time.deltaTime;
            yield return null;
        }
        player.gameObject.SetActive(true);
        targetElevator.OpenElevator(false);
        yield return new WaitForSeconds(.5f);//de deur moet eerst opengaan voordat de speler interactionComplete op true zet, anders loopt ie tegen de deur aan.
        player.interactionComplete = true;
        transform.gameObject.SetActive(false);
    }
}
