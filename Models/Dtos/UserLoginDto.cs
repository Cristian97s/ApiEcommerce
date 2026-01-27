using System;
using System.ComponentModel.DataAnnotations;

namespace ApiEcommerce.Models.Dtos;

public class UserLoginDto
{
    [Required(ErrorMessage = "El Campo username es requerido")]
    public string? Username { get; set; }
    [Required(ErrorMessage = "El Campo password es requerido")]
    public string? Password { get; set; }
}
