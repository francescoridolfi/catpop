using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum MoveType { REGEN, ATTACK, DEFENSE_BOOST, ATTACK_BOOST }


[CreateAssetMenu(fileName = "Move", menuName = "GATTO/ Create new Move")]
public class MoveBase : ScriptableObject
{
    [SerializeField] string _name;

    [TextArea]
    [SerializeField] string description;
    [SerializeField] int power;

    [SerializeField] MoveType moveType;
    [SerializeField] int accuracy;

    [SerializeField] Texture image;


    public Texture Image
    {
        get { return image; }
    }

    public int Power
    {
        get { return power; }
    }

    public bool IsAttack
    {
        get { return moveType == MoveType.ATTACK; }
    }

    public MoveType MoveType
    {
        get { return moveType; }
    }

    public string Description
    {
        get { return description; }
    }

    public string Name
    {
        get { return _name; }
    }

    public int Accuracy
    {
        get { return accuracy; }
    }

}
