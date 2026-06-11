using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class IntroController : MonoBehaviour
{
    public CinemachineCamera favelaCam;
    public CinemachineCamera eyesCam;
    public CinemachineCamera menuCam;

    public Animator camera_eyes;
    public Animator tarjaAnimator;
    public Animator playerAnimator;
    public Animator cm_menu;
    public AudioSource falaPersonagem;

    IEnumerator Start()
    {
        playerAnimator.enabled = false;
        tarjaAnimator.enabled = false;
        camera_eyes.enabled = false;
        cm_menu.enabled = false;

        favelaCam.Priority = 20;

        eyesCam.Priority = 10;
        menuCam.Priority = 10;
        yield return new WaitForSeconds(3f);
        falaPersonagem.Play();

        yield return new WaitForSeconds(1f);

        favelaCam.Priority = 10;
        eyesCam.Priority = 20;
        
        tarjaAnimator.enabled = true;
        tarjaAnimator.Play("Tarja_in");

        camera_eyes.enabled = true;
        camera_eyes.Play("Camera_Eyes");

        

        yield return new WaitForSeconds(2f);

        tarjaAnimator.Play("Tarja_out");
        eyesCam.Priority = 10;
        menuCam.Priority = 20;

        cm_menu.enabled = true;
        cm_menu.Play("camera_menu");
        playerAnimator.enabled = true;
    }
}