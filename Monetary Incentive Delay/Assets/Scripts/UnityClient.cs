using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.DataTypes;
using Monetary_client;

public class UnityClient : MonoBehaviour
{
 	
	public Button clientBtn;
	public Button sendBtn;
	public int counter = 0;

    Client client;

    void Start()
	{
		Button btnClient = clientBtn.GetComponent<Button>();
		btnClient.onClick.AddListener(SetupClient);

		Button btnSend = sendBtn.GetComponent<Button>();
		sendBtn.onClick.AddListener(SendMsg);
	}

    void Update()
    {
        if (client != null)
        {
            ReceiveParameters();
        }
    }

	void SetupClient()
	{
        client = new Client();
        client.Connect("127.0.0.1", 11111);
	}

	void SendMsg()
	{
        SendReaction(TaskType.Control, true, 190, 140);
	}

	public bool SendReaction(TaskType type, bool incentive, double reactionTime, double threshold)
	{
        Reaction reaction = new Reaction(type.ToString(), incentive, reactionTime, threshold);
        client.SendMsg(reaction.serialize());

        return true;
    }

	public bool ReceiveParameters()
	{

        client.MsgListener();

        return true;

	}
}

