using System.Data.SqlClient;
using System.Data.SQLite;
using Models.Usuario;
namespace RepositorioUsuario;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _cadenaDeConexion = "Data Source=DB/kanban.db;Cache=Shared";

    public void Create(Usuario usuario)
    {
        var query = $"INSERT INTO usuario(nombre_de_usuario) VALUES(@nombre)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void Update(int idUsuario, Usuario usuario)
    {
        var query = $"UPDATE usuario SET nombre_de_usuario = @nombre WHERE id = @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));  
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public Usuario GetUsuarioById(int idUsuario)
    {
        var query = $"SELECT * FROM usuario WHERE id = @idUsuario";
        var usuario = new Usuario();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                if (reader.Read())
                {
                    usuario.Id = Convert.ToInt32(reader["id"]);
                    usuario.Nombre_de_usuario = reader["nombre_de_usuario"].ToString();
                }
            }
            conexion.Close();
        }
        return usuario;
    }

    public List<Usuario> GetAllUsuarios()
    {
        var query = $"SELECT * FROM usuario";
        var usuarios = new List<Usuario>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while(reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre_de_usuario = reader["nombre_de_usuario"].ToString()
                    };
                    usuarios.Add(usuario);
                }
            }
            conexion.Close();
        }
        return usuarios;
    }

    public bool DeleteUsuarioById(int idUsuario)
    {
        var query = $"DELETE FROM usuario WHERE id = @idUsuario";
        bool usuarioEliminado = false;
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            if ( command.ExecuteNonQuery() > 0)
            {
                usuarioEliminado = true;
            }
            conexion.Close();
        }
        return usuarioEliminado;
      
    }


   
}