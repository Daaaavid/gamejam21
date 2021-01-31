using System;
using UnityEngine;

namespace Interaction
{
    public class GrowPlant : MonoBehaviour
    {
        public GameObject GrownPlant;
        public GameObject DeadPlant;

        private void Awake()
        {
            GrownPlant.SetActive(false);
            GetComponent<InteractableObject>().OnInteractAudio.AddListener(Grow);
        }

        public void Grow()
        {
            GrownPlant.SetActive(true);
            DeadPlant.SetActive(false);
        }
    }
}