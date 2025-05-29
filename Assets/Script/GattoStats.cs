
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Gatti", menuName = "GATTO/Create new Gatto")]

public class GattoStats : ScriptableObject
{

  [SerializeField] string _name;
  
  [SerializeField] Sprite frontSprite;

  //base Stats
  [SerializeField] int maxHp;
  [SerializeField] int attack;
  [SerializeField] int defense;


  [SerializeField] List<LearnableMove> learnableMoves;

  public Sprite FrontSprite {
    get { return frontSprite; }
  }


  public string Name {
    get { return _name; }
  }


  public int MaxHp {
    get { return maxHp; }
  }

  public int Attack
  {
    get { return attack; }
    set { attack = value; }
  }

  public int Defense
  {
    get { return defense; }
    set { defense = value; }
  }

  public List<LearnableMove> LearnableMoves {
    get { return learnableMoves; }
  }

}



[System.Serializable]

public class LearnableMove{
    [SerializeField] MoveBase moveBase;

    public MoveBase Base{
        get {return moveBase;}
    }
}
