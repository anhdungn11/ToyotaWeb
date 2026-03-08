using System.ComponentModel.DataAnnotations;
namespace ToyotaWeb.Models;
using System;
public class Contact
{
    public int Id { get; set; }

    [Required]
    public string FullName { get; set; }

    [Required]
    public string Phone { get; set; }

    public string Email { get; set; }

    public string CarName { get; set; }

    public string Message { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;
    public bool IsCalled { get; set; } = false;
    public string CallNote{get; set;}
}