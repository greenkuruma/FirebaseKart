using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FKart
{
    /// <summary>
    /// UGUIからの入力をBaseInputを通じてKartに伝える
    /// </summary>
    public class ArrowInput : KartGame.KartSystems.BaseInput
    {
        Dictionary<string, bool> pressKeys = new Dictionary<string, bool> ();

        void Awake ()
        {
            pressKeys["Left"] = false;
            pressKeys["Right"] = false;
            pressKeys["Up"] = false;
            pressKeys["Down"] = false;
        }

        public void OnPress (BaseEventData data)
        {
            pressKeys[data.selectedObject.name] = true;
        }
        public void OnRelease (BaseEventData data)
        {
            pressKeys[data.selectedObject.name] = false;
        }
        public override Vector2 GenerateInput ()
        {
            return new Vector2
            {
                x = (pressKeys["Left"] ? -1 : 0) + (pressKeys["Right"] ? 1 : 0),
                y = (pressKeys["Down"] ? -1 : 0) + (pressKeys["Up"] ? 1 : 0),
            };
        }
    }
}