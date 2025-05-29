using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Gatto : MonoBehaviour
{
    
  public GattoStats gattoStats { get; set; }

  public int HP { get; set; }

  public int MaxHp {
    get { return this.gattoStats.MaxHp; }
  }

  public int Attack
  {
    get { return this.gattoStats.Attack; }
    set { this.gattoStats.Attack = value; }
  }

  public int Defense
  {
    get { return this.gattoStats.Defense; }
    set { this.gattoStats.Defense = value; }
  }

  public List<Move> Moves { get; set; }

  public Gatto(GattoStats gattoStats) {
    this.gattoStats = gattoStats;
    HP = gattoStats.MaxHp;

    Moves = new List<Move>();

    var mosseCasuali = gattoStats.LearnableMoves
            .Take(4);

    foreach (var item in mosseCasuali)
    {
        Moves.Add(new Move(item.Base));
    }

  }

    public string Name {
        get { return gattoStats.Name; }
    }


    public bool TakeDamage(Move move, Gatto attacker) {
      if (!move.Base.IsAttack)
      {
        return false;
      }

      float modifiers = Random.Range(0.85f, 1f);
      float a = (2 * move.Base.Accuracy + 10 ) / 250f;
      float d = a * move.Base.Power * ((float)attacker.gattoStats.Attack / gattoStats.Defense) + 2;
      int damage = Mathf.FloorToInt(d * modifiers);

      HP -= damage;
      if (HP < 0)
        HP = 0;

      return HP == 0;
    }

    public Move GetRandomMove() {
      return Moves[Random.Range(0, Moves.Count)];
    }
}
