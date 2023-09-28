using AutoMapper;
using FilmesAPI.Data;
using FilmesAPI.Data.DTO;
using FilmesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FilmesAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class FilmeController : ControllerBase
{
    private FilmeContext _context;
    private IMapper _mapper;

    public FilmeController(FilmeContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    ///<summary>
    ///    Adiciona um filme ao banco de dados]
    ///    </summary>
    ///    <param name="filmeDTO"> Objeto com os campos necessários para criação de um filme</param>
    ///    <returns>IActionResult</returns>
    ///    <response code="201"> Caso inserção seja feita com sucesso</response>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public IActionResult AdicionaFilme([FromBody] CreateFilmeDTO filmeDTO)
    {
        Filme filme = _mapper.Map<Filme>(filmeDTO);
        _context.Filmes.Add(filme);
        _context.SaveChanges();
        return CreatedAtAction(nameof(RecuperarFilmePorId), new {id = filme.Id}, filme);
    }

    [HttpPut("{id}")]
    public IActionResult AtualizarFilme(int id, [FromBody] UpdateFilmeDTO filmeDTO)
    {

        var filme = _context.Filmes.FirstOrDefault(filme=> filme.Id == id);

        if (filme == null) return NotFound();

        _mapper.Map(filmeDTO, filme);
        _context.Filmes.Update(filme);
        _context.SaveChanges();
        return NoContent();
    }

    [HttpPatch("{id}")]
    public IActionResult AtualizarFilmeParcial(int id, JsonPatchDocument<UpdateFilmeDTO> patch)
    {

        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();

        var filmeParaAtualizar = _mapper.Map<UpdateFilmeDTO>(filme);

        patch.ApplyTo(filmeParaAtualizar, ModelState);

        if (!TryValidateModel(filmeParaAtualizar)) 
            return ValidationProblem(ModelState);


        _mapper.Map(filmeParaAtualizar, filme);

        _context.SaveChanges();
        
        return NoContent();
    }
    [HttpGet]
    public IEnumerable<ReadFilmeDTO> RecuperaFilmes([FromQuery]int skip=0,[FromQuery]int take = 50) 
    {

        return _mapper.Map<List<ReadFilmeDTO>>(_context.Filmes.Skip(skip).Take(take));

    }


    [HttpGet("{id}")]
    public IActionResult RecuperarFilmePorId(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);
        if (filme == null) return NotFound();

        var filmeDTO = _mapper.Map<ReadFilmeDTO>(filme);
        return Ok(filme);
    }
    [HttpDelete("{id}")]

    public IActionResult DeletaFilme(int id)
    {
        var filme = _context.Filmes.FirstOrDefault(filme => filme.Id == id);

        if (filme == null) return NotFound();
        _context.Filmes.Remove(filme);
        _context.SaveChanges();
        return NoContent();

    }
}
