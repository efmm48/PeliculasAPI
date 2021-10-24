using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PeliculasAPI.Data
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext>option): base (option)
        {
                
        }
        /*Comandos para agregar la migracion del proyecto a la base de datos:add-migration [Nombre de la migracion]*/
        /*Comandos para actualizar para que se confirmen los cambios en el proyecto:update-database*/
        
        public DbSet<Categoria> Categorias { get; set; } 
        public DbSet<Pelicula> Peliculas { get; set; }

    }
}
