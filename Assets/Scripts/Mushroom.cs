using UnityEngine;
using System.Collections;

public class Mushroom : ResponsiveEntity {
    /*
    Vector2 velocity;
    public float speed = 1;

    public Transform[] rightRayPoints;
    public Transform[] leftRayPoints;

    public LayerMask m_platform;
    */
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
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            mario.GetComponent<CharacterPhysics>().SetAnimatorState(CharacterPhysics.E_ANIM_STATE.DIE);
            mario.GetComponent<CharacterPhysics>().isDeath = true;
            mario.GetComponent<CharacterPhysics>().SetVelocity(new Vector2(0, 0));
            Destroy(gameObject);
        }
    }

    public override void OnMarioTouchedBot(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            mario.GetComponent<CharacterPhysics>().SetAnimatorState(CharacterPhysics.E_ANIM_STATE.DIE);
            mario.GetComponent<CharacterPhysics>().isDeath = true;
            mario.GetComponent<CharacterPhysics>().SetVelocity(new Vector2(0, 0));
            Destroy(gameObject);
        }
    }

    public override void OnMarioTouchedHor(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            mario.GetComponent<CharacterPhysics>().SetAnimatorState(CharacterPhysics.E_ANIM_STATE.DIE);
            mario.GetComponent<CharacterPhysics>().isDeath = true;
            mario.GetComponent<CharacterPhysics>().SetVelocity(new Vector2(0, 0));
            Destroy(gameObject);
        }
    }







 }
