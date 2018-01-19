using System;
using Assets.Scripts.DataTypes;
using Assets.Scripts.Interfaces;

namespace Assets.Scripts.Settings
{
    public class SpriteSettings : ISpriteSettings
    {
        public ISpriteTime BaselineSpriteSettings { get; private set; }
        public ISpriteTime TargetSpriteSettings { get; private set; }
        public ISpriteTime CorrectSpriteSettings { get; private set; }
        public ISpriteTime IncorrectSpriteSettings { get; private set; }
        public ISpriteTime ControlCueSpriteSettings { get; private set; }
        public ISpriteTime ControlNonincentiveSpriteSettings { get; private set; }
        public ISpriteTime RewardCueSpriteSettings{ get; private set; }
        public ISpriteTime RewardNonIncentiveSpriteSettings{ get; private set; }
        public ISpriteTime PunishmentCueSpriteSettings{ get; private set; }
        public ISpriteTime PunishmentNonIncentiveSpriteSettings{ get; private set; }

        public SpriteSettings()
        {
            const int afterAWhile = 600;
            const int cueTimes = 600;
            BaselineSpriteSettings = new SpriteTime(new Interval(1000, 2000),new Interval(180, 200), SpriteTypes.Baseline);
            TargetSpriteSettings = new SpriteTime(new Interval(2000, 4000),new Interval(180, 200), SpriteTypes.Target);
            CorrectSpriteSettings = new SpriteTime(new Interval(afterAWhile, afterAWhile),new Interval(cueTimes, cueTimes), SpriteTypes.Correct);
            IncorrectSpriteSettings = new SpriteTime(new Interval(afterAWhile, afterAWhile),new Interval(cueTimes, cueTimes), SpriteTypes.Incorrect);
            ControlCueSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.ControlCue);
            ControlNonincentiveSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.ControlNonIncentive);
            RewardCueSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.RewardCue);
            RewardNonIncentiveSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.RewardNonIncentive);
            PunishmentCueSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.PunishmentCue);
            PunishmentNonIncentiveSpriteSettings = new SpriteTime(new Interval(cueTimes, cueTimes),new Interval(cueTimes, cueTimes), SpriteTypes.PunishmentNonIncentive);
        }

        public ISpriteTime GetTimeSettings(SpriteTypes type)
        {
            ISpriteTime desired;
            switch (type)
            {
                case SpriteTypes.Correct:
                    desired = CorrectSpriteSettings;
                    break;
                case SpriteTypes.Incorrect:
                    desired = IncorrectSpriteSettings;
                    break;
                case SpriteTypes.Baseline:
                    desired = BaselineSpriteSettings;
                    break;
                case SpriteTypes.Target:
                    desired = TargetSpriteSettings;
                    break;
                case SpriteTypes.ControlCue:
                    desired = ControlCueSpriteSettings;
                    break;
                case SpriteTypes.ControlNonIncentive:
                    desired = ControlNonincentiveSpriteSettings;
                    break;
                case SpriteTypes.RewardCue:
                    desired = RewardCueSpriteSettings;
                    break;
                case SpriteTypes.RewardNonIncentive:
                    desired = RewardNonIncentiveSpriteSettings;
                    break;
                case SpriteTypes.PunishmentCue:
                    desired = PunishmentCueSpriteSettings;
                    break;
                case SpriteTypes.PunishmentNonIncentive:
                    desired = PunishmentNonIncentiveSpriteSettings;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(type), type, null);
            }
            return desired;
        }
    }
}