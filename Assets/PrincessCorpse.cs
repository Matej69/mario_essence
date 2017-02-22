using UnityEngine;
using System.Collections;

public class PrincessCorpse : ResponsiveEntity {

    public Sprite[] deathSprites;
    [Space]
    public GameObject EssenceJumpscarePrefab;

    bool isEssenceSpawned = false;

    MapManager mapManager;
    CameraShader cameraShader;

	// Use this for initialization
	void Start () {
        InitMasks();

        mapManager = FindObjectOfType<MapManager>();
        cameraShader = FindObjectOfType<CameraShader>();

        SetProperSprite();
        if (mapManager.princessState != MapManager.E_PRINCESS_STATE.ALIVE)
        {
            if(mapManager.princessDeathSpriteID > 4)
            cameraShader.SetEntityShader(CameraShader.E_ENTITY_SHADER_ID.GLITCHED_SMALL, MapManager.E_ENTITY_ID.PRINCESS_CORPSE);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}



    void SetProperSprite() {
        if (mapManager.princessState == MapManager.E_PRINCESS_STATE.ALIVE)
            GetComponent<SpriteRenderer>().sprite = null;
        if (mapManager.princessState == MapManager.E_PRINCESS_STATE.DYING)
            if (mapManager.princessDeathSpriteID + 1 < deathSprites.Length) {
                GetComponent<SpriteRenderer>().sprite = deathSprites[++mapManager.princessDeathSpriteID];
                }
            else {
                mapManager.princessState = MapManager.E_PRINCESS_STATE.DEAD;

            }
        if (mapManager.princessState == MapManager.E_PRINCESS_STATE.DEAD) {
            GetComponent<SpriteRenderer>().sprite = null;
            /*
            DO SHIT HERE
            */

        }
    }

    public void SetInitialSprite() {
        GetComponent<SpriteRenderer>().sprite = deathSprites[0];
    }





    public override void OnMarioTouchedHor(ref GameObject mario)
    {
        if (mapManager.princessDeathSpriteID > 5 && !isEssenceSpawned && mapManager.princessState != MapManager.E_PRINCESS_STATE.DEAD) { 
            Instantiate(EssenceJumpscarePrefab, transform.position, Quaternion.identity);
            isEssenceSpawned = true;
        }
    }






}
