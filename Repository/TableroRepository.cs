namespace TableroRepositorio;
using System.Data.SQLite;
using Models.Tablero;

public class TableroRepository : ITableroRepository
{
    private readonly string _cadenaDeConexion = "Data Source=DB/kanban.db;Cache=Shared";
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

    public bool Update(int IdTablero, Tablero tablero)
    {
        bool tableroActaulizado = false;
        var query = $"UPDATE tablero SET id_usuario_propietario = @id_usuario_propietario, nombre = @nombre,descripcion = @descripcion WHERE id = @IdTablero";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));
            command.Parameters.Add(new SQLiteParameter("@id_usuario_propietario", tablero.Id_usuario_propietario));
            command.Parameters.Add(new SQLiteParameter("@nombre", tablero.Nombre));
            command.Parameters.Add(new SQLiteParameter("@descripcion", tablero.Descripcion));
            if (command.ExecuteNonQuery() > 0)
            {
                tableroActaulizado = true;
            }
            conexion.Close();
        }
        return tableroActaulizado;
    }

    public Tablero GetTableroById(int IdTablero)
    {
        var query = $"SELECT * FROM tablero WHERE id = @IdTablero";
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
                    tablero.Nombre = reader["nombre"].ToString();
                    tablero.Descripcion = reader["descripcion"].ToString();
                }
            }
            conexion.Close();
        }
        return tablero;
    }

    public List<Tablero> GetTableros()
    {
        var query = $"SELECT * FROM tablero";
        var tableros = new List<Tablero>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var tablero = new Tablero
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Id_usuario_propietario = Convert.ToInt32(reader["id_usuario_propietario"]),
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
        var query = $"SELECT * FROM tablero WHERE id_usuario_propietario = @IdUsuario";
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

    public bool Delete(int IdTablero)
    {
        var query = $"DELETE FROM tablero WHERE id = @IdTablero";
        bool tableroEliminado = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@IdTablero", IdTablero));
            if (command.ExecuteNonQuery() > 0)
            {
                tableroEliminado = true;
            }
            conexion.Close();
        }
        return tableroEliminado;

    }


}