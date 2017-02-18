using UnityEngine;
using System.Collections;

public class Gumba : ResponsiveEntity {

	// Use this for initialization
	void Start () {
        InitMasks();
        velocity = new Vector2(speed, 0);
	
	}
	
	// Update is called once per frame
	void Update () {

        ApplyAllPhysics();
	
	}


    public override void OnMarioTouchedTop(ref GameObject mario) {
        mario.GetComponent<CharacterPhysics>().velocity.y = 10;
        Destroy(gameObject);        
    }
    public override void OnMarioTouchedBot(ref GameObject mario) {
        Destroy(gameObject);        
    }
    public override void OnMarioTouchedHor(ref GameObject mario) {
        Destroy(gameObject);        
    }



}
