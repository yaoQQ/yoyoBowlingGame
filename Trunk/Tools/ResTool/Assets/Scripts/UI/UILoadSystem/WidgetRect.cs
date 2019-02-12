using UnityEngine;
using System.Collections;

public class WidgetRect  {

	public static WidgetRect zero
	{
		get { return new WidgetRect(); }
	}
    public static WidgetRect full
    {
        get { return new WidgetRect(0,0,Screen.width,Screen.height); }
    }
	
	
	public float left = 0;
	public float top = 0;
	public float right = 0;
	public float bottom = 0;
	public float width=0;

    public float height=0;
	
	
	public WidgetRect()
	{
	}
	public WidgetRect(float l, float t, float w, float h)
	{
		this.left = l;
		this.top = t;
		this.right = l + w;
		this.bottom = t + h;
        width = this.right - this.left;
        height = this.bottom - this.top; 
	}

    public Vector4 GetBorder()
    {
        //LTRB
        return new Vector4(left,top, right,bottom );
    }
	
}
