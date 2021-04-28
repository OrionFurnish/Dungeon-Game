#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;

public class RoomEditor : EditorWindow {
    private const string ROOMPATH = "Assets/Resources/Data";

    private float indent = 20f;
    private string roomName;
    private EntityData[] entities;
    private int roomSize;

    private string[] roomNames;
    private string[] roomPaths;
    private int selectedRoom = 0;
    private string loadedRoom = "";

    [MenuItem("Game Tools/Room Editor")]
    static void Init() {
        RoomEditor window = (RoomEditor)CreateInstance(typeof(RoomEditor));
        window.Show();
    }

    void OnEnable() {
        UpdateRoomList();
    }

    void UpdateRoomList() {
        roomPaths = Directory.GetFiles(ROOMPATH, "*.asset", SearchOption.AllDirectories);
        roomNames = new string[roomPaths.Length];
        for (int i = 0; i < roomPaths.Length; i++) {
            roomPaths[i] = roomPaths[i].Split('.')[0];
            string[] pathParts = roomPaths[i].Split('/');
            roomNames[i] = pathParts[pathParts.Length - 1];
        }
    }

    void OnGUI() {
        SetupGUI();
        RoomGUI();
    }

    void SetupGUI() {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);

        EditorGUILayout.LabelField("Room:", GUILayout.Width(42f));
        selectedRoom = EditorGUILayout.Popup(selectedRoom, roomNames, GUILayout.Width(100f));
        if (roomPaths[selectedRoom] != loadedRoom) { LoadRoom(roomPaths[selectedRoom]); }

        if (GUILayout.Button("New Room", GUILayout.Width(100f))) {
            RoomLayout room = CreateRoomAsset(ROOMPATH + "/New Room");
            // Select new room
            for (int i = 0; i < roomNames.Length; i++) {
                if(roomNames[i].Equals(room.name)) {
                    selectedRoom = i;
                    break;
                }
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    void RoomGUI() {
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);

        EditorGUILayout.LabelField("Room Name:", GUILayout.Width(80f));
        roomName = EditorGUILayout.TextField(roomName, GUILayout.Width(100f));

        EditorGUILayout.LabelField("Room Size:", GUILayout.Width(70f));
        roomSize = EditorGUILayout.IntField(roomSize, GUILayout.Width(70f));

        EditorGUILayout.EndHorizontal();
        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(indent);

        if (GUILayout.Button("Instantiate", GUILayout.Width(100f))) {
            Instantiate(loadedRoom.TrimStart("Assets/Resources/".ToCharArray()));
        }

        if (GUILayout.Button("Save", GUILayout.Width(100f))) {
            SaveRoom(loadedRoom.TrimStart("Assets/Resources/".ToCharArray()));
        }

        EditorGUILayout.EndHorizontal();
    }

    void LoadRoom(string roomName) {
        if (roomName != "") {
            loadedRoom = roomName;
            string path = roomName.TrimStart("Assets/Resources/".ToCharArray());
            RoomLayout room = Resources.Load<RoomLayout>(path);
            if (room != null) {
                this.roomName = room.name;
                this.entities = room.GetCopy();
                this.roomSize = room.size;
            }
        }
    }

    void SaveRoom(string path) {
        RoomLayout room = Resources.Load<RoomLayout>(path);
        if (room != null) {
            UpdateEntities();
            room.entities = entities;
            room.size = roomSize;
            string assetPath = AssetDatabase.GetAssetPath(room);
            AssetDatabase.RenameAsset(assetPath, roomName);
            EditorUtility.SetDirty(room);
            UpdateRoomList();
        }
    }

    void UpdateEntities() {
        Transform level = GameObject.Find("Room Spawn").transform.GetChild(0);
        if(level != null) {
            Entity[] createdEntities = FindObjectsOfType<Entity>();
            if (createdEntities.Length > 0) {
                List<EntityData> entityList = new List<EntityData>();
                foreach (Entity entity in createdEntities) {
                    entity.transform.SetParent(level.GetChild(1));
                    GameObject obj = PrefabUtility.GetCorrespondingObjectFromSource(entity.gameObject);
                    if (obj == null) { obj = entity.prefab; }
                    EntityData data = new EntityData(entity.transform.position, obj);
                    entityList.Add(data);
                }
                entities = entityList.ToArray();
            }
        }
    }

    RoomLayout CreateRoomAsset(string path) {
        RoomLayout asset = CreateInstance<RoomLayout>();
        if (path == ROOMPATH + "/") { path = ROOMPATH + "/New Level"; }
        else if (Path.GetExtension(path) != "") { path = path.Replace(Path.GetFileName(path), ""); }
        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath(path + ".asset");
        AssetDatabase.CreateAsset(asset, assetPathAndName);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        UpdateRoomList();
        return asset;
    }

    void Instantiate(string path) {
        RoomLayout roomLayout = Resources.Load<RoomLayout>(path);
        if(roomLayout != null) {
            Room room = new Room(0, 0);
            room.layout = roomLayout.entities;
            room.size = roomLayout.size;
            RoomLoader roomLoader = GameObject.Find("Room Spawn").GetComponent<RoomLoader>();
            roomLoader.DestroyRoom();
            roomLoader.Load(room);
        }
    }

    void OnDestroy() {
        GameObject.Find("Room Spawn").GetComponent<RoomLoader>().DestroyRoom();
    }
}


#endif