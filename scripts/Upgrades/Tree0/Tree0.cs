using Godot;
using System;
using System.Collections.Generic;
public partial class Tree0 : TextureRect
{
    [Export]
    public PackedScene Apple0 { get; set; }
    int appleamt = 5;
	private bool _isDragging;
    private Vector2 _offset;

    private Timer apple_timer;

    private List<Marker2D> marker_list = new();
    Control Markers;

    public override void _Ready()
	{

        apple_timer = GetNode<Timer>("appletimer");
		apple_timer.Timeout += OnTimeout;
        Apple0 = GD.Load<PackedScene>("res://scenes/Upgrades/Tree/Tree0/apple_0.tscn");
		
        Markers = GetNode<Control>("Markers");
        foreach (Marker2D mark in Markers.GetChildren())
        {
            marker_list.Add(mark);
        }
	}
    
    public void data(int _appleamt){
        appleamt =  _appleamt;
    }

    public override void _GuiInput(InputEvent @event)
    {
        if (@event.IsActionPressed("select") && IsOpaque(GetLocalMousePosition()))
        {
            _isDragging = true;
            _offset = GetGlobalMousePosition() - GlobalPosition;
            AcceptEvent();
        }
        else if (@event.IsActionReleased("select"))
        {
            _isDragging = false;
        }
        else if (@event is InputEventMouseMotion && _isDragging)
        {
            GlobalPosition = GetGlobalMousePosition() - _offset;
			AcceptEvent();
        }
    }

	private bool IsOpaque(Vector2 localPos)
    {
        if (Texture == null) return false;
        
        Image img = Texture.GetImage();
        Vector2 pixelPos = localPos * (Texture.GetSize() / Size);
        
        // Ensure coordinates stay within texture dimensions
        int x = Mathf.Clamp((int)pixelPos.X, 0, img.GetWidth() - 1);
        int y = Mathf.Clamp((int)pixelPos.Y, 0, img.GetHeight() - 1);

        return img.GetPixel(x, y).A > 0.1f;
    }

    private void OnTimeout(){
        SpawnApple(appleamt);
    }

    private void SpawnApple(int val){
        

        var apple = Apple0.Instantiate<Apple0>();
        apple.data(appleamt);

        var marker = marker_list[GD.RandRange(0,marker_list.Count-1)];
        var pos = marker.GlobalPosition;
        
        GetTree().CurrentScene.AddChild(apple);
        
        apple.GlobalPosition = pos;
        
    }
}
