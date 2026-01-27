using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class CreateUserDto
{
    [Required(ErrorMessage ="El Campo name es requerido")]
    public string? Name { get; set; }
    [Required(ErrorMessage ="El Campo username es requerido")]
    public string? Username { get; set; }
    [Required(ErrorMessage ="El Campo password es requerido")]
    public string? Password { get; set; }
    [Required(ErrorMessage ="El Campo role es requerido")]
    public string? Role { get; set; }
}
