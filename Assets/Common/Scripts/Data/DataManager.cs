using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using ZeroFormatter;
using ZeroFormatter.Formatters;


public class DataManager : MonoBehaviour {

    private string UserDataFilePath;
    public UserDataV1 UserData;

    private string PuzzleDataFilePath;
    public PuzzleDataV1 PuzzleData;

    private string ReplayDataFilePath;
    public ReplayDataV1 ReplayData;

    public static DataManager Instance
    {
        get;
        private set;
    }

    void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        Formatter<DefaultResolver, UserDataV1>.Register(new UserDataV1Formatter<DefaultResolver>());
        UserDataFilePath = Path.Combine(Application.persistentDataPath, UserDataV1.USER_DATA_FILE_NAME);
        if (File.Exists(UserDataFilePath))
        {
            UserData = ZeroFormatterSerializer.Deserialize<UserDataV1>(File.ReadAllBytes(UserDataFilePath));
        }
        else
        {
            UserData = new UserDataV1();
        }

        Formatter<DefaultResolver, PuzzleDataV1>.Register(new PuzzleDataV1Formatter<DefaultResolver>());
        PuzzleDataFilePath = Path.Combine(Application.persistentDataPath, PuzzleDataV1.PUZZLE_DATA_FILE_NAME);
        if (File.Exists(PuzzleDataFilePath))
        {
            PuzzleData = ZeroFormatterSerializer.Deserialize<PuzzleDataV1>(File.ReadAllBytes(PuzzleDataFilePath));
        }
        else
        {
            PuzzleData = new PuzzleDataV1();
        }

        Formatter<DefaultResolver, ReplayDataV1>.Register(new ReplayDataV1Formatter<DefaultResolver>());
        ReplayDataFilePath = Path.Combine(Application.persistentDataPath, ReplayDataV1.REPLAY_DATA_FILE_NAME);
        if (File.Exists(ReplayDataFilePath))
        {
            ReplayData = ZeroFormatterSerializer.Deserialize<ReplayDataV1>(File.ReadAllBytes(ReplayDataFilePath));
        }
        else
        {
            ReplayData = new ReplayDataV1();
        }

    }

    public void Write()
    {
        File.WriteAllBytes(UserDataFilePath, ZeroFormatterSerializer.Serialize(UserData));
        File.WriteAllBytes(PuzzleDataFilePath, ZeroFormatterSerializer.Serialize(PuzzleData));
        ReplayData.Version = 0;
        File.WriteAllBytes(ReplayDataFilePath, ZeroFormatterSerializer.Serialize(ReplayData));
    }

    public void WriteReplay()
    {
        File.WriteAllBytes(ReplayDataFilePath, ZeroFormatterSerializer.Serialize(ReplayData));
    }
}
