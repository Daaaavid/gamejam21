using System;
using UnityEngine;

namespace Interaction
{
    public class GrowPlantOnInteract : MonoBehaviour
    {
        public GameObject GrownPlant;

        private void Awake()
        {
            GrownPlant.SetActive(false);
            GetComponent<InteractableObject>().OnInteractAudio.AddListener(Grow);
        }

        public void Grow()
        {
            GrownPlant.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}