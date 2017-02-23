using UnityEngine;
using System.Collections;



public class ResponsiveEntity : MonoBehaviour 
{

    public enum E_MARIO_TOUCHED {
        TOP,
        BOT,
        HOR,
        JUST_TOUCHED,
        SIZE
    }

    public MapManager.E_ENTITY_ID id;

    public Vector2 velocity;
    public float speed = 1;
    public float gravity = 0.2f;

    public Transform[] rightRayPoints;
    public Transform[] leftRayPoints;
    public Transform[] topRayPoints;
    public Transform[] botRayPoints;

    LayerMask m_platform;
    LayerMask m_mario;

    public bool grounded = false;

    [HideInInspector]
    public AudioManager audioManager;
    public MapManager mapManager;

    virtual public void OnMarioTouchedTop(ref GameObject mario) { 
    }
    virtual public void OnMarioTouchedBot(ref GameObject mario) { 
    }
    virtual public void OnMarioTouchedHor(ref GameObject mario) { 
    }

    public void OnMarioTouched(E_MARIO_TOUCHED _side, ref GameObject mario) {
        switch (_side) {
            case E_MARIO_TOUCHED.TOP: OnMarioTouchedTop(ref mario); break;
            case E_MARIO_TOUCHED.BOT: OnMarioTouchedBot(ref mario); break;
            case E_MARIO_TOUCHED.HOR: OnMarioTouchedHor(ref mario); break;
        }
    }
    
    void OnDestroy() {
        GameObject go = gameObject;
        if(FindObjectOfType<MapManager>())
            FindObjectOfType<MapManager>().RemoveFromList(ref go);
    }



    public void InitMasks() {
        m_platform = LayerMask.GetMask("Platform");
        m_mario = LayerMask.GetMask("Mario");
    }
    public void InitRefrences() {
        audioManager = FindObjectOfType<AudioManager>();
        mapManager = FindObjectOfType<MapManager>();
    }

      public void ApplyMovement() { 
        transform.Translate(velocity * Time.deltaTime);
    }     

    public void MoveBy(Vector2 _deltaPos) {
        Vector2 pos = new Vector2(transform.position.x + _deltaPos.x, transform.position.y + _deltaPos.y);
        transform.position = pos;
    }

    public void ApplyGravity() {
        velocity.y -= gravity;
    }

    //*****************RAYCASTING**********************

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

    }



    //*****************APPLY ALL PHYSICS MOVEMENT AND COLLISION**********************
    public void ApplyAllPhysics() {
        ApplyGravity();
        HorizontalRaycast();
        VerticalRaycast();
        ApplyMovement();
    }







}

