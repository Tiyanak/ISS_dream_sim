using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Flashing : MonoBehaviour
{
	public BoxType.Type BoxType;
	public Image FlashingImage;
	private const float EndTime = 5;
	private float currentTime = 0;

	public void Flash()
	{
		while (currentTime < EndTime)
		{
			Thread.Sleep(500);
			FlashingImage.enabled = !FlashingImage.enabled;
			currentTime += Time.deltaTime;
		}
	}
}
