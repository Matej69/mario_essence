﻿using UnityEngine;
using System.Collections;

public class CharacterPhysics : MonoBehaviour {

    enum E_MOVE_STATE {
        LEFT,
        STILL,
        RIGHT,
        SIZE
    }

    bool grounded = false;
    
    public enum E_ANIM_STATE {
        IDLE,
        WALK,
        RUN,
        DIE
    }

    [HideInInspector]
    public Vector2 velocity = new Vector2(0, 0);

    public Vector2 horTargetVelocity = new Vector2(0, 0);
    float reachTargetHorVel = 12f;
    public float walkSpeed = 6f;
    public float runSpeed = 12f;
    public float gravity = 0.5f;
    public float jumpAmount = 10;

    public KeyCode key_left, key_right, key_jump;
    E_MOVE_STATE movementState = E_MOVE_STATE.STILL;
    
    public Transform[] botRayPoints;
    public Transform[] rightRayPoints;
    public Transform[] leftRayPoints;
    public Transform[] topRayPoints;

    public LayerMask m_platform;
    public LayerMask m_entity;

    public Animator animator;
    E_ANIM_STATE animState = E_ANIM_STATE.IDLE;

    public GameObject MarioSprite;

    public bool isDeath = false;

    // Use this for initialization
    void Start () {
                	
	}
	
	// Update is called once per frame
	void Update () {

        //applying some values to velocity
        if(!isDeath)
            ChangeStateOnInput();
        ApplyHorizontalVelocity();
        ApplyGravity();
        //checking should playr move by those values using restrictions such as collision        
        VerticalRaycast();
        HorizontalRaycast();
        //applying final velocity to player position
        if (!isDeath)
            ApplyMovement();

        HandleSpriteRotation();

        ApplyAnimation();
    }


    //***************GET*************

    //***************SET*************
    public void SetPos(Vector2 _pos) {
        transform.position = _pos;
    }
    public void SetVelocity(Vector2 _vel) {
        velocity = _vel;
    }

    public void MoveBy(Vector2 _deltaPos) {
        Vector2 pos = new Vector2(transform.position.x + _deltaPos.x, transform.position.y + _deltaPos.y);
        transform.position = pos;
    }

    public void SetAnimatorState(E_ANIM_STATE _state) {        
        if(_state == E_ANIM_STATE.RUN) { 
            animator.SetBool("run", true);
            animator.SetBool("walk", false);
        }
        else if (_state == E_ANIM_STATE.WALK) { 
            animator.SetBool("walk", true);
            animator.SetBool("run", false);
        }
        else if (_state == E_ANIM_STATE.IDLE) {
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
        }
        else if(_state == E_ANIM_STATE.DIE) {
            animator.SetBool("run", false);
            animator.SetBool("walk", false);
            animator.SetBool("death", true);
        }


    }

    //***************HANDLERS*************
    public void ApplyGravity() {
        velocity.y -= gravity;
    }

    public void ApplyMovement() {
        transform.Translate(velocity * Time.deltaTime);
    }    

    public void ChangeStateOnInput() {
        //MOVEMENT
        if (Input.GetKey(key_left))
            movementState = E_MOVE_STATE.LEFT;
        else if (Input.GetKey(key_right))
            movementState = E_MOVE_STATE.RIGHT;
        else
            movementState = E_MOVE_STATE.STILL;
        //JUMP            
        if (Input.GetKey(key_jump) && grounded) {
            velocity.y = jumpAmount;
            grounded = false;
        }        
        
    }

    public void ApplyHorizontalVelocity() {
        float speed = ((animState == E_ANIM_STATE.RUN) ? runSpeed : ((animState == E_ANIM_STATE.WALK) ? walkSpeed : 0));
        switch (movementState) {
            case E_MOVE_STATE.LEFT: horTargetVelocity.x = -speed; break;
            case E_MOVE_STATE.RIGHT: horTargetVelocity.x = speed; break;
            case E_MOVE_STATE.STILL: horTargetVelocity.x = 0; break;
        }
        if (velocity.x != horTargetVelocity.x)
            velocity.x = Mathf.Lerp(velocity.x, horTargetVelocity.x, Time.deltaTime * reachTargetHorVel);
        if (movementState == E_MOVE_STATE.STILL) { 
            horTargetVelocity.x = 0;
            velocity.x = 0;
        }
        
    }
    

    public void VerticalRaycast() {
        
        float skin = 0.0015f;
        float velY = velocity.y * Time.deltaTime;
        int dir = (velY > 0 || velY < 0) ? (int)Mathf.Sign(velY) : 0;

        if (dir == 0)
            return;

        Transform[] startRayPoint = (dir == 1) ? topRayPoints : botRayPoints;

        //float distance = Mathf.Abs(velocity.y * Time.deltaTime) + skin;        

        float shortestGroundDist = 777777777f;
        //detect shortest distance
        bool rayHit = false;
        for(int i = 0; i < startRayPoint.Length; ++i) {
            Transform point = startRayPoint[i];
            RaycastHit2D ray = Physics2D.Raycast(new Vector2(point.position.x, point.position.y - skin * dir), Vector3.up * dir, Mathf.Abs(velY), m_platform);
            if (ray) {
                rayHit = true;
                if (ray.distance + skin * dir < shortestGroundDist){
                    if(dir == -1)
                        shortestGroundDist = ray.distance + skin * dir;
                    //no skin for hitting celling so it doesn't stuck for second ==> NO IDEA WHY
                    if(dir == 1)
                        shortestGroundDist = ray.distance - 0.1f;
                }
            }
        }

        if (rayHit) {
            velocity.y = 0;
            MoveBy(new Vector2(0, shortestGroundDist * dir));
            if (dir == -1)
                grounded = true;
        }
        else {
            if (dir == -1)
                grounded = false;
        }

        //OTHER ENTITIES COLLISION
        for(int i = 0; i < startRayPoint.Length; ++i) {
            Transform point = startRayPoint[i];
            RaycastHit2D ray = Physics2D.Raycast(new Vector2(point.position.x, point.position.y - skin * dir), Vector3.up * dir, Mathf.Abs(velY), m_entity);
            if (ray) {
                ResponsiveEntity entityScript = ray.collider.gameObject.GetComponent<ResponsiveEntity>();
                if (entityScript) {
                    GameObject go = gameObject;
                    entityScript.OnMarioTouched(ref go);
                    entityScript.OnMarioTouchedVert(ref go);
                }
            }
        }




    }





    public void HorizontalRaycast() {

        float skin = 0.015f;
        float yOffest = 0.05f;
        float velX = velocity.x * Time.deltaTime;
        int dirX = (velX > 0 || velX < 0) ? (int)Mathf.Sign(velX) : 0;
        
        if (dirX == 0) {
            return;
       }

        Transform[] startRayPoint = (dirX == 1) ? rightRayPoints : leftRayPoints ;

        if(movementState == E_MOVE_STATE.RIGHT || movementState == E_MOVE_STATE.LEFT) { 
            //detect shortest distance
            bool rayHit = false;
            for(int i = 0; i < startRayPoint.Length; ++i) {
                Transform point = startRayPoint[i];
                RaycastHit2D ray = Physics2D.Raycast(new Vector2(point.position.x + skin * dirX, point.position.y + yOffest), Vector3.right * dirX, Mathf.Abs(velX), m_platform);
                if (ray) {
                    velocity.x = 0;
                    horTargetVelocity.x = 0;
                    movementState = E_MOVE_STATE.STILL;
                }  
            }
        }


        //OTHER ENTITIES COLLISION
        for(int i = 0; i < startRayPoint.Length; ++i) {
            Transform point = startRayPoint[i];
            RaycastHit2D ray = Physics2D.Raycast(new Vector2(point.position.x, point.position.y - skin * dirX), Vector3.up * dirX, Mathf.Abs(velX), m_entity);
            if (ray) {
                ResponsiveEntity entityScript = ray.collider.gameObject.GetComponent<ResponsiveEntity>();
                if (entityScript) {
                    GameObject go = gameObject;
                    entityScript.OnMarioTouched(ref go);
                    entityScript.OnMarioTouchedHor(ref go);
                }
            }
        }




    }
    
    
       
    



    //********************ANIMATION**************************
    public void ApplyAnimation() {
        if (!Input.GetKey(KeyCode.LeftShift) && movementState != E_MOVE_STATE.STILL) {
            animState = E_ANIM_STATE.WALK;
            SetAnimatorState(E_ANIM_STATE.WALK);
            }
        if (Input.GetKey(KeyCode.LeftShift) && movementState != E_MOVE_STATE.STILL) {
            animState = E_ANIM_STATE.RUN;
            SetAnimatorState(E_ANIM_STATE.RUN);
        }            
        if(movementState == E_MOVE_STATE.STILL) {
            animState = E_ANIM_STATE.IDLE;
            SetAnimatorState(E_ANIM_STATE.IDLE);
        }
    }

    public void HandleSpriteRotation() {
        float scaleX = MarioSprite.transform.localScale.x;
        if (movementState == E_MOVE_STATE.RIGHT)
            scaleX = Mathf.Abs(MarioSprite.transform.localScale.x);
        if (movementState == E_MOVE_STATE.LEFT)
            scaleX = -Mathf.Abs(MarioSprite.transform.localScale.x);

        MarioSprite.transform.localScale = new Vector2(scaleX, MarioSprite.transform.localScale.y);
    }


    


}