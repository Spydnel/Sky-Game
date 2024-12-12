using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMovable
{

    public Vector3 DetermineVelocity(bool onGround, Vector3 velocity, Vector3 targetVelocity, float acceleration, bool jumping, float jumpStrength)
    {

        if (onGround)
        {
            velocity = Vector3.MoveTowards(velocity, targetVelocity, acceleration);
        }
        else
        {
            velocity = Vector3.MoveTowards(velocity,
                    new Vector3(targetVelocity.x, velocity.y, targetVelocity.z), acceleration / 2);
        }

        if (jumping && onGround) velocity.y = jumpStrength;

        return velocity;
    }


    void Start()
    {
        
    }
    void Update()
    {
        
    }
}
