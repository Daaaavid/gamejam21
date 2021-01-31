using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEnd : MonoBehaviour
{
    public float Seconds;
    private void Start() {
        StartCoroutine(WaitAndEnd());
    }
    IEnumerator WaitAndEnd() {
        yield return new WaitForSeconds(Seconds);
        SceneManager.LoadScene(2);
    }
}
