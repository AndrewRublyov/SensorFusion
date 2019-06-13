using System.ComponentModel.DataAnnotations;

namespace SensorFusion.Web.Api.Data.Dtos
{
  public class SensorRenameDto
  {
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
  }
}