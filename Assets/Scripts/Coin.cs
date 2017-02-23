using UnityEngine;
using System.Collections;

public class Coin : ResponsiveEntity{ 
    
    public bool pickedUp = false;
    float vertSpeed = 1;
    float opacityReduceSpeed = 2;
        

    // Use this for initialization
    void Start () {
        InitRefrences();

    }
	
	// Update is called once per frame
	void Update () {
        if (pickedUp) {
            Color col = GetComponent<SpriteRenderer>().color;
            Vector2 tran = transform.position;
            col.a -= opacityReduceSpeed * Time.deltaTime;
            tran.y += vertSpeed * Time.deltaTime;
            GetComponent<SpriteRenderer>().color = col;
            transform.position = tran; 

            if (col.a <= 0)
                Destroy(gameObject);
        }	
	}
    
    public override void OnMarioTouchedTop(ref GameObject mario) {
        if (!pickedUp)
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.COIN);
        pickedUp = true;
    }
    public override void OnMarioTouchedBot(ref GameObject mario) {
        if (!pickedUp)
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.COIN);
        pickedUp = true;
    }
    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (!pickedUp)
            audioManager.CreateFreeAudioObject(AudioManager.E_AUDIO_ID.COIN);
        pickedUp = true;
    }

    

}
