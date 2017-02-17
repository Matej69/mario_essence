using UnityEngine;
using System.Collections;

public class Coin : ResponsiveEntity{ 
    
    bool pickedUp = false;
    float vertSpeed = 1;
    float opacityReduceSpeed = 2;
    

    // Use this for initialization
    void Start () {
	
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
    
    public override void OnMarioTouched(ref GameObject mario) {
        pickedUp = true;
    }
    

}
