using Godot;
using System;

public partial class Constants : Node
{
	public static Constants Instance { get; private set; }
	public override void _Ready()
	{
		Instance = this;
	}
}
