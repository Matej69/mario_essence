using UnityEngine;
using System.Collections;

public class Gumba : ResponsiveEntity {

    public Sprite damagedSprite;
    public Sprite deadSprite;

    enum E_GUMBA_STATE {
        HEALTY,
        DAMAGED,
        DEAD
    }
    E_GUMBA_STATE state;

    SpriteRenderer renderer;

    Timer canStateChangeTimer;

    CameraShader camShader;     

    void Awake() {
        renderer = GetComponent<SpriteRenderer>();
        camShader = FindObjectOfType<CameraShader>();
    }

	// Use this for initialization
	void Start () {
        state = E_GUMBA_STATE.HEALTY;
        InitMasks();
        canStateChangeTimer = new Timer(0.1f);
        velocity = new Vector2(speed, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
        canStateChangeTimer.Tick(Time.deltaTime);
        if(state != E_GUMBA_STATE.DEAD)
            ApplyAllPhysics();
        
    }


    public void OnStateChange(ref GameObject mario) {
        if (!canStateChangeTimer.IsFinished())
            return;

        if (state == E_GUMBA_STATE.HEALTY) {
            mario.GetComponent<CharacterPhysics>().velocity.y = 10;
            renderer.sprite = damagedSprite;
            state = E_GUMBA_STATE.DAMAGED;
        }
        else if (state == E_GUMBA_STATE.DAMAGED) {
            mario.GetComponent<CharacterPhysics>().velocity.y = 15;
            renderer.sprite = deadSprite;
            state = E_GUMBA_STATE.DEAD;
        }

        if (canStateChangeTimer.IsFinished())
            canStateChangeTimer.Reset();
    }





    public override void OnMarioTouchedTop(ref GameObject mario) {        
        if (state != E_GUMBA_STATE.DEAD)
        {
            int rand = Random.Range(0, 22);
            if (rand == 0)
                camShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 0.7f);
        }

        OnStateChange(ref mario);
    }
    public override void OnMarioTouchedBot(ref GameObject mario) {

    }
    public override void OnMarioTouchedHor(ref GameObject mario) {
    }



}
