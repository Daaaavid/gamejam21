using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalData : MonoBehaviour
{
    public List<GlobalInt> data = new List<GlobalInt>();

    private void Awake() {
        foreach(GlobalInt variable in data) {
                PlayerPrefs.SetInt(variable.key, variable.value);
        }
    }

    void Update() {
        foreach (GlobalInt variable in data) {
            variable.value = PlayerPrefs.GetInt(variable.key);
        }
    }
}
