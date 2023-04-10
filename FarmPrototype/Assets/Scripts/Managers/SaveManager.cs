using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using VM.TerrainTools;
using Newtonsoft.Json;
using VM.Inventory;
using VM.Player;
using VM.SceneTools;
using VM.Managers;
using VM.Managers.Save;
using VM.UI;
using VM.Building;
using VM.Managers.Path;
using VM.Managers.Robots;
using System.Threading.Tasks;

namespace VM.Save
{
    public class SerVector
    {
        public float x;
        public float y;
        public float z;
     
        public SerVector(Vector3 vector3)
        {
            this.x = vector3.x;
            this.y = vector3.y;
            this.z = vector3.z;
        }
    }

    public class UnityVector3
    {
        public Vector3 vector;

        public UnityVector3 (SerVector serVector)
        {
            this.vector = new Vector3(serVector.x, serVector.y, serVector.z);
        }
    }

    public class SerQuaternion
    {
        public float x;
        public float y;
        public float z;
        public float w;

        public SerQuaternion (Quaternion rotation)
        {
            this.x = rotation.x;
            this.y = rotation.y;
            this.z = rotation.z;
            this.w = rotation.w;
        }
    }

    public class UnityQuaternion
    {
        public Quaternion quaternion;

        public UnityQuaternion(SerQuaternion rotation)
        {
            this.quaternion = new Quaternion(rotation.x, rotation.y, rotation.z, rotation.w);
        }
    }

    public class RobotUnitSaveData
    {
        public int robotId;
        public bool inited;
        public SerVector position;
        public SerQuaternion rotation;
        public InventoryManagerSaveData inventoryManager;
    }

    public class PathNodeSaveData
    {
        public int itemId;
        public PathType type;
        public SerVector position;
        public SerVector[] connections;
        public SerVector[] storageConnections;
    }

    public class PathSaveData
    {
        public PathNodeSaveData[] pathNodes;
    }

    public class SeedlingSaveData
    {
        public SerVector position;
        public SerQuaternion rotation;
        public int itemId;
        public float growthTime;
        public float amount;
    }

    public class InventoryBuildingSaveData
    {
        public SerQuaternion rotation;
        public SerVector position;
        public int itemId;
    }

    public class InventoryItemSaveData
    {
        public SerQuaternion rotation;
        public float amount;
        public int itemId;
        public SerVector position;
        public InventoryManagerSaveData inventory;
    }

    public class InventoryManagerSaveData
    {
        public int managerId;
        public Dictionary<int, InventoryItemSaveData> inventory;
        public SerVector position;
        public SerQuaternion rotation;
    }

    public class PlayerSaveData
    {
        public SerVector position;
        public SerQuaternion rotation;
        public InventoryManagerSaveData pockets;
        public InventoryManagerSaveData hands;
        public float money;
    }

    public class TerrainSaveData
    {
        public float[,] heights;
        public float[,,] colors;
        public Dictionary<int, int[,]> details;
    }

    public class SaveData
    {
        public string geniralSaveFilePath;
        public string shortInfoSaveFilePath;
        public string screenShotSaveFilePath;
    }

    public class SaveManager : MonoBehaviour
    {
        public static SaveManager Instance;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            if (SceneController.loadFile != "null")
            {
                this.Load(SceneController.loadFile);
            }
        }

        public void Save ()
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("terrain", this._SaveData(TerrainManager.Instance));
            data.Add("player", this._SaveData(PlayerManager.Instance));
            //data.Add("storages", this._SaveData(WIPInventoryStoragesManager.Instance));
            data.Add("items", this._SaveData(InventoryItemsManager.Instance));
            data.Add("buildings", this._SaveData(BuildingManager.instance));
            data.Add("seedlings", this._SaveData(SeedlingsManager.instance));
            data.Add("pathNodes", this._SaveData(PathManager.instance));
            data.Add("robots", this._SaveData(RobotManager.instance));

            string fileName = DateTime.Now.ToString("MM_dd_yyyy-HH_mm_ss");
            string saveDir = DirectoryManager.instance.saveDir;
            string shortDir = DirectoryManager.instance.shortSaveDir;
            string screenDir = DirectoryManager.instance.screenShotsDir;

            Dictionary<string, string> shortInfo = new Dictionary<string, string>();
            shortInfo.Add("date", fileName);
            shortInfo.Add("name", fileName);
            shortInfo.Add("customName", fileName + "!");

            Debug.Log($"{ saveDir}/{ fileName}.txt");

            DirectoryManager.instance.WriteFile(
                $"{saveDir}/{fileName}.txt", 
                JsonConvert.SerializeObject(data)
            );

            DirectoryManager.instance.WriteFile(
                $"{shortDir}/{fileName}.txt",
                JsonConvert.SerializeObject(shortInfo)
            );

            DirectoryManager.instance.CreateDir(screenDir);
            MenuController.instance.Hide();
            ScreenShotManager.Make($"{screenDir}/{fileName}.png");
        }

        public async void Load(string fileName)
        {
            string filePath = DirectoryManager.instance.saveDir + $"/{fileName}";
            string save = DirectoryManager.instance.ReadFile(filePath);

            if (save != "")
            {
                Dictionary<string, string> data = JsonConvert.DeserializeObject<Dictionary<string, string>>(save);

                //WIPInventoryStoragesManager.Instance.FullReset();
                InventoryItemsManager.Instance.FullReset();
                BuildingManager.instance.FullReset();
                SeedlingsManager.instance.FullReset();
                RobotManager.instance.FullReset();

                await Task.Delay(100);
                this._LoadData(TerrainManager.Instance, data["terrain"]);
                await Task.Delay(100);
                this._LoadData(PlayerManager.Instance, data["player"]);
                await Task.Delay(100);
                //this._LoadData(WIPInventoryStoragesManager.Instance, data["storages"]);
                this._LoadData(InventoryItemsManager.Instance, data["items"]);
                await Task.Delay(100);
                this._LoadData(BuildingManager.instance, data["buildings"]);
                await Task.Delay(100);
                this._LoadData(SeedlingsManager.instance, data["seedlings"]);
                await Task.Delay(100);
                this._LoadData(PathManager.instance, data["pathNodes"]);
                await Task.Delay(100);
                this._LoadData(RobotManager.instance, data["robots"]);
            }
        }

        public List<SaveData> GetSaves ()
        {
            List<SaveData> saves = new List<SaveData>();
            List<string> saveNames = DirectoryManager.instance.GetFilesNameFromDir(DirectoryManager.instance.saveDir);

            saveNames.ForEach((save) =>
            {
                string saveName = save.Split(".")[0];
                string geniralPath = DirectoryManager.instance.saveDir + $"{save}";
                string shortFilePath = DirectoryManager.instance.shortSaveDir + $"/{save}";
                string screenPath = DirectoryManager.instance.screenShotsDir + $"/{saveName}.png";

                if (File.Exists(shortFilePath) && File.Exists(screenPath))
                {
                    SaveData data = new SaveData()
                    {
                        geniralSaveFilePath = geniralPath,
                        shortInfoSaveFilePath = shortFilePath,
                        screenShotSaveFilePath = screenPath
                    };

                    saves.Add(data);
                }
            });

            return saves;
        }

        private string _SaveData (ObjectToSave objectToSave)
        {
            return objectToSave.GetSaveData();
        }

        private void _LoadData (ObjectToSave objectToSave, string data)
        {
            objectToSave.LoadSaveData(data);
        }
    }
}