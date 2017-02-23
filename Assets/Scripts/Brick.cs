using UnityEngine;
using System.Collections;

public class Brick : ResponsiveEntity {

    public GameObject DestroyedBlueBrickPrefab;
    public GameObject DestroyedBrickPrefab;
        

    // Use this for initialization
    void Start () {
        InitRefrences();	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    
    public override void OnMarioTouchedBot(ref GameObject mario) {
        mario.GetComponent<Mario>().velocity.y = 0;
        if (id == MapManager.E_ENTITY_ID.BRICK) 
            Instantiate(DestroyedBrickPrefab, transform.position, Quaternion.identity);
        if(id == MapManager.E_ENTITY_ID.UNDERGROUND_BRICK)
            Instantiate(DestroyedBlueBrickPrefab, transform.position, Quaternion.identity);
        audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.BREAK_BRICK);
        Destroy(gameObject);
    }



}
