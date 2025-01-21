using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tail : MonoBehaviour {
    
	void Update () {
        if (Controller.singleton == null)
            return;

        if (Controller.singleton.currentLevel == 0) {            
            foreach (Transform child in this.transform) {
                child.gameObject.SetActive(false);
            }
        } else {
            foreach (Transform child in this.transform) {
                child.gameObject.SetActive(true);
            }
        }
    }
}
