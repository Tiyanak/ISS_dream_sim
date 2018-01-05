using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
	public BoxType.Type BoxType;
	private const float EndTime = 5;
	private float currentTime = 0;
	private float waitTime = 0;
	private float endWaitTime = 3;
	public RawImage targetImage;
	public RawImage answeredImage;
	public bool isSpaceClicked = false;

	public void Start() {
		targetImage.enabled = false;
		answeredImage.enabled = false;
	}

	public void Update() {

		Flash();

	}

	public void Flash()
	{
		// wait until its time to show target square
		if (currentTime < EndTime) {
			currentTime += Time.deltaTime;
		
		// its time to show target square
		} else {
			
			// show only first time when space not clicked yet
			if (!isSpaceClicked) {
				targetImage.enabled = true;
			}

			// when space is clicked for the first time, hide real square and show the answered one
			if (Input.GetKeyDown("space") && !isSpaceClicked) {
				isSpaceClicked = true;				
				answeredImage.enabled = true;
				targetImage.enabled = false;

			// when space is clicked wait some time of showing answered square
			} else if (isSpaceClicked) {
				if (waitTime < endWaitTime) {
					waitTime += Time.deltaTime;

				// when wait time have ended, start the next test iteration
				} else {
					answeredImage.enabled = false;
					isSpaceClicked = false;
					currentTime = 0;
					waitTime = 0;
				}
			}
		}
	}
}
