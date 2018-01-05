using UnityEngine;
using UnityEngine.UI;

public class RewardFlashing : MonoBehaviour {

    private System.Random random = new System.Random();

    public GameObject panelBaseline;
    private GameObject clickSquare;
    private GameObject showSquare;

    private double reactionTime = 0;

    public int totalTrials = 30;
    public double waitUntilClickSquareTime = 4000;
    public double showSquareShowTime = 500;
    public double clickSquareShowTime = 200;
    public double feedbackTime = 500;
    public double currentTrialTime = 0;

    public int waitUntilClickSquareTimeMin = 4000;
    public int waitUntilClickSquareTimeMax = 4500;

    private bool isFlashing = false;
    private bool successfullClick = false;

    private int rewardCount = 20;
    private int nonRewardCount = 80;

    // Use this for initialization
    void Start()
    {
        clickSquare = MapSquares.GetClickSquare();
    }

    // Update is called once per frame
    void Update()
    {
        Flash();
    }

    public void Flash()
    {
        if (!isFlashing)
        {
            showSquare = MapSquares.PickRandomRewardSquare((double)rewardCount / nonRewardCount);
            waitUntilClickSquareTime = random.Next(waitUntilClickSquareTimeMin, waitUntilClickSquareTimeMax);
            isFlashing = true;
            successfullClick = false;
        }
        else
        {

            if (currentTrialTime < showSquareShowTime)
            {
                ShowGameObject(showSquare);
                currentTrialTime += Time.deltaTime * 1000;
            }
            else if (showSquareShowTime < currentTrialTime && currentTrialTime < waitUntilClickSquareTime + showSquareShowTime)
            {
                HideGameObject(showSquare);
                currentTrialTime += Time.deltaTime * 1000;
            }
            else if (waitUntilClickSquareTime + showSquareShowTime < currentTrialTime && currentTrialTime < waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime)
            {
                ShowGameObject(clickSquare);
                currentTrialTime += Time.deltaTime * 1000;

                if (Input.GetKeyDown("space"))
                {
                    reactionTime = currentTrialTime - (waitUntilClickSquareTime + showSquareShowTime);
                    successfullClick = true;
                }

            }
            else if (waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime < currentTrialTime && currentTrialTime < waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime + feedbackTime)
            {
                SendFeedback(reactionTime, successfullClick);
                currentTrialTime += Time.deltaTime * 1000;
            }
            else
            {
                HideGameObject(clickSquare);
                currentTrialTime = 0;
                showSquare = null;
                isFlashing = false;
            }
        }
        
    }

    public void SendFeedback(double reactionTime, bool successfullClick)
    {

    }

    public void ShowGameObject(GameObject gameObject)
    {
        gameObject.GetComponent<RawImage>().enabled = true;
    }

    public void HideGameObject(GameObject gameObject)
    {
        gameObject.GetComponent<RawImage>().enabled = false;
    }
}
