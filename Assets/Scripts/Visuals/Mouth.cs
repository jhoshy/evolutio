using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using LoLSDK;

public class Mouth : MonoBehaviour {

    public GameObject chewParticle;
    public Beak b1, b2;
    PlayerScript player;

    void Start() {
        EnemyEye.enemyTarget = this.transform;
        player = GetComponentInParent<PlayerScript>();
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Food") || other.CompareTag("FoodEnemy")) {
            b1.OpenBeak();
            b2.OpenBeak();
        } else if (other.CompareTag("Fossil")) {
            Controller.singleton.showPopUpQuiz();
        } else if (other.CompareTag("Enemy")) {
            Controller.singleton.removeScore(40, true);
            player.PushPlayer((player.transform.position - other.transform.position).normalized * 2);
            Controller.singleton.uiHit.SetActive(true);
            LOLSDK.Instance.PlaySound("FX/sfx_hit_by_enemy.mp3", false, false);
        }
    }
    
    void OnTriggerStay2D(Collider2D other) {
        if ((other.CompareTag("Food") || other.CompareTag("FoodEnemy")) && Vector2.Distance(other.transform.position, this.transform.position) < 0.25f) {
            b1.CloseBeak();
            b2.CloseBeak();
            Destroy(other.gameObject);
            Invoke("OnTriggerExit2D", 0.2f);
            //Instantiate particle
            GameObject temp = (GameObject)Instantiate(chewParticle, new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z+1), chewParticle.transform.rotation);
            temp.transform.SetParent(this.transform);
            if (other.CompareTag("Food")) {
                Controller.singleton.addScore(20, true);
            } else if (other.CompareTag("FoodEnemy")) {
                Controller.singleton.addScore(40, true);
            }
            //Play Chew Sound
            LOLSDK.Instance.PlaySound("FX/sfx_chew.mp3", false, false);
        }
        //Knockback
        if (other.CompareTag("Enemy")) {
            Controller.singleton.removeScore(40, true);
            player.PushPlayer((player.transform.position - other.transform.position).normalized * 1);
        }
    }
    void OnTriggerExit2D() {
        b1.NormalBeak();
        b2.NormalBeak();
    }

}
