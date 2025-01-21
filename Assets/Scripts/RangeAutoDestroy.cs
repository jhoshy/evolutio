using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeAutoDestroy : MonoBehaviour {

    Transform thisTransform, cameraTransform;
	
	void Start () {
        thisTransform = this.transform;
        cameraTransform = Camera.main.transform;
	}
	
	void Update () {
        //Destroys itself if not in camera range
        float distance = (new Vector3(thisTransform.position.x, thisTransform.position.y, 0) - new Vector3(cameraTransform.position.x, cameraTransform.position.y, 0)).magnitude;
        if (distance > Controller.cameraRange) {
            Destroy(this.gameObject);
        }
        //Also destroys an enemy if the fossil has appeared on the scene
        if (Controller.singleton.fossilSpawned && !this.CompareTag("Food")) {
            Destroy(this.gameObject);
        }
    }
}
