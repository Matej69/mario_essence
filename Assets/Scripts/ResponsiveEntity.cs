using UnityEngine;
using System.Collections;

/*
public interface ITouchedVert {
    void OnMarioTouchedVert();
}

public interface ITouchedHor {
    void OnMarioTouchedHor();
}

public interface ITouched { 
    void OnMarioTouched();
}
*/

public class ResponsiveEntity : MonoBehaviour 
{
    public MapManager.E_ENTITY_ID id;

    virtual public void OnMarioTouchedVert(ref GameObject mario) { 
    }
    virtual public void OnMarioTouchedHor(ref GameObject mario){ 
    }
    virtual public void OnMarioTouched(ref GameObject mario){ 
    }
}