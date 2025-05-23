using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName = "GATTO/ Create new Move")]

public class MoveBase : ScriptableObject
{
 [SerializeField] string _name;

 [TextArea] 
 [SerializeField] string description;
 [SerializeField] int power;

 [SerializeField] bool isRegen;
 [SerializeField] int accuracy;

 
    public int Power {
        get { return power; }
    }

    public bool IsRegen {
        get { return isRegen; }
    }

    public string Description {
        get { return description; }
    }

    public string Name {
        get { return _name; }
    }

    public int Accuracy {
        get { return accuracy; }
    }

}
