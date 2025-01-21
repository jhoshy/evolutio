using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

    RuntimePlatform platform = Application.platform;

    public float regularSpeed = 1.2f, tailSpeed = 1.5f, boostSpeed = 2.5f;
    float auxAngle;
    public Transform tail;
    public GameObject auraParticle;
    Transform tailPin;
    Vector3 externalForce;

    // Use this for initialization
    void Start() {
        //Pin the tail to a new object
        tail.parent = null;
        GameObject tempGo = new GameObject("TailPin");
        tailPin = tempGo.transform;
        tailPin.position = tail.position;
        tailPin.parent = transform;
    }

    // Update is called once per frame
    float lastTapTime, lastBoostTime;
    float boostCooldown = 1.0f;

    bool boost;
    void Update() {
        if (Controller.singleton.isPlay) {
            //Touch
            if (Input.touchCount > 0) {
                if (Input.GetTouch(0).phase == TouchPhase.Canceled || Input.GetTouch(0).phase == TouchPhase.Ended) {
                    boost = false;
                } else if (Input.GetTouch(0).phase == TouchPhase.Moved) {
                    checkTouch(Input.GetTouch(0).position);
                } else if (Input.GetTouch(0).phase == TouchPhase.Began) {
                    checkTouch(Input.GetTouch(0).position);
                    if (!boost && Time.time - lastTapTime < 0.5f) {
                        boost = true;
                    }
                    lastTapTime = Time.time;
                }
            } else {
                //Mouse/Keyboard
                if (Input.GetMouseButtonUp(0) || Input.GetKeyUp(KeyCode.Space)) {
                    boost = false;
                }
                if (Input.GetMouseButton(0)) {
                    checkTouch(Input.mousePosition);
                }
                if (Input.GetMouseButtonDown(0)) {
                    checkTouch(Input.mousePosition);
                    if (!boost && Time.time - lastTapTime < 0.5f) {
                        boost = true;
                    }
                    lastTapTime = Time.time;
                }
                if (Input.GetKey(KeyCode.Space)) {
                    boost = true;
                }
            }
         
            //Verifies which speed to use
            float speed;
            if (Controller.singleton.currentLevel == 0) {
                speed = regularSpeed;
            } else {
                if (boost) {
                    speed = boostSpeed;
                } else {
                    speed = tailSpeed;
                }
                //When boost is active, decrease 1 point from the score every second
                if (boost && Time.time - lastBoostTime > boostCooldown) {
                    lastBoostTime = Time.time;
                    Controller.singleton.removeScore(2, true);
                }
            }
            transform.position += transform.right * Time.deltaTime * speed;
            //Shows/hides boost particle
            auraParticle.SetActive(speed == boostSpeed);
        } else {
            boost = false;
        }        

        //Normal camera
        if (!Controller.singleton.fossilSpawned) {
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, new Vector3(this.transform.position.x, this.transform.position.y, Camera.main.transform.position.z), Time.deltaTime * 5);
        } else {
            //Fossil camera
            Vector3 fossilPos = Controller.singleton.fossilAux.transform.position;
            Vector3 pontoMedio = fossilPos + ((this.transform.position - fossilPos) / 2);
            pontoMedio.z = -10;
            Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, pontoMedio, Time.deltaTime * 2);

            //Limits player moviment when fossil is spawned
            if (Vector3.Distance(this.transform.position, fossilPos) > Controller.cameraRange) {
                forceTouch(fossilPos);
                PushPlayer((fossilPos - this.transform.position).normalized * 1.5f);
            }
        }

        updateRotation();
        updateExternalForce();
    }

    private void checkTouch(Vector3 pos) {

        Vector3 screenPoint = Camera.main.WorldToScreenPoint(transform.localPosition);
        Vector3 offset = new Vector2(pos.x - screenPoint.x, pos.y - screenPoint.y);
        auxAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }
    private void forceTouch(Vector3 pos) {

        Vector3 offset = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y);
        auxAngle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
        //transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void updateRotation() {
        //Smooth body rotation
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.Euler(0, 0, auxAngle), Time.deltaTime * 13);
        //Tail rotation delay               
        tail.rotation = Quaternion.LerpUnclamped(tail.rotation, Quaternion.Euler(0, 0, auxAngle), Time.deltaTime * 3f);
        tail.position = tailPin.position;
    }

    public void PushPlayer(Vector3 forceDirection) {
        forceDirection.z = this.transform.position.z;
        this.externalForce = forceDirection;
    }
    private void updateExternalForce() {
        if (externalForce.magnitude >= 0.5f) {
            this.transform.position += externalForce * Time.deltaTime * 10;
            externalForce = Vector3.MoveTowards(externalForce, Vector3.zero, Time.deltaTime * 10);
        } else {
            externalForce = Vector3.zero;
        }
        //Debug.Log(externalForce.magnitude);
    }

    void OnTriggerEnter2D(Collider2D coll) {
        if (coll.CompareTag("Fossil")) {
            Controller.singleton.showPopUpQuiz();
        }
    }
}
