using UnityEngine;
using UnityEngine.UI;
using Monetary_client;
using Assets.Scripts.DataTypes;

public class SRTFlashing : MonoBehaviour {

    Client client;

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
    private bool isFeedbackSent = false;


    // Use this for initialization
    void Start () {
        SetupClient();
        clickSquare = MapSquares.GetClickSquare();
    }
	
	// Update is called once per frame
	void Update () {
        Flash();    
	}

    public void Flash()
    {
        if (!isFlashing)
        {
            showSquare = MapSquares.PickRandomAllSquare();
            waitUntilClickSquareTime = random.Next(waitUntilClickSquareTimeMin, waitUntilClickSquareTimeMax);
            isFlashing = true;
            successfullClick = false;
            isFeedbackSent = false;
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

                if (Input.GetKeyDown("space")) {
                    reactionTime = currentTrialTime - (waitUntilClickSquareTime + showSquareShowTime);
                    successfullClick = true;
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
