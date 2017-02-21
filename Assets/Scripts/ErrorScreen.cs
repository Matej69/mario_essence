using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;

public class ErrorScreen : MonoBehaviour {

    public Text message;
    public AudioSource audioSource;

    public enum E_MESSAGE_ID {
        CONFUSED,
        HELP_ME,
        IT_LEARNS,
        SYSTEM_TAKEN,
        WEIRD_ERROR,
        ASSEMBLY,
        SIZE
    }


	// Use this for initialization
	void Start () {
        audioSource.pitch = (float)Random.Range(130, 250) / 1000;
        SetRandomText();
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKey(KeyCode.Space)) {
            Destroy(gameObject);
        }

    }

    public void SetRandomText() {
        E_MESSAGE_ID rand = (E_MESSAGE_ID)( Random.Range(0, (int)E_MESSAGE_ID.SIZE) );
        string fileName = null;

        switch (rand) {
            case E_MESSAGE_ID.CONFUSED: fileName = "CONFUSED"; break;
            case E_MESSAGE_ID.HELP_ME: fileName = "HELP_ME"; break;
            case E_MESSAGE_ID.IT_LEARNS: fileName = "IT_LEARNS"; break;
            case E_MESSAGE_ID.SYSTEM_TAKEN: fileName = "SYSTEM_TAKEN"; break;
            case E_MESSAGE_ID.WEIRD_ERROR: fileName = "WEIRD_ERROR"; break;
            case E_MESSAGE_ID.ASSEMBLY: fileName = "ASSEMBLY"; break;
        }        
        string fileText = (Resources.Load(fileName) as TextAsset).text;
        message.text = fileText;
    }



}
