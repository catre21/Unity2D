using UnityEngine;
using TMPro;

public class Gun : MonoBehaviour
{
    private float rotationOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shootDelay = 0.15f;
    [SerializeField] private int maxAmmo = 24;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private AudioManager audioManager;

    private float nextShot;
    public int currentAmmo;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        RotateGun();
        Shoot();
        Reload();
    }

    void RotateGun()
    {
        if (Input.mousePosition.x < 0 || Input.mousePosition.x > Screen.width ||
            Input.mousePosition.y < 0 || Input.mousePosition.y > Screen.height)
        {
            return;
        }

        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 displacement = mousePos - transform.position;
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, angle + rotationOffset);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, -1, 1);
        else
            transform.localScale = new Vector3(1, 1, 1);
    }

    void Shoot()
    {
        if (Input.GetMouseButtonDown(0) && currentAmmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shootDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAmmo--;
            UpdateAmmoText();
            audioManager.PlayShootSound();
        }
    }

    void Reload()
    {
        if (Input.GetMouseButton(1) && currentAmmo < maxAmmo)
        {
            currentAmmo = maxAmmo;
            UpdateAmmoText();
            
        }
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = currentAmmo > 0 ? currentAmmo.ToString() : "Empty";
        }
    }
}
