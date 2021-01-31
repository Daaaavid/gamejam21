using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoToEnd : MonoBehaviour
{
    private void Start() {
        StartCoroutine(WaitAndEnd());
    }
    IEnumerator WaitAndEnd() {
        yield return new WaitForSeconds(240);
        SceneManager.LoadScene(2);
    }
}
