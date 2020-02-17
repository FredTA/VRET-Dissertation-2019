using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SystemMode {
    Claustrophobia, 
    Heights, 
    Spider,
    Wasp, 
    Airplane, 
    LevelSelection
}

public class Master : MonoBehaviour {

    private const string SAVE_FILE_NAME = "savefile.save";
    public Save save;


    private SystemMode currentMode;

	// Use this for initialization
	void Start () {
        //Ensures the GameObject this script is attached to will persist from scene to scene
        DontDestroyOnLoad(gameObject);

        save = loadSave();

        //We'll start on the level selection screen
        currentMode = SystemMode.LevelSelection;

        SceneManager.LoadScene("Level Selection");
	}
	
	// Update is called once per frame
	void Update () {
        //For ease of debugging. TODO remove
		if (Input.GetKeyDown(KeyCode.KeypadEnter)) {
            save.updateData(SystemMode.Spider, 5, 100, 5);
            saveState();
            Debug.Log("Completed spider level 5");
        }
	}

    public void completeLevel(int levelCompleted, int score, int sudsValue) {
        save.updateData(currentMode, levelCompleted, score, sudsValue);
        saveState();
    }

    public int[] getUnlockedLevels() {
        int[] unlockedLevels = new int[5];
        for (int mode = 0; mode < 5; mode++) {
            unlockedLevels[mode] = save.getLevelUnlockedForMode((SystemMode)mode);
        }

        return unlockedLevels;
    }

    private Save loadSave() {
        Debug.Log("Looking for save " + Application.persistentDataPath + "/" + SAVE_FILE_NAME);
        if (File.Exists(Application.persistentDataPath + "/" + SAVE_FILE_NAME)) {
            Debug.Log("Found save file, loading...");
            //Open the file, deserialize the byte stream, and leave it as a Save object
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/" + SAVE_FILE_NAME, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();
            //Debug.Log("FROM MASTER " + save.getLevelUnlockedForMode(SystemMode.Spider));
            return save;
        } else {
            
            Debug.Log("No save file found");
            return new Save(); //No need to save this as a file yet
        }
    }

    //Serializes the Save object and stores as file
    public void saveState() {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/" + SAVE_FILE_NAME);
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("System State Saved");
    }

    public void changeMode(SystemMode newMode) {
        switch (newMode) {
            case SystemMode.Claustrophobia:

                break;
            case SystemMode.Heights:

                break;
            case SystemMode.Spider:

                break;
            case SystemMode.Wasp:

                break;
            case SystemMode.Airplane:

                break;
            case SystemMode.LevelSelection:

                break;
        }
    }

    
}
