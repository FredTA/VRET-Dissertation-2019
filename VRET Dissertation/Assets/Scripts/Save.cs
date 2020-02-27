using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; //For TimeDate

[System.Serializable]
public struct SUDSRecord {
    public int rating;
    public DateTime date;
}

//Unlike our other scripts, this one wont be attached to a specific GameObject 
//Therefore we don't inherit from MonoBehaviour. Instead, we make sure this class
//is Serializable, so that we can convert it to a byte stream and save our data
[System.Serializable]
public class Save{

    //The number at each element indicates the number of levels unlocked 
    //For the corresponding mode. E.G, if element 2 = 5, the user has reached 
    //level 5 on the spider mode
    private int[] unlockedLevels = new int[5];

    //Stores the score for each of the 50 levels 
    private int[,] achievedHighScores = new int[5,10];

    //A list for each of the 50 levels, stores any number of SUDS ratings, with their date
    private List<SUDSRecord>[,] sudsRatings = new List<SUDSRecord>[5,10];

    public Save() {
        for (int mode = 0; mode < 5; mode++) {
            //Each mode starts with just the intro level(#0) being unlocked
            unlockedLevels[mode] = 0;
            for (int level = 0; level < 10; level++) {
                achievedHighScores[mode, level] = -1;
                sudsRatings[mode, level] = new List<SUDSRecord>();
                //Debug.Log("Popuilating..., level " + mode + ", " + level);
            }
        }
    }

    public void updateData(SystemMode mode, int levelCompleted, int score, int sudsValue) {
        unlockLevel(mode, levelCompleted + 1);

        //Only update the score if it';s the highest so far for that level
        if (achievedHighScores[(int)mode, levelCompleted] < score) {
            achievedHighScores[(int)mode, levelCompleted] = score;
        }

        SUDSRecord sudsRecord;
        sudsRecord.date = System.DateTime.Now;
        sudsRecord.rating = sudsValue;

        //Debug.Log("SUDSRATING, level " + mode + ", " + levelCompleted + ": " + sudsValue);
        sudsRatings[(int)mode, levelCompleted].Add(sudsRecord);
    }

    private void unlockLevel(SystemMode mode, int levelUnlocked) {
        Debug.Log(mode + "mode, " + unlockedLevels[(int)mode] + " lvls unlocked, checking " + levelUnlocked);
        //Check if we've already unlocked higher levels than the one we're progressing to
        if (unlockedLevels[(int)mode] < levelUnlocked) {
            Debug.Log("From Save class: Unlocked level " + mode + " " + levelUnlocked);
            unlockedLevels[(int)mode] = levelUnlocked;

            //Once the introduction has been completed, level 1 on all modes should unlock
            if (levelUnlocked == 1) {
                foreach (SystemMode modeToUpdate in Enum.GetValues(typeof(SystemMode))) {
                    //No need top update the current mode, no levels for LS
                    if (modeToUpdate == mode || modeToUpdate == SystemMode.LevelSelection) {
                        continue;
                    }
                    else if (unlockedLevels[(int)modeToUpdate] == 0) {
                        //Unlock just level 1 (the first after the intro), if not already unlocked
                        unlockedLevels[(int)modeToUpdate] = 1;
                    }
                }
            }
        }
        else {
            Debug.Log("Didn't unlock new level");
        }

    }

    public int getLevelUnlockedForMode(SystemMode mode) {
        return unlockedLevels[(int)mode];
    }

    public int getHighScoreForLevel(SystemMode mode, int level) {
        return achievedHighScores[(int)mode, level];
    }


}
