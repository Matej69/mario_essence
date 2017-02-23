using UnityEngine;
using System.Collections;

public class FocusPlayer : MonoBehaviour {

    Vector2 camPos;


    Mario Mario;

	// Use this for initialization
	void Start () {
        Mario = FindObjectOfType<Mario>();

    }
	
	// Update is called once per frame
	void Update () {
        camPos = transform.position;

        if (!Mario && FindObjectOfType<Mario>())
            Mario = FindObjectOfType<Mario>();
        if (!Mario)
            return;

        LerpTo(Mario.transform.position);
        transform.position = new Vector3(camPos.x, camPos.y, transform.position.z);
    }



    public void LerpTo(Vector2 _pos) {
        float speed = 1.2f;
        camPos.x = Mathf.Lerp(camPos.x, _pos.x + Mario.velocity.x, Time.deltaTime * speed);
    }

    


}
