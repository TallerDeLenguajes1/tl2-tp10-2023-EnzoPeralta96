using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Models.Tarea;
using System.Data.SQLite;
namespace TareaRepositorio;


public class TareaRepository : ITareaRepository
{
    private readonly string _cadenaDeConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void Create(int idTablero, Tarea tarea)
    {
        var query = $"INSERT INTO tarea(id_tablero,nombre,estado,descripcion,color,id_usuario_asignado) VALUES(@idTablero,@nombre,@estado,@descripcion,@color,@id_usuario_asignado)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.EstadoTarea));
            if (string.IsNullOrWhiteSpace(tarea.Descripcion) || string.IsNullOrEmpty(tarea.Descripcion))
            {
                command.Parameters.Add(new SQLiteParameter("@descripcion", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            }

            if (string.IsNullOrWhiteSpace(tarea.Color) || string.IsNullOrEmpty(tarea.Color))
            {
                command.Parameters.Add(new SQLiteParameter("@color", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            }

            if (tarea.Id_usuario_asignado <= 0)
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            }
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }
    public bool Update(int IdTarea, Tarea tarea)
    {

        bool tareaActualizada = false;

        var query = $"UPDATE tarea SET id_tablero=@idTablero, nombre=@nombre, estado=@estado, descripcion=@descripcion, color=@color, id_usuario_asignado=@id_usuario_asignado WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", IdTarea));
            command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.Id_tablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.EstadoTarea));

            if (string.IsNullOrWhiteSpace(tarea.Descripcion) || string.IsNullOrEmpty(tarea.Descripcion))
            {
                command.Parameters.Add(new SQLiteParameter("@descripcion", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            }

            if (string.IsNullOrWhiteSpace(tarea.Color) || string.IsNullOrEmpty(tarea.Color))
            {
                command.Parameters.Add(new SQLiteParameter("@color", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            }

            if (tarea.Id_usuario_asignado <= 0)
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            }

            if (command.ExecuteNonQuery() > 0)
            {
                tareaActualizada = true;
            }
            conexion.Close();
        }
        return tareaActualizada;
    }

    public Tarea GetTareaById(int idTarea)
    {
        var query = $"SELECT * FROM tarea WHERE id = @idTarea";
        var tarea = new Tarea();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tarea.Id = Convert.ToInt32(reader["id"]);
                    tarea.Id_tablero = Convert.ToInt32(reader["id_tablero"]);
                    tarea.Nombre = reader["nombre"].ToString();
                    tarea.EstadoTarea = (Estado)Enum.Parse(typeof(Estado), reader["estado"].ToString());
                    tarea.Descripcion = reader["descripcion"].ToString();
                    tarea.Color = reader["color"].ToString();
                    tarea.Id_usuario_asignado = Convert.ToInt32(reader["id_usuario_asignado"]);
                }
            }
            conexion.Close();
        }
        return tarea;
    }

    public List<Tarea> GetTareasByUser(int idUsuario)
    {
        var query = $"SELECT * FROM tarea WHERE id_usuario_asignado = @IdUsuario";
        var tareas = new List<Tarea>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_tablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (Estado)Enum.Parse(typeof(Estado), reader["estado"].ToString()),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null
                    };
                    tareas.Add(tarea);
                }
            }
            conexion.Close();
        }
        return tareas;
    }

    public List<Tarea> GetTareasByTablero(int idTablero)
    {
        var query = $"SELECT * FROM tarea WHERE id_tablero = @idTablero";
        var tareas = new List<Tarea>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTablero", idTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {

                    var tarea = new Tarea
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_tablero = Convert.ToInt32(reader["id_tablero"]),
                        Nombre = reader["nombre"].ToString(),
                        EstadoTarea = (Estado)Enum.Parse(typeof(Estado), reader["estado"].ToString()),
                        Descripcion = reader["descripcion"].ToString(),
                        Color = reader["color"].ToString(),
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null
                    };
                    tareas.Add(tarea);
                }
            }
            conexion.Close();
        }
        return tareas;
    }

    public bool Delete(int idTarea)
    {
        var query = $"DELETE FROM tarea WHERE id = @idTarea";
        bool tareaEliminada = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            if (command.ExecuteNonQuery() > 0)
            {
                tareaEliminada = true;
            }
            conexion.Close();
        }
        return tareaEliminada;
    }

    public bool AsignarUsuario(int idTarea, int idUsuario)
    {
        bool usuarioAsignado = false;
     
        var query = $"UPDATE tarea SET id_usuario_asignado=@id_usuario_asignado WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            if (idUsuario <= 0)
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", null));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", idUsuario));
            }

            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            if (command.ExecuteNonQuery() > 0)
            {
                usuarioAsignado = true;
            }
            conexion.Close();
        }
        return usuarioAsignado;
    }

    /*
        public bool AsignarUsuario(int idTarea, int idUsuario)
{
    bool usuarioAsignado = false;

    // Verificar si el usuario existe
    if (UsuarioExiste(idUsuario))
    {
        var query = $"UPDATE tarea SET id_usuario_asignado=@id_usuario_asignado WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            if (idUsuario <= 0)
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", DBNull.Value));
            }
            else
            {
                command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", idUsuario));
            }

            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));

            if (command.ExecuteNonQuery() > 0)
            {
                usuarioAsignado = true;
            }
            conexion.Close();
        }
    }

    return usuarioAsignado;
}

private bool UsuarioExiste(int idUsuario)
{
    // Realiza una consulta para verificar si el usuario con el id proporcionado existe
    // Retorna true si existe, false de lo contrario
    // Esto podría implicar ejecutar un SELECT COUNT(*) FROM Usuario WHERE id = @idUsuario
}

    */


}