using UnityEngine;
using System.Collections;

public class AnimationController : MonoBehaviour {

    enum E_ANIM_STATE {
        PLAYING,
        PAUSED,
        SIZE
    }

    public Sprite[] sprites;
    public float speed;

    E_ANIM_STATE state = E_ANIM_STATE.PLAYING;
    int currentSpriteID = 0;
    Timer nextFrameTimer;

    SpriteRenderer renderer;
    

	// Use this for initialization
	void Start () {
        nextFrameTimer = new Timer(speed);
        renderer = GetComponent<SpriteRenderer>();

    }
	
	// Update is called once per frame
	void Update () {
        nextFrameTimer.Tick(Time.deltaTime);
        HandleAnimation();

    }



    //*****************GET***********************

    //*****************SET***********************
    void SetState(E_ANIM_STATE _state) {
        state = _state;
    }

    //*****************HANDLE***********************
    void RunAnimation() {
        renderer.sprite = sprites[(++currentSpriteID) % sprites.Length];
    }

    void HandleAnimation() {
        if (nextFrameTimer.IsFinished()) {
            RunAnimation();
            nextFrameTimer.Reset();
        }
    }

}
