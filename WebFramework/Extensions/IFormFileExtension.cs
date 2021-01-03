using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Http;

namespace WebFramework.Extensions
{
    public static class IFormFileExtension
    {
        public static bool IsImage(this IFormFile Image)
        {
            //Check Mime Type
            if
            (
                Image.ContentType.ToLower() != "image/jpg"  &&
                Image.ContentType.ToLower() != "image/jpeg" &&
                Image.ContentType.ToLower() != "image/png"
            )
            {
                return false;
            }

            //Check Extension
            if
            (
                Path.GetExtension(Image.FileName).ToLower() != ".jpg"  &&
                Path.GetExtension(Image.FileName).ToLower() != ".jpeg" &&
                Path.GetExtension(Image.FileName).ToLower() != ".png"
            )
            {
                return false;
            }

            //Check Readable file & Security
            try
            {
                if (!Image.OpenReadStream().CanRead)
                    return false;

                byte[] buffer = new byte[(int) Image.Length];
                Image.OpenReadStream().Read(buffer, 0, (int) Image.Length);
                string content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Image.OpenReadStream().Close();
            }

            return true;
        }
        
        public static bool IsVideo(this IFormFile Video)
        {
            //Check Mime Type
            if
            (
                Video.ContentType.ToLower() != "video/mp4" &&
                Video.ContentType.ToLower() != "video/avi"
            )
            {
                return false;
            }

            //Check Extension
            if
            (
                Path.GetExtension(Video.FileName).ToLower() != ".mp4" &&
                Path.GetExtension(Video.FileName).ToLower() != ".avi"
            )
            {
                return false;
            }

            //Check Readable file & Security
            try
            {
                if (!Video.OpenReadStream().CanRead)
                    return false;

                byte[] buffer = new byte[(int) Video.Length];
                Video.OpenReadStream().Read(buffer, 0, (int) Video.Length);
                string content = Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy", RegexOptions.IgnoreCase))
                    return false;
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                Video.OpenReadStream().Close();
            }

            return true;
        }
    }
}