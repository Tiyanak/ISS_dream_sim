using System;
using Assets.Scripts.DataTypes;
using JetBrains.Annotations;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Assets.Scripts.Handlers
{
    public class SpriteHandler : MonoBehaviour
    {
        public static SpriteHandler Sh;
        
        public Sprite CorrectSprite;
        public Sprite IncorrectSprite;
        public Sprite BaselineSprite;
        public Sprite TargetSprite;
        public Sprite ControlCueSprite;
	    public Sprite ControlNonIncentiveSprite;
		public Sprite RewardCueSprite;
	    public Sprite RewardNonIncentiveSprite;
		public Sprite PunishmentCueSprite;
	    public Sprite PunishmentNonIncentiveSprite;
        
	    [UsedImplicitly]
		private void Awake()
        {
            if(Sh != null)
                Destroy(Sh);
            else
                Sh = this;
         
            DontDestroyOnLoad(this);
        }
        
        public GameObject CreateSprite(SpriteTypes spriteType, GameObject panel)
        {
            var newObj = new GameObject();
            var newImage = newObj.AddComponent<Image>();
           
            switch (spriteType)
            {
                case SpriteTypes.Baseline:
                    newImage.sprite = BaselineSprite;
                    break;
                case SpriteTypes.Target:
                    newImage.sprite = TargetSprite;
                    break;
                case SpriteTypes.ControlCue:
                    newImage.sprite = ControlCueSprite;
                    break;
	            case SpriteTypes.ControlNonIncentive:
		            newImage.sprite = ControlNonIncentiveSprite;
		            break;
				case SpriteTypes.RewardCue:
                    newImage.sprite = RewardCueSprite;
                    break;
                case SpriteTypes.PunishmentCue:
                    newImage.sprite = PunishmentCueSprite;
                    break;
	            case SpriteTypes.RewardNonIncentive:
		            newImage.sprite = RewardNonIncentiveSprite;
					break;
	            case SpriteTypes.PunishmentNonIncentive:
		            newImage.sprite = PunishmentNonIncentiveSprite;
					break;
                case SpriteTypes.Correct:
                    newImage.sprite = CorrectSprite;
                    break;
                case SpriteTypes.Incorrect:
                    newImage.sprite = IncorrectSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spriteType), spriteType, null);
            }
            newObj.GetComponent<RectTransform>().SetParent(panel.transform);
	        newObj.transform.localPosition = new Vector3(0,0);
            newObj.SetActive(true);
            return newObj;
        }

        public void DestroySprite(GameObject sprite)
        {
            DestroyObject(sprite);
        }
    }
}