using Godot;
using System;
using Godot.Collections;
public partial class SaveManager : Node
{
    public static SaveManager Instance { get; private set; }
    const string SAVE_DIR = "user://saves/";
	
 

    public override void _Ready()
    {
        Instance = this;
		EventBus.Instance.SaveGame += OnSaveGame;
		EventBus.Instance.LoadGame += OnLoadGame;
    }

	public void OnSaveGame(int slot =0){
		GD.Print("SAVE");
		Save();
	}
	public void OnLoadGame(int slot =0){
		GD.Print("LOAD");
		Load();
	}
    public void Save(int slot = 0)
    {
        // Ensure the save directory exists before writing
        DirAccess.MakeDirAbsolute(SAVE_DIR);

        Dictionary data = GameState.Instance.ToDict();
        string jsonString = Json.Stringify(data, "  ");

        using var file = FileAccess.Open(GetPath(slot), FileAccess.ModeFlags.Write);
        if (file != null)
        {
            file.StoreString(jsonString);
          
            GD.Print("Game successfully saved to slot: " + slot);
        }
        else
        {
            GD.PrintErr("Failed to open save file for writing at: " + GetPath(slot));
        }
    }

    public void Load(int slot = 0)
    {
        string path = GetPath(slot);
        
        if (!FileAccess.FileExists(path))
        {
            GD.Print("No save file found at slot " + slot);
            return;
        }

        using var file = FileAccess.Open(path, FileAccess.ModeFlags.Read);
        if (file != null)
        {
            string jsonString = file.GetAsText();
            var json = new Json();
            var error = json.Parse(jsonString);

            if (error == Error.Ok)
            {
				
                var data = json.Data.AsGodotDictionary();
                GameState.Instance.FromDict(data);
                GD.Print("Game successfully loaded from slot: " + slot);
				EventBus.Instance.TriggerGameLoaded();
            }
            else
            {
                GD.PrintErr("Json Parse Error: ", json.GetErrorMessage(), " in file ", path, " at line ", json.GetErrorLine());
            }
        }
        else
        {
            GD.PrintErr("Failed to open save file for reading at: " + path);
        }
    }

    private string GetPath(int slot)
    {
        return SAVE_DIR + "slot_" + slot + ".json";
    }
}