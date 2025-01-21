using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour {
        
    public float speed;

	void Update () {
        //Vector3 newPos = new Vector3(target.position.x, target.position.y, this.transform.position.z);
        //this.transform.position = Vector3.Lerp(this.transform.position, newPos, Time.deltaTime * speed);

        Vector3 direction = this.transform.right;
        this.transform.position += direction.normalized * Time.deltaTime * speed;
    }
}
