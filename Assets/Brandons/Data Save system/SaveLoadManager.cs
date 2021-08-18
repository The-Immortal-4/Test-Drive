using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public static class SaveLoadManager
{
    public static void SavePlayer(Player player)
    {
        BinaryFormatter bf = new BinaryFormatter();
        FileStream stream = new FileStream(Application.persistentDataPath + "/[player.sav", FileMode.Create);

        PlayerData data = new PlayerData(player);

        bf.Serialize(stream, data);
        stream.Close();

    }

  public static int[] LoadPlayer()
  {
    if (File.Exists(Application.persistentDataPath + "/player.sav"))
    {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream stream = new FileStream(Application.persistentDataPath + "/[player.sav", FileMode.Open);

            PlayerData data = bf.Deserialize(stream) as PlayerData;

            stream.Close();
            return data.stats;

    }
    else
    {
            Debug.LogError("File does not exist");
            return new int[4];
     }
  }

  [Serializable]
  public class PlayerData
    {
        public int[] stats; 

        public PlayerData(Player player)
        {
            stats = new int[4];
            stats[0] = player.level;
            stats[1] = player.health;
            stats[2] = player.attack;
            stats[3] = player.defense;
        }
    }
}
