using UnityEngine;

public class Popup : MonoBehaviour {

    public GameObject CoverPrefab;
    public Transform CoverParent;
    GameObject Cover;
    
    void OnEnable() {
        Cover = (GameObject)Instantiate(CoverPrefab, CoverPrefab.transform.position, CoverPrefab.transform.rotation);
        Cover.transform.SetParent(CoverParent);
    }

	void Update () {
        Controller.singleton.isPlay = false;
	}

    void OnDisable() {
        Controller.singleton.isPlay = true;
        Destroy(Cover);
    }
}
