﻿using System.ComponentModel.DataAnnotations;

namespace FilmesAPI.Models;

public class Filme
{
    [Key]
    [Required]
    public int Id { get; set; }
    [Required(ErrorMessage =" O título é obrigatório")]
    public string Titulo { get; set; }
    [Required(ErrorMessage = " O genero é obrigatório")]
    [MaxLength(50, ErrorMessage ="O tamanho de genero não pode exceder 50 caracteres")]
    public string Genero { get; set; }
    [Required]
    [Range(70,600,ErrorMessage ="A duração deve ter entre 70 e 600 minutos ")]
    public int Duracao { get; set; }

}