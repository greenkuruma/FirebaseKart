using UnityEngine;
using System.Collections;
#if FIREBASE_NONE
namespace FKart { public class GhostLoader : MonoBehaviour { } }
#else
using Firebase.Database;

namespace FKart
{
    /// <summary>
    /// Firebaseからゴースト情報を読み込み
    /// </summary>
    public class GhostLoader : MonoBehaviour
    {
        DataSnapshot snapshot = null;

        void Start ()
        {
            FirebaseDatabase.DefaultInstance
            .GetReference ("ghost")
            .GetValueAsync ().ContinueWith (task => {
                if (task.IsFaulted)
                {
                    Debug.Log ("GhostLoader Read Error");
                    // Handle the error...
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
            var wait = new WaitForSeconds(0.1f);
            while (snapshot == null)
                yield return wait;

            var timeArray = snapshot.Child ("time").Value.ToString ().Split(',');

            var positionXArray = snapshot.Child ("positionX").Value.ToString ().Split (',');
            var positionYArray = snapshot.Child ("positionY").Value.ToString ().Split (',');
            var positionZArray = snapshot.Child ("positionZ").Value.ToString ().Split (',');

            var rotationXArray = snapshot.Child ("rotationX").Value.ToString ().Split (',');
            var rotationYArray = snapshot.Child ("rotationY").Value.ToString ().Split (',');
            var rotationZArray = snapshot.Child ("rotationZ").Value.ToString ().Split (',');
            var rotationWArray = snapshot.Child ("rotationW").Value.ToString ().Split (',');

            var ghostData = new GhostData ();
            for (int i = 0 ; i < timeArray.Length ; i++)
            {
                if (string.IsNullOrEmpty(timeArray[i]))
                    continue;

                ghostData.Add (
                    float.Parse(timeArray[i]),
                    new Vector3 (
                        float.Parse (positionXArray[i]),
                        float.Parse (positionYArray[i]),
                        float.Parse (positionZArray[i])),
                    new Quaternion (
                        float.Parse (rotationXArray[i]),
                        float.Parse (rotationYArray[i]),
                        float.Parse (rotationZArray[i]),
                        float.Parse (rotationWArray[i])
                ));
            }

            var driver = CommonUtility.ColorCodeTo (snapshot.Child ("driverColor").Value.ToString ());
            var kart = CommonUtility.ColorCodeTo (snapshot.Child ("kartColor").Value.ToString ());
            ghostData.avatar = new Avatar ()
            {
                name = "Ghost",
                driver = driver,
                kart = kart
            };

            Global.instance.rivalGhostData = ghostData;
            Debug.Log ($"Ghost Loaded!");
        }
    }
}
#endif