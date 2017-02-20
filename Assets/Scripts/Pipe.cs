using UnityEngine;
using System.Collections;

public class Pipe : ResponsiveEntity {

    GameObject MarioSprite;

    MapManager mapManager;

    bool goingThroughPipe = false;
    Timer marioInsidePipeTimer;

	// Use this for initialization
	void Start () {
        mapManager = FindObjectOfType<MapManager>();
        marioInsidePipeTimer = new Timer(1.9f);
    }
	
	// Update is called once per frame
	void Update () {

        if (MarioSprite)
            MarioEnteringPipeHandler();
        else if(FindObjectOfType<CharacterPhysics>())
                MarioSprite = FindObjectOfType<CharacterPhysics>().gameObject.transform.FindChild("MarioSprite").gameObject;
        
    }



    public override void OnMarioTouchedTop(ref GameObject mario) {
        Debug.Log("PIPE");
    }
    public override void OnMarioTouchedHor(ref GameObject mario) {
        Debug.Log("PIPE");
    }

    public void MarioEnteringPipeHandler() {
        //get mario gameObject
        CharacterPhysics mario = MarioSprite.transform.parent.GetComponent<CharacterPhysics>();

        if (goingThroughPipe) {
            mario.transform.position = new Vector2(mario.transform.position.x, mario.transform.position.y - 1f * Time.deltaTime);

            marioInsidePipeTimer.Tick(Time.deltaTime);
            if (marioInsidePipeTimer.IsFinished()) {
                mapManager.CreateMap(MapManager.E_MAP_ID.UNDERGROUND_MAP);
                return;
            }
        }

        
        //get mario info
        Vector2 marioSize = MarioSprite.GetComponent<SpriteRenderer>().bounds.size;
        Vector2 marioPos = MarioSprite.transform.position;
        Vector2 marioFeetPoint = new Vector2(marioPos.x, marioPos.y - marioSize.y / 2);
        //get pipe info
        Vector2 collSize = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x / 3, 2.5f);
        Vector2 collPos = new Vector2(transform.position.x, transform.position.y + GetComponent<SpriteRenderer>().bounds.size.y/2);
        //if it's inside and is down pressed
        if (marioFeetPoint.x > collPos.x - collSize.x / 2 && marioFeetPoint.x < collPos.x + collSize.x / 2 &&
            marioFeetPoint.y > collPos.y - collSize.y / 2 && marioFeetPoint.y < collPos.y + collSize.y / 2 &&
            (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) && mario.grounded)
        {
            MarioSprite.GetComponent<SpriteRenderer>().sortingLayerName = "BackgroundEntities";
            MarioSprite.transform.parent.GetComponent<CharacterPhysics>().canBeControled = false;
            goingThroughPipe = true;
        } 
    }
    



}
