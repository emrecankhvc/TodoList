using System;
using System.ComponentModel.DataAnnotations;

public class FutureDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        if (value is DateTime date)
        {
            // Tarih bugünden (şimdiden) küçükse false döner
            return date >= DateTime.Now;
        }
        return false; // null ya da DateTime değilse geçersiz sayılır
    }

    public override string FormatErrorMessage(string name)
    {
        return $"{name} alanı bugünden önce olamaz.";
    }
}
