using UnityEngine;
using System.Collections;
using TMPro;

public class Gun : MonoBehaviour
{
    [SerializeField] private Transform firePoint;
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletSpeed = 10f;
    [SerializeField] private float shotDelay = 0.15f;
    [SerializeField] private int maxAmmo = 10;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float reloadTime = 1.5f;

    private float nextShot;
    private int currentAmmo;
    private bool isReloading = false;

    void Start()
    {
        currentAmmo = maxAmmo;
        UpdateAmmoText();
    }

    void Update()
    {
        if (isReloading) return;

        if (Input.GetMouseButton(0))
        {
            Vector3 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            touchPos.z = 0;

            RotateGunTowards(touchPos);
            Shoot();
        }
    }

    void RotateGunTowards(Vector3 targetPos)
    {
        Vector3 direction = targetPos - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void Shoot()
    {
        if (Time.time < nextShot) return;
        if (currentAmmo <= 0)
        {
            StartCoroutine(ReloadCoroutine());
            return;
        }

        nextShot = Time.time + shotDelay;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.linearVelocity = firePoint.right * bulletSpeed;

        currentAmmo--;
        UpdateAmmoText();

        if (audioManager != null) audioManager.PlayShootSound();
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        if (audioManager != null) audioManager.PlayReLoadSound();

        yield return new WaitForSeconds(reloadTime);

        currentAmmo = maxAmmo;
        UpdateAmmoText();
        isReloading = false;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = currentAmmo.ToString();
    }
}
