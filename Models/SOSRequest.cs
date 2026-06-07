
namespace ToyotaWeb.Models;
public class SOSRequest
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Phone { get; set; }

    public string? Description { get; set; }
    public string?ImagePath { get; set; }

    public double Lat { get; set; }
    public double Lng { get; set; }

    public string Status { get; set; } // Pending / Assigned / Done

    public int? GarageId { get; set; }
}