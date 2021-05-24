using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
#if FIREBASE_NONE
namespace FKart
{
    public class Ranking : MonoBehaviour
    {
        [SerializeField] int rankingLength = 20;
        [SerializeField] TMP_Text textPrefab = default;
        [SerializeField] Transform textParent = default;

        [SerializeField] Color normalColor = Color.white;
        [SerializeField] Color resultColor = Color.cyan;
    }
}
#else
using Firebase.Database;
namespace FKart
{
    [Serializable]
    public class Record
    {
        public string key = "";
        public string name = "";
        public float time = 999f;
    }

    /// <summary>
    /// RealtimeDatabaseへの登録用ユーティリティ
    /// </summary>
    public static class RecordEntry
    {
        public static Dictionary<string, System.Object> ToDictionary (string name, float time)
        {
            var result = new Dictionary<string, System.Object> ();
            result["name"] = name;
            result["time"] = CommonUtility.ToRankingTime(time);

            return result;
        }
    }

    /// <summary>
    /// ランキング表示生成
    /// データベースを監視して、データベース更新時にランキング表示も更新
    /// </summary>
    public class Ranking : MonoBehaviour
    {
        [SerializeField] int rankingLength = 20;
        [SerializeField] TMP_Text textPrefab = default;
        [SerializeField] Transform textParent = default;

        [SerializeField] Color normalColor = Color.white;
        [SerializeField] Color resultColor = Color.cyan;

        string writeKey;

        DatabaseReference rankingRef;

        void OnEnable ()
        {
            // まずは現在のレコードを登録
            var name = Global.instance.avatar.name;
            var time = Global.instance.endTime;
            WriteRecord (name, time);

            // ランキング購読
            // ここでDatabaseReferenceを保存しとかないと解除できなくなる
            rankingRef = FirebaseDatabase.DefaultInstance.GetReference ("ranking");
            rankingRef.ValueChanged += HandleValueChanged;
        }

        void OnDisable ()
        {
            // 購読解除
            if (rankingRef != null)
                rankingRef.ValueChanged -= HandleValueChanged;
        }

        void WriteRecord (string name, float time)
        {
            var mDatabase = FirebaseDatabase.DefaultInstance.GetReference("ranking");

            var writeDatabase = mDatabase.Push ();
            writeKey = writeDatabase.Key;
            
            var entryValues = RecordEntry.ToDictionary (name, time);
            writeDatabase.UpdateChildrenAsync (entryValues);
        }

        void HandleValueChanged (object sender, ValueChangedEventArgs args)
        {
            if (args.DatabaseError != null)
            {
                Debug.LogError (args.DatabaseError.Message);
                return;
            }

            foreach (Transform child in textParent)
                Destroy (child.gameObject);

            var snapshot = args.Snapshot;

            var records = new List<Record>();
            foreach (var record in snapshot.Children)
                records.Add (new Record ()
                {
                    key = record.Key,
                    name = record.Child ("name").Value.ToString (),
                    time = float.Parse (record.Child ("time").Value.ToString ())
                });

            // 速い順にソート
            records.Sort ((a, b) => (int)Mathf.Sign (a.time - b.time));

            for (int i = 0 ; i < records.Count && i < rankingLength ; i++)
            {
                var record = records[i];
                int rank = i+1;

                var text = Instantiate (textPrefab);
                text.text = $"#{rank} {record.name} {CommonUtility.ToRankingTimeString(record.time)}";

                text.color = record.key == writeKey ? resultColor : normalColor;
                text.transform.SetParent (textParent, false);
            }
        }
    }
}
#endif