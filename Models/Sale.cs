using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ToyotaWeb.Models;
public class Sale
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string Phone { get; set; }

    public string Image { get; set; }

    public string Description { get; set; }
}