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
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.SceneManager_OnLoad))]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.openingDoorsSequence), MethodType.Enumerator)]
        [HarmonyPatch(typeof(StartOfRound), nameof(StartOfRound.waitingForOtherPlayersToRevive), MethodType.Enumerator)]
        [HarmonyPostfix]
        public static void DarkenHudChanged()
        {
            if (_tipsObject == null ||
                HUDManager.Instance == null ||
                StartOfRound.Instance == null ||
                RoundManager.Instance == null ||
                StartOfRound.Instance.shipHasLanded ||
                StartOfRound.Instance.shipDoorsEnabled ||
                RoundManager.Instance.dungeonCompletedGenerating)
                return;
            
            _tipsObject.SetActive(HUDManager.Instance.loadingText.enabled);
            
            // Vanilla game doesn't show the darkened overlay immediately for some reason, let's fix that
            HUDManager.Instance.loadingDarkenScreen.enabled = HUDManager.Instance.loadingText.enabled;
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
            tipsText.font.fallbackFontAssetTable.Add(HUDManager.Instance.controlTipLines[0].font);
            tipsText.fontSize = 18;
            tipsText.horizontalAlignment = HorizontalAlignmentOptions.Center;
            tipsText.enableWordWrapping = true;
            tipsText.overflowMode = TextOverflowModes.Masking;
            tipsText.color = HUDManager.Instance.loadingText.color;
        }
    }
}