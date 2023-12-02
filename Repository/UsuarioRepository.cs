using System.Data.SqlClient;
using System.Data.SQLite;
using tl2_tp10_2023_EnzoPeralta96.Models;
namespace RepositorioUsuario;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly string _cadenaDeConexion;

    public UsuarioRepository(string CadenaDeConexion)
    {
        _cadenaDeConexion = CadenaDeConexion;
    }

    public void Create(Usuario usuario)
    {
        var query = $"INSERT INTO usuario(nombre_de_usuario,password,rol) VALUES(@nombre,@pass,@rol)";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));
            command.Parameters.Add(new SQLiteParameter("@pass", usuario.Password));
            var rolString = Enum.GetName(typeof(RolUsuario), usuario.Rol);
            command.Parameters.Add(new SQLiteParameter("@rol", rolString));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public void Update(int idUsuario, Usuario usuario)
    {
        var query = $"UPDATE usuario SET nombre_de_usuario = @nombre, password = @pass, rol = @rol WHERE id = @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.Parameters.Add(new SQLiteParameter("@nombre", usuario.Nombre_de_usuario));
            command.Parameters.Add(new SQLiteParameter("@pass", usuario.Password));
            var rolString = Enum.GetName(typeof(RolUsuario), usuario.Rol);
            command.Parameters.Add(new SQLiteParameter("@rol", rolString));
            command.ExecuteNonQuery();
            conexion.Close();
        }
    }

    public Usuario GetUsuarioById(int idUsuario)
    {
        var query = $"SELECT * FROM usuario WHERE id = @idUsuario";
        Usuario usuario = null;
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
                    usuario.Password = reader["password"].ToString();
                    usuario.Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString());
                }
            }
            conexion.Close();
        }
        if (usuario == null) throw new Exception("Usuario no encontrado");
        return usuario;
    }

    public List<Usuario> GetAllUsers()
    {
        var query = $"SELECT * FROM usuario";
        var usuarios = new List<Usuario>();
        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);

            using (SQLiteDataReader reader = command.ExecuteReader())
            {
                while (reader.Read())
                {
                    var usuario = new Usuario
                    {
                        Id = Convert.ToInt32(reader["id"]),
                        Nombre_de_usuario = reader["nombre_de_usuario"].ToString(),
                        Password = reader["password"].ToString(),
                        Rol = (RolUsuario)Enum.Parse(typeof(RolUsuario), reader["rol"].ToString())
                    };
                    usuarios.Add(usuario);
                }
            }
            conexion.Close();
        }
        return usuarios;
    }

    public void DeleteUsuarioById(int idUsuario)
    {
        var query = $"DELETE FROM usuario WHERE id = @idUsuario";

        using (SQLiteConnection conexion = new SQLiteConnection(_cadenaDeConexion))
        {
            conexion.Open();
            var command = new SQLiteCommand(query, conexion);
            command.Parameters.Add(new SQLiteParameter("@idUsuario", idUsuario));
            command.ExecuteNonQuery();
            conexion.Close();
        }

    }



}