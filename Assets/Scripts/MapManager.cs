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
        ALL_COINS_START,
        ALL_COINS_UNDERGROUND,
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
        CLOUD_TRAVEL,
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
    public int coinsCollected = 0;
    int gumbasKilled = 0;

    bool outOfBoundsEffectsTriggered = false;

    bool shouldSpawnGlitchScreen = false;
    float timeOfBlueScreenSpawn = 0.0f;
    List<GameObject> listOfActiveGlitches = new List<GameObject>();

    public GameObject[] glitchedVoices;
    //*******************************************

    public List<TextureIDPair> mapTextures = new List<TextureIDPair>();
    public List<ColorGameObjectPair> colorObjectPair = new List<ColorGameObjectPair>();

    public GameObject BlueScreenPrefab;
    public GameObject FinalBossPrefab;
    public GameObject MorseCodeEntityPrefab;
    public GameObject BarcodePrefab;

    List<GameObject> entities = new List<GameObject>();
    [HideInInspector] public GameObject marioRefrence;

    public GameObject MusicThemePlaying = null;

    public Transform mapSpawnPos;

    const float tileSize = 1;

    CameraShader cameraShader;
    AudioManager audioManager;


    // Use this for initialization
    void Start () {
        cameraShader = FindObjectOfType<CameraShader>();
        audioManager = FindObjectOfType<AudioManager>();
  
        CreateMap(E_MAP_ID.START_MAP);
    }
	
	// Update is called once per frame
	void Update () {

        HandleOnMarioDead();
        EnoughCoinsCollectedHnadler();

    }




    //************SET************
    public void CreateMap(E_MAP_ID _id) {

        DeleteMap();
        cameraShader.ResetEntityShaders();
        currentMap = _id;
        InitLevelMusic(currentMap);

        outOfBoundsEffectsTriggered = false;

        ClearAllGlitchVoiceAreas();
        SpawnGlitchedVoiceHandler();
        StartCoroutine(SpawnBlueScreenHandler());
                       
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
        marioRefrence = FindObjectOfType<Mario>().gameObject;

        //PLACE CAMERA
        Camera.main.transform.position = new Vector3(marioRefrence.transform.position.x, 5, Camera.main.transform.position.z);
        if (currentMap == E_MAP_ID.UNDERGROUND_MAP || currentMap == E_MAP_ID.CLOUD_MAP || currentMap == E_MAP_ID.ALL_COINS_START)
            Camera.main.backgroundColor = new Color32(0, 0, 20, 255);

        //IF CLOUD_MAP -> ADD FINAL BOSS ENTITY
        if (currentMap == E_MAP_ID.CLOUD_MAP)
            entities.Add((GameObject)Instantiate(FinalBossPrefab, new Vector2(216,4), Quaternion.identity));
        //IF ALL_COINS_START -> ADD MORSE CODE ENTITY
        if (currentMap == E_MAP_ID.ALL_COINS_START)
            entities.Add((GameObject)Instantiate(MorseCodeEntityPrefab, new Vector2(176, 5), Quaternion.identity));
        if (currentMap == E_MAP_ID.ALL_COINS_UNDERGROUND)
            entities.Add((GameObject)Instantiate(BarcodePrefab, new Vector2(230, 6.5f), Quaternion.identity));

        //MAYBE SET CAMERA SHADER        
        SetRandomCameraShader();

    }
    

   public void DeleteMap() {
        for(int i = entities.Count - 1; i >= 0; --i) {
            Destroy(entities[i]);
            entities.RemoveAt(i);
        }
        Destroy(marioRefrence);
    }



    public void SetRandomCameraShader() {
        switch (Random.Range(0, 30)) {
            case 1: cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 999f); break;
            case 2: cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.NOCTURNO, 999f); break;
            case 3: case 4: case 5: cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.TAN, 999f); break;
        }
    }



    //DESTROY ALL VOICE AREA GLITCH GAME OBJECTS
    void ClearAllGlitchVoiceAreas() {
        foreach (GameObject glitch in listOfActiveGlitches)
            Destroy(glitch);
        listOfActiveGlitches.Clear();
    }

    //INIT LEVEL MUSIC
    void InitLevelMusic(E_MAP_ID _id) {
        Destroy(MusicThemePlaying);
        if (_id == E_MAP_ID.START_MAP || _id == E_MAP_ID.ALL_COINS_START)
            MusicThemePlaying = audioManager.GetAudioObject(AudioManager.E_AUDIO_ID.MUSIC_LVL1);
        if (_id == E_MAP_ID.UNDERGROUND_MAP || _id == E_MAP_ID.CLOUD_MAP || _id == E_MAP_ID.ALL_COINS_UNDERGROUND)
            MusicThemePlaying = audioManager.GetAudioObject(AudioManager.E_AUDIO_ID.MUSIC_LVL2);
    }


    //******************SPAWN GLITCHED VOICE****************
    void SpawnGlitchedVoiceHandler() {
        //RANDOM SPAWN GLITCHES
        bool shouldSpawn = (Random.Range(0, 4) == 3) ? true : false;
        Vector3 placeToSpawn = (currentMap == E_MAP_ID.START_MAP) ? new Vector3((float)(Random.Range(30, 60)), 5, 0) : new Vector3((float)(Random.Range(25, 55)), 5, 0);
        
        if (shouldSpawn) {
            //lvl normal 0
            if (currentMap == E_MAP_ID.START_MAP) { 
                listOfActiveGlitches.Add((GameObject)Instantiate(glitchedVoices[0], placeToSpawn, Quaternion.identity));
            }
            //lvl underground 1 || 2
            if (currentMap == E_MAP_ID.UNDERGROUND_MAP)
                listOfActiveGlitches.Add((GameObject)Instantiate(glitchedVoices[Random.Range(1,3)], placeToSpawn, Quaternion.identity));            
        }

        //UNDERGROUND LEVEL GLITCH
        shouldSpawn = (Random.Range(0, 3) == 0) ? true : false;
        placeToSpawn = new Vector3(65, 1, 0);
        if (currentMap == E_MAP_ID.UNDERGROUND_MAP && shouldSpawn)
            listOfActiveGlitches.Add((GameObject)Instantiate(glitchedVoices[3], placeToSpawn, Quaternion.identity));
    }


    //******************SPAWN BLUE SCREEN VOICE****************
    IEnumerator SpawnBlueScreenHandler() {
        yield return new WaitForSeconds((float)Random.Range(7,20));
        if (Random.Range(0, 6) == 1)
            Instantiate(BlueScreenPrefab);
                
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
                GameObject marioSprite = marioRefrence.GetComponent<Mario>().MarioSprite;
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

        if(marioRefrence.transform.position.y < -3 && !outOfBoundsEffectsTriggered) {
            marioRefrence.GetComponent<Mario>().SetVelocity(new Vector2(0, 0));
            marioRefrence.GetComponent<Mario>().isDeath = true;
            outOfBoundsEffectsTriggered = true;
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.MARIO_DIED);
            cameraShader.SetMaterial(CameraShader.E_CAM_MATERIAL_ID.GLITCHED, 2.7f);            
            StartCoroutine(MarioOutOfBounds());
        }
    }

    IEnumerator MarioOutOfBounds() {
        yield return new WaitForSeconds(2.7f);
        CreateMap(currentMap);

    }



    public void EnoughCoinsCollectedHnadler() {
        Debug.Log(coinsCollected);
        if (currentMap == E_MAP_ID.START_MAP && coinsCollected > 14 && princessState == E_PRINCESS_STATE.DEAD)
        {
            CreateMap(E_MAP_ID.ALL_COINS_START);
            coinsCollected = 0;
        }
        if (currentMap == E_MAP_ID.UNDERGROUND_MAP && coinsCollected > 32)
        {
            CreateMap(E_MAP_ID.ALL_COINS_UNDERGROUND);
            coinsCollected = 0;
        }

    }





}
