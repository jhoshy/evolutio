using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScore : MonoBehaviour {

    Text scoreText;
	// Use this for initialization
	void Start () {
        scoreText = GetComponent<Text>();
    }
	
	// Update is called once per frame
	void Update () {
        scoreText.text = SharedState.score.ToString();//int.Parse("");

    }
}
