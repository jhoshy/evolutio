using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mouth : MonoBehaviour {

    public GameObject chewParticle;
    public Beak b1, b2;

    void Start() {
        EnemyEye.enemyTarget = this.transform;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Food")){
            b1.OpenBeak();
            b2.OpenBeak();
        }
    }
    void OnTriggerStay2D(Collider2D other) {
        if (other.CompareTag("Food") && Vector3.Distance(other.transform.position, this.transform.position) < 0.25f) {
            b1.CloseBeak();
            b2.CloseBeak();
            Destroy(other.gameObject);
            Invoke("OnTriggerExit2D", 0.2f);
            //Instantiate particle
            GameObject temp = (GameObject) Instantiate(chewParticle, this.transform.position, chewParticle.transform.rotation);
            temp.transform.SetParent(this.transform);
            //Play Chew Sound
        }
    }
    void OnTriggerExit2D() {
        b1.NormalBeak();
        b2.NormalBeak();
    }

}
