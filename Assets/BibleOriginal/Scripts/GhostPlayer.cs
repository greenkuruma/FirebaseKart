using UnityEngine;

namespace FKart
{
    /// <summary>
    /// ゴースト
    /// ゴーストより先にプレイヤーがゴールすると、ゴースト自身がその場で停止しますが仕様です
    /// (runningTimeのみを見ているため)
    /// </summary>
    public class GhostPlayer : MonoBehaviour
    {
        GhostData data;

        Watch watch;

        [SerializeField] SkinnedMeshRenderer kart = default;
        [SerializeField] SkinnedMeshRenderer driver = default;

        const float alpha = 0.3f;

        void Start ()
        {
            watch = FindObjectOfType<Watch> ();
            data = Global.instance.rivalGhostData;

            var kartMat = Instantiate (kart.material);
            var driverMat = Instantiate (driver.material);
            kart.material = kartMat;
            driver.material = driverMat;

            var avatar = Global.instance.avatar;
            var kartColor = data.avatar.kart;
            var driverColor = data.avatar.driver;

            kartColor.a *= alpha;
            driverColor.a *= alpha;

            kartMat.color = kartColor;
            driverMat.color = driverColor;
        }

        void Update ()
        {
            if (data.IsFinishTime(watch.runningTime))
            {
                Destroy (gameObject);
                return;
            }

            var keyFrame = data.Get (watch.runningTime);

            transform.localPosition = keyFrame.position;
            transform.localRotation = keyFrame.rotation;
        }
    }
}
