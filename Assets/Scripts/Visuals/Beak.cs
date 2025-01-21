using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beak : MonoBehaviour {

    public float zOpen, zNormal, zClosed;
    float newZ;
    Transform thisTransform;
    void Start () {
        thisTransform = this.transform;
    }    
    
    void Update () {        
        //Does the rotation
        thisTransform.eulerAngles = new Vector3(0,0, Mathf.MoveTowardsAngle(thisTransform.eulerAngles.z, newZ, Time.deltaTime * 500));
        
        //Debug - Funcoes de abrir a boca
        if (Input.GetKeyDown(KeyCode.UpArrow)) OpenBeak();        
        if (Input.GetKeyDown(KeyCode.DownArrow)) CloseBeak();        
        if (Input.GetKeyDown(KeyCode.Space)) NormalBeak();
    }

    public void OpenBeak() {
        newZ = zOpen;
    }
    public void CloseBeak() {
        newZ = zClosed;
    }
    public void NormalBeak() {
        newZ = zNormal;
    }
}
