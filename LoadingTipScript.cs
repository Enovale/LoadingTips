using System;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoadingTips
{
    internal class LoadingTipScript : MonoBehaviour
    {
        private TextMeshProUGUI _text = null!;
        private static int _tipIndex = -1;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            if (LoadingTips.Config.RandomizeTip.Value)
                SetRandomTip();
            else
                SetTip(_tipIndex++);
        }

        public void SetRandomTip()
        {
            if (LoadingTips.Config.Tips.Count <= 0)
            {
                LoadingTips.Logger.LogError("Tips are empty, can't set tip");
                return;
            }
            
            var index = Random.Range(0, LoadingTips.Config.Tips.Count);
            while (LoadingTips.Config.Tips.Count > 1 && index == _tipIndex)
                index = Random.Range(0, LoadingTips.Config.Tips.Count);

            SetTip(index);
            _tipIndex = index;
        }

        public void SetTip(int tipIndex)
        {
            if (LoadingTips.Config.Tips.Count <= 0)
            {
                LoadingTips.Logger.LogError("Tips are empty, can't set tip");
                return;
            }
            
            tipIndex = Math.Clamp(tipIndex, 0, LoadingTips.Config.Tips.Count - 1);
            SetTip(LoadingTips.Config.Tips[tipIndex]);
        }

        public void SetTip(string tip)
        {
            _text.text = LoadingTips.Config.TipPrefix.Value + tip;
        }
    }
}