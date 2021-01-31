using UnityEngine;

namespace DefaultNamespace
{
    [ExecuteInEditMode]
    public class Tool : MonoBehaviour
    {
        public GameObject empty;

        public void DoStuff()
        {
            var r = FindObjectsOfType<Transform>();
            Debug.Log(r.Length);
            foreach (var transform1 in r)
            {
                var p = Instantiate(empty);
                transform1.SetParent(p.transform);
                p.name = "prefab_" + transform1.name;
            }
        }
    }
}