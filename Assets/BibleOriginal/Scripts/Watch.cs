using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace FKart
{
    /// <summary>
    /// スタートからゴールするまでの時間計測
    /// </summary>
    public class Watch : MonoBehaviour
    {
        [SerializeField] TMP_Text text = default;

        bool watching;
        float startedTime;
        float updatedTime;

        public float runningTime {get; private set;} = 0f;
        
        public UnityAction onFinish;

        // Start is called before the first frame update
        void Start()
        {
        }

        void OnEnable ()
        {
            var timeManager = FindObjectOfType<TimeManager> ();
            timeManager.OnRaceToggle += OnRaceToggle;
        }
        void OnDisable ()
        {
            var timeManager = FindObjectOfType<TimeManager> ();
            if (timeManager != null)
                timeManager.OnRaceToggle -= OnRaceToggle;

            if (Global.instance != null)
                Global.instance.endTime = updatedTime - startedTime;
        }
        // Update is called once per frame
        void Update ()
        {
            if (watching)
                updatedTime = Time.realtimeSinceStartup;
            runningTime = updatedTime - startedTime;

            text.text = CommonUtility.ToRankingTimeString (runningTime);
        }

        void OnRaceToggle (bool start)
        {
            watching = start;
            if (start)
                startedTime = Time.realtimeSinceStartup;
            else
                onFinish?.Invoke ();
        }
    }
}