package hr.fer.iss.model;

import java.io.Serializable;

/**
 * Created by Igor Farszky on 7.12.2017..
 */
public class PlayMsg implements Serializable {

    private String type;
    private Boolean incentive;
    private Double reactionTime;
    private Double threshHold;

    public PlayMsg() {
    }

    public PlayMsg(String type, Boolean incentive, Double reactionTime, Double threshHold) {
        this.type = type;
        this.incentive = incentive;
        this.reactionTime = reactionTime;
        this.threshHold = threshHold;
    }

    public String getType() {
        return type;
    }

    public void setType(String type) {
        this.type = type;
    }

    public Boolean getIncentive() {
        return incentive;
    }

    public void setIncentive(Boolean incentive) {
        this.incentive = incentive;
    }

    public Double getReactionTime() {
        return reactionTime;
    }

    public void setReactionTime(Double reactionTime) {
        this.reactionTime = reactionTime;
    }

    public Double getThreshHold() {
        return threshHold;
    }

    public void setThreshHold(Double threshHold) {
        this.threshHold = threshHold;
    }

    @Override
    public String toString() {
        return "PlayMsg{" +
                "type='" + type + '\'' +
                ", incentive=" + incentive +
                ", reactionTime=" + reactionTime +
                ", threshHold=" + threshHold +
                '}';
    }
}
