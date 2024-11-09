using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TipsManager : MonoBehaviour
{
    public TipTypeWriter typewriter;
    public string[] tips;
    public float tipChangeInterval = 5.0f;

    private List<string> shuffledTips;
    private string lastTipDisplayed;

    private void Start()
    {
        if (tips.Length > 0)
        {
            shuffledTips = new List<string>(tips);
            lastTipDisplayed = null;
            StartCoroutine(ShowTipsInRandomOrder());
        }
    }

    IEnumerator ShowTipsInRandomOrder()
    {
        while (true)
        {
            ShuffleTips(shuffledTips);
            while (lastTipDisplayed != null && shuffledTips[0] == lastTipDisplayed)
            {
                ShuffleTips(shuffledTips);
            }

            foreach (string tip in shuffledTips)
            {
                typewriter.DisplayText(tip);
                yield return new WaitForSeconds(tipChangeInterval);
                lastTipDisplayed = tip;
            }
        }
    }

    void ShuffleTips(List<string> tipsList)
    {
        for (int i = 0; i < tipsList.Count; i++)
        {
            int swapIndex = Random.Range(i, tipsList.Count);
            string temp = tipsList[i];
            tipsList[i] = tipsList[swapIndex];
            tipsList[swapIndex] = temp;
        }
    }
}
