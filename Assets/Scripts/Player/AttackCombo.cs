/* using UnityEngine;

public class AttackCombo : MonoBehaviour
{
    public int currentCombo = 1;
    public float maxCombo = 3;
    public float comboTimer = 0;
    public float comboTiming = 0.6f;
    public bool attacking;
    public bool readyAttack;

    private Animator anim;

    void Awake()
    {
        anim = GetComponent<Animator>();
        comboTimer = comboTiming;
        readyAttack = true;
    }

    void Update()
    {
        Combo();
    }

    private void Combo()
    {
        if (Input.GetMouseButton(1)) readyAttack = false;
        if (Input.GetMouseButtonUp(1)) readyAttack = true;
        // Giảm comboTimer theo thời gian khung hình
        comboTimer -= Time.deltaTime;

        // if lệnh tấn công và comboTimer < 0 (bắt đầu comboo)
        if (Input.GetMouseButtonDown(0) && comboTimer < 0f && readyAttack == true)
        {
            // bật trạng thái tấn công;
            attacking = true;

            // attack animation;
            anim.SetTrigger("attack" + currentCombo);

            // set comboTimer = comboTiming;
            comboTimer = comboTiming;
        }

        // else if lệnh tấn công và comboTimer > 0(trong thời gian combo) và comboTimer < timing animation
        else if (Input.GetMouseButtonDown(0) && comboTimer > 0f && comboTimer < 0.5f)
        {
            // bật trạng thái tấn công;
            attacking = true;

            // tăng biến đếm combo và kiểm tra vượt giới hạn;
            currentCombo++;
            if (currentCombo > maxCombo) currentCombo = 1;

            // attack animation
            anim.SetTrigger("attack" + currentCombo);
            comboTimer = comboTiming;
        }

        // else if comboTimer < 0 và thao tác khác thao tác tấn công thì tắt trạng thái tấn công
        else if (comboTimer < 0 && !Input.GetMouseButtonDown(0)) attacking = false;

        if (comboTimer < 0) currentCombo = 1;
    }
}
 */