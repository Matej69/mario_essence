using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class ClipWithID {
    public AudioManager.E_AUDIO_ID id;
    public AudioClip audioClip;    
}

public class AudioManager : MonoBehaviour {

    public enum E_AUDIO_ID {
        MUSIC_LVL1,
        MUSIC_LVL2,
        GAMEOVER,
        JUMP_SMALL,
        JUMP_BIG,
        BREAK_BRICK,
        MARIO_DIED,
        MUSHROOM_GREEN,
        MUSHROOM_RED,
        GUMBA_TOUCHED,
        COIN,
        PIPE,
        SCREAM,
        SPLASH,
        SIX_ES,
        FINAL_BOSS,
        SIZE
    }

    public List<ClipWithID> allAudioClips;


    public GameObject GetAudioObject(E_AUDIO_ID _id) {
        AudioClip clip = GetAudioClip(_id);
        GameObject audioObject = new GameObject("AUDIO: "+_id);
        audioObject.AddComponent<AudioSource>().clip = clip;
        SetAudioOptions(ref audioObject, _id);
        audioObject.active = false;
        audioObject.active = true;
        return audioObject;
    }



    public void CreateFreeAudioObject(E_AUDIO_ID _id) {
        AudioClip clip = GetAudioClip(_id);
        GameObject audioObject = new GameObject("AUDIO : "+_id);
        audioObject.AddComponent<AudioSource>().clip = clip;
        SetAudioOptions(ref audioObject, _id);
        audioObject.active = false;
        audioObject.active = true;
        StartCoroutine(DestroyAfter(audioObject, 4f));
                
    }
    IEnumerator DestroyAfter(GameObject _go, float _sec) {
        yield return new WaitForSeconds(_sec);
        Destroy(_go);
    }



    public AudioClip GetAudioClip(E_AUDIO_ID _id)
    {
        foreach (ClipWithID clip in allAudioClips)
            if (clip.id == _id)
                return clip.audioClip;
        Debug.LogError("CLIP WITH ID = " + _id + " WAS NOT FOUND");
        return null;
    }

    public void SetAudioOptions(ref GameObject _sourceObj, E_AUDIO_ID _audioID)
    {
        AudioSource source = _sourceObj.GetComponent<AudioSource>();
        switch (_audioID)
        {
            case E_AUDIO_ID.BREAK_BRICK:    { source.volume = 0.75f;    break; }
            case E_AUDIO_ID.COIN:           { source.volume = 0.02f;    break; }
            case E_AUDIO_ID.GAMEOVER:       { source.volume = 0.5f;     break; }
            case E_AUDIO_ID.GUMBA_TOUCHED:  { source.volume = 0.4f;     break; }
            case E_AUDIO_ID.JUMP_BIG:       { source.volume = 1;        break; }
            case E_AUDIO_ID.JUMP_SMALL:     { source.volume = 0.2f;     break; }
            case E_AUDIO_ID.MARIO_DIED:     { source.volume = 0.25f;    break; }
            case E_AUDIO_ID.MUSHROOM_GREEN: { source.volume = 0.05f;    break; }
            case E_AUDIO_ID.MUSHROOM_RED:   { source.volume = 0.035f;   break; }
            case E_AUDIO_ID.MUSIC_LVL1:     { source.volume = 0.1f;     break; }
            case E_AUDIO_ID.MUSIC_LVL2:     { source.volume = 0.1f;     break; }
            case E_AUDIO_ID.PIPE:           { source.volume = 0.5f;     break; }
            case E_AUDIO_ID.SCREAM:         { source.volume = 0.2f;     break; }
            case E_AUDIO_ID.SPLASH:         { source.volume = 0.75f;    break; }
            case E_AUDIO_ID.FINAL_BOSS:     { source.volume = 1f;       break; }
        }
        if(_audioID == E_AUDIO_ID.SCREAM)
            source.pitch = 1f;
        else
            source.pitch = ((float)Random.Range(780f, 1050f) / 1000);
    }
        

}
