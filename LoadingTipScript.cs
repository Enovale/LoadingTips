using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

namespace LoadingTips
{
    internal class LoadingTipScript : MonoBehaviour
    {
        private TextMeshProUGUI _text = null!;
        
        private void Awake()
        {
            _text = GetComponent<TextMeshProUGUI>();
        }

        private void OnEnable()
        {
            var rand = LoadingTips.Config.Tips[Random.Range(0, LoadingTips.Config.Tips.Count)];
            _text.text = LoadingTips.Config.TipPrefix.Value + rand;
        }
    }
}