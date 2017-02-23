using UnityEngine;
using System.Collections;

public class EssenceJumpscare : MonoBehaviour {

    float scaleFactor = 1.05f;

	// Use this for initialization
	void Start () {
        StartCoroutine(DestroyAfter(2.5f));

    }
	
	// Update is called once per frame
	void Update () {
        ScaleUp();       
        
    }



    void ScaleUp() {
        transform.localScale *= scaleFactor;
    }

    IEnumerator DestroyAfter(float _sec) {
        yield return new WaitForSeconds(_sec);
        FindObjectOfType<MapManager>().GetComponent<MapManager>().CreateMap(MapManager.E_MAP_ID.START_MAP);
        Destroy(gameObject);
    }


}
