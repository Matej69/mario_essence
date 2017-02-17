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
        SIZE
    }

    public List<TextureIDPair> mapTextures = new List<TextureIDPair>();
    public List<ColorGameObjectPair> colorObjectPair = new List<ColorGameObjectPair>();
    
    public Transform mapSpawnPos;

    const float tileSize = 1;
    



    // Use this for initialization
    void Start () {

        SpawnEntities(E_MAP_ID.START_MAP);

    }
	
	// Update is called once per frame
	void Update () {
        	
	}




    //************SET************
    void SpawnEntities(E_MAP_ID _id) {
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
                if (prefab != null)                
                    Instantiate(prefab, pos, Quaternion.identity);                
            }
        }
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
    

    //************HANDLERS************


}
