using Raylib_cs;
using RPG_JKL;
using System.Numerics;

public class Player
{
    private Texture2D dashIcon = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\player\\player.png");

    private float speed = 200;
    private float sprintSpeed = 400;
    private float walkSpeed = 200;

    private float multiRecoveryStamina = 10f;
    private float multiRemoveStamina = 20f;

    public List<Bullet> bullets = new List<Bullet>();

    public Vector2 position;
    public float rotation = 0f;

    public float stamina = 100f;
    public bool isSprinting = false;
    private bool canSprint = true;

    public float hp = 100;

    private Vector2 gunTipOffset = new Vector2(255, 114); // Punkt końca lufy w teksturze
    private float scale = 0.5f; // Skalowanie tekstury

    private float fireRate = 0.5f; // Czas odnowienia strzału (w sekundach)
    private float lastShotTime = 0f;

    public Player()
    {
       position = new Vector2((Constantly.mapWidth * Constantly.tileSize) / 2, (Constantly.mapHeight * Constantly.tileSize) / 2);
    }

    public void Draw()
    {
        Vector2 origin = new Vector2(58, 82); // Punkt obrotu

        // Rysowanie gracza z uwzględnieniem rotacji
        Raylib.DrawTexturePro(
            dashIcon,
            new Rectangle(0, 0, 256, 150), // Pełna tekstura
            new Rectangle(position.X, position.Y, 256 * scale, 150 * scale), // Skalowanie i pozycja
            origin, // Punkt obrotu
            rotation, // Obrót w stopniach
            Color.White // Kolor
        );

        // Rysowanie pocisków
        foreach (var bullet in bullets)
        {
            bullet.Draw();
        }
    }

    // Metoda do rotacji gracza w kierunku kursora
    public void RotateTowardsMouse(Camera2D camera)
    {
        Vector2 mousePosition = Raylib.GetScreenToWorld2D(Raylib.GetMousePosition(), camera);
        Vector2 directionToMouse = mousePosition - position;

        if (directionToMouse.Length() > 0)
        {
            rotation = MathF.Atan2(directionToMouse.Y, directionToMouse.X) * (180.0f / MathF.PI);
        }
    }

    // Metoda ruchu gracza
    public void Move(Vector2 direction, float deltaTime)
    {
        if (direction.Length() > 0)
        {
            direction = Vector2.Normalize(direction);
            position += direction * speed * deltaTime;
          //  Console.WriteLine("X: " + position.X + "Y: " + position.Y);
        }
    }
    public void Sprint(float deltaTime)
    {
        if (isSprinting && stamina > 5 && canSprint)
        {
            speed = sprintSpeed;
            stamina -= deltaTime * multiRemoveStamina;
            stamina = Math.Max(stamina, 0);

            if (stamina <= 5)
            {
                canSprint = false; // Wyłącz sprint, dopóki stamina nie osiągnie odpowiedniego poziomu
                isSprinting = false;
            }
        }
        else
        {
            speed = walkSpeed;
            stamina += deltaTime * multiRecoveryStamina;
            stamina = Math.Min(stamina, 100);

            if (stamina >= 20)
            {
                canSprint = true; // Ponownie pozwól na sprint po regeneracji staminy
            }
        }
    //    Console.WriteLine(stamina);

    }

    // Metoda strzelania
    public void TryShoot(float deltaTime)
    {
        lastShotTime += deltaTime;

        if (Raylib.IsMouseButtonDown(MouseButton.Left) && lastShotTime >= fireRate)
        {
            Shoot();
            lastShotTime = 0f; // Resetowanie czasu od ostatniego strzału
        }
    }

    // Metoda tworzenia pocisku
    private void Shoot()
    {
        // Obliczanie pozycji końca lufy
        Vector2 gunTip = position + gunTipOffset;
        gunTip.X -= 175;
        gunTip.Y -= 140;
        gunTip = RotatePoint(gunTip, position, rotation); // Obracamy punkt lufy

        // Wyliczamy kierunek pocisku
        Vector2 direction = new Vector2(
            MathF.Cos(MathF.PI / 180f * rotation),
            MathF.Sin(MathF.PI / 180f * rotation)
        );

        // Tworzymy nowy pocisk
        bullets.Add(new Bullet(gunTip, direction)); // Dodajemy pocisk do listy
    }

    // Metoda do obliczenia obrotu
    public static Vector2 RotatePoint(Vector2 point, Vector2 center, float angle)
    {
        float radian = MathF.PI / 180f * angle;
        float cosA = MathF.Cos(radian);
        float sinA = MathF.Sin(radian);

        float dx = point.X - center.X;
        float dy = point.Y - center.Y;

        return new Vector2(
            center.X + cosA * dx - sinA * dy,
            center.Y + sinA * dx + cosA * dy
        );
    }

    // Zaktualizowanie pocisków
    public void UpdateBullets(float deltaTime)
    {
        foreach (var bullet in bullets)
        {
            bullet.Update(deltaTime); // Aktualizacja pozycji pocisku
        }

        // Usuwanie pocisków, które opuściły ekran
        bullets.RemoveAll(bullet => Vector2.Distance(bullet.position, position) > Constantly.renderBullets);
    }
    public float ReturnStamina()
    {
        return stamina;
    }
    public float ReturnHp()
    {
        return hp;
    }
}
