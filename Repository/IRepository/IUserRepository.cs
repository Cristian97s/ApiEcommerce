using System;
using ApiEcommerce.Models;
using ApiEcommerce.Models.Dtos;

namespace ApiEcommerce.Repository.IRepository;

// 1. Crear una interfaz llamada IUserRepository.
public interface IUserRepository
{
    //    - GetUsers
    //        → Devuelve todos los usuarios en ICollection del tipo User.
    ICollection<User> GetUsers();

    //    - GetUser
    //        → Recibe un id y devuelve un solo objeto User o null si no se encuentra.
    User? GetUser(int id);
    //    - IsUniqueUser
    //        → Recibe un nombre de usuario y devuelve un bool indicando si el nombre de usuario es único.
    bool IsUniqueUser(string username);

    //    - Login
    //        → Recibe un objeto UserLoginDto y devuelve un UserLoginResponseDto de forma asíncrona (Task).
    Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
    //    - Register
    //        → Recibe un objeto CreateUserDto y devuelve un objeto User de forma asíncrona (Task).
    Task<User> Register(CreateUserDto createUserDto);

}
