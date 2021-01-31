using System.Collections;
using System.Collections.Generic;
using Interaction;
using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    public class Tool : MonoBehaviour
    {
        public GameObject empty;

        public void DoStuff()
        {
            // var r = FindObjectsOfType<Transform>();
            // Debug.Log(r.Length);
            // foreach (var transform1 in r)
            // {
            //     var p = Instantiate(empty);
            //     transform1.SetParent(p.transform);
            //     p.name = "prefab_" + transform1.name;
            // }
            
            StartCoroutine(CaptureAll());
        }

        IEnumerator CaptureAll()
        {
            Debug.Log(transform.childCount);
            Debug.Log("trying");
            for(int i = 0; i < transform.childCount; i++)
            {
                var obj = transform.GetChild(i);
                obj.gameObject.SetActive(true);
                yield return new WaitForSeconds(1f);
                obj.gameObject.SetActive(false);
                yield return null;
            }
        }
    }
}