using System;
using System.IO;
using UnityEngine;

public class FileDataHandler
{
    private string fullPath;

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        fullPath = Path.Combine(dataDirPath, dataFileName);
    }

    public void SaveData(GameData gameData)
    {
        try
        {
            //如果没有该路径，则创建文件保存路径
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            //将游戏数据转换为Json形式
            string dataToSave = JsonUtility.ToJson(gameData, true);
            
            //打开或创建存储文件
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                //写入保存的数据
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToSave);
                }
            }
        }
        catch(Exception e)
        {
            Debug.LogError("存储数据时出现错误" + fullPath + '\n' + e);
        }
    }

    public GameData LoadData()
    {
        GameData loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";

                //读取指定路径下的所有数据
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                //将Json数据转为GameData类型
                loadData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e)
            {
                Debug.LogError("读取数据时出现错误" + fullPath + '\n' + e);
            }
        }

        return loadData;
    }

    public void Delete()
    {
        if(File.Exists(fullPath))
            File.Delete(fullPath);
    }
}
