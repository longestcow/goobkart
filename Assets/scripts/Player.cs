using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class Player : MonoBehaviour
{
    [Header("Kart Stats")]
    public float spe = 10; //forward speed (10 is default)
    public float eps = -5; //reverse speed (-5 is default)
    public float rotspeed = 20; //speed of turning (20 is default)
    public float weight = 1; //weight of kart (1 is default)
    public float stylemultiplier = 1; //amount of style points earnt per style thing (not implemented yet)
    public float boostmultiplier = 1; //the amount of boost the kart gets from boosting on boost panels (not implemented yet)

    [Header("Crayon Stats")]
    public float shootspeed;
    public float shootstrengthchargeupspeed;
    public float deliverycamoffset;



    [Header("Everything else")]
    public GameObject mesh;
    public GameObject cycle;
    public GameObject parentobject;
    public GameObject fakecam; //camera that moves based on velocity
    public GameObject cam; //camera that lerps to movement based on velocity
    public GameObject maincamera;
    public GameObject campos1;
    public GameObject campos2;
    public GameObject campos3;
    public GameObject camposdelivery;
    public GameObject mousecam; //camera that moves based on mouse movement
    public LayerMask groundLayer;
    public Transform wheel1;
    public Transform wheel2;
    public Transform frontthing;
    public Transform cyclemain;
    public Transform LLegHandlePos;
    public Transform LLegHintHandlePos;
    public Transform LLegDriftPos;
    public Transform LLegHintDriftPos;
    public Transform LeftLegPos;
    public Transform LeftHintPos;
    public Transform RLegHandlePos;
    public Transform RLegHintHandlePos;
    public Transform RLegDriftPos;
    public Transform RLegHintDriftPos;
    public Transform RightLegPos;
    public Transform RightHintPos;
    public GameObject crosshair;
    public GameObject delivery;
    public ProceduralGeneration proceduralgen;
    public Scrollbar strengthscrollbar;
    public DeliverySystem deliverysys;
    public Text DeliveryQueueText;
    public LineRenderer liner;
    public Transform linepoint0;
    public Transform linepoint1;
    public GameObject linelengthcanvas;
    public Text linelengthtext;
    public Text starstext;
    public TextMeshProUGUI scoretext;
    public TextMeshProUGUI multipliertext;
    public Image[] starsobjs;
    public AudioSource successjingle;
    public AudioSource failurejingle;
    public GameObject NotificationTemplate;
    public Transform notifrestspot;
    public Transform notifpushedspot;
    public Transform notifhiddenspot;
    public Transform notifdiespot;
    public Material linem;
    public Transform minimapcamhinge;
    public GameObject newscoreaddedtemplate;
    public List<ScoreThing> newscoreaddings = new List<ScoreThing>();
    public Transform canvastrans;
	public Transform everythinginsidecnavas;
    Vector3 canvastransogpositoin;
    public Transform scorenewposition;
    public Transform scoresecondposition;
    public Transform scorehiddenposition;
    public Transform strengthbar;
    public Transform strengthbarmask;
    public Transform strengthbarempty;
    public Transform strengthbaremptypos;
    public Transform strengthbarfilledpos;
    public GameObject turnbacknow;
    public Text ReturningText;
    public LayerMask roadandfootpath;
    public ParticleSystem speedlines;
    public TrailRenderer trail1;
    public TrailRenderer trail2;
    public ParticleSystem particles1;
    public ParticleSystem particles2;
    public AudioSource skid;
    public AudioSource windres;
    public AudioSource wheelsound;
    public AudioSource tapesfx;
    public GameObject pausemenu;
    public Slider sfx;
    public Slider music;
    public Slider sens;
    public audiovolumecontrol[] refreshthese;
    public GameObject gameovermenu;
    public Text gameovertextleft;
    public Text gameovertextright;
    public Text gameovertextscoredisplay;
    public AudioSource CRAYON;
    public Text controlstext1;
    public Text controlstext2;
    public Text controlstext3;
    public Image controlimage1;
    public Image controlimage2;
    public Image controlimage3;
    public MoveEverything EverythingMover;
    public Transform spawnedparent;

    public static int score;
    public float difficulty;
    int stars;
    int multiplier;

    SphereCollider coll;
    Rigidbody rb;
    float input = 0f;
    float rotinput = 0f;
    float driftdir;
    bool driftstart;
    bool drifting;
    Quaternion rot;
    bool isgrounded;
    bool aimmode;
    bool aimodebuffer;
    float shootstrength;
    bool shootstrengthincreasing;
    public static int deliverycam;
    public AudioSource hitsfx;

    [NonSerialized]
    public int latestindex;
    public GeneratedRoad latestroad;
    GameObject latestdelivery;
    Rigidbody latestdeliveryrb;
    Vector3 savedvelocity;
    float lastdeliverytime;
    float lastdeliverytimelimit;
    public struct DeliveryRequest
    {
        public GeneratedRoad road;
        public GameObject house;
        public float starttime;
        public int timelimit;
        public int distlimit;
        public DeliveryNotification notif;
    }
    public DeliveryRequest[] deliveriesqueue = new DeliveryRequest[2];
    public int currentdelivery;
    public GameObject currentdeliveryhouse;
    public DeliveryRequest currentdeliveryreq;
    float deliverycamstarttime;
    int queueddeliveries;
    bool lastwasmidair;
    bool lastwassniped;
    float aspectratio;
    float tanfov;
    bool pausenewdeliveries;
    bool returningtimerstarted;
    bool paused;
    bool gameover;
    bool tutorial;

    void Start()
    {
        tutorial = false;
        stars = 5;
        multiplier = 1;
        pausenewdeliveries = false;
        //difficulty = 1;
        score = 0;
        liner.positionCount = 2;
        liner.enabled = false;
        coll = GetComponent<SphereCollider>();
        rb = GetComponent<Rigidbody>();
        mesh.transform.parent = parentobject.transform;
        rb.mass = weight;
        rb.mass = weight;
        for (int i = 0; i < deliveriesqueue.Length; i++)
        {
            deliveriesqueue[i].road = null;
            deliveriesqueue[i].house = null;
        }
        deliverycam = 0;
        currentdelivery = 0;
        queueddeliveries = 0;
        paused = false;
        gameover = false;

        canvastransogpositoin = everythinginsidecnavas.position;
        aspectratio = Screen.width / Screen.height;
        tanfov = Mathf.Tan(Mathf.Deg2Rad *  maincamera.GetComponent<Camera>().fieldOfView/2f);

        if (PlayerPrefs.GetInt("highscore") < 1)
        {
            StartCoroutine(explaincontrols());
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }
    void FixedUpdate()
    {
        if (paused ||gameover)
        {
            return;
        }
        if (deliverycam != 0)
        {
            rb.velocity = Vector3.zero;
            return;
        }
        rb.AddForce(mesh.transform.forward * input, ForceMode.Force);
        wheel2.localRotation = Quaternion.Euler(0,0,wheel2.localRotation.eulerAngles.z - rb.velocity.magnitude);
        wheel1.localRotation = Quaternion.Euler(0,0,wheel1.localRotation.eulerAngles.z - rb.velocity.magnitude);
        frontthing.transform.localRotation = Quaternion.Lerp(frontthing.transform.localRotation,Quaternion.Euler(0,rotinput*100,0),0.1f * 25 / 50);
        if (Vector3.Dot(rb.velocity,mesh.transform.forward) > 0.1f)
        {
            cyclemain.localRotation = Quaternion.Euler(0.3f * ((Vector3.SignedAngle(mesh.transform.forward, rb.velocity, mesh.transform.up))), 270, 0);
        }
        if ((Vector3.SignedAngle(mesh.transform.forward, rb.velocity, mesh.transform.up)) > 60 && isgrounded)
        {
            LeftLegPos.position = Vector3.Lerp(LeftLegPos.position, LLegDriftPos.position, 0.1f);
            LeftLegPos.rotation = Quaternion.Lerp(LeftLegPos.rotation, LLegDriftPos.rotation, 0.1f);
            LeftHintPos.rotation = Quaternion.Lerp(LeftHintPos.rotation, LLegHintDriftPos.rotation, 0.1f);
            LeftHintPos.position = Vector3.Lerp(LeftHintPos.position, LLegHintDriftPos.position, 0.1f);


            RightLegPos.position = Vector3.Lerp(RightLegPos.position, RLegHandlePos.position, 0.1f);
            RightLegPos.rotation = Quaternion.Lerp(RightLegPos.rotation, RLegHandlePos.rotation, 0.1f);
            RightHintPos.rotation = Quaternion.Lerp(RightHintPos.rotation, RLegHintHandlePos.rotation, 0.1f);
            RightHintPos.position = Vector3.Lerp(RightHintPos.position, RLegHintHandlePos.position, 0.1f);
        }
        else if (Vector3.SignedAngle(mesh.transform.forward, rb.velocity, mesh.transform.up) < -60 && isgrounded)
        {
            LeftLegPos.position = Vector3.Lerp(LeftLegPos.position, LLegHandlePos.position, 0.1f);
            LeftLegPos.rotation = Quaternion.Lerp(LeftLegPos.rotation, LLegHandlePos.rotation, 0.1f);
            LeftHintPos.rotation = Quaternion.Lerp(LeftHintPos.rotation, LLegHintHandlePos.rotation, 0.1f);
            LeftHintPos.position = Vector3.Lerp(LeftHintPos.position, LLegHintHandlePos.position, 0.1f);


            RightLegPos.position = Vector3.Lerp(RightLegPos.position, RLegDriftPos.position, 0.1f);
            RightLegPos.rotation = Quaternion.Lerp(RightLegPos.rotation, RLegDriftPos.rotation, 0.1f);
            RightHintPos.rotation = Quaternion.Lerp(RightHintPos.rotation, RLegHintDriftPos.rotation, 0.1f);
            RightHintPos.position = Vector3.Lerp(RightHintPos.position, RLegHintDriftPos.position, 0.1f);
        }
        else
        {
            LeftLegPos.position = Vector3.Lerp(LeftLegPos.position, LLegHandlePos.position, 0.1f);
            LeftLegPos.rotation = Quaternion.Lerp(LeftLegPos.rotation, LLegHandlePos.rotation, 0.1f);
            LeftHintPos.rotation = Quaternion.Lerp(LeftHintPos.rotation, LLegHintHandlePos.rotation, 0.1f);
            LeftHintPos.position = Vector3.Lerp(LeftHintPos.position, LLegHintHandlePos.position, 0.1f);


            RightLegPos.position = Vector3.Lerp(RightLegPos.position, RLegHandlePos.position, 0.1f);
            RightLegPos.rotation = Quaternion.Lerp(RightLegPos.rotation, RLegHandlePos.rotation, 0.1f);
            RightHintPos.rotation = Quaternion.Lerp(RightHintPos.rotation, RLegHintHandlePos.rotation, 0.1f);
            RightHintPos.position = Vector3.Lerp(RightHintPos.position, RLegHintHandlePos.position, 0.1f);
        }

        difficulty = Mathf.Clamp(difficulty + 0.0001f,1,3);
        spe = difficulty * 10;
 
        if (drifting)
        {
            if (!Physics.Raycast(mesh.transform.position, -mesh.transform.up, 1, groundLayer))
            {
                trail1.emitting = false;
                trail2.emitting = false;
            }
            else
            { 
                trail1.emitting = true;
                trail2.emitting = true;
                skid.UnPause();
            }
        }

        if (input > 0)
        {
            var p1e = particles1.emission;
            var p2e = particles2.emission;
            p1e.enabled = true;
            p2e.enabled = true;
        }
        else
        {

            var p1e = particles1.emission;
            var p2e = particles2.emission;
            p1e.enabled = false;
            p2e.enabled = false;
        }
        //Softlock Checks
        bool onroad = Physics.Raycast(mesh.transform.position, -mesh.transform.up, 10 ,roadandfootpath);
        if ((!onroad || rb.velocity.magnitude < 0.01f) && !returningtimerstarted)
        {
            StartCoroutine(returningtoground());
        }
        if (rb.velocity.magnitude > 0.5f && onroad)
        {
            returningtimerstarted = false;
            StopCoroutine(returningtoground());
        }
    }

    private void LateUpdate()
    {
        if(paused || gameover)
        {
            return;
        }

        cam.transform.position = mesh.transform.position;
        fakecam.transform.position = mesh.transform.position;
        //mousecam.transform.position = mesh.transform.position;
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.LookRotation(rb.velocity);// + new Vector3(0, input * 20f, 0);
        if (rb.velocity.magnitude > 0.1f) fakecam.transform.rotation = Quaternion.Euler(0, fakecam.transform.rotation.eulerAngles.y + driftdir * 45, 0);// + new Vector3(0, input * 20f, 0);
        cam.transform.rotation = Quaternion.Euler( Quaternion.Lerp(cam.transform.rotation, fakecam.transform.rotation, 0.05f).eulerAngles + (!aimmode && (deliverycam == 0)?new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0): Vector3.zero));
        //mousecam.transform.rotation = Quaternion.Euler(mousecam.transform.rotation.eulerAngles + new Vector3(0, Input.GetAxis("Mouse X"), 0));
        if (latestroad != null) minimapcamhinge.rotation = Quaternion.Lerp(minimapcamhinge.rotation, latestroad.transform.rotation, 0.05f * 250 * Time.deltaTime);
        minimapcamhinge.position = mesh.transform.position;
        if (!aimmode && deliverycam == 0)
        {
            //maincamera.transform.position += new Vector3(0, UnityEngine.Random.Range(-0.0002f, 0.0002f) * rb.velocity.magnitude, 0);
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, campos1.transform.position, 0.05f * 250 * Time.deltaTime);
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, campos1.transform.rotation, 0.05f * 250 * Time.deltaTime);
            campos1.transform.localRotation = Quaternion.Lerp(campos1.transform.localRotation, Quaternion.Euler(new Vector3(33.27f, 0, (drifting?2f:0) * (rb.velocity.magnitude/5) * rotinput )), 0.1f * 25 * Time.deltaTime);
            maincamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(maincamera.GetComponent<Camera>().fieldOfView, 80 + rb.velocity.magnitude * 2.5f, 0.05f * 250 * Time.deltaTime);
            everythinginsidecnavas.rotation = Quaternion.Lerp(everythinginsidecnavas.rotation,Quaternion.Euler(new Vector3(0, 0, -5 * (rb.velocity.magnitude / 25)  * rotinput)), 0.1f * 25 * Time.deltaTime );
            everythinginsidecnavas.localScale = (1 - 0.002f * rb.velocity.magnitude) * new Vector3(1,1,1);
            var speedlinesmain = speedlines.main;
            var speedlinesemission = speedlines.emission;
            if (rb.velocity.magnitude >= 10)
            {
                speedlinesmain.startSpeed = rb.velocity.magnitude / 2;
                speedlinesemission.rateOverTime = rb.velocity.magnitude * 5;
            }
            else
            {
                speedlinesemission.rateOverTime = 0;
            }
            windres.volume = MainMenu.soundvolume * rb.velocity.magnitude / 30f;
            wheelsound.volume = MainMenu.soundvolume * rb.velocity.magnitude / 90f;
        }
        else if (deliverycam == 0)
        {
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, (campos2.transform.localRotation.eulerAngles.y > 180 ? campos2 : campos3).transform.position, 1 - Mathf.Exp(0.69315f * (-Time.deltaTime / 0.01f)));
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, (campos2.transform.localRotation.eulerAngles.y > 180 ? campos2 : campos3).transform.rotation, 1 - Mathf.Exp(0.69315f * (-Time.deltaTime / 0.01f)));
            campos2.transform.localRotation = Quaternion.Euler(campos2.transform.localRotation.eulerAngles + (MainMenu.sensitivity + 0.1f) * 2 * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
            campos3.transform.localRotation = Quaternion.Euler(campos2.transform.localRotation.eulerAngles + (MainMenu.sensitivity + 0.1f) * 2 * new Vector3(-Input.GetAxis("Mouse Y"), Input.GetAxis("Mouse X"), 0));
            speedlines.gameObject.SetActive(false);
            maincamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(maincamera.GetComponent<Camera>().fieldOfView, 100, 0.05f * 250 * Time.deltaTime);
            windres.volume = 0;
            wheelsound.volume = 0;
        }
        else 
        {
            if (deliverycam == 1)
            {
                camposdelivery.transform.position = latestdelivery.transform.position + (transform.position - latestdelivery.transform.position).normalized * deliverycamoffset;
                camposdelivery.transform.LookAt(latestdelivery.transform.position);

                if (latestdeliveryrb.velocity.magnitude < 0.1f)
                {
                    StartCoroutine("resultcam");
                    StopCoroutine("deliverycamtimeout");
                }
            }
            if (deliverycam == 2)
            {
                float offset = ((currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position - latestdelivery.transform.position).magnitude / 2f / aspectratio) / tanfov;
                Vector3 midpoint = (currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position + latestdelivery.transform.position) / 2f;
                camposdelivery.transform.position =  midpoint + (transform.position - latestdelivery.transform.position).normalized * 2 * (offset + deliverycamoffset);
                camposdelivery.transform.position = new Vector3(camposdelivery.transform.position.x, latestdelivery.transform.position.y + 1, camposdelivery.transform.position.z);
                camposdelivery.transform.LookAt(latestdelivery.transform.position);
                linepoint0.position = Vector3.Lerp(linepoint0.position, latestdelivery.transform.position, Time.deltaTime * 10 * difficulty);
                linepoint1.position = Vector3.Lerp(linepoint1.position, currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position, Time.deltaTime * 10 * difficulty);
                liner.SetPosition(0, linepoint0.transform.position);
                liner.SetPosition(1, linepoint1.transform.position);
                linelengthcanvas.transform.position = ((currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position + latestdelivery.transform.position) / 2f) + Vector3.up;
                linelengthcanvas.transform.LookAt(camposdelivery.transform.position);
                linelengthtext.text = ((linepoint0.position - linepoint1.position).magnitude * 10).ToString("F1");
                if (float.Parse(linelengthtext.text) > 50)
                {
                    linem.color = Color.red;
                    linelengthtext.color = Color.red;
                }
                else
                {
                    linem.color = Color.black;
                    linelengthtext.color = Color.black;
                }

            }
            maincamera.transform.position = Vector3.Lerp(maincamera.transform.position, camposdelivery.transform.position, 0.05f * 250 * Time.deltaTime);
            maincamera.transform.rotation = Quaternion.Lerp(maincamera.transform.rotation, camposdelivery.transform.rotation, 0.05f * 250 * Time.deltaTime);
            maincamera.GetComponent<Camera>().fieldOfView = Mathf.Lerp(maincamera.GetComponent<Camera>().fieldOfView, 100, 0.05f * 250 * Time.deltaTime);
        }
    }

    private void Update()
    {

        //if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);  //Restart code

        if (gameover) return;

        if (Input.GetKeyDown(KeyCode.Escape) && !paused)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && paused)
        {
            UnPauseGame();
        }

        if (paused)
        {
            return;
        }

        if (deliverycam != 0)
        {
            return;
        }

        AlignKart();


        if (Input.GetButton("Drift") && !drifting && isgrounded && rb.velocity.magnitude > 0.5f)
        {
            driftstart = true;
        }
        if (Input.GetButtonDown("Drift") && !isgrounded)
        {
            Trick();
        }
        if (driftstart)
        {
            if (Input.GetAxisRaw("Horizontal") > 0.1f)
            {
                driftdir = 1;
                driftstart = false;
                drifting = true;
            }
            if (Input.GetAxisRaw("Horizontal") < -0.1f)
            {
                driftdir = -1;
                driftstart = false;
                drifting = true;
            }
            
        }
        if (!Input.GetButton("Drift"))
        {
            driftdir = 0;
            drifting = false;
            driftstart = false;
            trail1.emitting = false;
            trail2.emitting = false;
            skid.Pause();
        }

        rotinput = (driftdir + Input.GetAxisRaw("Horizontal"))/2f;
        if (!isgrounded) rotinput *= 0.3f;

        mesh.transform.Rotate(0, rotinput * Time.deltaTime * rotspeed * rb.velocity.magnitude ,0, Space.World);


        mesh.transform.position = this.transform.position;

        if (Input.GetButton("Fire1"))
        {
            input = spe;
        }
        else if (Input.GetButton("Fire2"))
        {
            input = eps;
        }
        else
        {
            input = 0;
        }


        //Aim Mode 
        if (Input.GetButtonDown("Aim"))
        {
            Time.timeScale = 0.1f;
            aimmode = true;
            speedlines.gameObject.SetActive(false);
            bool leftside = true;
            if (currentdelivery != 0)
            {
                leftside = !(Vector3.SignedAngle(mesh.transform.forward, currentdeliveryhouse.transform.position - transform.position, Vector3.up) > 0);
            }
            campos2.transform.localRotation = Quaternion.Euler(0, leftside ? -90 : 90, 0);
            campos3.transform.localRotation = Quaternion.Euler(0, leftside ? 90 : -90, 0);
            //campos2.transform.LookAt(currentdeliveryhouse.transform.GetChild(0).position + Vector3.up * 2);
            crosshair.SetActive(true);
        }
        if (Input.GetButtonUp("Aim"))
        {
            aimmode = false;
            speedlines.gameObject.SetActive(true);
            crosshair.SetActive(false);
            Time.timeScale = 1;
            shootstrengthincreasing = false;
            strengthscrollbar.gameObject.SetActive(false);
            strengthbarempty.gameObject.SetActive(false);
        }

        if (aimmode && Input.GetButtonDown("Shoot"))
        {
            shootstrengthincreasing = true;
            shootstrength = 1;
            strengthscrollbar.gameObject.SetActive(true);
            strengthbarempty.gameObject.SetActive(true);
        }
        if (aimmode && shootstrengthincreasing)
        {
            shootstrength = Mathf.Clamp(shootstrength+(shootstrengthchargeupspeed * Time.deltaTime),1,20);
            strengthscrollbar.size = (shootstrength-1)/19;
            strengthbarmask.transform.position = ((strengthbaremptypos.position * (20 - shootstrength)) + (strengthbarfilledpos.position * shootstrength)) / 20f;
            strengthbar.transform.position = strengthbarempty.transform.position;
        }
        if (aimmode && shootstrengthincreasing && Input.GetButtonUp("Shoot"))
        {
            shootstrengthincreasing = false;
            latestdelivery = Instantiate(delivery, mesh.transform.position, Quaternion.Euler(Vector3.zero), spawnedparent);
            latestdeliveryrb = latestdelivery.GetComponent<Rigidbody>();
            latestdeliveryrb.velocity = maincamera.transform.forward * shootspeed * shootstrength;
            aimmode = false;
            deliverycam = 1;
            skid.Pause();
            deliverycamstarttime = Time.time;
            StartCoroutine("deliverycamtimeout");
            crosshair.SetActive(false);
            savedvelocity = rb.velocity;
            Time.timeScale = 1;
            lastdeliverytime = Time.time - currentdeliveryreq.starttime;
            lastdeliverytimelimit = currentdeliveryreq.timelimit;
            strengthscrollbar.gameObject.SetActive(false);
            strengthbarempty.gameObject.SetActive(false);
        }
        DeliveryQueueText.text = "";
        for (int i = 0; i < deliveriesqueue.Length; i++)
        {
            if (deliveriesqueue[i].road == null) break;
            if (Time.time - deliveriesqueue[i].starttime > deliveriesqueue[i].timelimit + 1)
            {
                AddRating(deliveriesqueue[i], false);
                UpdateDeliveryQueue(deliveriesqueue[i], false); 
                continue;
            }
            deliveriesqueue[i].notif.timeleft.text = (Mathf.Clamp((deliveriesqueue[i].timelimit - (Time.time - deliveriesqueue[i].starttime)),0,60).ToString("00"));
            deliveriesqueue[i].notif.timeleft2.text = (Mathf.Clamp((deliveriesqueue[i].timelimit - (Time.time - deliveriesqueue[i].starttime))*100%100, 0, 100).ToString("00"));
            DeliveryQueueText.text += "Order: " + deliveriesqueue[i].road.index + "\nTime - " + ((deliveriesqueue[i].timelimit - (Time.time - deliveriesqueue[i].starttime)).ToString("F2")) + "\nDistance - " + Vector3.Distance(transform.position, deliveriesqueue[i].house.transform.position).ToString("F0") + "\n---------\n";
        }
    }


    void AlignKart(){
        Ray ray = new Ray(mesh.transform.position, -mesh.transform.up);
        RaycastHit info;
        isgrounded = Physics.Raycast(ray, out info, 1f, groundLayer);
        if (Physics.Raycast(ray, out info, 2f, groundLayer))
        {
            // mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation, 
            mesh.transform.rotation = Quaternion.Lerp(mesh.transform.rotation,Quaternion.FromToRotation(mesh.transform.up, info.normal) * mesh.transform.rotation,0.01f * 250 * Time.deltaTime);

            if (Vector3.Angle(mesh.transform.forward, info.transform.forward) > 120)
            {
                turnbacknow.SetActive(true);
                if (turnbacknow.activeSelf)
                    turnbacknow.GetComponent<Image>().DOFade(1, 3);
            }
            else
            {
                turnbacknow.GetComponent<Image>().DOKill();
                turnbacknow.GetComponent<Image>().color -= new Color(0,0,0,0.01f);
                turnbacknow.SetActive(false);
            }
        }
    }

    void AddRating(DeliveryRequest req,bool passed)
    {
        if ((linepoint0.position - linepoint1.position).magnitude * 10 > 51)
        {
            passed = false;
        }
        if (passed)
        {

            StartCoroutine("AddScore");
            multiplier++;
            if (multiplier > Stats.statcount[1]) Stats.statcount[1] = multiplier;
            if (stars < 5 && multiplier % 5 == 0)
            {
                stars++;
                for (int i = 0; i < 5; i++)
                {
                    if (stars > i)
                    {
                        starsobjs[i].color = Color.white;
                    }
                    else
                    {
                        starsobjs[i].color = Color.black;
                    }
                }
            }
        }
        else
        {
            failurejingle.Play();
            successjingle.pitch = 1;
            stars--;
            if (stars < 1)
            {
                StartCoroutine(endrun());
            }
            multiplier = 1;
            for (int i = 0; i < 5; i++)
            {
                if (stars > i)
                {
                    starsobjs[i].color = Color.white;
                }
                else
                {
                    starsobjs[i].color = Color.black;
                }
            }
        }
        multipliertext.text = multiplier + "x";
        multipliertext.fontSize = 73 + multiplier;
        scoretext.text = score.ToString();
        starstext.text = "";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 8 && gameObject.layer == 3)
        {
            proceduralgen.laststate = proceduralgen.GenerateNextThing(proceduralgen.laststate);
            latestindex = other.transform.parent.gameObject.GetComponent<GeneratedRoad>().index;
            latestroad = other.transform.parent.gameObject.GetComponent<GeneratedRoad>();
            Destroy(other.gameObject);
            if (latestindex > currentdelivery + 3)
            {
                if (queueddeliveries != 0)
                {   
                    print(currentdeliveryreq.starttime + " " + Time.time);
                    AddRating(currentdeliveryreq, false);
                }
                UpdateDeliveryQueue(currentdeliveryreq, false);
            }
            if (difficulty > 2)
            {
                if (UnityEngine.Random.Range(0,100) < 2 && !pausenewdeliveries)
                {
                    StartCoroutine("breakfromdeliveries");
                }
            }
            if (UnityEngine.Random.Range(0,100) < (queueddeliveries == 0?20 * difficulty:2 * difficulty) && queueddeliveries < deliveriesqueue.Length && Time.timeSinceLevelLoad > 1f && (!pausenewdeliveries || UnityEngine.Random.Range(0,10) < 1 ) && !tutorial)
            {
                UpdateDeliveryQueue(deliverysys.DecideDelivery(),true);
            }
        }
    }
    

    IEnumerator AddScore()
    {
        //base score
        SpawnScore("Delivery",100 * multiplier);
        Stats.statcount[0]++;
        Stats.statcount[2]++;

        //midair check
        if (lastwasmidair)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Mid-Air",30 * multiplier);
            Stats.statcount[3]++;
            Stats.statcount[2]++;
        }


        //by a hair check
        if (float.Parse(linelengthtext.text) > 45f)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("By A Hair", 50 * multiplier);
            Stats.statcount[6]++;
            Stats.statcount[2]++;
        }

        //microscopic precision check
        if (float.Parse(linelengthtext.text) < 5.1f)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Crazy Precision", 80 * multiplier);
            Stats.statcount[4]++;
            Stats.statcount[2]++;
        }

        //snipe check
        if (lastwassniped)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Sniped", 50 * multiplier);
            Stats.statcount[5]++;
            Stats.statcount[2]++;
        }
        //less than a sec check
        if (lastdeliverytime < 1f)
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Less Than A Sec", 70 * multiplier);
            Stats.statcount[7]++;
            Stats.statcount[2]++;
        }

        //funny number check
        if ((linelengthtext.text) == "6.9")
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Funny Number", 690);
            Stats.statcount[8]++;
            Stats.statcount[2]++;
        }
        if ((linelengthtext.text) == "42.0")
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Funny Number", 420);
            Stats.statcount[8]++;
            Stats.statcount[2]++;
        }
        if (linelengthtext.text == "6.7")
        {
            yield return new WaitForSeconds(0.1f);
            SpawnScore("Funny Number", 670);
            Stats.statcount[8]++;
            Stats.statcount[2]++;
        }



        //under truck check


        yield return new WaitForSeconds(1);
        successjingle.pitch = 1;
        while (newscoreaddings.Count > 0)
        {
            StartCoroutine(AddScoreThingToScore(newscoreaddings[0]));
            newscoreaddings.RemoveAt(0);
            yield return new WaitForSeconds(0.1f);
        }
    }
    
    IEnumerator AddScoreThingToScore(ScoreThing st)
    {
        st.transform.DOMoveY(scoretext.transform.position.y,0.5f).SetEase(Ease.InBack);
        yield return new WaitForSeconds(0.5f);
        score += st.number;
        scoretext.text = score.ToString();
        st.gameObject.SetActive(false);
        Destroy(st.gameObject, 1f);
    }

    void SpawnScore(String s,int n)
    {
        GameObject mainscore = Instantiate(newscoreaddedtemplate, scorenewposition.position + (newscoreaddings.Count * (scoresecondposition.position-scorenewposition.position)) + (scorehiddenposition.position - scorenewposition.position),Quaternion.Euler(Vector3.zero),everythinginsidecnavas);
        ScoreThing mainscorething = mainscore.GetComponent<ScoreThing>();
        newscoreaddings.Add(mainscorething);
        mainscorething.number = n;
        mainscorething.category = s;
        mainscorething.text.text = mainscorething.category + " +" + mainscorething.number;
        mainscore.transform.DOMoveX(scorenewposition.position.x, 0.5f).SetEase(Ease.OutCubic);
        successjingle.Play();
        successjingle.pitch += 0.1f;
    }

    void UpdateDeliveryQueue(DeliveryRequest req, bool enqueue)
    {
        if (req.road == null) return;
        GeneratedRoad road = req.road;
        int roadnumber = road.index;
        if (enqueue)
        {
            for (int i = deliveriesqueue.Length-1; i > 0; i--)
            {
                deliveriesqueue[i] = deliveriesqueue[i - 1];
            }
            deliveriesqueue[0] = req;
            queueddeliveries++;
            if (queueddeliveries == 1)
            {
                req.notif.transform.DOMove(notifrestspot.position, 0.5f).SetEase(Ease.InOutCubic);
            }
            else
            {
                req.notif.transform.DOMove(notifhiddenspot.position, 0.5f).SetEase(Ease.InOutCubic);
                deliveriesqueue[1].notif.transform.DOMove(notifpushedspot.position, 0.5f).SetEase(Ease.InOutCubic);
            }
        }
        else
        {
            req.house.GetComponent<HouseScript>().YellowDelivery.SetActive(false);
            req.notif.transform.DOMove(notifdiespot.position, 0.5f).SetEase(Ease.InOutCubic);
            Destroy(req.notif.gameObject,1);
            if (queueddeliveries == 2)
            {
                deliveriesqueue[0].notif.transform.DOMove(notifrestspot.position,0.5f).SetEase(Ease.InOutCubic);
            }
            for (int i = deliveriesqueue.Length - 1; i >= 0; i--)
            {
                if ((deliveriesqueue[i].road != null ? deliveriesqueue[i].road.index : 0) == roadnumber)
                {
                    for (int j = deliveriesqueue.Length-2; j >= i; j--)
                    {
                        deliveriesqueue[i] = deliveriesqueue[i + 1];
                    }
                    deliveriesqueue[deliveriesqueue.Length - 1].road = null;
                }
            }
            queueddeliveries--;
        }
        for (int i = deliveriesqueue.Length-1; i >= 0; i--)
        {
            if (deliveriesqueue[i].road != null)
            {
                currentdelivery = deliveriesqueue[i].road.index;
                currentdeliveryhouse = deliveriesqueue[i].house;
                currentdeliveryreq = deliveriesqueue[i];
                currentdeliveryhouse.GetComponent<HouseScript>().RedDelivery.SetActive(true);
                //currentdeliveryhouse.transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                //currentdeliveryhouse.transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                goto updatedeliverytext;
            }
        }
        currentdelivery = 0;
        currentdeliveryhouse = null;
        DeliveryRequest nullreq;
        nullreq.house = null;
        nullreq.road = null;
        nullreq.starttime = 0;
        nullreq.timelimit = 0;
        nullreq.distlimit = 0;
        nullreq.notif = null;
        currentdeliveryreq = nullreq;

    updatedeliverytext:;
    }

    void PauseGame()
    {
        Time.timeScale = 0;
        paused = true;
        pausemenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        sens.value = MainMenu.sensitivity;
        music.value = MainMenu.musicvolume;
        sfx.value = MainMenu.soundvolume;
    }

    public void UnPauseGame()
    {
        Time.timeScale = aimmode ? 0.1f : 1;
        paused = false;
        pausemenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void applysettings()
    {
        MainMenu.sensitivity = sens.value;
        MainMenu.musicvolume = music.value;
        MainMenu.soundvolume = sfx.value;

        for (int i = 0; i < refreshthese.Length; i++)
        {
            refreshthese[i].RefreshVolume();
        }

        PlayerPrefs.SetFloat("sens", MainMenu.sensitivity);
        PlayerPrefs.SetFloat("music", MainMenu.musicvolume);
        PlayerPrefs.SetFloat("sfx", MainMenu.soundvolume);

    }

    public void Trick()
    {

        hitsfx.Play();
        switch (UnityEngine.Random.Range(0, 2))
        {
            case 0:
                cycle.transform.DOLocalRotate(cycle.transform.localRotation.eulerAngles + new Vector3(0, 360, 0), 0.5f, RotateMode.FastBeyond360);
                break;
            case 1:
                cycle.transform.DOLocalRotate(cycle.transform.localRotation.eulerAngles + new Vector3(0, -360, 0), 0.5f, RotateMode.FastBeyond360);
                break;
        }
    }

    public void endrunfunc()
    {
        UnPauseGame();
        StartCoroutine(endrun());
    }

    IEnumerator endrun()
    {
        gameover = true;
        if (score > PlayerPrefs.GetInt("highscore"))
        {
            PlayerPrefs.SetInt("highscore", score);
        }
        CRAYON.DOFade(0,1);
        gameovermenu.SetActive(true);
        gameovermenu.GetComponent<mainmenutoplaytransition>().StartTrans();
        gameovertextleft.text = "\n";
        gameovertextright.text = "\n";
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameovertextscoredisplay.text = score.ToString();
        for (int i = 0; i < Stats.statnames.Length; i++)
        {
            if (Stats.statcount[i] > 0)
            {
                gameovertextleft.text += "\n" + Stats.statnames[i];
                gameovertextright.text += "\n" + Stats.statcount[i];
            }
        }
        yield return new WaitForSeconds(1);

    }

    IEnumerator deliverycamtimeout()
    {
        yield return new WaitForSeconds(5f);
        StartCoroutine("resultcam");
    }

    IEnumerator resultcam()
    {
        lastwasmidair = !Physics.Raycast(mesh.transform.position, -mesh.transform.up, 1, groundLayer);
        lastwassniped = Vector3.Distance(latestdelivery.transform.position, mesh.transform.position) > 30;
        if (currentdelivery != 0)
        {
            deliverycam = 2;
            yield return new WaitForSeconds(0.2f);
            liner.enabled = true;
            tapesfx.pitch = difficulty;
            tapesfx.Play();
            linelengthtext.gameObject.SetActive(true);
            linepoint0.position = (currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position + latestdelivery.transform.position) / 2f;
            linepoint1.position = (currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position + latestdelivery.transform.position) / 2f;
            linelengthcanvas.transform.position = (currentdeliveryhouse.GetComponent<HouseScript>().YellowDelivery.transform.position + latestdelivery.transform.position) / 2f;
            yield return new WaitForSeconds(1/difficulty);
        }
        trail1.emitting = false;
        trail2.emitting = false;
        trail1.Clear();
        trail2.Clear();
        EverythingMover.ShiftOrigin();
        print("Origin Shifted");
        deliverycam = 0;
        rb.velocity = savedvelocity;
        for (int i = 0; i < deliveriesqueue.Length; i++)
        {
            deliveriesqueue[i].starttime += Time.time - deliverycamstarttime;
        }
        currentdeliveryreq.starttime += Time.time - deliverycamstarttime;
        speedlines.gameObject.SetActive(true);
        if (currentdelivery != 0)
        {
            AddRating(currentdeliveryreq,true);
            liner.enabled = false;
            linelengthtext.gameObject.SetActive(false);
            //linelengthtext.text = "";
            UpdateDeliveryQueue(currentdeliveryreq, false);
        }
    }

    IEnumerator returningtoground()
    {
        returningtimerstarted = true;
        yield return new WaitForSeconds(3);
        ReturningText.gameObject.SetActive(true);
        for (int i = 5; i > 0; i--)
        {
            if (!returningtimerstarted) goto basicallytheend;
            ReturningText.text = "Returning in " + i;
            yield return new WaitForSeconds(1);
        }
        rb.isKinematic = true;
        rb.position = latestroad.transform.position + new Vector3(0, 0.5f, 0);
        mesh.transform.rotation = latestroad.transform.rotation;
        yield return new WaitForSeconds(0.1f);
        rb.isKinematic = false;
        basicallytheend:
        returningtimerstarted =  false;
        ReturningText.gameObject.SetActive(false);
    }

    IEnumerator breakfromdeliveries()
    {
        pausenewdeliveries = true;
        yield return new WaitForSecondsRealtime(15);
        pausenewdeliveries = false;
    }

    IEnumerator explaincontrols()
    {
        tutorial = true;
        yield return new WaitForSeconds(1);
        controlimage1.DOFade(1, 1f);
        controlstext1.DOFade(1, 1f);
        yield return new WaitForSeconds(3);
        controlstext1.DOFade(0, 1f);
        controlimage1.DOFade(0, 1f);
        yield return new WaitForSeconds(1.5f);
        controlimage2.DOFade(1, 1f);
        controlimage3.DOFade(1, 1f);
        controlstext2.DOFade(1, 1f);
        yield return new WaitForSeconds(3);
        controlstext2.DOFade(0, 1f);
        controlimage2.DOFade(0, 1f);
        controlimage3.DOFade(0, 1f);
        yield return new WaitForSeconds(1.5f);
        controlstext3.DOFade(1, 1f);
        yield return new WaitForSeconds(3);
        controlstext3.DOFade(0, 1f);
        yield return new WaitForSeconds(1);
        UpdateDeliveryQueue(deliverysys.DecideDelivery(), true);
        tutorial = false;
    }
}
