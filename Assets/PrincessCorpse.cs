using UnityEngine;
using System.Collections;

public class PrincessCorpse : ResponsiveEntity {

    public Sprite[] deathSprites;

    MapManager mapManager;

	// Use this for initialization
	void Start () {
        mapManager = FindObjectOfType<MapManager>();

        SetProperSprite();
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
    



}
