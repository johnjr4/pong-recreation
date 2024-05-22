using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // Quick and dirty way of saving variables between scenes
    public static int winScore = 3;

    public void setWinScore(int score)
    {
        winScore = score;
    }
}
