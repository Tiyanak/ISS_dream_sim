using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MapSquares {

    private GameObject ClickSquare, ControlSquare, NonControlSquare, RewardSquare, NonRewardSquare, PunishmentSquare, NonPunishmentSquare;
    private System.Random random;
    private Dictionary<int, GameObject> ALL_SQUARES;

    public MapSquares() {
        this.random = new System.Random();
        this.ClickSquare = GameObject.Find("ClickSquare");
        this.ControlSquare = GameObject.Find("ControlSquare");
        this.NonControlSquare = GameObject.Find("NonControlSquare");
        this.RewardSquare = GameObject.Find("RewardSquare");
        this.NonRewardSquare = GameObject.Find("NonRewardSquare");
        this.PunishmentSquare = GameObject.Find("PunishmentSquare");
        this.NonPunishmentSquare = GameObject.Find("NonPunishmentSquare");

        this.ALL_SQUARES = new Dictionary<int, GameObject>() {
            { 0, ClickSquare },
            { 1, ControlSquare }, { 2,  NonControlSquare },
            { 3, RewardSquare }, { 4, NonRewardSquare },
            { 5, PunishmentSquare }, { 6, NonPunishmentSquare }
        };
    }

    public GameObject GetClickSquare()
    {
        return ClickSquare;
    }
    
    public GameObject GetNonPunishmentSquare() {
        return NonPunishmentSquare;
    }

    public GameObject GetNonRewardSquare() {
        return NonRewardSquare;
    }

    public GameObject GetNonControlSquare() {
        return NonControlSquare;
    }

    public GameObject PickRandomAllSquare()
    {
        int squareIndex = random.Next(1, 7);

        return ALL_SQUARES[squareIndex];
    }

    public GameObject PickRandomControlSquare(double controlChance)
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

    public GameObject PickRandomRewardSquare(double rewardChance)
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

    public GameObject PickRandomPunishmentSquare(double punishmentChance)
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
