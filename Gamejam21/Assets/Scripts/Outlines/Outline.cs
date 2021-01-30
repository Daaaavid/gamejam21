using System;
using UnityEngine;

namespace Outlines
{
    public class Outline : MonoBehaviour
    {
        public Material OutlineMaterial;
        public float ScaleFactor = 1f;
        public Color OutlineColor;

        private Renderer outlineRenderer;

        private void Start()
        {
            outlineRenderer = CreateOutline(OutlineMaterial, ScaleFactor, OutlineColor);
        }

        Renderer CreateOutline(Material outlineMat, float scaleFactor, Color color){

            GameObject outlineObject = Instantiate(this.gameObject, transform.position, transform.rotation ,transform);
            Renderer rend = outlineObject.GetComponent<Renderer>();
            rend.material = outlineMat;
            rend.material.SetColor("_OutlineColor", color);
            rend.material.SetFloat("_ScaleFactor", scaleFactor);
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            outlineObject.GetComponent<Outline>().enabled = false;
            outlineObject.GetComponent<Collider>().enabled = false;
            rend.enabled = false;

            return rend;
        }

        private void OnMouseEnter()
        {
            outlineRenderer.enabled = true;
        }

        private void OnMouseExit()
        {
            outlineRenderer.enabled = false;
        }
    }
}