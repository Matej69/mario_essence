using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Princess : ResponsiveEntity {


    bool marioPushedPeach = false;
    bool isPeachMovable = true;
    bool isFalling = false;
    public Sprite peachFalling;

    public GameObject EssenceJumpscarePrefab;
    bool isEssenceSpawned = false;
    bool canEssenceBeSpawned = false;

    MapManager mapManager;
    CameraShader cameraShader;

	// Use this for initialization
	void Start () {
        mapManager = FindObjectOfType<MapManager>();
        cameraShader = FindObjectOfType<CameraShader>();

        gravity = 0.095f;

        InitMasks();
        velocity = new Vector2(0, 0);
        
        if (mapManager.princessState != MapManager.E_PRINCESS_STATE.ALIVE)
        {
            cameraShader.SetEntityShader(CameraShader.E_ENTITY_SHADER_ID.GHOST, MapManager.E_ENTITY_ID.PRINCESS);
            isPeachMovable = false;
        }

        canEssenceBeSpawned = (Random.Range(0, 9) == 5) ? true : false;

    }
	
	// Update is called once per frame
	void Update () {
        if (!isPeachMovable)
            return;

        ApplyAllPhysics();
        if(mapManager.princessState == MapManager.E_PRINCESS_STATE.ALIVE)
            HandlePrincessFalling();
    }



    public void HandlePrincessFalling() {
        //MOMENT WHEN MARIO TOUCHED PEACH
        if (!isFalling && marioPushedPeach) { 
            GetComponent<SpriteRenderer>().sprite = peachFalling;
            isFalling = true;
            cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 0.1f);
        }
        //MOMENT WHEN PEACH HITS GROUND
        else if (grounded && marioPushedPeach) {
            PrincessCorpse princessCorpse = mapManager.GetEntities(MapManager.E_ENTITY_ID.PRINCESS_CORPSE)[0].GetComponent<PrincessCorpse>();
            princessCorpse.SetInitialSprite();
            mapManager.princessState = MapManager.E_PRINCESS_STATE.DYING;            
            Destroy(gameObject);            
        }

    }

    



    public override void OnMarioTouchedHor(ref GameObject mario) {
        
        if (!marioPushedPeach && isPeachMovable) {
            gameObject.transform.position = new Vector2(transform.position.x + 0.93f, transform.position.y);
            marioPushedPeach = true;
        }
        //spawn essence 
        if (canEssenceBeSpawned && mapManager.princessDeathSpriteID > 5 && !isEssenceSpawned && mapManager.princessState == MapManager.E_PRINCESS_STATE.DEAD)
        {
            Instantiate(EssenceJumpscarePrefab, transform.position, Quaternion.identity);
            isEssenceSpawned = true;
        }
    }






}
