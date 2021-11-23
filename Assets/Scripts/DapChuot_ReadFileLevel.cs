using UnityEngine;
using System;

public class DapChuot_ReadFileLevel : MonoBehaviour
{
    public static DapChuot_ReadFileLevel ins;

    public int tableSize;

    [System.Serializable]
    public class Level {
        public float bot1_start;
        public float bot1_end;
        public float bot2_start;
        public float bot2_end;
        public float player_start;
        public float player_end;
        public float bot4_start;
        public float bot4_end;
        public float bot5_start;
        public float bot5_end;
    }

    [System.Serializable]
    public class LevelList
    {
        public Level[] levels;
    }

    public LevelList levelList = new LevelList();
    public TextAsset textAsset;

    private void Awake()
    {
        ins = this;    
    }
    
    void Start()
    {
        ReadCSV();
    }

    private void ReadCSV()
    {
        string[] data = textAsset.text.Split(new string[] {",", "\n"}, StringSplitOptions.None);
        tableSize = data.Length / 10 - 1;
        levelList.levels = new Level[tableSize];

        Debug.Log("Đây là độ dài danh sách: " + tableSize.ToString());

        for(int i = 0; i < tableSize; i++)
        {
            levelList.levels[i] = new Level();

            levelList.levels[i].bot1_start   = float.Parse(data[10 * (i + 1) + 0]);
            levelList.levels[i].bot1_end     = float.Parse(data[10 * (i + 1) + 1]);

            levelList.levels[i].bot2_start   = float.Parse(data[10 * (i + 1) + 2]);
            levelList.levels[i].bot2_end     = float.Parse(data[10 * (i + 1) + 3]);

            levelList.levels[i].player_start = float.Parse(data[10 * (i + 1) + 4]);
            levelList.levels[i].player_end   = float.Parse(data[10 * (i + 1) + 5]);

            levelList.levels[i].bot4_start   = float.Parse(data[10 * (i + 1) + 6]);
            levelList.levels[i].bot4_end     = float.Parse(data[10 * (i + 1) + 7]);

            levelList.levels[i].bot5_start   = float.Parse(data[10 * (i + 1) + 8]);
            levelList.levels[i].bot5_end     = float.Parse(data[10 * (i + 1) + 9]);
        }
    }
}
