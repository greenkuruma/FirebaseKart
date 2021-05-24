using UnityEngine;

namespace FKart
{
    /// <summary>
    /// アバター情報をカートに反映
    /// </summary>
    public class AvatarSetter : MonoBehaviour
    {
        [SerializeField] SkinnedMeshRenderer kart = default;
        [SerializeField] SkinnedMeshRenderer driver = default;

        // Start is called before the first frame update
        void Start ()
        {
            var kartMat = Instantiate (kart.material);
            var driverMat = Instantiate (driver.material);
            kart.material = kartMat;
            driver.material = driverMat;

            var avatar = Global.instance.avatar;
            kartMat.color = avatar.kart;
            driverMat.color = avatar.driver;
        }

        // Update is called once per frame
        void Update ()
        {

        }
    }
}