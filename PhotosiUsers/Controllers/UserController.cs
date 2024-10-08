﻿using System.Diagnostics.CodeAnalysis;
using Microsoft.AspNetCore.Mvc;
using PhotosiUsers.Dto;
using PhotosiUsers.Exceptions;
using PhotosiUsers.Service;

namespace PhotosiUsers.Controllers;

// Esclusi dalla code coverage perche' testabili tramite tramite eventualiu integration test al posto di unit test
[ExcludeFromCodeCoverage]
[Route("api/v1/users")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<IActionResult> Get() => Ok(await _userService.GetAsync());

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        return Ok(await _userService.GetByIdAsync(id));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UserDto userDto)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        try
        {
            var result = await _userService.UpdateAsync(id, userDto);
            return Ok($"Utente con ID {result.Id} salvato successo");
        }
        catch (UserException e)
        {
            return BadRequest($"Errore nella richiesta di inserimento: {e.Message}");
        }
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] UserDto userDto) => Ok(await _userService.AddAsync(userDto));

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
    {
        try
        {
            return Ok(await _userService.LoginAsync(loginDto));
        } 
        catch (UserException e)
        {
            // Per questioni di sicurezza torno sempre un 500 con messaggio generico
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        if (id < 1)
            return BadRequest("ID fornito non valido");

        var deleted = await _userService.DeleteAsync(id);
        if (!deleted)
            return StatusCode(500, "Errore nella richiesta di eliminazione");

        return Ok($"Utente con ID {id} eliminato con successo");
    }
}