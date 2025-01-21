using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyEye : MonoBehaviour {

    //Static fields
    //Target is given on "Start()" method of Player's "Mouth.cs" script
    public static Transform enemyTarget;

    //Public fields
    public float maxDistance;

    //Private fields
    Transform thisTransform, eyePivot;
    Vector3 originalPos;
    Vector3 newPos;

    void Start() {
        thisTransform = this.transform;
        originalPos = this.transform.position;
        GameObject tempGo = new GameObject("MyEyePivot");
        eyePivot = tempGo.transform;
        eyePivot.position = originalPos;
        eyePivot.parent = this.transform.parent;
    }

    void Update() {
        Vector3 newPos = eyePivot.position + ((enemyTarget.position - eyePivot.position).normalized * maxDistance);
        newPos.z = eyePivot.position.z;
        thisTransform.position = newPos;
    }
}
