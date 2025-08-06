using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiFest.Services.Helpers
{
    public class ImageConversion
    {
        public static byte[] HexToByteArray(string hex)
        {
            hex = hex.Replace("0x", "");
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }


        public static string ConvertImageToBase64String(string folder, string imageName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string imagePath = Path.Combine(currentDirectory, folder, imageName);




            try
            {
                if (File.Exists(imagePath))
                {
                    byte[] imageBytes = File.ReadAllBytes(imagePath);
                    return Convert.ToBase64String(imageBytes);
                }
                else
                {
                    Console.WriteLine("Image file not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading image file: {ex.Message}");
                return null;
            }
        }



        public static byte[] ConvertImageToByteArray(string folder, string imageName)
        {
            string currentDirectory = Directory.GetCurrentDirectory();
            string imagePath = Path.Combine(currentDirectory, folder, imageName);


            try
            {
                if (File.Exists(imagePath))
                {
                    return File.ReadAllBytes(imagePath);
                }
                else
                {
                    Console.WriteLine("Image file not found.");
                    return null;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading image file: {ex.Message}");
                return null;
            }
        }
    }
}
