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

  public List<Move> Moves { get; set; }

  public Gatto(GattoStats gattoStats) {
    this.gattoStats = gattoStats;
    HP = gattoStats.MaxHp;

    Moves = new List<Move>();

    var mosseCasuali = gattoStats.LearnableMoves
            .OrderBy(m => Random.value)
            .Take(3);

    foreach (var item in mosseCasuali)
    {
        Moves.Add(new Move(item.Base));
    }

  }

}
