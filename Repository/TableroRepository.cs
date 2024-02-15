namespace tl2_tp10_2023_EnzoPeralta96.Repository.Tablero;
using System.Data.SQLite;
using tl2_tp10_2023_EnzoPeralta96.Models.Tablero;

public class TableroRepository : ITableroRepository
{
    private readonly string _cadenaDeConexion;

    public TableroRepository(string CadenaDeConexion)
    {
        _cadenaDeConexion = CadenaDeConexion;
    }

    public void Create(Tablero tablero)
    {
        var query = $"INSERT INTO tablero(id_usuario_propietario,nombre,descripcion) VALUES(@id_usuario_propietario,@nombre,@descripcion)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@id_usuario_propietario", tablero.Id_usuario_propietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void Update(int IdTablero, Tablero tablero)
    {
        var query = $"UPDATE tablero SET nombre = @nombre, descripcion = @descripcion WHERE id = @IdTablero";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public Tablero GetTableroById(int IdTablero)
    {
        var query = $"SELECT tablero.id, id_usuario_propietario, nombre_de_usuario, nombre, descripcion FROM tablero INNER JOIN usuario ON tablero.id_usuario_propietario = usuario.id WHERE tablero.id = @IdTablero";
        bool tableroEncontrado = false;
        var tablero = new Tablero();

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    tablero.Id = Convert.ToInt32(reader["id"]);
                    tablero.Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]);
                    tablero.Usuario_propietario = reader["nombre_de_usuario"].ToString();
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                    tableroEncontrado = true;
                }
            }
            conexion.Close();
        }

        if (!tableroEncontrado) throw new Exception("Tablero no creado");

        return tablero;
    }

    public List<Tablero> GetRestTableros(int IdUsuario)
    {
        var query = $"SELECT tablero.id, id_usuario_propietario, nombre_de_usuario, nombre, descripcion FROM tablero INNER JOIN usuario ON tablero.id_usuario_propietario = usuario.id WHERE tablero.id_usuario_propietario <> @IdUsuario AND tablero.activo = 1";
        var tableros = new List<Tablero>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario",IdUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Usuario_propietario = reader["nombre_de_usuario"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero);
                }
            }
            conexion.Close();
        }
        return tableros;
    }

    public List<Tablero> GetTableroByUser(int IdUsuario)
    {
        var query = $"SELECT tablero.id, id_usuario_propietario, nombre_de_usuario, nombre, descripcion FROM tablero INNER JOIN usuario ON tablero.id_usuario_propietario = usuario.id WHERE id_usuario_propietario = @IdUsuario AND tablero.activo = 1";
        var tableros = new List<Tablero>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario", IdUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Usuario_propietario = reader["nombre_de_usuario"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero);
                }
            }
            conexion.Close();
        }
        return tableros;
    }

    public List<Tablero> GetTableroByTareasAsignadas(int IdUsuario)
    {
        var query = "SELECT DISTINCT tab.id, id_usuario_propietario, nombre_de_usuario, tab.nombre, tab.descripcion FROM tablero tab INNER JOIN usuario ON tab.id_usuario_propietario = usuario.id INNER JOIN tarea ON tab.id = tarea.id_tablero WHERE tarea.id_usuario_asignado = @IdUsuario AND tab.activo = 1 AND tarea.activo = 1";

        var tableros = new List<Tablero>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdUsuario", IdUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
                        Usuario_propietario = reader["nombre_de_usuario"].ToString(),
                        Nombre = reader["nombre"].ToString(),
                        Descripcion = reader["descripcion"].ToString()
                    };
                    tableros.Add(tablero);
                }
            }
            conexion.Close();
        }
        return tableros;
    }

    public bool TableroByUserConTareasAsignadas(int IdUsuario)//Verifica si el usuario tiene tablero donde haya tareas que fueron asignadas a otro usuario
    {
        var query = $"SELECT COUNT(*) FROM tablero LEFT JOIN tarea ON tablero.id = tarea.id_tablero WHERE tablero.id_usuario_propietario = @idUsuario AND tarea.id_usuario_asignado IS NOT NULL";

        bool tableroEnUso = false;

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();

            var command = new SQLiteCommand(query, conexion);

            command.Parameters.Add(new SQLiteParameter("@idUsuario", IdUsuario));

            int count = Convert.ToInt32(command.ExecuteScalar());

            tableroEnUso = count > 0;

            conexion.Close();
        }
        return tableroEnUso;
    }

    public bool TableroConTareasAsignadas(int IdTablero)
    {
        var query = $"SELECT COUNT(*) FROM tablero LEFT JOIN tarea ON tablero.id = tarea.id_tablero WHERE tablero.id = @IdTablero AND tarea.id_usuario_asignado IS NOT NULL AND tablero.activo = 1 AND tarea.activo = 1";

        bool tableroConTareasAsignadas = false;

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();

            var command = new SQLiteCommand(query, conexion);

            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));

            int count = Convert.ToInt32(command.ExecuteScalar());

            tableroConTareasAsignadas = count > 0;

            conexion.Close();
        }
        return tableroConTareasAsignadas;
    }

    public void Delete(int IdTablero)
    {
        DeleteTareasByTablero(IdTablero);
        DeleteTablero(IdTablero);
    }

    private void DeleteTablero(int IdTablero)
    {
        var query = $"UPDATE tablero SET activo = 0 WHERE id = @IdTablero";
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    private void DeleteTareasByTablero(int IdTablero)
    {
        var query = $"UPDATE tarea SET activo = 0 WHERE id_tablero = @IdTablero";
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }
}