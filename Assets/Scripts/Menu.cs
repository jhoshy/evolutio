using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using LoLSDK;


public class Menu : MonoBehaviour
{
    public GameObject popUpHTP;

    // Use this for initialization
    void Start()
    {
        if (!LOLSDK.Instance.IsInitialized)
        {
            LOLSDK.Init("com.baiducasoft.evolutio");
            LOLSDK.Instance.PlaySound("Music/music_the_lift.mp3", true, true);
            //LOLSDK.Instance.PlaySound("FX/sfx_ambience_water.mp3", true, true);
        }
        
        LOLSDK.Instance.SubmitProgress(SharedState.score, SharedState.level, SharedState.maxLevel);
    }

    public void loadScene(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void showPopUpHTP()
    {
        popUpHTP.transform.localScale = new Vector3(0, 0, 1);

        popUpHTP.transform.localPosition = new Vector3(-1000, 0, 0);

        popUpHTP.SetActive(true);

        StartCoroutine(EaseFunctions.modificaEscala(popUpHTP.transform, 1, 1, 0, 1f, "easeBackOut"));
        StartCoroutine(EaseFunctions.movimenta(popUpHTP.transform, 1000, 0, 0, 1f, "easeBackOut", false));
    }

    public void hidePopUpHTP()
    {
        StartCoroutine(EaseFunctions.movimenta(popUpHTP.transform, 0, -2000, 0, 1f, "easeBounceOut", true));
    }
}

public class SharedState
{
    public static int score = 0;
    public static int level = 0;
    public static int maxLevel = 9;
}
