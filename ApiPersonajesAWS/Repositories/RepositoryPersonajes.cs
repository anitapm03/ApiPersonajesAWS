﻿using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiPersonajesAWS.Repositories
{
    public class RepositoryPersonajes
    {
        private PersonajesContext context;

        public RepositoryPersonajes(PersonajesContext context)
        {
            this.context = context;
        }

        public async Task<List<Personaje>> GetPersonajesAsync()
        {
            return await this.context.Personajes.ToListAsync();
        }

        public async Task<Personaje> FindPersonajeAsync(int id)
        {
            return await this.context.Personajes.FirstOrDefaultAsync
                (x => x.IdPersonaje == id);
        }

        public async Task<int> GetMaxIdPersonajeAsync()
        {
            return await this.context.Personajes.
                MaxAsync(x => x.IdPersonaje) + 1;
        }

        public async Task CreatePersonajeAsync
            (string nombre, string imagen)
        {
            Personaje p = new Personaje 
            { 
                IdPersonaje = await this.GetMaxIdPersonajeAsync(),
                Nombre = nombre,
                Imagen = imagen
            };

            this.context.Personajes.Add(p);
            await this.context.SaveChangesAsync();
        }

    }
}
