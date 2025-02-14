using HarmonyLib;
using TMPro;
using UnityEngine;

namespace LoadingTips.Patches
{
    [HarmonyPatch(typeof(HUDManager))]
    [HarmonyWrapSafe]
    internal class LoadingTipsPatches
    {
        private static GameObject? _tipsObject;

        [HarmonyPatch(nameof(HUDManager.Start))]
        [HarmonyPostfix]
        public static void HudStart(HUDManager __instance)
        {
            InitializeText(__instance.loadingText.transform.parent);
        }

        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.GenerateNewLevelClientRpc))]
        [HarmonyPatch(typeof(RoundManager), nameof(RoundManager.FinishGeneratingNewLevelClientRpc))]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SceneManager_OnLoadComplete1))]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.openingDoorsSequence), MethodType.Enumerator)]
        [HarmonyPostfix]
        public static void DarkenHudChanged()
        {
            _tipsObject?.SetActive(HUDManager.Instance.loadingDarkenScreen.enabled);
        }

        private static void InitializeText(Transform parent)
        {
            _tipsObject = new GameObject("LoadingTips", typeof(TextMeshProUGUI), typeof(LoadingTipScript));
            _tipsObject.SetActive(false);
            _tipsObject.layer = LayerMask.NameToLayer("UI");
            
            var transform = _tipsObject.GetComponent<RectTransform>();
            transform.SetParent(parent);
            transform.SetAsLastSibling();
            
            transform.localScale = new Vector3(1f, 1f, 1f);
            transform.anchorMin = new Vector2(0.5f, 0.5f);
            transform.anchorMax = new Vector2(0.5f, 0.5f);
            transform.sizeDelta = new Vector2(500f, 100f);
            transform.anchoredPosition3D = new Vector3(0f, -200f, 0f);
            
            var tipsText = _tipsObject.GetComponent<TextMeshProUGUI>();
            tipsText.font = HUDManager.Instance.loadingText.font;
            tipsText.fontMaterial = HUDManager.Instance.loadingText.fontMaterial;
            tipsText.fontSize = 18;
            tipsText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            tipsText.enableWordWrapping = true;
            // Same as lethal's loading text
            tipsText.color = new Color(0.6462264f, 0.95584375f, 1f, 0.5529412f);
        }
    }
}