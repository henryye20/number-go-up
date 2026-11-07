using Godot;
using System;
using System.Collections.Generic;
public partial class SceneSwitcher : Node
{
	public static SceneSwitcher Instance { get; private set; }

	private Node CurrentScene;
	private string CurrentPath;
	private Dictionary<string,Node> PaintingScenes = new Dictionary<string,Node>();

	public override void _Ready()
	{
		Instance = this;
		//root is above all and includes children of the Global scripts as well
		Window root = GetTree().Root;
		//Autoloads/Globals are always loaded before the main scene, so the last scene in the childlist is always your main scene
		CurrentScene = root.GetChild(root.GetChildCount() - 1);
		CurrentPath = CurrentScene.SceneFilePath;
		EventBus.Instance.First3DTransition += OnFirst3DTransition;
	}
	public void SwitchScene(string path,bool keepAlive=false)
    {
        CallDeferred(MethodName.PerformSwitch, path,keepAlive);
    }
	private void PerformSwitch(string path,bool keepAlive)
    {
        Window root = GetTree().Root;
		root.RemoveChild(CurrentScene);
		PaintingScenes[CurrentPath]= CurrentScene;
		
	
		
		if(keepAlive){PaintingScenes[CurrentPath] = CurrentScene;}
		else{CurrentScene.QueueFree();}

		Node next;
		if (PaintingScenes.TryGetValue(path, out Node scene))
		{
			next = scene;
			PaintingScenes.Remove(path);
		}
		else
		{
			var packed = GD.Load<PackedScene>(path);
			

			next = packed.Instantiate();
		}

        root.AddChild(next);
		GetTree().CurrentScene = next;
		CurrentScene = next;
		CurrentPath = path;
    }
	private void OnFirst3DTransition(){
		SwitchScene("res://scenes/3D/Gallery.tscn",true);
	}
	
}
