using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyEnemyParticle : MonoBehaviour {

    public GameObject particlePrefab;

    void OnDestroy () {
        if(Controller.singleton.fossilSpawned)
            Instantiate(particlePrefab, this.transform.position, particlePrefab.transform.rotation);
	}
}
