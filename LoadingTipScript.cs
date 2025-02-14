using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoadingTips
{
    internal class LoadingTipScript : MonoBehaviour
    {
        private TextMeshProUGUI _text = null!;
        private static int _tipIndex;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            if (LoadingTips.Config.RandomizeTip.Value)
                SetRandomTip();
            else
                SetTip(_tipIndex);
        }

        public void SetRandomTip()
        {
            var rand = LoadingTips.Config.Tips[Random.Range(0, LoadingTips.Config.Tips.Count)];
            SetTip(rand);
        }

        public void SetTip(int tipIndex)
        {
            tipIndex = Math.Clamp(tipIndex, 0, LoadingTips.Config.Tips.Count - 1);
            SetTip(LoadingTips.Config.Tips[tipIndex]);
        }

        public void SetTip(string tip)
        {
            _text.text = LoadingTips.Config.TipPrefix.Value + tip;
            _tipIndex++;
        }
    }
}