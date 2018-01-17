using Assets.Scripts.DataTypes;

namespace Interfaces
{
    public interface ISpriteSettings
    {
        ISpriteTime BaselineSpriteSettings { get; }
        ISpriteTime TargetSpriteSettings { get; }
        ISpriteTime CorrectSpriteSettings { get; }
        ISpriteTime IncorrectSpriteSettings { get; }
        ISpriteTime ControlCueSpriteSettings { get; }
        ISpriteTime ControlNonincentiveSpriteSettings { get; }
        ISpriteTime RewardCueSpriteSettings{ get; }
        ISpriteTime RewardNonIncentiveSpriteSettings{ get; }
        ISpriteTime PunishmentCueSpriteSettings{ get; }
        ISpriteTime PunishmentNonIncentiveSpriteSettings{ get; }

        ISpriteTime GetTimeSettings(SpriteTypes type);
    }
}