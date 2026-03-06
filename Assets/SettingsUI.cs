using UnityEngine;
using UnityEngine.UI;

public class SettingsUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private Slider bgmSlider;

    private void Start()
    {
        if (BGMManager.Instance != null)
        {
            float currentVolume = BGMManager.Instance.GetVolume();
            bgmSlider.value = currentVolume;
        }

        bgmSlider.onValueChanged.AddListener(OnBgmSliderChanged);
    }

    public void ToggleSettings()
    {
        settingsPanel.SetActive(!settingsPanel.activeSelf);
    }

    private void OnBgmSliderChanged(float value)
    {
        if (BGMManager.Instance != null)
            BGMManager.Instance.SetVolume(value);
    }
}