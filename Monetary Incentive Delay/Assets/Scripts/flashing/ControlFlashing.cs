using UnityEngine;
using UnityEngine.UI;

public class ControlFlashing : MonoBehaviour {

    private System.Random random = new System.Random();
    private MapSquares mapSquares;

    public GameObject panelBaseline;
    private GameObject clickSquare;
    private GameObject showSquare;
    public Text scoreText;

    private double reactionTime = 0;
    public double waitUntilClickSquareTime = 4000;
    public double showSquareShowTime = 500;
    public double clickSquareShowTime = 200;
    public double feedbackTime = 500;
    public double currentTrialTime = 0;

    public int waitUntilClickSquareTimeMin = 4000;
    public int waitUntilClickSquareTimeMax = 4500;

    private bool isFlashing = false;
    private bool successfullClick = false;

    public int controlTrial = 20;
    public int nonControlTrial = 80;
    public int totalTrials = 100; // control + nonControl
    private int currentTrialNumber = 1;
    private int playerScore = 20;
    private string lastSquare;

    // Use this for initialization
    void Start()
    {
        mapSquares = new MapSquares();
        scoreText.enabled = true;
        clickSquare = mapSquares.GetClickSquare();
    }

    // Update is called once per frame
    void Update()
    {
        if (currentTrialNumber <= totalTrials)
        {
            Flash();
        }
    }

    public void Flash()
    {
        if (!isFlashing)
        {
            
            if (lastSquare.Equals("ControlSquare"))
            {
                showSquare = mapSquares.GetNonControlSquare();
                lastSquare = showSquare.name;
                controlTrial -= 1;
            }
            else
            {
                showSquare = mapSquares.PickRandomControlSquare((double)controlTrial / nonControlTrial);
                lastSquare = showSquare.name;
                if (lastSquare.Equals("ControlSquare"))
                {
                    controlTrial -= 1;
                }
                else
                {
                    nonControlTrial -= 1;
                }
            }

            showSquare = mapSquares.PickRandomControlSquare((double) controlTrial /nonControlTrial);
            waitUntilClickSquareTime = random.Next(waitUntilClickSquareTimeMin, waitUntilClickSquareTimeMax);
            isFlashing = true;
            successfullClick = false;
            scoreText.text = playerScore.ToString() + "/" + currentTrialNumber.ToString();
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

                if (Input.GetKeyDown("space") && !successfullClick)
                {
                    reactionTime = currentTrialTime - (waitUntilClickSquareTime + showSquareShowTime);
                    successfullClick = true;
                    playerScore += 1;
                    scoreText.text = playerScore.ToString() + "/" + currentTrialNumber.ToString();
                }

            }
            else if (waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime < currentTrialTime && currentTrialTime < waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime + feedbackTime)
            {
                HideGameObject(clickSquare);
                SendFeedback(reactionTime, successfullClick);
                currentTrialTime += Time.deltaTime * 1000;
            }
            else
            {
                currentTrialTime = 0;
                showSquare = null;
                isFlashing = false;
                currentTrialNumber += 1;
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
