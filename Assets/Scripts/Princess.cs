using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Princess : ResponsiveEntity {


    bool marioPushedPeach = false;
    public Sprite peachFalling;

    MapManager mapManager;
    CameraShader cameraShader;

	// Use this for initialization
	void Start () {
        mapManager = FindObjectOfType<MapManager>();
        cameraShader = FindObjectOfType<CameraShader>();

        InitMasks();
        velocity = new Vector2(0, 0);

        Debug.Log(mapManager.princessState);
        //if (mapManager.princessState != MapManager.E_PRINCESS_STATE.ALIVE)
        //    cameraShader.SetEntityShader(CameraShader.E_ENTITY_SHADER_ID.GHOST, MapManager.E_ENTITY_ID.PRINCESS);
	
	}
	
	// Update is called once per frame
	void Update () {
        ApplyAllPhysics();
        if(mapManager.princessState == MapManager.E_PRINCESS_STATE.ALIVE)
            HandlePrincessFalling();
    }



    public void HandlePrincessFalling() {
        //MOMENT WHEN MARIO TOUCHED PEACH
        if (!grounded && marioPushedPeach)
            GetComponent<SpriteRenderer>().sprite = peachFalling;
        //MOMENT WHEN PEACH HITS GROUND
        else if (grounded && marioPushedPeach) {
            PrincessCorpse princessCorpse = mapManager.GetEntities(MapManager.E_ENTITY_ID.PRINCESS_CORPSE)[0].GetComponent<PrincessCorpse>();
            princessCorpse.SetInitialSprite();
            Destroy(gameObject);
            mapManager.princessState = MapManager.E_PRINCESS_STATE.DYING;
        }

    }




    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (!marioPushedPeach) {
            gameObject.transform.position = new Vector2(transform.position.x + 0.93f, transform.position.y);
            marioPushedPeach = true;
        }
    }






}
