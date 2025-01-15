using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollisionDetectorController : MonoBehaviour
{
    public Controle_player player;

    private void OnTriggerEnter2D(Collider2D collision)
    {

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.gameObject.tag == "StickyWall" && player.hasWallJump)//resetar pulo
        {
            if (!player.canJump)
            {
                Vector2 contactPoint = collision.contacts[0].point;
                Vector2 playerPosition = transform.position;

                player.rigidbody.drag = 10;
                player.isGrabbingWall = true;
                player.anim.SetBool("Jumping", false);
                player.anim.SetBool("WallJump", true);

                if (contactPoint.x < playerPosition.x)
                {
                    player.SetWallJumpDirection(Controle_player.WallJumpDirection.Left);
                }
                else
                {
                    player.SetWallJumpDirection(Controle_player.WallJumpDirection.Right);
                }

                player.canWallJump = true;

                player.isDashing = false;

            }
        }

        
        if (collision.gameObject.tag == "Enemy" && player.imortal < 0)
        {
            player.startBlinking = true;
            player.lifes--;
            player.imortal = 1;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "StickyWall" && player.hasWallJump)//resetar pulo
        {
            if (!player.canJump)
            {
                player.canWallJump = false;

                player.rigidbody.drag = 1;
                player.isGrabbingWall = false;
            }
        }
    }


}
