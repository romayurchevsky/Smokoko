using Scripts.CaneraCode;
using Scripts.CombatCode;
using Scripts.PlayerCode;
using UnityEngine;

namespace Scripts.CommonCode
{
    public class DependencyStorage : MonoBehaviour
    {
        public static PlayerStorage PlayerStorage { get; set; }
        public static AssetLoader AssetLoader { get; set; }
        public static TargetStorage TargetStorage { get; set; }
        public static BaseHero BaseHeroController { get; set; }
        public static CameraController CameraController { get; set; }
        public static Joystick Joystick { get; set; }
        public static BulletPool BulletPool { get; set; }
    }
}
