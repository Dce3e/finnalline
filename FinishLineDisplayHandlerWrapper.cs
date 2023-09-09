using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineDisplayHandlerWrapper
{
    FinishLineDisplayHandler x = new FinishLineDisplayHandler(10f, 1f, 6f);
    FinishLineDisplayHandler y = new FinishLineDisplayHandler(0f, 1f, 0f);

    public void Calculate(int tick, ref Vector2 speed, Vector2 start, Vector2 pos)
    {
        x.Calculate(tick, ref speed.x, start.x, pos.x);

        float speedY = -speed.y;
        float posY = -pos.y;

        y.Calculate(tick, ref speedY, start.y, posY);

        speed.y = -speedY;
        pos.y = -posY;
    }
}
