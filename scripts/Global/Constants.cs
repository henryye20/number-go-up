using Godot;
using System;

public partial class Constants : Node
{
	public static Constants Instance { get; private set; }
	public const float button_scale = 1.08f;
	public const float button_time = 0.03f;
	
	public override void _Ready()
	{
		Instance = this;
	}
}
