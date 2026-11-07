using Godot;
using System;

public partial class Constants : Node
{
	public static Constants Instance { get; private set; }
	public const float button_scale = 1.08f;
	public const float button_time = 0.03f;
	public const int levels_per_stage = 5;
	public struct StageObj
	{
		public int stage;
		public int LevelCap;
		public StageObj()
		{
			stage = 0;
			LevelCap = 5;
		}
		public StageObj(int new_stage)
		{
			stage = new_stage;
			LevelCap = (new_stage+1)*5;
		}
		public StageObj(int new_stage,int level_cap)
		{
			stage = new_stage;
			LevelCap = level_cap;
		}

		public void UpgradeStage()
		{	
			stage++;
			LevelCap = LevelCap +  5;
			
		}
		pub
		
	}
	public override void _Ready()
	{
		Instance = this;
	}
}
