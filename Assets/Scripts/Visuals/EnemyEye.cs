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
    Transform thisTransform;
    Vector3 originalPos;
    Vector3 newPos;

    void Start() {
        thisTransform = this.transform;
        originalPos = this.transform.position;
    }

    void Update() {
        Vector3 newPos = originalPos + ((enemyTarget.position - originalPos).normalized * maxDistance);
        newPos.z = originalPos.z;
        thisTransform.position = newPos;
    }
}
