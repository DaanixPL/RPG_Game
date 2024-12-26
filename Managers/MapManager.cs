using Raylib_cs;
using System;
using System.Numerics;
using System.Runtime.InteropServices;

namespace RPG_JKL.Managers
{
    internal class MapManager
    {
        private int[,] tilemap;
        private Texture2D[] textures;

        public void Init()
        {
            // Inicjalizacja tekstur
            textures = new Texture2D[3];
            textures[0] = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\grass.png");  // Trawa
            textures[1] = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\stone.png");  // Kamień
            textures[2] = Raylib.LoadTexture("C:\\Users\\DaanixPL\\source\\repos\\RPG_JKL\\Assets\\water.png");  // Woda

            tilemap = new int[Constantly.mapWidth, Constantly.mapHeight];

            // Generowanie mapy
            GenerateMapUsingPerlinNoise();

       //     SaveNoiseImage(Constantly.mapWidth, Constantly.mapHeight);
        }
//  private void SaveNoiseImage(int width, int height)
//  {
//      Image noiseImage = Raylib.GenImageColor(width, height, new Color(255, 255, 255, 255));
//
//      for (int y = 0; y < height; y++)
//      {
//          for (int x = 0; x < width; x++)
//          {
//              // Pobierz wartość z tilemapy i przypisz kolor
//              int value = tilemap[x, y];
//              byte colorValue = (byte)(value * 255 / 3);  // Normalizacja (z uwzględnieniem liczby typów terenu)
//
//              // Przypisanie odpowiedniego koloru na podstawie wartości
//              Raylib.ImageDrawPixel(ref noiseImage, x, y, new Color((byte)colorValue, (byte)colorValue, (byte)colorValue, (byte)255));
//          }
//      }
//
//      // Ścieżka do zapisania obrazu w folderze Dokumentów
//      string savePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "perlin_noise_map.png");
//
//      // Zapisz obraz
//      Raylib.ExportImage(noiseImage, savePath);
//
//      // Zwolnij pamięć po obrazie
//      Raylib.UnloadImage(noiseImage);
//
//      Console.WriteLine($"Szum zapisany w: {savePath}");
//  }
        private unsafe void GenerateMapUsingPerlinNoise()
        {
            int width = Constantly.mapWidth;
            int height = Constantly.mapHeight;
            float scale = 10.10f;

            tilemap = new int[width, height];

            for (int octave = 1; octave <= 3; octave++)
            {
                int offsetX = Raylib.GetRandomValue(0, 1000);
                int offsetY = Raylib.GetRandomValue(0, 1000);
                float octaveScale = scale / octave;

                Image perlinNoiseImage = Raylib.GenImagePerlinNoise(width, height, offsetX, offsetY, octaveScale);
                byte[] pixels = new byte[width * height * 4];
                nint pixelPointer = (nint)Raylib.LoadImageColors(perlinNoiseImage);
                Marshal.Copy(pixelPointer, pixels, 0, pixels.Length);

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        byte r = pixels[(y * width + x) * 4];
                        float brightness = r / 255.0f;

                        // Zastosowanie różnych kolorów w zależności od jasności
                        if (brightness < 0.2f)
                        {
                            tilemap[x, y] = 2;  // Woda (ciemniejszy)
                        }
                        else if (brightness < 0.5f)
                        {
                            tilemap[x, y] = 0;  // Trawa (średnia jasność)
                        }
                        else if (brightness < 0.8f)
                        {
                            tilemap[x, y] = 1;  // Kamień (jaśniejszy)
                        }
                        else
                        {
                            tilemap[x, y] = 3;  // Dodatkowy teren (bardzo jasny)
                        }
                    }
                }

                Raylib.UnloadImage(perlinNoiseImage);
            }
        }



        public void Draw(Camera2D camera)
        {
            // Obliczanie zakresu kafelków do wyświetlenia
            int startX = (int)Math.Floor((camera.Target.X - Constantly.windowWidht / 2) / Constantly.tileSize);
            int startY = (int)Math.Floor((camera.Target.Y - Constantly.windowHeight / 2) / Constantly.tileSize);
            int endX = (int)Math.Ceiling((camera.Target.X + Constantly.windowWidht / 2) / Constantly.tileSize);
            int endY = (int)Math.Ceiling((camera.Target.Y + Constantly.windowHeight / 2) / Constantly.tileSize);

            startX = Math.Max(0, startX);
            startY = Math.Max(0, startY);

            // Zapewniamy, że endX i endY nie wychodzą poza mapę
            endX = Math.Min(Constantly.mapWidth, endX + 2); // Zmieniono na mapWidth
            endY = Math.Min(Constantly.mapHeight, endY + 2); // Zmieniono na mapHeight

            // Rysowanie kafelków mapy w określonym zakresie
            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    int tileType = tilemap[x, y];
                    Vector2 screenPosition = GetScreenPosition(x, y, camera);
                    Raylib.DrawTexture(textures[tileType], (int)screenPosition.X, (int)screenPosition.Y, Color.White);
                }
            }
        }



        private Vector2 GetScreenPosition(int x, int y, Camera2D camera)
        {
            return new Vector2(
             x * Constantly.tileSize - camera.Target.X / (Constantly.windowWidht / 2),
             y * Constantly.tileSize - camera.Target.Y / (Constantly.windowHeight / 2)
         );
        }

    }
}
