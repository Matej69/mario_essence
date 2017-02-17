using UnityEngine;
using System.Collections;

public class Mushroom : ResponsiveEntity{

    Vector2 velocity;
    public float speed = 1;

    public Transform[] rightRayPoints;
    public Transform[] leftRayPoints;

    public LayerMask m_platform;

    // Use this for initialization
    void Start () {
        velocity = new Vector2(speed, 0);
	
	}
	
	// Update is called once per frame
	void Update () {
        
        HorizontalRaycast();
        ApplyMovement();

    } 



    

    public void ApplyMovement() { 
        transform.Translate(velocity * Time.deltaTime);
    }     

    public void HorizontalRaycast() {

        float skin = 0.015f;
        float yOffest = 0.05f;
        float velX = velocity.x * Time.deltaTime;
        int dirX = (velX > 0 || velX < 0) ? (int)Mathf.Sign(velX) : 0;
        
        Transform[] startRayPoint = (dirX == 1) ? rightRayPoints : leftRayPoints ;
        
            //detect shortest distance
            bool rayHit = false;
            for(int i = 0; i < startRayPoint.Length; ++i) {
                Transform point = startRayPoint[i];
                RaycastHit2D ray = Physics2D.Raycast(new Vector2(point.position.x + skin * dirX, point.position.y + yOffest), Vector3.right * dirX, Mathf.Abs(velX), m_platform);
                if (ray) {
                    velocity.x *= -1;
                    return;
                }  
            }
    }

     public override void OnMarioTouched(ref GameObject mario) {
        if (id == MapManager.E_ENTITY_ID.MUSHROOM_POISON) {
            mario.GetComponent<CharacterPhysics>().SetAnimatorState(CharacterPhysics.E_ANIM_STATE.DIE);
            mario.GetComponent<CharacterPhysics>().isDeath = true;
            Destroy(gameObject);
        }
    }







 }
