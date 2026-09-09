using Godot;
using System;

[GlobalClass]
public partial class Item : Resource
{
    [Export] public string ItemName { get; set; } = "Default Item";
    [Export] public int BaseCost { get; set; } = 100;
    [Export] public Texture2D Icon { get; set; }
}
