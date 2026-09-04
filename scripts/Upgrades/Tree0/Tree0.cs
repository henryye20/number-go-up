using Godot;
using System;

public partial class Tree0 : TextureRect
{
	private bool _isDragging;
    private Vector2 _offset;

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
}
