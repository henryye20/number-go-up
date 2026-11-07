using Godot;
using System;
using Godot.Collections;
public partial class ShopButton : Button
{	
	[Export] public string ButtonID { get; set; }
	[Export] public PackedScene spawnable;
	[Export] public int InitialCost {get; set;}

	private TextureRect lock_img;
	private Label cost_text;
	private Label level_text;
	
	public override void _Ready()
	{
		cost_text = GetNode<Label>("Cost");
		level_text = GetNode<Label>("Level");
		lock_img = GetNode<TextureRect>("Lock");

		Pressed += OnPressed;
		UpgradeManager.Instance.EmitSignal(UpgradeManager.SignalName.ButtonAdded,this);
	

	}
	private void OnPressed(){
		
		UpgradeManager.Instance.TryBuy(this);
		
	}


	public void UpdateUI(int level, int cost, bool capped)
	{
		GD.Print(ButtonID.ToString() +" is being updated");
		cost_text.Text = cost.ToString();

		if(capped){
			level_text.Text = "MAX";
			cost_text.Text = "";
		}
		//not unlocked
		else if (level == -1)
		{
			level_text.Text = "";
			lock_img.Visible = true;
		}

		//level up
		else
		{
			if (level == 0) {level_text.Text = "";}
			else {level_text.Text = level.ToString();}
			lock_img.Visible = false;
		}
		
	}
	
	//called in UpgradeManager OnButtonUpgrade
	public void FirstBuy()
	{
		Node spawnedNode = spawnable.Instantiate();
		GetTree().CurrentScene.AddChild(spawnedNode);
		
	}
	
}
