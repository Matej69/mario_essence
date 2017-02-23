using UnityEngine;
using System.Collections;

public class Mushroom : ResponsiveEntity {


    CameraShader cameraShader;
    MapManager mapManager;

    bool isGreenSoundCreated = false;

    // Use this for initialization
    void Start () {
        InitMasks();
        InitRefrences();
        velocity = new Vector2(speed, 0);

        cameraShader = FindObjectOfType<CameraShader>();
        mapManager = FindObjectOfType<MapManager>().GetComponent<MapManager>();

    }
	
	// Update is called once per frame
	void Update () {

        ApplyAllPhysics();

    } 

    
  

     public override void OnMarioTouchedTop(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON && !mario.GetComponent<Mario>().isDeath) {
            OnMarioTouchedPoison(ref mario);
        }
        else if(id == MapManager.E_ENTITY_ID.MUSHROOM)
            OnMarioTouchedNormal(ref mario);
    }

    public override void OnMarioTouchedBot(ref GameObject mario ) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON && !mario.GetComponent<Mario>().isDeath) {
            OnMarioTouchedPoison(ref mario);
        }
        else if (id == MapManager.E_ENTITY_ID.MUSHROOM)
            OnMarioTouchedNormal(ref mario);
    }

    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON && !mario.GetComponent<Mario>().isDeath) {
            OnMarioTouchedPoison(ref mario);
        }
        else if (id == MapManager.E_ENTITY_ID.MUSHROOM)
            OnMarioTouchedNormal(ref mario);
    }



    void OnMarioTouchedPoison(ref GameObject mario) {
        //set new mario state
        mario.GetComponent<Mario>().SetAnimatorState(Mario.E_ANIM_STATE.DIE);
        mario.GetComponent<Mario>().isDeath = true;
        mario.GetComponent<Mario>().SetVelocity(new Vector2(0, 0));
        //set shader
        CameraShader.E_CAM_MATERIAL_ID camShaderID = (CameraShader.E_CAM_MATERIAL_ID)Random.Range(0, (int)CameraShader.E_CAM_MATERIAL_ID.SIZE);
        cameraShader.SetMaterial(camShaderID, 0.2f);
        //hide mushroom becouse if we destroy it, we can't call StartRoutine();
        GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(ResetMap(3f));
        //create sound
        if (!isGreenSoundCreated)
        {
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.MUSHROOM_GREEN);
            isGreenSoundCreated = true;
        }
    }

    void OnMarioTouchedNormal(ref GameObject mario) {
        mapManager.marioRefrence.GetComponent<Mario>().canRun = true;
        audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.MUSHROOM_RED);
        Destroy(gameObject);
    }



    IEnumerator ResetMap(float _sec) {
        yield return new WaitForSeconds(_sec);
        mapManager.CreateMap(mapManager.currentMap);        
    }







 }
