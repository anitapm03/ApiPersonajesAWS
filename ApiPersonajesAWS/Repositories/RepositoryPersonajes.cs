using ApiPersonajesAWS.Data;
using ApiPersonajesAWS.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MySqlConnector;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ApiPersonajesAWS.Repositories
{
    #region SQL
    /*DELIMITER //

    CREATE PROCEDURE EditarPersonaje(
        IN p_IDPERSONAJE INT,
        IN p_PERSONAJE VARCHAR(255),
        IN p_IMAGEN VARCHAR(255)
    )
    BEGIN
        UPDATE PERSONAJES
        SET
            PERSONAJE = p_PERSONAJE,
            IMAGEN = p_IMAGEN
        WHERE
            IDPERSONAJE = p_IDPERSONAJE;
    END //

    DELIMITER;*/

    #endregion
    public class RepositoryPersonajes
    {
        private PersonajesContext context;
        private string connectionString;

        public RepositoryPersonajes(PersonajesContext context, IConfiguration configuration)
        {
            this.context = context;
            this.connectionString = configuration.GetConnectionString("BBDD");
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

        public void EditarPersonaje(int id, string nombre, string imagen)
        {
            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();

                using (MySqlCommand cmd = new MySqlCommand("EditarPersonaje", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@p_IDPERSONAJE", id);
                    cmd.Parameters.AddWithValue("@p_PERSONAJE", nombre);
                    cmd.Parameters.AddWithValue("@p_IMAGEN", imagen);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}
