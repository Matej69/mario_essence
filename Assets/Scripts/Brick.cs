using UnityEngine;
using System.Collections;

public class Brick : ResponsiveEntity {

    public GameObject DestroyedBlueBrickPrefab;
    public GameObject DestroyedBrickPrefab;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
    
    public override void OnMarioTouchedBot(ref GameObject mario) {
        if (FindObjectOfType<CharacterPhysics>())
            FindObjectOfType<CharacterPhysics>().velocity.y = 0;
        if (id == MapManager.E_ENTITY_ID.BRICK)
            Instantiate(DestroyedBrickPrefab, transform.position, Quaternion.identity);
        else
            Instantiate(DestroyedBlueBrickPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }



}
