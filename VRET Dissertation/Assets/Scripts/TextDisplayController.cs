using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TextDisplayController : MonoBehaviour {

    public Text spiderModeTVText;
    public Text heightsModeTVText;
    public Text wallsModeTVtext;
    public Text modeChangingTVText;
    public Text spiderModeTableText;
    public Text heightsModeTableText;
    public Text wallsModeTabletext;
    public Text modeChangingTableText;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void HighLightSpiderText() {
        DeactivateChangingModeText();

        modeChangingTVText.enabled = false;
        modeChangingTableText.enabled = false;

        spiderModeTVText.color = Color.green;
        heightsModeTVText.color = Color.white;
        wallsModeTVtext.color = Color.white;

        spiderModeTableText.enabled = true;
    }

    public void HighLightHeightsText() {
        DeactivateChangingModeText();

        modeChangingTVText.enabled = false;
        modeChangingTableText.enabled = false;

        spiderModeTVText.color = Color.white;
        heightsModeTVText.color = Color.green;
        wallsModeTVtext.color = Color.white;

        heightsModeTableText.enabled = true;
    }

    public void HighLightWallsText() {
        DeactivateChangingModeText();

        modeChangingTVText.enabled = false;
        modeChangingTableText.enabled = false;

        spiderModeTVText.color = Color.white;
        heightsModeTVText.color = Color.white;
        wallsModeTVtext.color = Color.green;

        wallsModeTabletext.enabled = true;
    }

    public void ActivateChangingModeText() {
        spiderModeTVText.enabled = false;
        heightsModeTVText.enabled = false;
        wallsModeTVtext.enabled = false;
        spiderModeTableText.enabled = false;
        heightsModeTableText.enabled = false;
        wallsModeTabletext.enabled = false;

        modeChangingTVText.enabled = true;
        modeChangingTableText.enabled = true;
    }

    private void DeactivateChangingModeText() {
        spiderModeTVText.enabled = true;
        heightsModeTVText.enabled = true;
        wallsModeTVtext.enabled = true;

        modeChangingTVText.enabled = false;
        modeChangingTableText.enabled = false;
    }
}
