using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Move", menuName = "GATTO/ Create new Move")]

public class MoveBase : ScriptableObject
{
 [SerializeField] string name;

 [TextArea] 
 [SerializeField] string description;
 [SerializeField] int power;

 [SerializeField] bool isRegen;
 [SerializeField] int accuracy;

 
}
