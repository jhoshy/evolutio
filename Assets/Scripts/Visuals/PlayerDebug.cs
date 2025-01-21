using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDebug : MonoBehaviour {

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        this.transform.position = Vector3.MoveTowards(this.transform.position, this.transform.position + this.transform.right, Time.deltaTime * 2);
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 5);
    }
}
