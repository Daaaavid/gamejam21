using System.Collections;
using System.IO;
using UnityEngine;

namespace Inventory
{
    public class IconRenderCam : MonoBehaviour
    {

        public Transform contents;
        public KeyCode screenshotKey;
        private Camera Camera
        {
            get
            {
                if (!_camera)
                {
                    _camera = Camera.main;
                }
                return _camera;
            }
        }
        [SerializeField]
        private Camera _camera;
 
        private void LateUpdate()
        {
            if (Input.GetKeyDown(screenshotKey))
            {
                StartCoroutine(CaptureAll());
            }
        }

        IEnumerator CaptureAll()
        {
            for(int i = 0; i < contents.childCount; i++)
            {
                var obj = contents.GetChild(i);
                obj.gameObject.SetActive(true);
                yield return null;
                Capture();
                yield return null;
                obj.gameObject.SetActive(false);
                yield return null;
            }
        }

        public void Capture()
        {
            Debug.Log("starting");
            RenderTexture activeRenderTexture = RenderTexture.active;
            RenderTexture.active = Camera.targetTexture;
 
            Camera.Render();
 
            Texture2D image = new Texture2D(Camera.targetTexture.width, Camera.targetTexture.height);
            image.ReadPixels(new Rect(0, 0, Camera.targetTexture.width, Camera.targetTexture.height), 0, 0);
            image.Apply();
            RenderTexture.active = activeRenderTexture;
 
            byte[] bytes = image.EncodeToPNG();
            Destroy(image);
 
            File.WriteAllBytes("Assets/Models/Icons/" + GetActiveFish()+ "_icon.png", bytes);
        }

        private string GetActiveFish()
        {
            for (int i = 0; i < contents.childCount; i++)
            {
                var r = contents.GetChild(i).gameObject;
                if (r.activeSelf) return r.name.ToLower().Replace(" ", "");
            }
            return "";
        }
    }
}
