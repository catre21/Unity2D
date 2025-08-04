using System.Collections;
using TMPro;
using UnityEngine;

public class Gun : MonoBehaviour
{
    private float rotateOffset = 180f;
    [SerializeField] private Transform firePos;
    [SerializeField] private GameObject bulletPrefabs;
    [SerializeField] private float shotDelay = 0.15f;
    private float nextShot;
    [SerializeField] private int maxAnmo = -1;
    [SerializeField] private TextMeshProUGUI ammoText;
    [SerializeField] private AudioManager audioManager;
    [SerializeField] private float reloadTime = 1.5f; // Thời gian thay đạn
    public int currentAnmo;

    private bool isReloading = false; // Đang reload?

    void Start()
    {
        currentAnmo = maxAnmo;
        UpdateAmmoText();
    }

    void Update()
    {
        if (isReloading) return; // Nếu đang thay đạn thì không xoay & bắn

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

        Vector3 displacement = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float angle = Mathf.Atan2(displacement.y, displacement.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle + rotateOffset);

        if (angle < -90 || angle > 90)
            transform.localScale = new Vector3(1, 1, 1);
        else
            transform.localScale = new Vector3(1, -1, 1);
    }

    void Shoot()
    {
        if (Input.GetMouseButton(0) && currentAnmo > 0 && Time.time > nextShot)
        {
            nextShot = Time.time + shotDelay;
            Instantiate(bulletPrefabs, firePos.position, firePos.rotation);
            currentAnmo--;
            UpdateAmmoText();
            audioManager.PlayShootSound();
        }
    }

    void Reload()
    {
        if (Input.GetMouseButtonDown(1) && currentAnmo < maxAnmo)
        {
            StartCoroutine(ReloadCoroutine());
        }
    }

    IEnumerator ReloadCoroutine()
    {
        isReloading = true;
        audioManager.PlayReLoadSound();
        Debug.Log("Reloading...");

        yield return new WaitForSeconds(reloadTime);

        currentAnmo = maxAnmo;
        UpdateAmmoText();
        isReloading = false;
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
            ammoText.text = currentAnmo.ToString();
        else
            ammoText.text = "Empty";
    }
}
