using Microsoft.AspNetCore.Mvc.ApiExplorer;
using tl2_tp10_2023_EnzoPeralta96.Models;
using System.Data.SQLite;
namespace TareaRepositorio;
public class TareaRepository : ITareaRepository
{
    private readonly string _cadenaDeConexion;

    public TareaRepository(string CadenaDeConexion)
    {
        _cadenaDeConexion = CadenaDeConexion;
    }

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
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }
    public void Update(int IdTarea, Tarea tarea)
    {

        var query = $"UPDATE tarea SET id_tablero = @idTablero, nombre = @nombre, estado = @estado, descripcion = @descripcion, color = @color, id_usuario_asignado = @id_usuario_asignado WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", IdTarea));
            command.Parameters.Add(new SQLiteParameter("@idTablero", tarea.Id_tablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tarea.Nombre));
            command.Parameters.Add(new SQLiteParameter("@estado", tarea.EstadoTarea));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tarea.Descripcion));
            command.Parameters.Add(new SQLiteParameter("@color", tarea.Color));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_asignado", tarea.Id_usuario_asignado));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void UpdateEstado(int idTarea, Estado nuevoEstado)
    {
        var query = $"UPDATE tarea SET  estado=@nuevoEstado WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@nuevoEstado", nuevoEstado));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void AssignUser(int idTarea, int idUsuario)
    {
        var query = $"UPDATE tarea SET  id_usuario_asignado = @idUsuario WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void RemoveUser(int IdTarea)
    {
        var query = $"UPDATE tarea SET  id_usuario_asignado = NULL WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", IdTarea));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public Tarea GetTareaById(int idTarea)
    {
        var query = $"SELECT * FROM tarea WHERE id = @idTarea AND activo = 1";
        bool tareaEncontrada = false;
        Tarea tarea = new Tarea();
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
                    tarea.Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null;
                    tareaEncontrada = true;
                }
            }
            conexion.Close();
        }

        if (!tareaEncontrada) throw new Exception("Tarea no creada");

        return tarea;
    }



    public List<Tarea> GetTareasAsignadasByTablero(int idTablero, int idUsuario)
    {
        var query = $"SELECT tar.id, tar.id_tablero, tar.nombre, tar.estado, tar.descripcion, tar.color, tar.id_usuario_asignado, usu.nombre_de_usuario AS usuario_asignado FROM tarea tar INNER JOIN usuario usu ON tar.id_usuario_asignado = usu.id WHERE tar.id_tablero = @IdTablero AND tar.id_usuario_asignado = @IdUsuario AND tar.activo = 1";
        var tareas = new List<Tarea>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", idTablero));
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
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null,
                        Usuario_asignado = reader["usuario_asignado"].ToString()
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
        var query = $"SELECT tar.id, tar.id_tablero, tar.nombre, tar.estado, tar.descripcion,tar.color, tar.id_usuario_asignado, usu.nombre_de_usuario AS usuario_asignado FROM tarea tar LEFT JOIN usuario usu ON tar.id_usuario_asignado = usu.id WHERE tar.id_tablero = @idTablero AND tar.activo = 1";
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
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null,
                        Usuario_asignado = reader["usuario_asignado"].ToString()
                    };
                    tareas.Add(tarea);
                }
            }
            conexion.Close();
        }
        return tareas;
    }

    public List<Tarea> GetTareasByUsuario(int IdUsuario)
    {
        var query = $"SELECT tar.id, tar.id_tablero, tar.nombre, tar.estado, tar.descripcion,tar.color, tar.id_usuario_asignado, usu.nombre_de_usuario AS usuario_asignado, tar.activo FROM tablero tab INNER JOIN tarea tar ON tab.id = tar.id_tablero LEFT JOIN usuario usu ON tar.id_usuario_asignado = usu.id WHERE tab.id_usuario_propietario = @IdUsuario AND tab.activo = 1 AND tar.activo = 1";
        var tareas = new List<Tarea>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario", IdUsuario));

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
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null,
                        Usuario_asignado = reader["usuario_asignado"].ToString()
                    };
                    tareas.Add(tarea);
                }
            }
            conexion.Close();
        }
        return tareas;
    }

    public List<Tarea> GetTareasAsignadasByUsuario(int IdUsuario)
    {
        var query = $"SELECT tar.id, tar.id_tablero, tar.nombre, tar.estado, tar.descripcion, tar.color, tar.id_usuario_asignado, usu.nombre_de_usuario AS usuario_asignado FROM tarea tar INNER JOIN usuario usu ON tar.id_usuario_asignado = usu.id WHERE tar.id_usuario_asignado = @IdUsuario AND tar.activo = 1";
        var tareas = new List<Tarea>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario", IdUsuario));

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
                        Id_usuario_asignado = reader["id_usuario_asignado"] != DBNull.Value ? Convert.ToInt32(reader["id_usuario_asignado"]) : (int?)null,
                        Usuario_asignado = reader["usuario_asignado"].ToString()
                    };
                    tareas.Add(tarea);
                }
            }
            conexion.Close();
        }
        return tareas;
    }

    public bool UsuarioTieneTareasAsignadas(int idUsuario)
    {
        var query = $"SELECT COUNT(*) FROM tarea WHERE id_usuario_asignado = @idUsuario AND activo = 1";

        bool userTieneTareasAsignadas = false;

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();

            var command = new SQLiteCommand(query, conexion);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            int count = Convert.ToInt32(command.ExecuteScalar());

            userTieneTareasAsignadas = count > 0;

            conexion.Close();
        }
        return userTieneTareasAsignadas;
    }

    public bool TareaConUsuarioAsignado(int IdTarea)
    {
        var query = $"SELECT COUNT(*) FROM tarea WHERE id = @IdTarea AND id_usuario_asignado IS NOT NULL AND activo = 1";

        bool tareaAsignada = false;

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();

            var command = new SQLiteCommand(query, conexion);

            command.Parameters.Add(new SQLiteParameter("@IdTarea", IdTarea));

            int count = Convert.ToInt32(command.ExecuteScalar());

            tareaAsignada = count > 0;

            conexion.Close();
        }
        return tareaAsignada;
    }

    public void Delete(int idTarea)
    {
        var query = $"UPDATE tarea SET activo = 0 WHERE id = @idTarea";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idTarea", idTarea));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

}