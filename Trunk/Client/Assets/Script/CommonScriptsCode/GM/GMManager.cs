using UnityEngine;
using System.Collections;

public class GMManager : Singleton<GMManager>
{

    GMView gmView=new GMView();
    public void GMRender()
    {
        gmView.Render();
    }


}
