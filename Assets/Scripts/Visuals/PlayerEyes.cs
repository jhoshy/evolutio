using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEyes : MonoBehaviour {

    public GameObject closedEye;

    // Use this for initialization
    void Start() {

    }

    float blinkTime, cooldown;
    // Update is called once per frame
    void Update() {
        if (Time.time >= cooldown) {
            //blinkTime = Time.time;
            cooldown = Time.time + Random.Range(3.0f, 7.0f);
            closedEye.SetActive(true);
            Invoke("OpenEye", 0.1f);
        }
    }

    void OpenEye() {
        closedEye.SetActive(false);
    }
}
