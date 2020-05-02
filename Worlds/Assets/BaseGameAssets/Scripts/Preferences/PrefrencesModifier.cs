using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class PrefrencesModifier : MonoBehaviour
{
    public RenderPipelineAsset[] piplines;

    public Dropdown qualityDropDown;
    public Dropdown resolutionsDropdown;

    public Toggle fullscreenToggle;

    private void Start()
    {
        PreferencesManager.LoadSettings();

        SetupFullScreenToggler();
        SetupResolutions();
        SetupQualitySettings();

        LoadInPrefs(PreferencesManager.current_prefs);
    }

    private void SetupFullScreenToggler()
    {
        fullscreenToggle.isOn = PreferencesManager.current_prefs.fullScreen;

        fullscreenToggle.onValueChanged.AddListener((bool val) =>
        {
            PreferencesManager.current_prefs.fullScreen = val;
            LoadInPrefs(PreferencesManager.current_prefs);
        });
    }

    private void SetupResolutions()
    {
        // Setup the resolutions
        resolutionsDropdown.ClearOptions(); // Clear the list so we can start fresh
        var options = new List<Dropdown.OptionData>(); // Store a temporary resolution options variable
        var resolutions = Screen.resolutions;

        var currentResIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            var item = resolutions[i];

            if (Screen.width == item.width && Screen.height == item.height)
                currentResIndex = i;

            options.Add(new Dropdown.OptionData($"{item.width}*{item.height}")); // add our current res to the list
        }

        resolutionsDropdown.AddOptions(options); // add all the options to the list

        resolutionsDropdown.SetValueWithoutNotify(currentResIndex); // set the current index

        resolutionsDropdown.onValueChanged.AddListener((int index) =>
        {
            var option = resolutionsDropdown.options[index].text;

            var dat = option.Split('*');

            if (dat == null || dat.Length <= 1) return;
            var x = dat[0];
            var y = dat[1];

            PreferencesManager.current_prefs.screenX = int.Parse(x);
            PreferencesManager.current_prefs.screenY = int.Parse(y);
            LoadInPrefs(PreferencesManager.current_prefs);
        });
    }

    private void SetupQualitySettings()
    {
        qualityDropDown.ClearOptions();

        var options = new List<Dropdown.OptionData>();

        var currentIndex = 0;

        for (int i = 0; i < piplines.Length; i++)
        {
            RenderPipelineAsset item = piplines[i];
            var itemName = item.name.Split('_').LastOrDefault();

            if (item == QualitySettings.renderPipeline) currentIndex = i;

            options.Add(new Dropdown.OptionData()
            {
                text = itemName
            });
        }

        qualityDropDown.SetValueWithoutNotify(currentIndex);

        qualityDropDown.AddOptions(options);
        qualityDropDown.onValueChanged.AddListener(
        (int index) =>
        {
            GraphicsSettings.renderPipelineAsset = piplines[index];
            QualitySettings.renderPipeline = piplines[index];

            PreferencesManager.current_prefs.quality = index;

            PreferencesManager.SaveSettings();
        });
    }

    public void LoadInPrefs(PreferencesManager.Preferences preferences)
    {
        Screen.SetResolution(preferences.screenX, preferences.screenY, preferences.fullScreen);
        GraphicsSettings.renderPipelineAsset = piplines[preferences.quality];
        QualitySettings.renderPipeline = piplines[preferences.quality];

        PreferencesManager.SaveSettings();
    }
}