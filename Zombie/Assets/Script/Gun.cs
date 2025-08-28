using System.Collections;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        Reloading,
    }

    private State currentState = State.Ready;

    public State CurrentState
    {
        get { return currentState; }
        private set
        {
            currentState = value;
            switch (currentState)
            {
                case State.Ready:
                    UpdateReady();
                    break;
                case State.Empty:
                    UpdateEmpty();
                    break;
                case State.Reloading:
                    UpdateReloading();
                    break;

            }
        }
    }

    private void UpdateReady()
    {

    }
    private void UpdateEmpty()
    {

    }
    private void UpdateReloading()
    {

    }

    public GunData gunData;

    public UIManager uiManager;

    public ParticleSystem muzzleEffect;
    public ParticleSystem shellEffect;

    private LineRenderer lineRenderer;
    private AudioSource audioSource;

    public Transform firePosition;

    public int ammoRemain;
    public int magAmmo;

    private float lastFireTime;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        lineRenderer = GetComponent<LineRenderer>();

        lineRenderer.enabled = false;
        lineRenderer.positionCount = 2;
    }

    private void OnEnable()
    {
        ammoRemain = gunData.startAmmoRemain;
        magAmmo = gunData.magCapacity;
        lastFireTime = 0f;

        CurrentState = State.Ready; 
        uiManager.SetAmmoText(magAmmo, ammoRemain);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            Reload();
        }

        switch (currentState)
        {
            case State.Ready:
                UpdateReady();
                break;
            case State.Empty:
                UpdateEmpty();
                break;
            case State.Reloading:
                UpdateReloading();
                break;

        }
    }

    private IEnumerator CoShotEffect(Vector3 hitPosition)
    {
        audioSource.PlayOneShot(gunData.shootClip);

        muzzleEffect.Play();
        shellEffect.Play();
        lineRenderer.enabled = true;
        lineRenderer.SetPosition(0, firePosition.position);

        

        lineRenderer.SetPosition(1, hitPosition);

        yield return new WaitForSeconds(0.2f);

        lineRenderer.enabled = false;
    }

    public void Fire()
    {
        if (currentState == State.Ready && Time.time > lastFireTime + gunData.timeBetFire)
        {
            lastFireTime = Time.time;

            Shoot();
        }
    }

    public void Shoot()
    {
        Vector3 hitPosition = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(firePosition.position, firePosition.forward, out hit, gunData.fireDistance))
        {
            hitPosition = hit.point;
            var target = hit.collider.GetComponent<IDamagable>();
            if (target != null)
            {
                target.OnDamage(gunData.damage, hit.point, hit.normal);
            }
        }
        else
        {
            hitPosition = firePosition.position + firePosition.forward * gunData.fireDistance;
        }

        StartCoroutine(CoShotEffect(hitPosition));

        --magAmmo;

        if (magAmmo == 0)
        {
            CurrentState = State.Empty;
        }

        uiManager.SetAmmoText(magAmmo, ammoRemain);

    }

    public bool Reload()
    {
        if (CurrentState == State.Reloading || ammoRemain ==0 || magAmmo == gunData.magCapacity)
            return false;
            
       // CurrentState = State.Reloading;

        StartCoroutine(CoReload());

        return true;
        uiManager.SetAmmoText(magAmmo, ammoRemain);
    }

    IEnumerator CoReload()
    {
        CurrentState = State.Reloading;
        audioSource.PlayOneShot(gunData.reloadClip);

        yield return new WaitForSeconds(gunData.reloadTime);

        magAmmo += ammoRemain;
        if (magAmmo >= gunData.magCapacity)
        {
            magAmmo = gunData.magCapacity;
            ammoRemain -= magAmmo;
        }
        else
        {
            ammoRemain = 0;
        }
        CurrentState = State.Ready;
    }

    public void AddAmmo(int amount)
    {
        ammoRemain = Mathf.Min(ammoRemain + amount, gunData.startAmmoRemain);
        uiManager.SetAmmoText(magAmmo, ammoRemain);
    }
}
