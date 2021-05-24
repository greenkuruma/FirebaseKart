using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FKart
{
    /// <summary>
    /// アバターのボタン
    /// UGUI用コンポーネント
    /// </summary>
    public class AvatarButton : MonoBehaviour
    {
        [SerializeField] Image bg = default;
        [SerializeField] TMP_Text text = default;

        Global global;

        Avatar avatar;

        // Start is called before the first frame update
        void Start()
        {
            global = FindObjectOfType<Global>();
        }

        public void Initialize (Avatar avatar)
        {
            this.avatar = avatar;
            text.text = avatar.name;
            bg.color = avatar.driver;
        }

        public void OnClick ()
        {
            global.avatar = avatar;
        }
    }
}