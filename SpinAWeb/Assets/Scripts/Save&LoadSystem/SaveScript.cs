using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Linq.Expressions;
public static class SaveScript
{

    public static void Save(GameManager gameManager)
    { 
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.data";
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerDataScript data = new PlayerDataScript(gameManager);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerDataScript Load(GameManager gameManager)
    {
        string path = Application.persistentDataPath + "/game.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            PlayerDataScript data = formatter.Deserialize(stream) as PlayerDataScript;
            stream.Close();
            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }
}
