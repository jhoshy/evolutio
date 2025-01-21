using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedEnemy : MonoBehaviour {
        
    Transform thisTransform;    
    float speed = 0.65f;    
    float originalZ;

    // Use this for initialization
    void Start() {
        thisTransform = this.transform;
        originalZ = thisTransform.position.z;
        InvokeRepeating("randomize", 0, 6);
    }
    Vector3 direction;
    // Update is called once per frame
    void Update() {
        if (!Controller.singleton.isPlay) return;
                
        this.thisTransform.position += direction.normalized * Time.deltaTime * speed;
        this.thisTransform.position = new Vector3(this.thisTransform.position.x, this.thisTransform.position.y, originalZ);        
    }

    void randomize() {
        direction = Random.insideUnitSphere; //(EnemyEye.enemyTarget.position - this.thisTransform.position);
    }

    //When destroyed, object removes itself from its list
    void OnDestroy() {
        Controller.singleton.spawnedRedEnemies.Remove(this.gameObject);
    }
}
