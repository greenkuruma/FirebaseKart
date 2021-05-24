using UnityEngine;
using UnityEngine.Events;

namespace FKart
{
    /// <summary>
    /// アバター情報
    /// </summary>
    public class Avatar
    {
        public string name;
        public Color driver;
        public Color kart;
    }

    /// <summary>
    /// 全シーン共通で使う
    /// </summary>
    public class Global : MonoBehaviour
    {
        public static Global _instance;
        public static Global instance {
            get {
                if (_instance == null)
                    _instance = FindObjectOfType<Global> ();
                return _instance;
            }
        }

        public UnityAction<Avatar> onChangeAvatar;

        public GhostData lastGhostData { get; set; }

        public GhostData rivalGhostData { get; set; }

        // 未設定アバターはなんとなく白にしとく
        Avatar _avatar = new Avatar ()
        {
            name = "empty",
            driver = Color.white,
            kart = Color.grey
        };

        [SerializeField, Header ("リザルトシーンから直接開いた時のタイム")]
        float defaultClearTime = 3.14f;

        public float endTime { get; set; }

        public Avatar avatar {
            get {
                return _avatar;
            }
            set {
                _avatar = value;
                onChangeAvatar?.Invoke (avatar);
            }
        }
        void Awake ()
        {
            // 既に存在しているならgameObjectごと消す
            if (instance != this)
            {
                Destroy (gameObject);
                return;
            }

            DontDestroyOnLoad (gameObject);

            endTime = defaultClearTime;
        }
    }
}