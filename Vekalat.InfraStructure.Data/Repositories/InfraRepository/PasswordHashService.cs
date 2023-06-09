﻿using System;
using System.Security.Cryptography;
using System.Text;
using Vekalat.Application.Common.InfraServices;

namespace Vekalat.InfraStructure.Data.Repositories.InfraRepository
{
    public class PasswordHashService : IPasswordHashService
    {
        public string EncodePasswordMD5(string pass)
        {
            Byte[] originalBytes;
            Byte[] encodedBytes;
            MD5 md5;
            //Instantiate MD5CryptoServiceProvider, get bytes for original password and compute hash (encoded password)
            md5 = new MD5CryptoServiceProvider();

          
            originalBytes = ASCIIEncoding.Default.GetBytes(pass);
            encodedBytes = md5.ComputeHash(originalBytes);
            //Convert encoded bytes back to a 'readable' string
            return BitConverter.ToString(encodedBytes);

          //  return BitConverter.ToString(encodedBytes).Replace("-", string.Empty);
            
        }
    }
}