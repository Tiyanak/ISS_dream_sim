using System;
using DataTypes;
using UnityEngine;
using Image = UnityEngine.UI.Image;

namespace Classes
{
    public class SpriteHandler : MonoBehaviour
    {
        public static SpriteHandler Sh;
        
        public Sprite BaselineSprite;
        public Sprite NonIncentiveSprite;
        public Sprite TargetSprite;
        public Sprite ControlCueSprite;
        public Sprite RewardCueSprite;
        public Sprite PunishmentCueSprite;

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
                case SpriteTypes.NonIncentive:
                    newImage.sprite = NonIncentiveSprite;
                    break;
                case SpriteTypes.Target:
                    newImage.sprite = TargetSprite;
                    break;
                case SpriteTypes.ControlCue:
                    newImage.sprite = ControlCueSprite;
                    break;
                case SpriteTypes.RewardCue:
                    newImage.sprite = RewardCueSprite;
                    break;
                case SpriteTypes.PunishmentCue:
                    newImage.sprite = PunishmentCueSprite;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(spriteType), spriteType, null);
            }
            newObj.GetComponent<RectTransform>().SetParent(panel.transform);
            newObj.SetActive(true);
            return newObj;
        }

        public void DestroySprite(GameObject sprite)
        {
            DestroyObject(sprite);
        }
    }
}