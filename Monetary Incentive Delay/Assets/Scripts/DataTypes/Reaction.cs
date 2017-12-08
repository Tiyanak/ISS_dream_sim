using UnityEngine.Networking;
using Assets.Scripts.DataTypes;
using UnityEngine;

public class Reaction : MessageBase {

    public Reaction() {}

    public Reaction(TaskType taskType, bool incentive, double reactionTime, double threshold) {
        this.taskType = taskType;
        this.incentive = incentive;
        this.reactionTime = reactionTime;
        this.threshold = threshold;
    }

    private TaskType taskType;
    private bool incentive;
    private double reactionTime;
    private double threshold;

    public override string ToString() {
        return "Task type: " + this.taskType.ToString() +
         " Incentive: " + this.incentive.ToString() + 
         " ReactionTime: " + this.reactionTime.ToString() + 
         " Threshold: " + this.threshold.ToString(); 
    }


}