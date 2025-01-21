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
        InvokeRepeating("UpdateA", 0, 1);
    }

    void UpdateA() {
        newZ = Random.Range(0,360);
        newPos = Random.insideUnitCircle * 1.0f;
    }

    void Update() {
        thisTransform.eulerAngles = new Vector3(0, 0, Mathf.MoveTowardsAngle(thisTransform.eulerAngles.z, newZ, Time.deltaTime * 40));
        thisTransform.localPosition = Vector3.Slerp(thisTransform.localPosition, originalPos + newPos, Time.deltaTime * 0.2f);
    }
}
