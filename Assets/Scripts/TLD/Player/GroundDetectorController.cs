using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundDetectorController : MonoBehaviour
{
    public Controle_player player;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Shelf")//resetar pulo
        {
            player.anim.SetBool("Jumping", false);
            player.canJump = true;
            player.canDash = true;
            player.isDashing = false;

            if (player.canWallJump)
            {
                player.canWallJump = false;
                player.anim.SetBool("WallJump", false);
                player.rigidbody.drag = 1;
                player.isGrabbingWall = false;
            }
        }
        if (collision.gameObject.tag == "Enemy" && player.imortal < 0)
        {
            player.startBlinking = true;
            player.lifes--;
            player.imortal = 1;
        }
    }
    void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Shelf")//resetar pulo
        {
            player.canJump = true;

            if (player.canWallJump)
            {
                player.canWallJump = false;
                player.anim.SetBool("WallJump", false);
                player.rigidbody.drag = 1;
                player.isGrabbingWall = false;
            }
        }
    }
    async void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground" || collision.gameObject.tag == "Shelf")//tirar pulo
        {
            player.canJump = false;
            player.anim.SetBool("Running", false);
        }
    }
}
