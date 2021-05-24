using System.Collections;
using UnityEngine;
#if !FIREBASE_NONE
using Firebase.Database;
#endif

namespace FKart
{
    /// <summary>
    /// Firebaseから取得したアバター情報でボタン生成
    /// </summary>
    public class AvatarSelector : MonoBehaviour
    {
        [SerializeField] AvatarButton avatarButtonPrefab = default;

        [SerializeField] SkinnedMeshRenderer kart = default;
        [SerializeField] SkinnedMeshRenderer driver = default;

        Material kartMat = null;
        Material driverMat = null;

#if !FIREBASE_NONE
        DataSnapshot snapshot = null;

        // Start is called before the first frame update
        void Start ()
        {
            kartMat = Instantiate (kart.material);
            driverMat = Instantiate (driver.material);
            kart.material = kartMat;
            driver.material = driverMat;

            // 現状の設定を反映
            ChangeAvatar (Global.instance.avatar);

            FirebaseDatabase.DefaultInstance
            .GetReference ("avatars")
            .GetValueAsync ().ContinueWith (task => {
                if (task.IsFaulted)
                {
                    Debug.Log ("AvatarSelector Read Error");
                }
                else if (task.IsCompleted)
                {
                    snapshot = task.Result;
                }
            });

            StartCoroutine (ReadProcess ());
        }

        IEnumerator ReadProcess ()
        {
            var wait = new WaitForSeconds (0.1f);
            while (snapshot == null)
                yield return wait;

            foreach (var avatar in snapshot.Children)
            {
                var name = avatar.Child ("name").Value.ToString ();
                var driver = CommonUtility.ColorCodeTo (avatar.Child ("driver").Value.ToString ());
                var kart = CommonUtility.ColorCodeTo (avatar.Child ("kart").Value.ToString ());

                var button = Instantiate (avatarButtonPrefab);
                button.Initialize (new Avatar ()
                {
                    name = name,
                    driver = driver,
                    kart = kart
                });
                button.transform.SetParent (transform, false);
            }
        }

        private void OnEnable ()
        {
            Global.instance.onChangeAvatar += ChangeAvatar;
        }

        private void OnDisable ()
        {
            if (Global.instance != null)
                Global.instance.onChangeAvatar -= ChangeAvatar;
        }

        void ChangeAvatar (Avatar avatar)
        {
            kartMat.color = avatar.kart;
            driverMat.color = avatar.driver;
        }
#endif 
    }
}