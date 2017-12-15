using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UserInfo : MonoBehaviour {

	float timeOnScreen = 8;
	float passedTime = 0;

	List<GameObject> panelList;
	int shownPanel;
	public GameObject Panel1;
	public GameObject Panel2;
	public GameObject Panel3;
	public GameObject Panel4;
	public Image Image;

	// Use this for initialization
	void Start () {
		shownPanel = 0;
		panelList = new List<GameObject> {Panel1, Panel2, Panel3, Panel4};
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		passedTime += Time.deltaTime;
		if (!(passedTime > timeOnScreen)) return;
		
		passedTime = 0;
		ChangePanel();
	}

	private void ChangePanel()
	{
		if (shownPanel + 1 == panelList.Count)
			return;
		panelList[shownPanel++].SetActive(false);
		panelList[shownPanel].SetActive(true);
		//if (shownPanel == 3)
			//Image.GetComponent<Scr>()
	}
}
