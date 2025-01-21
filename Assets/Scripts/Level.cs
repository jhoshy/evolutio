using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level : MonoBehaviour {

    public GameObject myLevelPopup;
    public bool spawnRedEnemies, spawnFoodEnemies, spawnFood;

    void OnEnable() {
        myLevelPopup.SetActive(true);
        try {
            Controller.singleton.clearEnemies();            
        } catch { }
    }

    void Start() {
        //Spawn system
        if (spawnFood)
            Controller.singleton.foodSpawn(false);
        if (spawnFoodEnemies)
            Controller.singleton.foodEnemySpawn(true);
        if (spawnRedEnemies)
            Controller.singleton.redEnemySpawn(true);
    }

    void Update() {
        //Procedural spawn system       
        if (spawnFood)
            Controller.singleton.foodSpawn(true);
        if (spawnFoodEnemies)
            Controller.singleton.foodEnemySpawn(true);
        if (spawnRedEnemies)
            Controller.singleton.redEnemySpawn(true);
    }

}
