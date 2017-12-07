package hr.fer.iss.model;

import java.io.Serializable;

/**
 * Created by Igor Farszky on 7.12.2017..
 */
public class WaitMsg implements Serializable {

    private Double squareShowTime;
    private Double threshold;
    private Double waitTime;

    public WaitMsg() {
    }

    public WaitMsg(Double squareShowTime, Double threshold, Double waitTime) {
        this.squareShowTime = squareShowTime;
        this.threshold = threshold;
        this.waitTime = waitTime;
    }

    public Double getSquareShowTime() {
        return squareShowTime;
    }

    public void setSquareShowTime(Double squareShowTime) {
        this.squareShowTime = squareShowTime;
    }

    public Double getThreshold() {
        return threshold;
    }

    public void setThreshold(Double threshold) {
        this.threshold = threshold;
    }

    public Double getWaitTime() {
        return waitTime;
    }

    public void setWaitTime(Double waitTime) {
        this.waitTime = waitTime;
    }

    @Override
    public String toString() {
        return "WaitMsg{" +
                "squareShowTime=" + squareShowTime +
                ", threshold=" + threshold +
                ", waitTime=" + waitTime +
                '}';
    }
}
