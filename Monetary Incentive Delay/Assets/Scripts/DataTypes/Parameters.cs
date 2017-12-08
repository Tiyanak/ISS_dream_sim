//out double targetDisplayTime, out double cueToTargetTime, out double threshold
using UnityEngine.Networking;

public class Parameters : MessageBase {

    public Parameters() {}

    public Parameters(double targetDisplayTime, double cueToTargetTime, double threshold) {
        this.targetDisplayTime = targetDisplayTime;
        this.cueToTargetTime = cueToTargetTime;
        this.threshold = threshold;
    }

    private double targetDisplayTime;
    private double cueToTargetTime;
    private double threshold;

    public override string ToString() {
        return "TargetDisplayTime: " + this.targetDisplayTime.ToString() + 
        " CuoToTargetTime: " + this.cueToTargetTime.ToString() + 
        " Threshold: " + this.threshold.ToString();
    }

}