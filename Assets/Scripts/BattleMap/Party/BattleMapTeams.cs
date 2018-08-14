using System.Collections;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// すべてのチーム
/// </summary>
public class BattleMapTeams
{

    private List<BattleMapTeam> teamList = new List<BattleMapTeam>(4);

    public List<BattleMapTeam> TeamList
    {
        get { return this.teamList; }
    }


    public BattleMapTeam GetByColor(BattleMapTeamColorType colorType)
    {
        //foreach (BattleMapTeam team in teamList)
        //{
        //    if (team.TeamColor == colorType)
        //    {
        //        return team;
        //    }
        //}

        BattleMapTeam team = teamList.First(t => t.TeamColor == colorType);

        return team;
    }
}
