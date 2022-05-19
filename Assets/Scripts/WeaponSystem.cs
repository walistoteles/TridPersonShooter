using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSystem : MonoBehaviour
{

    Animator anim;
    private bool aim, shot;
    Transform chest;
    public Transform pos;
    public Vector3 Offset;
    public GameObject tridCam, fristcam;
    public GameObject handWeapom, ChestWeapom;


    public GameObject hitPaticle;
    public LayerMask ignoremask;

    void Start()
    {
        anim = GetComponent<Animator>();
        chest = anim.GetBoneTransform(HumanBodyBones.Chest);
    }

    void Update()
    {
        anim.SetBool("Aim", aim);
        anim.SetBool("Firing", shot);
        aim = (Input.GetMouseButton(1)) ? true : false;
        shot = (Input.GetMouseButton(0)) ? true : false;

        WeaponModel();
        FireSystem();
    }

    void FireSystem()
    {
        if (!aim) { return; }
        bool fire = Input.GetMouseButtonDown(0);


        Camera cam = Camera.main;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if(Physics.Raycast(ray, out hit, ignoremask))
        {

            if (fire && hit.distance < 50)
            {
                Debug.Log("Fire");

                var hitparticle = Instantiate(hitPaticle, hit.point, Quaternion.identity);
                Destroy(hitparticle, 0.4f);
            }
        }


    }



    float timmer;
    void WeaponModel()
    {
        ChestWeapom.active = (handWeapom.active == true) ? false : true;

        if (aim)
        {
            handWeapom.active = true;
            timmer = 0;

        }
        else if(aim == false && handWeapom.active == true)
        {
            timmer += Time.deltaTime;

            if (timmer <= 5)
            {
                anim.SetBool("Rifle", true);

            }

            if (timmer > 5)
            {
                anim.SetBool("Rifle", false);
                handWeapom.active = false;
                timmer = 0;

            }
        }
    }


    private void LateUpdate()
    {
        if (aim)
        {
            chest.LookAt(pos.position);
            chest.rotation = chest.rotation * Quaternion.Euler(Offset);
        }
    }


    private void OnAnimatorIK(int layerIndex)
    {
        if (!aim)
        {
            anim.SetLookAtWeight(0.7f);
            anim.SetLookAtPosition(pos.position);
        }

    }


}
