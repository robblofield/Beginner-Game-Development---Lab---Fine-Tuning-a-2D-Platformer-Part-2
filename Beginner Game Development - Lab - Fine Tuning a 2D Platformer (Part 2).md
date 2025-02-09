# Beginner Game Development - Lab - Fine Tuning a 2D Platformer (Part 2)

*In this lab, we will build upon our **2D platformer project** by enhancing the player controller with advanced movement techniques: **Coyote Time, Wall Sliding, and Wall Jumping**. These mechanics improve the feel and responsiveness of the game, making movement more fluid and enjoyable for players.*

![Platformer Examples - Trinki by Rob Blofield](<Platformer Player Controller (Trinki).gif>)
---

## **Core Concepts for This Lab**

### **Core Concept: Coyote Time**  
*Coyote time* is a small grace period where the player can still jump even after leaving the ground. This allows for more forgiving platforming and reduces frustration when mistiming jumps. It simulates the idea that the player still has a fraction of a second to react before truly falling.

### **Core Concept: Wall Sliding & Wall Jumping**  
Wall sliding allows the player to slow their descent when against a wall, while wall jumping enables them to push off walls to reach new heights. These mechanics create opportunities for vertical movement and skill-based traversal.

### **Core Concept: Improved Collision Detection for Jumping**  
To implement coyote time and wall mechanics, we must refine how we detect when the player is in the air or near a wall. This involves adding **timing buffers** and detecting wall contacts with **Raycasts** or **collision checks**.

---

## **Section 1 - Setting Up the Project**

### **Step 1: Use the Updated Player Controller**
Ensure that you have the **updated \`PlayerController\` script** from Part 1. It includes structured movement, acceleration, deceleration, and surface-based physics adjustments.

### **Step 2: Modify the Player Prefab**
We will need additional colliders or logic to detect walls:
1. **Ensure \`Player\` has a \`Rigidbody2D\` with Gravity Scale set to \`1\`.**
2. **Add two empty GameObjects (\`LeftWallCheck\`, \`RightWallCheck\`)** as child objects of the player.
3. **Position them slightly to the left and right of the player** to detect wall collisions.
4. Assign appropriate \`LayerMasks\` for ground and walls.

---

## **2. Implementing Advanced Mechanics**

### **Step 3: Add Coyote Time**
Modify the \`PlayerController\` script to include a short delay that allows jumping slightly after falling off a ledge.

#### **Starting Code for Coyote Time:**
\`\`\`csharp
private float coyoteTime = 0.1f; // How long the player has after leaving ground to still jump
private float coyoteTimeCounter;

void Update()
{
    if (isGrounded)
    {
        coyoteTimeCounter = coyoteTime;
    }
    else
    {
        coyoteTimeCounter -= Time.deltaTime;
    }

    if (Input.GetKeyDown(KeyCode.Space) && coyoteTimeCounter > 0f)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        coyoteTimeCounter = 0f;
    }
}
\`\`\`
✅ This ensures the player has a small buffer to jump after leaving the ground, improving responsiveness.

---

### **Step 4: Implement Wall Sliding**
When the player moves against a wall, they should slide down instead of falling at full speed.

#### **Starting Code for Wall Sliding:**
\`\`\`csharp
private bool isWallSliding;
private float wallSlideSpeed = 2f;

void Update()
{
    isWallSliding = false;

    if (IsTouchingWall() && !isGrounded && rb.velocity.y < 0)
    {
        isWallSliding = true;
        rb.velocity = new Vector2(rb.velocity.x, -wallSlideSpeed);
    }
}

private bool IsTouchingWall()
{
    return Physics2D.OverlapCircle(leftWallCheck.position, 0.1f, wallLayer) ||
           Physics2D.OverlapCircle(rightWallCheck.position, 0.1f, wallLayer);
}
\`\`\`
✅ This limits downward speed when touching a wall, making the descent feel controlled.

---

### **Step 5: Implement Wall Jumping**
After sliding down a wall, the player should be able to jump off it.

#### **Starting Code for Wall Jumping:**
\`\`\`csharp
private bool isWallJumping;
private float wallJumpTime = 0.2f;
private Vector2 wallJumpDirection = new Vector2(1, 1.5f);

void HandleJump()
{
    if (Input.GetKeyDown(KeyCode.Space))
    {
        if (isWallSliding)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(-moveInput * wallJumpDirection.x * jumpForce, 
                                      wallJumpDirection.y * jumpForce);
            Invoke("ResetWallJump", wallJumpTime);
        }
    }
}

void ResetWallJump()
{
    isWallJumping = false;
}
\`\`\`
✅ This allows the player to jump away from walls dynamically, adding a new layer of platforming control.

---

## **3. Testing & Debugging**

### **Step 6: Verify Mechanics**
1. **Test Coyote Time** by stepping off a platform and pressing jump at the last moment.
2. **Test Wall Sliding** by moving against a wall and observing slower descent.
3. **Test Wall Jumping** by sliding down a wall and jumping away from it.

### **Step 7: Debug Logging**
Modify the script to include debug logs for testing.
\`\`\`csharp
if (isWallSliding) Debug.Log("Wall Sliding");
if (isWallJumping) Debug.Log("Wall Jumping");
\`\`\`
✅ These logs help verify when the mechanics activate.

---

## **Section 2 - Lab Task**
Now that you have implemented these mechanics, discuss within your group how to improve their feel and usability. Adjust the parameters (timing, speeds, forces) for smoother movement.

#### **Minimum Requirements:**
- Fine-tune **Coyote Time**, **Wall Sliding**, and **Wall Jumping** for better control.
- Debug Log messages confirming when each mechanic is triggered.

#### **Desirable Enhancements:**
- Allow slight movement after wall jumping before regaining full control.
- Adjust wall jump direction based on input.
- Implement optional **Wall Climbing** (slowly ascending a wall by holding a key).

#### **Extension Task (Optional Homework - for eager beavers! 🤓)**
- Implement **Wall Jump Directional Influence** (holding a direction modifies jump trajectory).
- Add **Wall Climb with stamina mechanics**.
- Implement **Jump Buffering** (if the player presses jump slightly before landing, still execute a jump).

---

## **Next Steps**
In future labs, we will enhance the **player physics** further with:
- **Variable Jump Height (Holding Jump for Higher Jumps)**
- **Dash Mechanics**
- **Advanced Gravity & Momentum Control**

---

## **Summary**
✅ Implemented **Coyote Time** for better jump forgiveness  
✅ Added **Wall Sliding** for controlled wall descent  
✅ Introduced **Wall Jumping** for dynamic movement expansion  
✅ Tested and fine-tuned new mechanics  

This lab sets the foundation for **smooth and responsive** platformer movement. Experiment with values and tweak the mechanics to find what feels best! 🎮
