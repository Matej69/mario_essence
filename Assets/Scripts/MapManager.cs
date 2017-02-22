using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class TextureIDPair
{
    public MapManager.E_MAP_ID id;
    public Texture2D tex;
}
[System.Serializable]
public class ColorGameObjectPair
{
    public MapManager.E_ENTITY_ID id;
    public Color32 color;
    public GameObject entity;
}

public class MapManager : MonoBehaviour {

    public enum E_MAP_ID {
        START_MAP,
        CLOUD_MAP,
        UNDERGROUND_MAP,
        SIZE
    }

    public enum E_ENTITY_ID {
        MARIO,
        PRINCESS,
        GUMBA,
        PIPE,
        GROUND_NORMAL,
        BLOCK_NORMAL,
        BLOCK_COIN,
        COIN,
        MUSHROOM,
        MUSHROOM_POISON,
        CLOUD,
        BRICK,
        UNDERGROUND_BRICK,
        PRINCESS_CORPSE,
        SIZE
    }

    //*************MAP EVENTS INFO***************
    public E_MAP_ID currentMap = E_MAP_ID.START_MAP;
    public enum E_PRINCESS_STATE {
        ALIVE,
        DYING,
        DEAD
    }
    [HideInInspector]
    public E_PRINCESS_STATE princessState = E_PRINCESS_STATE.ALIVE;
    public int princessDeathSpriteID = 1;
    int coinsCollected = 0;
    int gumbasKilled = 0;
    //*******************************************

    public List<TextureIDPair> mapTextures = new List<TextureIDPair>();
    public List<ColorGameObjectPair> colorObjectPair = new List<ColorGameObjectPair>();

    List<GameObject> entities = new List<GameObject>();
    [HideInInspector] public GameObject marioRefrence;

    public Transform mapSpawnPos;
       
    const float tileSize = 1;

    CameraShader cameraShader;



    // Use this for initialization
    void Start () {
        cameraShader = FindObjectOfType<CameraShader>();

        CreateMap(E_MAP_ID.START_MAP);
    }
	
	// Update is called once per frame
	void Update () {

        HandleOnMarioDead();

    }




    //************SET************
    public void CreateMap(E_MAP_ID _id) {
        DeleteMap();
        cameraShader.ResetEntityShaders();
        currentMap = _id;

        Camera.main.transform.position = new Vector3(0, 5, Camera.main.transform.position.z);

        Texture2D map = GetTexture(_id);
        Color32[] pixels = map.GetPixels32();
        int w = map.width;
        int h = map.height;

        //for every pixel(from bot-left) if it's color is assigned to some GameObject, spawn that GameObject on position relative to the position
        //of that pixel
        for(int i = 0; i < h; ++i) { 
            for(int j = 0; j < w; ++j) {          
                GameObject prefab = GetPrefab(pixels[(i * w) + j]);
                //Vector3 pos = new Vector3(mapSpawnPos.position.x + (tileSize * j), mapSpawnPos.position.y + (tileSize * i), 0);
                Vector3 pos = new Vector3(mapSpawnPos.position.x + (tileSize * j), mapSpawnPos.position.y + (tileSize * i), 0);
                if (prefab != null) {        
                    GameObject newEntity = (GameObject)Instantiate(prefab, pos, Quaternion.identity);
                    newEntity.transform.parent = gameObject.transform;
                    entities.Add(newEntity);
                }                
            }
        }
        //GET MARIO REFRENCE
        marioRefrence = FindObjectOfType<CharacterPhysics>().gameObject;
    }


   public void DeleteMap() {
        for(int i = entities.Count - 1; i >= 0; --i) {
            Destroy(entities[i]);
            entities.RemoveAt(i);
        }
        Destroy(marioRefrence);
    }








    //************GET************
    Texture2D GetTexture(E_MAP_ID _id) {
        foreach (TextureIDPair pair in mapTextures)
            if (pair.id == _id)
                return pair.tex;
        Debug.LogError("TEXTURE WITH ID(ENUM) "+_id+" DOES NOT EXISTS");
        return null;

    }

    GameObject GetPrefab(Color32 _color) {
        foreach (ColorGameObjectPair pair in colorObjectPair) { 
            if (pair.color.Equals(_color))
                return pair.entity;            
        }
        if (_color.a == 0)
            return null;
        Debug.LogError("COLOR+ "+ _color +"IS NOT CONNECTED TO ANY GAME OBJECT");
        return null;
    }

    public GameObject GetPrefab(E_ENTITY_ID _id) {
        foreach (ColorGameObjectPair pair in colorObjectPair) { 
            if (pair.id == _id)
                return pair.entity;            
        }
        Debug.LogError("CAN NOT GET ENTITY WITH ID : + "+ _id);
        return null;
    }

    
    public List<GameObject> GetEntities(E_ENTITY_ID _entityID) {
        List<GameObject> targetedEntities = new List<GameObject>();
        foreach (GameObject go in entities) {
            if(_entityID != E_ENTITY_ID.MARIO) {  
                if (go!= null && go.GetComponent<ResponsiveEntity>() != null && go.GetComponent<ResponsiveEntity>().id == _entityID)
                    targetedEntities.Add(go);
                }
            else {
                GameObject marioSprite = marioRefrence.GetComponent<CharacterPhysics>().MarioSprite;
                targetedEntities.Add(marioSprite);
            }
            
                
        }

        if(targetedEntities.Count == 0)
            Debug.LogError("LIST OF ENTITIES WITH ID = "+ _entityID + " IS EMPTY= ");

        return targetedEntities;
    }

    //************HANDLERS************
    public void RemoveFromList(ref GameObject _objToRemove) {
        for(int i = 0; i < entities.Count; ++i) {
            if (entities[i].gameObject == _objToRemove)
                entities.Remove(_objToRemove);
        }
    }

    public void AddToList(ref GameObject _go) {
        entities.Add(_go);
    }

    public void HandleOnMarioDead() {
        if (marioRefrence == null)
            return;

        if(marioRefrence.transform.position.y < - 3)
            CreateMap(currentMap);
    }


}
