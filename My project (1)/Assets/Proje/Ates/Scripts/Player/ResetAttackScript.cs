using UnityEngine;

public class ResetAttackScript : StateMachineBehaviour
{
    // Animasyon bittiğinde (Exit olduğunda) çalışır
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // Animator'daki integer'ı sıfırla (0 genellikle Idle/Boş durumdur)
        animator.SetInteger("currentAttack", -1); // Veya Idle için hangi değeri kullanıyorsan
        
        // KnightWeapon scriptindeki isAttacking bool'unu kapat
        KnightWeapon weapon = animator.GetComponent<KnightWeapon>();
        if (weapon != null)
        {
            weapon.isAttacking = false;
        }
    }
}