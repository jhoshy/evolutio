using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Food : MonoBehaviour {

    Vector3 originalPos;
    Transform thisTransform;
    float newZ;
    Vector3 newPos;

    void Start() {
        thisTransform = this.transform;
        originalPos = this.transform.localPosition;        
        InvokeRepeating("randomize", 0, 1);         
    }

    void randomize() {
        newZ = Random.Range(0,360);
        newPos = Random.insideUnitCircle * 1.0f;
    }

    void Update() {
        if (!Controller.singleton.isPlay) return;
        thisTransform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(thisTransform.eulerAngles.z, newZ, Time.deltaTime * 40));
        thisTransform.localPosition = Vector3.Slerp(thisTransform.localPosition, originalPos + newPos, Time.deltaTime * 0.2f);
    }

    //When destroyed, object removes itself from its list
    void OnDestroy() {
        Controller.singleton.spawnedFoods.Remove(this.gameObject);        
    }
}
