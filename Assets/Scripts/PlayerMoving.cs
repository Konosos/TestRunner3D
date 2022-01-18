using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoving : MonoBehaviour
{
    private float horizolInput;
    private float verticalInput;
    private int maxHealth=100;
    private int maxStamina=800;
    private int currentStamina;
    private int currentHealth;
    private bool isAttacking=false;
    private bool isSlashing=false;
    private bool canSlash=false;
    private bool shiftIsUp=true;

    private CharacterController cha_Control;
    private Animator anim;
    private Vector3 moveDirection;
    private Vector3 velocity;
    private GameObject mySword=null;
    private Sword swordScr;
    private Inventory inventory;
    private bool isShowBag=false;
    [SerializeField]private bool isGround;
    [SerializeField]private float walkSpeed=3f;
    [SerializeField]private float jumpHight=5f;
    [SerializeField]private float gravity=-9.81f;
    [SerializeField]private LayerMask groundMask;
    [SerializeField]private Transform rightHand;
    [SerializeField]private HealthBar healthBar;
    [SerializeField]private HealthBar staminaBar;
    [SerializeField]private UI_Inventory uiInventory;

    // Start is called before the first frame update
    void Start()
    {
        cha_Control=GetComponent<CharacterController>();
        anim=GetComponentInChildren<Animator>();
        healthBar.SetMaxHealth(maxHealth);
        staminaBar.SetMaxHealth(maxStamina);
        currentHealth=maxHealth;
        currentStamina=maxStamina;
        inventory=new Inventory();
        uiInventory.SetInventory(inventory);
        uiInventory.gameObject.SetActive(false);
        ItemWorld.SpawnItemWorld(new Vector3(2,0,0),new Item{itemType=Item.ItemType.HealthPotion, amount=1});
        ItemWorld.SpawnItemWorld(new Vector3(4,1,2),new Item{itemType=Item.ItemType.Sword, amount=1});
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        if(Input.GetKey(KeyCode.Mouse0) && isGround && mySword!=null && isAttacking==false)
        {
            StartCoroutine(Attacking());
        }
        if(Input.GetKey(KeyCode.Mouse0) && isGround && mySword!=null && canSlash==true && isSlashing==false)
        {
            StartCoroutine(Slashing());
        }
        if(Input.GetKeyDown(KeyCode.I))
        {
            if(!isShowBag)
            {
                uiInventory.gameObject.SetActive(true);isShowBag=true;
                uiInventory.RefreshSlot();
            }
            else
            {
                uiInventory.gameObject.SetActive(false);isShowBag=false;
            }
        }
    }
    public void RemoveOneItem(Item item)
    {
        inventory.RemoveItem(item);
        uiInventory.RefreshSlot();
    }
    public void UseNewSword(Item item)
    {
        if(mySword!=null)
        {
            ItemWorld itemWorld=mySword.GetComponent<ItemWorld>();
            if(itemWorld!=null)
            {
                inventory.AddItemToList(itemWorld.GetItem());
                itemWorld.DestroyMySelf();
            }
            mySword=null;
            swordScr=null;
            SwordToHand(item);
        }
        else
        {
            SwordToHand(item);
        }
    }
    private void SwordToHand(Item item)
    {
        ItemWorld ItemWorld=ItemWorld.SpawnItemWorld(new Vector3(0,0,0),item);
        ItemWorld.gameObject.transform.SetParent(rightHand);
        ItemWorld.gameObject.transform.localPosition=Vector3.zero;
        ItemWorld.gameObject.transform.localRotation=Quaternion.Euler(Vector3.zero);
        ItemWorld.gameObject.transform.localScale=Vector3.one;
        ItemWorld.gameObject.tag="InHand";
        mySword=ItemWorld.gameObject;
        swordScr=mySword.gameObject.GetComponent<Sword>();
        swordScr.coll.enabled=false;
    }
    public void AddHP()
    {
        if(currentHealth<maxHealth)
        {
            currentHealth+=40;
            healthBar.SetHealth(currentHealth);
        }
    }
    private IEnumerator Attacking()
    {
        anim.SetBool("isAttack",true);
        isAttacking=true;
        swordScr.coll.enabled=true;
        yield return new WaitForSeconds(1f);
        anim.SetBool("isAttack",false);
        canSlash=true;
        swordScr.coll.enabled=false;
        yield return new WaitForSeconds(0.5f);
        if(isSlashing!=true)
        {
            isSlashing=false;canSlash=false;isAttacking=false;
        }
    }
    private IEnumerator Slashing()
    {
        anim.SetBool("isSlash",true);
        isSlashing=true;
        swordScr.coll.enabled=true;
        yield return new WaitForSeconds(1.7f);
        anim.SetBool("isSlash",false);
        isSlashing=false;canSlash=false;isAttacking=false;
        swordScr.coll.enabled=false;
    }
    private void Move()
    {
        isGround=Physics.CheckSphere(transform.position,0.2f,groundMask);
        if(isGround && velocity.y<0)
        {
            velocity.y=-2f;
        }
        StatePlayer();
        moveDirection=new Vector3(horizolInput,0,verticalInput);
        moveDirection=transform.TransformDirection(moveDirection);
        moveDirection*=walkSpeed;
        if(isGround)
        {
            if(moveDirection!=Vector3.zero)
            {
                if(Input.GetKey(KeyCode.LeftShift) && currentStamina>0  && shiftIsUp)
                {
                    anim.SetFloat("MoveX",horizolInput,0.1f,Time.deltaTime);
                    anim.SetFloat("MoveZ",Mathf.Abs(verticalInput),0.1f,Time.deltaTime);
                    cha_Control.Move(moveDirection*Time.deltaTime*2);
                    UpStamina(false);
                }
                else
                {
                    shiftIsUp=false;
                    if(Input.GetKeyDown(KeyCode.LeftShift))
                    {
                        shiftIsUp=true;
                    }
                    anim.SetFloat("MoveX",horizolInput/2,0.1f,Time.deltaTime);
                    anim.SetFloat("MoveZ",Mathf.Abs(verticalInput)/2,0.1f,Time.deltaTime);
                    cha_Control.Move(moveDirection*Time.deltaTime);
                    UpStamina(true);
                }
            }
            else
            {
                anim.SetFloat("MoveX",0f,0.1f,Time.deltaTime);
                anim.SetFloat("MoveZ",0f,0.1f,Time.deltaTime);
                UpStamina(true);
            }
            anim.SetBool("isJumping",false);
            if(Input.GetKeyDown(KeyCode.Space))
            {
                velocity.y=Mathf.Sqrt(jumpHight*-2*gravity);
                
            }
        }
        else{anim.SetBool("isJumping",true);}
        velocity.y+=gravity*Time.deltaTime;
        
        cha_Control.Move(velocity*Time.deltaTime);
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthBar.SetHealth(currentHealth);
        //if (health <= 0) Invoke(nameof(DestroyEnemy), 0.5f);
    }
    private void UpStamina(bool isUp)
    {
        switch(isUp)
        {
            case true:if(currentStamina<maxStamina){currentStamina++;}break;
            case false:if(currentStamina>0){currentStamina--;}break;
        }
        staminaBar.SetHealth(currentStamina);
    }
    private void StatePlayer()
    {
        float hor=Input.GetAxis("Horizontal");
        float ver=Input.GetAxis("Vertical");
        
        if(ver==0)
        {
            verticalInput=0;
            if(hor==0)
            {
                horizolInput=0;
            }
            else 
            {
                if(Mathf.Abs(horizolInput)<Mathf.Abs(hor))
                {
                    horizolInput+=Time.deltaTime*(hor/Mathf.Abs(hor))*2f;
                }else
                {
                    horizolInput=hor;
                }
            }
        }
        else
        {
            horizolInput=0;
            if(Mathf.Abs(verticalInput)<Mathf.Abs(ver))
            {
                verticalInput+=Time.deltaTime*(ver/Mathf.Abs(ver))*2f;
            }else
            {
                verticalInput=ver;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag!="InHand")
        {
            ItemWorld itemWorld=other.GetComponent<ItemWorld>();
            if(itemWorld!=null)
            {
                inventory.AddItemToList(itemWorld.GetItem());
                itemWorld.DestroyMySelf();
            }
        }
    }
}
