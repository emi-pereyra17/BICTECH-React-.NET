using System;
using Microsoft.AspNetCore.Identity;
using BicTechBack.src.Core.Entities; 

class PasswordHashGenerator
{
    static void Main()
    {
        var hasher = new PasswordHasher<Usuario>();
        var usuario = new Usuario();
        var hash = hasher.HashPassword(usuario, "vegeta123");
        Console.WriteLine("Hash generado:");
        Console.WriteLine(hash);
    }
}