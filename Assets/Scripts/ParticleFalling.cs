using UnityEngine;
using System.Collections;

public class ParticleFalling : MonoBehaviour {

	
    Vector3 rotVelocity;
    Vector2 velocity;
    float grav = 0.45f;

    float timePassed;

    // Use this for initialization
    void Start()
    {
        velocity = new Vector2((float)Random.Range(4, 30) / 10, (float)Random.Range(10, 100) / 10);
        rotVelocity = new Vector3(0, 0, (float)Random.Range(2000, 5000) / 10);
        ChangeDirectionX((Random.Range(0, 2) == 0) ? 1 : -1);
        ChangeRotationZ((Random.Range(0, 2) == 0) ? 1 : -1);

        timePassed = Time.time;
    }

    // Update is called once per frame
    void Update() {

        timePassed += Time.deltaTime;
        if (Time.time - timePassed > 4)
            Destroy(gameObject);

        ApplyRotation();

        ApplyGravity();
        ApplyMovement();
    }


    //*********************MOVEMENT**************************
    void ApplyGravity()
    {
        velocity = new Vector2(velocity.x, velocity.y - grav);
    }
    void ApplyMovement()
    {
        Vector2 pos = transform.position;
        transform.Translate(velocity * Time.deltaTime, Space.World);
    }
    void ChangeDirectionX(int _dir)
    {
        velocity.x = velocity.x * _dir;
    }

    //*********************MOVEMENT**************************
    void ApplyRotation()
    {
        Vector3 rot = transform.rotation.eulerAngles;
        transform.Rotate(rotVelocity * Time.deltaTime);
    }
    void ChangeRotationZ(int _dir)
    {
        rotVelocity.z = rotVelocity.z * _dir;
    }




}
