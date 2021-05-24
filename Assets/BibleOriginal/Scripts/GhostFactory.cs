using UnityEngine;

namespace FKart
{
    /// <summary>
    /// ゴースト生成
    /// </summary>
    public class GhostFactory : MonoBehaviour
    {
        [SerializeField] GhostPlayer ghostPrefab = default;

        // Start is called before the first frame update
        void Start ()
        {
            // もしライバルのデータが無いなら、前回の自身の走りをゴーストに
            if (Global.instance.rivalGhostData == null)
                Global.instance.rivalGhostData = Global.instance.lastGhostData;

            if (Global.instance.rivalGhostData != null)
                Instantiate (ghostPrefab);
        }
        private void OnDisable ()
        {
            if (Global.instance != null)
                Global.instance.rivalGhostData = null;
        }
    }
}