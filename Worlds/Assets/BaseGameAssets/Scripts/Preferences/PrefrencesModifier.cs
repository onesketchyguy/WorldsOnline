using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PrefrencesModifier : MonoBehaviour
{
    public Dropdown resolutionsDropdown;

    public Toggle fullscreenToggle;

    private void Start()
    {
        PreferencesManager.LoadSettings();

        SetupFullScreenToggler();
        SetupResolutions();

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

        var currentResIndex = 0;

        for (int i = 0; i < Screen.resolutions.Length; i++)
        {
            Resolution item = Screen.resolutions[i];

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

    public void LoadInPrefs(PreferencesManager.Preferences preferences)
    {
        Screen.SetResolution(preferences.screenX, preferences.screenY, preferences.fullScreen);

        PreferencesManager.SaveSettings();
    }
}