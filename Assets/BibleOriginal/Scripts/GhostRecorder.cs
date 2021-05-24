using System.Collections.Generic;
using System.Text;
using UnityEngine;
#if !FIREBASE_NONE
using Firebase.Database;
#endif

namespace FKart
{
    /// <summary>
    /// ゴースト情報
    /// </summary>
    public class GhostData
    {
        public class KeyFrame
        {
            public float time;
            public Vector3 position;
            public Quaternion rotation;
        }

        public List<KeyFrame> data { get; private set; } = new List<KeyFrame> ();

        public Avatar avatar { get; set; }

        public void Add (float time, Vector3 position, Quaternion rotation)
        {
            data.Add (new KeyFrame ()
            {
                time = time,
                position = position,
                rotation = rotation
            });
        }

        public KeyFrame Get (float time)
        {
            if (data.Count <= 1)
                return data[0];

            if (time < data[0].time)
                return data[0];

            for (int i = 0 ; i < data.Count - 1 ; i++ )
            {
                if (time > data[i+1].time)
                    continue;

                var sectionDuration = data[i+1].time - data[i].time;

                // 保険処理
                if (sectionDuration <= 0f)
                    return  data[i+1];

                var sectionTime = time - data[i].time;

                return new KeyFrame ()
                {
                    time = sectionTime,
                    position = Vector3.Lerp (data[i].position, data[i + 1].position, sectionTime / sectionDuration),
                    rotation = Quaternion.Lerp (data[i].rotation, data[i + 1].rotation, sectionTime / sectionDuration),
                };
            }

            return data[data.Count - 1];
        }

        public bool IsFinishTime (float time)
        {
            return time >= data[data.Count-1].time;
        }

        public Dictionary<string, System.Object> ToDictionary ()
        {
            var result = new Dictionary<string, System.Object> ();

            // カンマ区切りにしましょう
            var timeText = new StringBuilder (8192);
            var positionXText = new StringBuilder (8192);
            var positionYText = new StringBuilder (8192);
            var positionZText = new StringBuilder (8192);
            var rotationXText = new StringBuilder (8192);
            var rotationYText = new StringBuilder (8192);
            var rotationZText = new StringBuilder (8192);
            var rotationWText = new StringBuilder (8192);

            for (int i = 0 ; i < data.Count ; i++)
            {
                var keyFrame = data[i];
                timeText.Append (keyFrame.time);
                timeText.Append (",");

                positionXText.Append (keyFrame.position.x);
                positionXText.Append (",");
                positionYText.Append (keyFrame.position.y);
                positionYText.Append (",");
                positionZText.Append (keyFrame.position.z);
                positionZText.Append (",");

                rotationXText.Append (keyFrame.rotation.x);
                rotationXText.Append (",");
                rotationYText.Append (keyFrame.rotation.y);
                rotationYText.Append (",");
                rotationZText.Append (keyFrame.rotation.z);
                rotationZText.Append (",");
                rotationWText.Append (keyFrame.rotation.w);
                rotationWText.Append (",");
            }

            result["time"] = timeText.ToString ();

            result["positionX"] = positionXText.ToString ();
            result["positionY"] = positionYText.ToString ();
            result["positionZ"] = positionZText.ToString ();

            result["rotationX"] = rotationXText.ToString ();
            result["rotationY"] = rotationYText.ToString ();
            result["rotationZ"] = rotationZText.ToString ();
            result["rotationW"] = rotationWText.ToString ();

            result["driverColor"] = ColorUtility.ToHtmlStringRGB (avatar.driver);
            result["kartColor"] = ColorUtility.ToHtmlStringRGB (avatar.kart);

            return result;
        }
    }

    /// <summary>
    /// ターゲットの情報をゴースト用に記録
    /// </summary>
    public class GhostRecorder : MonoBehaviour
    {
        [SerializeField] Transform recordTarget = default;

        [SerializeField, Header("保存間隔")]
        float saveInterval = 0.2f;

        GhostData data = new GhostData();

        float lastSaveTime;

        Watch watch;

        void Start ()
        {
            data.avatar = Global.instance.avatar;

            watch = FindObjectOfType<Watch> ();
            watch.onFinish += () =>
            {
                if (watch.runningTime != lastSaveTime)
                    Save ();
                Global.instance.lastGhostData = data;

                Write ();
            };

            Save ();
        }

        private void Update ()
        {
            if (watch.runningTime >= (lastSaveTime + saveInterval))
                Save ();
        }
        void Save ()
        {
            lastSaveTime = watch.runningTime;
            data.Add (lastSaveTime, recordTarget.localPosition, recordTarget.localRotation);
        }
#if FIREBASE_NONE
        void Write (){ }
#else
        void Write ()
        {
            var mDatabase = FirebaseDatabase.DefaultInstance.GetReference ("ghost");

            mDatabase.UpdateChildrenAsync (data.ToDictionary ());
        }
#endif
    }
}
