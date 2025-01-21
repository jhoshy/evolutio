using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodEnemy : MonoBehaviour {


    public GameObject chewParticle;
    public float speed = 2.2f;
    Vector3 euler;
    Transform thisTransform;

    // Use this for initialization
    void Start() {
        thisTransform = this.transform;
        InvokeRepeating("randomRotation", 0, 2);
    }

    // Update is called once per frame
    void Update() {
        if (Controller.singleton != null && !Controller.singleton.isPlay) return;

        Vector3 direction = this.thisTransform.right;
        this.thisTransform.position += direction.normalized * Time.deltaTime * speed;

        thisTransform.eulerAngles = Vector3.Slerp(thisTransform.eulerAngles, euler, Time.deltaTime * 1);
    }

    void randomRotation() {
        euler = thisTransform.eulerAngles;
        euler.z = Random.Range(0.0f, 360.0f);
    }

    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Food") && Vector2.Distance(other.transform.position, this.thisTransform.position) < 0.25f) {
            Destroy(other.gameObject);
            GameObject temp = (GameObject)Instantiate(chewParticle, new Vector3(this.thisTransform.position.x, this.thisTransform.position.y, this.thisTransform.position.z + 1), chewParticle.transform.rotation);
            temp.transform.SetParent(this.thisTransform);
        }
    }


    //When destroyed, object removes itself from its list
    void OnDestroy() {
        Controller.singleton.spawnedFoodEnemies.Remove(this.gameObject);
    }
}
