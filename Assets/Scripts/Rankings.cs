using UnityEngine;
using UnityEngine.UI;

public class Rankings : MonoBehaviour
{
    //static int SortByScore(Tuple f1, Tuple f2)
    //{
    //    return f1.item2.CompareTo(f2.item2);
    //}

    public void DisplayRankings(Button selectedTabButton)
    {
        Text rankingDisplay = GameObject.FindGameObjectWithTag("RankingsText").GetComponent<Text>();

        string rankingText = "";
        switch (selectedTabButton.name)
        {
            case "BeginnerTabButton":
                //Options.Instance.BeginnerRankings.Sort(SortByScore);
                foreach (Tuple key in Options.Instance.BeginnerRankings)
                {
                    rankingText += key.item1 + " - " + key.item2 + "s (" + key.BoardSize + ")\n";
                }
                break;
            case "IntermediateTabButton":
                //Options.Instance.IntermediateRankings.Sort(SortByScore);
                foreach (Tuple key in Options.Instance.IntermediateRankings)
                {
                    rankingText += key.item1 + " - " + key.item2 + "s (" + key.BoardSize + ")\n";
                }
                break;
            case "ExpertTabButton":
                //Options.Instance.ExpertRankings.Sort(SortByScore);
                foreach (Tuple key in Options.Instance.ExpertRankings)
                {
                    rankingText += key.item1 + " - " + key.item2 + "s (" + key.BoardSize + ")\n";
                }
                break;
            default:
                break;
        }

        if (string.IsNullOrEmpty(rankingText))
        {
            rankingDisplay.text = "No rankings for this mode yet.";
        }
        else
        {
            rankingDisplay.text = rankingText;
        }
    }
}
