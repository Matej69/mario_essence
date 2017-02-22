using UnityEngine;
using System.Collections;

public class Mushroom : ResponsiveEntity {


    CameraShader cameraShader;
    MapManager mapManager;

    // Use this for initialization
    void Start () {
        InitMasks();
        velocity = new Vector2(speed, 0);

        cameraShader = FindObjectOfType<CameraShader>();
        mapManager = FindObjectOfType<MapManager>().GetComponent<MapManager>();

    }
	
	// Update is called once per frame
	void Update () {

        ApplyAllPhysics();

    } 

    
  

     public override void OnMarioTouchedTop(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            OnMarioTouchedPoison(ref mario);
        }
        else
            OnMarioTouchedNormal(ref mario);
    }

    public override void OnMarioTouchedBot(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            OnMarioTouchedPoison(ref mario);
        }
        else
            OnMarioTouchedNormal(ref mario);
    }

    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            OnMarioTouchedPoison(ref mario);
        }
        else
            OnMarioTouchedNormal(ref mario);
    }



    void OnMarioTouchedPoison(ref GameObject mario) {
        //set new mario state
        mario.GetComponent<CharacterPhysics>().SetAnimatorState(CharacterPhysics.E_ANIM_STATE.DIE);
        mario.GetComponent<CharacterPhysics>().isDeath = true;
        mario.GetComponent<CharacterPhysics>().SetVelocity(new Vector2(0, 0));
        //set shader
        CameraShader.E_CAM_MATERIAL_ID camShaderID = (CameraShader.E_CAM_MATERIAL_ID)Random.Range(0, (int)CameraShader.E_CAM_MATERIAL_ID.SIZE);
        cameraShader.SetMaterial(camShaderID, 0.2f);
        GetComponent<SpriteRenderer>().sprite = null;
        StartCoroutine(ResetMap(3f));
    }

    void OnMarioTouchedNormal(ref GameObject mario) {
        mapManager.marioRefrence.GetComponent<CharacterPhysics>().canRun = true;
        Destroy(gameObject);
    }



    IEnumerator ResetMap(float _sec) {
        yield return new WaitForSeconds(_sec);
        mapManager.CreateMap(mapManager.currentMap);        
    }







 }
