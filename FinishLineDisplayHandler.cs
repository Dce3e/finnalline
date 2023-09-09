using UnityEngine;

public class FinishLineDisplayHandler
{
	private float _distance;
    private float _duration;
    private float _endSpeed;

	public FinishLineDisplayHandler(float distance, float duration, float endSpeed)
	{
        _distance = distance;
        _duration = duration;
        _endSpeed = endSpeed;
    }

    public void Calculate(int tick, ref float speed, float startX, float x)
    {
        if (_duration <= 0f) return;
        if (_endSpeed < 0) return;

        //if (tick == 1)
        //{
        //    _distance = startX + _distance - x;
        //}

        if (x >= _distance)
        {
            speed = 0f;
            return;
        }

        if (tick == 1)
        {
            // use v1 move one tick
            // v1 = start speed
        }
        else if (tick == 2)
        {
            // calculate v2
            // we only have v1
            speed = CalculateV2(speed);
        }
        else if (tick <= 300)
        {
            // calculate v3
            // we only have v2
            // v4 will the same
            speed = CalculateV3(speed, startX, x, startX + _distance, tick);
        }
    }

    private float CalculateV2(float v1)
    {
        // we calculate v1 ~ end 's duration
        float avgSpeed = (v1 + _endSpeed) * 0.5f;
        float duration = _distance / avgSpeed;

        if (duration > _duration || avgSpeed <= 0)
        {
            // cant finish in _duration
            // insert a mid speed

            // ((m + s)/2 + (m + e)/2)/2 * t = d
            // ((m + s)/2 + (m + e)/2)/2 = d / t
            // (m + s)/2 + (m + e)/2) = targetAvgSpeed * 2
            // m + s + m + e = targetAvgSpeed * 4
            // 2m = targetAvgSpeed * 4 - s - e
            // m = (targetAvgSpeed * 4 - s - e) * 0.5
            var targetAvgSpeed = _distance / _duration;
            var midSpeed = (targetAvgSpeed * 4 - v1 - _endSpeed) * 0.5f;
            Debug.Log($"midSpeed {midSpeed}");
            var acceleration = (midSpeed - v1) / (_duration * 0.5f);
            return v1 + acceleration * Time.fixedDeltaTime;
        }
        else
        {
            // can finish
            var acceleration = (_endSpeed - v1) / duration;
            return v1 + acceleration * Time.fixedDeltaTime;
        }
    }

    private float CalculateV3(float v2, float startX, float x, float endX, int t3)
    {
        // we calculate v2 ~ end 's duration
        // endX - x dont have v2 * Time.fixedDeltaTime we add here
        float avgSpeed = (v2 + _endSpeed) * 0.5f;
        float distance = (endX - x + v2 * Time.fixedDeltaTime);
        float duration = distance / avgSpeed;

        // t1 ~ t2 's tick time + v2 ~ end 's duration = tatal time
        // t2 = t3 - 1
        // t1 = 1
        var t = ((t3 - 1) - 1) * Time.fixedDeltaTime;

        if (duration + t > _duration || avgSpeed <= 0)
        {
            // cant finish in _duration
            // insert a mid speed

            // (m + c)/2 * ta + (m + e) / 2 * tb = d
            // m = (2d - c * ta - e * tb) / (ta + tb)
            // calculate by chatgpt

            var ta = _duration * 0.5f - t;
            var tb = _duration * 0.5f;
            var midSpeed = (2 * distance - v2 * ta - _endSpeed * tb) / (ta + tb);

            // _endSpeed > midSpeed
            // cant break limit
            if (ta < Time.fixedDeltaTime)
            {
                return distance / (_duration - t) * 2 - _endSpeed;
            }

            var acceleration = (midSpeed - v2) / ta;
            return v2 + acceleration * Time.fixedDeltaTime;

            // cant calculate v1
            // if v1 = 0, v2 = 1
            // (v1 + v2) / 2 * t = d
            // but d = 0
            // v1 = -1
            // acceleration = 2
            // then it will be more quickly

            //// calculate v1
            //// (v1 + v2) / 2 * t = d
            //// v1 = (d * 2) / t - v2
            //// t = t1 ~ t2 's tick time
            //var v1 = ((x - startX - v2 * Time.fixedDeltaTime) * 2) / t - v2;

            //var acceleration = (v2 - v1) / t;
            //return v2 + acceleration * Time.fixedDeltaTime;
        }
        else
        {
            // can finish
            var acceleration = (_endSpeed - v2) / duration;
            return v2 + acceleration * Time.fixedDeltaTime;
        }
    }
}
