using UnityEngine;
using System.Collections;

public class OffsetScroll : MonoBehaviour {

    public float scrollSpeed = 0.1f;
    new Renderer renderer;
    Transform cameraTransform, thisTransform;

    void Start() {
        renderer = GetComponent<Renderer>();       
        cameraTransform = Camera.main.transform;
        thisTransform = this.transform;
    }

    void Update() {
        float x = cameraTransform.position.x * -scrollSpeed;
        float y = cameraTransform.position.y * -scrollSpeed;
        
        renderer.sharedMaterial.SetTextureOffset("_MainTex", new Vector2(x, y));
        thisTransform.position = new Vector3(cameraTransform.position.x, cameraTransform.position.y, thisTransform.position.z);        
    }
}