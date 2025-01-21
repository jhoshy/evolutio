using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaserEnemy : MonoBehaviour {

    public float speed = 1.65f;
    float originalZ;
    Transform thisTransform;

    // Use this for initialization
    void Start() {
        thisTransform = this.transform;
        originalZ = thisTransform.position.z;
    }

    // Update is called once per frame
    void Update() {
        if (!Controller.singleton.isPlay) return;

        Vector3 direction = (EnemyEye.enemyTarget.position - this.thisTransform.position);
        this.thisTransform.position += direction.normalized * Time.deltaTime * speed;
        this.thisTransform.position = new Vector3(this.thisTransform.position.x, this.thisTransform.position.y, originalZ);

        //Destroys this enemy if the fossil has appeared on the scene
        if (Controller.singleton.fossilSpawned && !this.CompareTag("Food")) {
            Destroy(this.gameObject);
        }
    }    
}