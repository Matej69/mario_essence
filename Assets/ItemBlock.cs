using UnityEngine;
using System.Collections;

public class ItemBlock : ResponsiveEntity {

    public GameObject[] spawnObjects;
    bool hasBeenHit = false;

    public Sprite emptyBlock;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    
    
    public override void OnMarioTouchedBot(ref GameObject mario) {
        if (hasBeenHit)
            return;

        MapManager mapManager = FindObjectOfType<MapManager>();
        int rand = Random.Range(0, 3);
        GameObject prefab = null;
        switch (rand) {
            case 0:
                {
                    prefab = mapManager.GetPrefab(MapManager.E_ENTITY_ID.COIN);                    
                }break;
            case 1:
                {
                    prefab = mapManager.GetPrefab(MapManager.E_ENTITY_ID.MUSHROOM);
                }break;
            case 2:
                {
                    prefab = mapManager.GetPrefab(MapManager.E_ENTITY_ID.MUSHROOM_POISON);
                }break;
        }        
        GameObject newObj = (GameObject)Instantiate(prefab, new Vector2(transform.position.x, transform.position.y + 1), Quaternion.identity);
        if(prefab.GetComponent<ResponsiveEntity>().id == MapManager.E_ENTITY_ID.COIN)
            newObj.GetComponent<Coin>().pickedUp = true;

        FindObjectOfType<MapManager>().AddToList(ref newObj);       

        GetComponent<SpriteRenderer>().sprite = emptyBlock;
        hasBeenHit = true;
    }





}
