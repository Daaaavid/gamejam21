using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulDeform : MonoBehaviour
{
    public float scaleRate;
	public bool growing;
	public int maxSize = 120;
	public int minSize = 45;
	
	public void ApplyScaleRate(){
		//transform.localScale += Vector3.one * scaleRate;
	}
	
	public void main(){
		if(this.transform.localScale.y >= maxSize){
			growing = false;
		}
		
		if(this.transform.localScale.y <= minSize){
			growing = true;
		}
		
		if(growing == true){
			this.transform.localScale += new Vector3(-1, 1, 0) * scaleRate;
			this.transform.Rotate(0.0f, 1.0f, 0.0f);
		} else {
			this.transform.localScale -= new Vector3(-1, 1, 0) * scaleRate;
			this.transform.Rotate(1.0f, 0.0f, 0.0f);
		}
		
		/*if(this.transform.localScale.y <= 150) {
			this.transform.localScale += new Vector3(0, 1, 0);
		}
		else if(this.transform.localScale.y >= 20) {
			this.transform.localScale -= new Vector3(0, 1, 0);
		}*/
		//ApplyScaleRate();
	}
	
	// Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
       main();
    }
}
