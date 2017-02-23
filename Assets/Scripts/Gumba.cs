using UnityEngine;
using System.Collections;

public class Gumba : ResponsiveEntity {

    public Sprite damagedSprite;
    public Sprite deadSprite;

    bool gumbaDamaged = false;
    bool isGumbaTouchedAudioSpawn = false;
    bool gumbaKilledMario = false;

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
        InitRefrences();
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
            mario.GetComponent<Mario>().velocity.y = 10;
            renderer.sprite = damagedSprite;
            state = E_GUMBA_STATE.DAMAGED;
        }
        else if (state == E_GUMBA_STATE.DAMAGED) {
            mario.GetComponent<Mario>().velocity.y = 16;
            renderer.sprite = deadSprite;
            state = E_GUMBA_STATE.DEAD;
        }

        if (canStateChangeTimer.IsFinished())
            canStateChangeTimer.Reset();
    }



    void OnMarioDiedByGumba(ref GameObject mario) {
        if (gumbaDamaged)
            return;

        if (gumbaKilledMario)
            return; 
        //set new mario state
        mario.GetComponent<Mario>().SetAnimatorState(Mario.E_ANIM_STATE.DIE);
        mario.GetComponent<Mario>().isDeath = true;
        mario.GetComponent<Mario>().SetVelocity(new Vector2(0, 0));
        //set shader
        camShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 0.05f);        
        GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(ResetMap(3f));
        //create sound
        if (!isGumbaTouchedAudioSpawn){
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.GUMBA_TOUCHED);
            isGumbaTouchedAudioSpawn = true;
        }
        gumbaKilledMario = true;
    }
    IEnumerator ResetMap(float _sec) {
        yield return new WaitForSeconds(_sec);
        mapManager.CreateMap(mapManager.currentMap);           
    }




    public override void OnMarioTouchedTop(ref GameObject mario) {
        camShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.EARTQUAKE, 0.1f);
        if (state != E_GUMBA_STATE.DEAD)
        {
            //spawn proper audio
            if (!gumbaDamaged)
                audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.JUMP_SMALL);
            else
                audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.JUMP_BIG);

            gumbaDamaged = true;
            int rand = Random.Range(0, 22);
            if (rand == 0)
                camShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 0.7f);
        }

        OnStateChange(ref mario);
    }
    public override void OnMarioTouchedBot(ref GameObject mario) {
        OnMarioDiedByGumba(ref mario);
    }
    public override void OnMarioTouchedHor(ref GameObject mario) {
        OnMarioDiedByGumba(ref mario);
    }



}
