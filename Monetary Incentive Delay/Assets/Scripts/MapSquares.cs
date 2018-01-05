using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public static class MapSquares {

    private static GameObject ClickSquare = GameObject.Find("ClickSquare");
    private static GameObject ControlSquare = GameObject.Find("ControlSquare");
    private static GameObject NonControlSquare = GameObject.Find("NonControlSquare");
    private static GameObject RewardSquare = GameObject.Find("RewardSquare");
    private static GameObject NonRewardSquare = GameObject.Find("NonRewardSquare");
    private static GameObject PunishmentSquare = GameObject.Find("PunishmentSquare");
    private static GameObject NonPunishmentSquare = GameObject.Find("NonPunishmentSquare");

    private static readonly System.Random random = new System.Random();

    private static Dictionary<int, GameObject> ALL_SQUARES = new Dictionary<int, GameObject>() {
        { 0, ClickSquare }, { 1, ControlSquare }, { 2,  NonControlSquare }, { 3, RewardSquare }, { 4, NonRewardSquare }, { 5, PunishmentSquare }, { 6, NonPunishmentSquare }
    };

    public static GameObject GetClickSquare()
    {
        return ClickSquare;
    }
    
    public static GameObject PickRandomAllSquare()
    {
        int squareIndex = random.Next(1, 7);

        return ALL_SQUARES[squareIndex];
    }

    public static GameObject PickRandomControlSquare(double controlChance)
    {
        double chance = random.NextDouble();          

        if (chance < controlChance)
        {
            return ALL_SQUARES[1];
        } else
        {
            return ALL_SQUARES[2];
        }
    }

    public static GameObject PickRandomRewardSquare(double rewardChance)
    {
        double chance = random.NextDouble();

        if (chance < rewardChance)
        {
            return ALL_SQUARES[3];
        }
        else
        {
            return ALL_SQUARES[4];
        }
    }

    public static GameObject PickRandomPunishmentSquare(double punishmentChance)
    {
        double chance = random.NextDouble();

        if (chance < punishmentChance)
        {
            return ALL_SQUARES[5];
        }
        else
        {
            return ALL_SQUARES[6];
        }
    }

}
