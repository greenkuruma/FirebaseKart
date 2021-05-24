using UnityEngine;

namespace FKart
{
    /// <summary>
    /// ユーティリティ
    /// </summary>
    public static class CommonUtility
    {
        public static string ToRankingTimeString (float time)
        {
            time = ToRankingTime(time);
            var minute = (int)time / 60;
            var second = (int)time % 60;
            var centisecond = (int)(time * 100) % 100;
            return $"{minute}:{second:00}:{centisecond:00}";
        }
        public static float ToRankingTime (float time)
        {
            return Mathf.Round (time * 100) / 100f;
        }
        public static Color ColorCodeTo (string code)
        {
            if (code.IndexOf ("#") != 0)
                code = $"#{code}";

            Color color;
            ColorUtility.TryParseHtmlString (code, out color);
            return color;
        }
    }
}