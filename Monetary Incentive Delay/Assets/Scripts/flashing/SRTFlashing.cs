using UnityEngine;
using UnityEngine.UI;
using Monetary_client;
using Assets.Scripts.DataTypes;
using Classes;

public class SRTFlashing : MonoBehaviour {

    Client client;

    private System.Random random = new System.Random();
    private MapSquares mapSquares;

    public GameObject panelBaseline;
    private GameObject clickSquare;
    private GameObject showSquare;
    public Text scoreText;

    private double reactionTime = 0;

    public int totalTrials = 20;
    public int playerScore = 0;
    public int currentTrialNumber = 1;
    public double waitUntilClickSquareTime = 4000;
    public double showSquareShowTime = 500;
    public double clickSquareShowTime = 200;
    public double feedbackTime = 500;
    public double currentTrialTime = 0;

    public int waitUntilClickSquareTimeMin = 4000;
    public int waitUntilClickSquareTimeMax = 4500;

    private bool isFlashing = false;
    private bool successfullClick = false;
    private bool isFeedbackSent = false;


    // Use this for initialization
    void Start () {
        mapSquares = new MapSquares();
        SetupClient();
        clickSquare = mapSquares.GetClickSquare();
        scoreText.enabled = true;
    }
	
	// Update is called once per frame
	void Update () {
        if (currentTrialNumber <= totalTrials) {
            Flash();    
        } else {
            LoadIncentiveTask();
        }
    }

    private void LoadIncentiveTask() {

        int squareIndex = random.Next(2, 5);

        //GuiHandler.LoadScene(squareIndex);

    }

    public void Flash()
    {
        if (!isFlashing)
        {
            showSquare = mapSquares.PickRandomAllSquare();
            waitUntilClickSquareTime = random.Next(waitUntilClickSquareTimeMin, waitUntilClickSquareTimeMax);
            isFlashing = true;
            successfullClick = false;
            isFeedbackSent = false;            
            scoreText.text = playerScore.ToString() + "/" + currentTrialNumber.ToString();
        } else
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

                if (Input.GetKeyDown("space") && !successfullClick) {
                    reactionTime = currentTrialTime - (waitUntilClickSquareTime + showSquareShowTime);
                    successfullClick = true;
                    playerScore += 1;
                    scoreText.text = playerScore.ToString() + "/" + currentTrialNumber.ToString();
                }

            }
            else if (waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime < currentTrialTime && currentTrialTime < waitUntilClickSquareTime + showSquareShowTime + clickSquareShowTime + feedbackTime)
            {
                HideGameObject(clickSquare);

                if (!isFeedbackSent)
                {
                    SendFeedback(reactionTime, successfullClick);
                    isFeedbackSent = true;
                    reactionTime = 0;
                }
                
                currentTrialTime += Time.deltaTime * 1000;
            }
            else
            {
                currentTrialNumber += 1;
                currentTrialTime = 0;
                showSquare = null;
                isFlashing = false;                
            }
        }
        
       
      
    }

    public void SendFeedback(double reactionTime, bool successfullClick)
    {
        SendReaction(TaskType.Control, true, reactionTime, 200);
    }

    public void ShowGameObject(GameObject gameObject)
    {
        gameObject.GetComponent<RawImage>().enabled = true;       
    }

    public void HideGameObject(GameObject gameObject)
    {
        gameObject.GetComponent<RawImage>().enabled = false;
    }

    void SetupClient()
    {
        client = new Client();
        client.Connect("127.0.0.1", 11111);
    }
    
    public void SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
    {
        Reaction reaction = new Reaction(type.ToString(), incentive, reactionTime, threshold);
        client.SendMsg(reaction.serialize());
    }

    public void ReceiveParameters()
    {
        client.MsgListener();
    }

}
